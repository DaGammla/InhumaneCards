using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace InhumaneCards.Classes.Networking {
    class WebSocketServer {

        TcpListener tcpServer;

        public WebSocketServer(IPAddress ip, int port) {
            tcpServer = new TcpListener(ip, port);
        }

        public void Start() {
            tcpServer.Start();
        }

        public void Stop() {
            tcpServer.Stop();
        }

        public WebSocketClient AccecptWebSocketClient() {

            Retry:

            TcpClient tcpClient = tcpServer.AcceptTcpClient();

            NetworkStream stream = tcpClient.GetStream();

            
            while (!stream.DataAvailable)
                ;
            while (tcpClient.Available < 3)
                ; // match against "get"

            byte[] bytes = new byte[tcpClient.Available];
            stream.Read(bytes, 0, tcpClient.Available);
            string s = Encoding.UTF8.GetString(bytes);

            if (Regex.IsMatch(s, "^GET", RegexOptions.IgnoreCase)) {
                Console.WriteLine("=====Handshaking from client=====\n{0}", s);

                // 1. Obtain the value of the "Sec-WebSocket-Key" request header without any leading or trailing whitespace
                // 2. Concatenate it with "258EAFA5-E914-47DA-95CA-C5AB0DC85B11" (a special GUID specified by RFC 6455)
                // 3. Compute SHA-1 and Base64 hash of the new value
                // 4. Write the hash back as the value of "Sec-WebSocket-Accept" response header in an HTTP response
                string swk = Regex.Match(s, "Sec-WebSocket-Key: (.*)").Groups[1].Value.Trim();
                string swka = swk + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
                byte[] swkaSha1 = System.Security.Cryptography.SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(swka));
                string swkaSha1Base64 = Convert.ToBase64String(swkaSha1);

                // HTTP/1.1 defines the sequence CR LF as the end-of-line marker
                byte[] response = Encoding.UTF8.GetBytes(
                    "HTTP/1.1 101 Switching Protocols\r\n" +
                    "Connection: Upgrade\r\n" +
                    "Upgrade: websocket\r\n" +
                    "Sec-WebSocket-Accept: " + swkaSha1Base64 + "\r\n\r\n");

                stream.Write(response, 0, response.Length);

                var webClient = new WebSocketClient(tcpClient);

                return webClient;
            } else {
                goto Retry;
            }
        }
    }
}
