using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.NetworkInformation {
	public class IPAddress {
		public static IPAddress Any;
	}


}

namespace System.Net.Sockets {
	public class TcpListener {
		public TcpListener(IPAddress a, int b) {

		}

		public void Start() {
		}
		public void Stop() {
		}

		public TcpClient AcceptTcpClient() {
			return null;
		}

		public async Task<TcpClient> AcceptTcpClientAsync() {
			return null;
		}
	}

	public class TcpClient {

		public int ReceiveTimeout;
		public int SendTimeout;
		public int Available;

		Retyped.dom.WebSocket webSocket;

		NetworkStream stream;

		public void Close() {
			stream.Close();
		}

		public void Connect(string ip, int port) {
			port++; //From 7674 to 7675
			webSocket = new Retyped.dom.WebSocket($"ws://{ip}:{port}");
			this.stream = new NetworkStream(webSocket);
		}

		public NetworkStream GetStream() {
			return stream;
		}
	}

	public class NetworkStream : IDisposable {

		Retyped.dom.WebSocket webSocket;

		byte[] newestMessage = null;

		public NetworkStream(Retyped.dom.WebSocket webSocket) {
			this.webSocket = webSocket;
			webSocket.onmessage = (msg) => {
				newestMessage = Encoding.UTF8.GetBytes((string) msg.data);
				Console.WriteLine((string)msg.data);
			};
			webSocket.onopen = (_) => {

				//webSocket.send("Hello");

				//Console.WriteLine("HELL YEAS");
			};
		}

		public bool DataAvailable {
			get {
				return true;
			}
		}

		public void Write(byte[] bytes, int b, int c) {
			string stringBytes = Encoding.UTF8.GetString(bytes);
			//Console.WriteLine($"Sending \"{stringBytes}\"");
			webSocket.send(stringBytes);
		}

		public async Task ReadAsync(byte[] a, int b, int c) {

			while (newestMessage == null) {
				await Task.Delay(10);
			}

			var newMesg = newestMessage;
			newestMessage = null;

			for (int i = 0; i < c; i++) {
				if (i < newMesg.Length) {
					a[i] = newMesg[i];
				}
			}
		}

		public byte[] Read(byte[] a, int b, int c) {
			var newMesg = newestMessage;
			newestMessage = null;
			return newMesg;
		}

		public void Close() {
			webSocket.close();
		}

		public void Dispose() {
			Close();
		}
	}
}


namespace System.Security.Cryptography {
	public class SHA1 {
		public static SHA1 Create() {
			return null;
		}

		public byte[] ComputeHash(byte[] a) {
			return null;
		}
	}

}
