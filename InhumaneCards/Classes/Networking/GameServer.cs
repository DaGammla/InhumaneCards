using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InhumaneCards.Classes.Networking {
	public class GameServer {

		InhumaneGame game;

		private List<ClientConnection> connections = new List<ClientConnection>();
		private TcpListener socket;

		private WebSocketServer webSocket;

		private bool running = false;
		Action<NetworkingData, /* CLient ID */ byte> onDataReceived = (_, __) => { };

		private bool acceptingClients = false;

		public GameServer(InhumaneGame game) {
			this.game = game;
		}

		public void SetDataReceiver(Action<NetworkingData, byte> onDataReceived) {
			this.onDataReceived = onDataReceived;
		}

		public void StartServer() {

			/*if (!NetworkInterface.GetIsNetworkAvailable()) {
				throw new Exception("Not connected to any Network");
			}*/

			IPAddress ip = IPAddress.Any;

			try {

				socket = new TcpListener(ip, 7674);
				socket.Start();

				webSocket = new WebSocketServer(ip, 7675);
				webSocket.Start();

			} catch (Exception e){
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);
				return;
			}

			new Thread(() => {

				running = true;

				acceptingClients = true;

				try {


					while (acceptingClients) {

						TcpClient client = socket.AcceptTcpClient();
						byte clientId = (connections.Count + 1).B();

						if (acceptingClients) {
							ClientConnection connection = new ClientConnection(client, clientId, DataReceivedWrapper);
							connections.Add(connection);
							game.OnClientConnected(clientId);
						} else {
							client.Close();
						}
					}
				} catch {

					game.BackToMainMenu();
				}

			}).Start();

			new Thread(() => {
				try {


					while (acceptingClients) {

						WebSocketClient client = webSocket.AccecptWebSocketClient();
						byte clientId = (connections.Count + 1).B();

						if (acceptingClients) {
							ClientConnection connection = new ClientConnection(client, clientId, DataReceivedWrapper);
							connections.Add(connection);
							game.OnClientConnected(clientId);
						} else {
							client.Close();
						}
					}
				} catch {

					game.BackToMainMenu();
				}

			}).Start();

		}

		public void StopServer() {
			try {

				StopAcceptingClients();
				for (byte i = 0; i < connections.Count; i++) {
					connections[i].StopConnection();
				}
				running = false;
				socket.Stop();
				webSocket.Stop();
				socket = null;
				onDataReceived = (_, __) => { };
			} catch {

			}
		}

		private void DataReceivedWrapper(NetworkingData data, byte clientId) {
			onDataReceived(data, clientId);
		}

		public void StopAcceptingClients() {
			this.acceptingClients = false;
		}

		public void MulticastData(NetworkingData data) {

			/*if (!running)
				throw new Exception("Server is not running!");*/

			//i does not represent the clients id but is just an iterator
			for (byte i = 0; i < connections.Count; i++) {
				byte copy_i = i;
				new Task(() => connections[copy_i].SendData(data)).Start();
			}
		}

		public void MulticastDataExcept(NetworkingData data, params byte[] excluded_ids) {
			/*if (!running)
				throw new Exception("Server is not running!");*/

			//Client ids start at 1 as id 0 is always the server
			for (byte id = 1; id < connections.Count + 1; id++) {
				if (excluded_ids.Contains(id))
					continue;

				byte copy_id = id;

				//An client with the id n is positioned at n - 1 in the connections list
				new Task(() => connections[copy_id - 1].SendData(data)).Start();
			}
		}


		public ClientConnection GetConnection(byte id) {
			return connections[id];
		}

		public bool IsRunning() {
			return running;
		}

		public byte ConnectionCount() {
			return connections.Count.B();
		}

	}

	public class ClientConnection {

		TcpClient client;
		WebSocketClient webClient;
		NetworkStream stream;

		Thread check_thread;

		bool webSocket = false;


		public ClientConnection(TcpClient client, byte id, Action<NetworkingData, byte> onDataReceived) {

			client.ReceiveTimeout = 5000;
			client.SendTimeout = 5000;

			this.client = client;
			this.stream = client.GetStream();

			byte[] id_arr = new byte[1];
			id_arr[0] = id;
			stream.Write(id_arr, 0, 1);

			this.check_thread = new Thread(async () => {
				try {

					while (true) {
						byte[] data = new byte[2048];
						await stream.ReadAsync(data, 0, 2048);
						onDataReceived(NetworkingData.FromBytes(data), id);
					}
				} catch {

				}
			});
			this.check_thread.Start();
		}

		public ClientConnection(WebSocketClient client, byte id, Action<NetworkingData, byte> onDataReceived) {

			webSocket = true;

			this.webClient = client;

			byte[] id_arr = new byte[1];
			id_arr[0] = id;
			webClient.Write(id_arr);

			this.check_thread = new Thread(async () => {
				try {

					while (true) {
						byte[] data = await webClient.ReadAsync(2048);
						onDataReceived(NetworkingData.FromBytes(data), id);
					}
				} catch {

				}
			});
			this.check_thread.Start();
		}



		public void SendData(NetworkingData data) {
			byte[] byteData = data.ToByteArray();

			if (webSocket) {
				webClient.Write(byteData);
			} else {
				stream.Write(byteData, 0, byteData.Length);
			}
			
		}

		public void StopConnection() {
			try {
				check_thread = null;

				if (webSocket) {
					webClient.Close();
				} else {
					client.Close();
					stream.Close();
				}
				
				client = null;
				stream = null;

			} catch {

			}
		}
	}
}