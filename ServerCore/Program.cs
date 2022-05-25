using System;
using System.Threading;

namespace ServerCore
{
    class Program
    {
        static void MainThread(object state)
        {
            for (int i = 0; i < 5 ; i++)
            {
                Console.WriteLine("Hello Thread");
            }
            
        }

        static void Main(string[] args)
        {
            ThreadPool.QueueUserWorkItem(MainThread);

            // 쓰레드 생성 비용은 비싸기 때문에 쓰레드풀을 이용한다.
            //Thread t = new Thread(MainThread);
            //t.Name = "Test Thread";
            //t.IsBackground = true;  // 메인쓰레드가 종료되면 바로 프로세스를 종료한다.
            //t.Start();
            //Console.WriteLine("Waiting for Thread");
            //t.Join();
            //Console.WriteLine("Hello World!");
        }
    }
}
