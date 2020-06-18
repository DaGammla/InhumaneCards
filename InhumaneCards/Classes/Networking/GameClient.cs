using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InhumaneCards.Classes.Networking {
	public class GameClient {

		//InhumaneGame game;
		private TcpClient socket;
		private bool connected = false;

		NetworkStream stream;

		Thread check_thread;

		Action<NetworkingData> onDataReceived = (_) => { };

		bool waiting_for_data = false;
		NetworkingData awaited_data = default;
		NetworkingData last_received_data = default;

		/*public GameClient(InhumaneGame game) {
			this.game = game;
		}*/

		public GameClient() {
			
		}

		public void SetDataReceiver(Action<NetworkingData> onDataReceived) {
			this.onDataReceived = onDataReceived;
		}

		public byte Connect(string address) {

			socket = new TcpClient();
			try {

				socket.ReceiveTimeout = 5000;
				socket.SendTimeout = 5000;

				socket.Connect(address, 16151);
				stream = socket.GetStream();

				while (!stream.DataAvailable) {
					Thread.Sleep(10);
				}

				byte[] id_arr = new byte[1];
				stream.Read(id_arr, 0, 1);
				byte id = id_arr[0];

				this.check_thread = new Thread(async () => {
					while (true) {
					
						byte[] data = new byte[2048];
						await stream.ReadAsync(data, 0, 2048);

						var networkingData = NetworkingData.FromBytes(data);

						onDataReceived(networkingData);
						last_received_data = networkingData;
						if (waiting_for_data) {
							awaited_data = networkingData;
							waiting_for_data = false;
						}
						
					}

				});
				this.check_thread.Start();
				

				connected = true;

				return id;
			} catch (Exception e) {
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);

				return 0;
			}
		}

		/*public NetworkingData WaitForData() {
			waiting_for_data = true;
			while (waiting_for_data) {
				Thread.Sleep(10);
			}
			return awaited_data;
		}*/

		public NetworkingData LastReceivedData() {
			return last_received_data;
		}

		public void SendDataToServer(NetworkingData data) {
			if (!connected)
				throw new Exception("Not connected to any Server");

			var byteData = data.ToByteArray();

			new Task(() => stream.Write(byteData, 0, byteData.Length)).Start();
		}

		public bool IsConnected() {
			return connected;
		}

	}
}