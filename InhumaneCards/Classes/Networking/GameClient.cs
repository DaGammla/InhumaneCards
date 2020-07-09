
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

		Task check_thread;

		Action<NetworkingData> onDataReceived = (_) => { };

		NetworkingData last_received_data = default;

		/*public GameClient(InhumaneGame game) {
			this.game = game;
		}*/

		public GameClient() {
			
		}

		public void SetDataReceiver(Action<NetworkingData> onDataReceived) {
			this.onDataReceived = onDataReceived;
		}

		public async Task<byte> Connect(string address) {

			socket = new TcpClient();
			try {

				socket.ReceiveTimeout = 5000;
				socket.SendTimeout = 5000;

				socket.Connect(address, 7674);
				stream = socket.GetStream();

				byte[] id_arr = new byte[1];
				await stream.ReadAsync(id_arr, 0, 1);
				byte id = id_arr[0];

				this.check_thread = new Task(async () => {
					try {

						while (true) {

							byte[] data = new byte[2048];
							await stream.ReadAsync(data, 0, 2048);

							var networkingData = NetworkingData.FromBytes(data);

							onDataReceived(networkingData);
							last_received_data = networkingData;
						}
					} catch {

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

		public void StopClient() {
			try {

				onDataReceived = (_) => { };
				last_received_data = default;
				connected = false;
				stream = null;
				check_thread = null;
				socket.Close();
				socket = null;
			} catch {

			}
		}

		public NetworkingData LastReceivedData() {
			return last_received_data;
		}

		

		public void SendDataToServer(NetworkingData data) {
			/*if (!connected)
				throw new Exception("Not connected to any Server");*/

			var byteData = data.ToByteArray();

			new Task(() => stream.Write(byteData, 0, byteData.Length)).Start();
		}

		public bool IsConnected() {
			return connected;
		}

	}
}