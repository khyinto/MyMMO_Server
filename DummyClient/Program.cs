using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DummyClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // DNS ( Domain Name System )
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            // ip는 다수일수도 일다.
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            while (true)
            {
                // 휴대폰 설정
                Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    // 문지기 한테 입장 문의.
                    socket.Connect(endPoint);
                    Console.WriteLine("Connected To {0}", socket.RemoteEndPoint.ToString());

                    // 보낸다.
                    byte[] sendBuff = Encoding.UTF8.GetBytes("Request Client....................");
                    int sendBytes = socket.Send(sendBuff);

                    // 받는다.
                    byte[] recvBuff = new byte[1024];
                    int recvBytes = socket.Receive(recvBuff);
                    string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvBytes);
                    Console.WriteLine("From Server : {0}", recvData);

                    // 나간다
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex);
                }

                Thread.Sleep(100);
            }
        }
    }
}
