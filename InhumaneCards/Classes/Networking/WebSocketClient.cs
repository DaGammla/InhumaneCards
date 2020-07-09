using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace InhumaneCards.Classes.Networking {
	public class WebSocketClient {
		TcpClient tcpClient;
        NetworkStream stream;
		public WebSocketClient(TcpClient tcpClient) {
			this.tcpClient = tcpClient;
            this.stream = tcpClient.GetStream();
		}

        public byte[] Read(int size) {
            byte[] buffer = new byte[size];
            stream.Read(buffer, 0, size);
            return GetDecodedData(buffer, size);
        }

        public async Task<byte[]> ReadAsync(int size) {
            byte[] buffer = new byte[size];
            await stream.ReadAsync(buffer, 0, size);
            return GetDecodedData(buffer, size);
        }

        public void Write(byte[] bytes) {

            byte[] frame = GetFrameFromBytes(bytes);

            stream.Write(frame, 0, frame.Length);
        }

        public void Close() {
            stream.Close();
            tcpClient.Close();
        }

        public static byte[] GetDecodedData(byte[] buffer, int length) {
            byte b = buffer[1];
            int dataLength = 0;
            int totalLength = 0;
            int keyIndex = 0;

            if (b - 128 <= 125) {
                dataLength = b - 128;
                keyIndex = 2;
                totalLength = dataLength + 6;
            }

            if (b - 128 == 126) {
                dataLength = BitConverter.ToInt16(new byte[] { buffer[3], buffer[2] }, 0);
                keyIndex = 4;
                totalLength = dataLength + 8;
            }

            if (b - 128 == 127) {
                dataLength = (int)BitConverter.ToInt64(new byte[] { buffer[9], buffer[8], buffer[7], buffer[6], buffer[5], buffer[4], buffer[3], buffer[2] }, 0);
                keyIndex = 10;
                totalLength = dataLength + 14;
            }

            if (totalLength > length)
                throw new Exception("The buffer length is small than the data length");

            byte[] key = new byte[] { buffer[keyIndex], buffer[keyIndex + 1], buffer[keyIndex + 2], buffer[keyIndex + 3] };

            int dataIndex = keyIndex + 4;
            int count = 0;
            for (int i = dataIndex; i < totalLength; i++) {
                buffer[i] = (byte)(buffer[i] ^ key[count % 4]);
                count++;
            }

            byte[] retBytes = new byte[dataLength];

            Array.Copy(buffer, dataIndex, retBytes, 0, dataLength);

            return retBytes;
        }

        public static byte[] GetFrameFromBytes(byte[] bytesRaw) {
            byte[] response;
            byte[] frame = new byte[10];

            int indexStartRawData = -1;
            int length = bytesRaw.Length;

            frame[0] = (byte)(128 + (int)1);
            if (length <= 125) {
                frame[1] = (byte)length;
                indexStartRawData = 2;
            } else if (length >= 126 && length <= 65535) {
                frame[1] = (byte)126;
                frame[2] = (byte)((length >> 8) & 255);
                frame[3] = (byte)(length & 255);
                indexStartRawData = 4;
            } else {
                frame[1] = (byte)127;
                frame[2] = (byte)((length >> 56) & 255);
                frame[3] = (byte)((length >> 48) & 255);
                frame[4] = (byte)((length >> 40) & 255);
                frame[5] = (byte)((length >> 32) & 255);
                frame[6] = (byte)((length >> 24) & 255);
                frame[7] = (byte)((length >> 16) & 255);
                frame[8] = (byte)((length >> 8) & 255);
                frame[9] = (byte)(length & 255);

                indexStartRawData = 10;
            }

            response = new byte[indexStartRawData + length];

            int i, reponseIdx = 0;

            //Add the frame bytes to the reponse
            for (i = 0; i < indexStartRawData; i++) {
                response[reponseIdx] = frame[i];
                reponseIdx++;
            }

            //Add the data bytes to the response
            for (i = 0; i < length; i++) {
                response[reponseIdx] = bytesRaw[i];
                reponseIdx++;
            }

            return response;
        }
    }
}
