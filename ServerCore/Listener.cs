using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace ServerCore
{
    class Listener
    {
        Socket _listenSocket;

        Action<Socket> _onAcceptHandler;


        public void Init(IPEndPoint endPoint , Action<Socket> onAcceptHandler )
        {
            _listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _onAcceptHandler += onAcceptHandler;
            // 문지기 교육
            _listenSocket.Bind(endPoint);
            // 영업시작
            // backlog : 최대대기수
            _listenSocket.Listen(10);

            //  두 가지 방식의 수락 방식.
            // 직접적인 호출 방식과 , Completed 즉 델리게이트 방식의 연결 방식.
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
            RegisterAccept(args);
        }

        /// <summary>
        /// 등록 수락.
        /// </summary>
        /// <param name="args"></param>
        void RegisterAccept ( SocketAsyncEventArgs args)
        {
            // 재사용시 데이터 초기화
            args.AcceptSocket = null;

            //(어떤 일이) 있을 때 까지, …을 기다리는 동안
            bool pending = _listenSocket.AcceptAsync(args);
            // 펜딩없이 완료가 되었다.
            if (pending == false)
                OnAcceptCompleted( null , args);
        }
        void OnAcceptCompleted (object sender , SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                // TODO
                _onAcceptHandler.Invoke(args.AcceptSocket);
            }
            else
                Console.WriteLine(args.SocketError.ToString());

            RegisterAccept(args);

        }



        public Socket Accept()
        {
            _listenSocket.AcceptAsync();
            return _listenSocket.Accept();
        }
    }
}
