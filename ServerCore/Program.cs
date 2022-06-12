﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    class Program
    {





        #region 
        /*
        volatile static bool _stop = false;

        static void ThreadMain()
        {
            Console.WriteLine("쓰레드 시작");

            while (_stop == false)
            {
                // 누군가가 stop 신호를 해주기를 기다린다.
            }

            Console.WriteLine("쓰레드 종료");
        }
        */
        #endregion


        static void Main(string[] args)
        {

            #region 
            /*
            Task t = new Task(ThreadMain);
            t.Start();

            //////////////////////////////////////////////////////////////////////////
            // 1초 후에 스탑 하겠지. 
            Thread.Sleep(1000);

            _stop = true;

            Console.WriteLine("Stop 호출");
            Console.WriteLine("종료 대기중");
            t.Wait();
            Console.WriteLine("종료성공");
            */
            #endregion

            InterLocked();


        }

        private static void Cashed()
        {
            #region 캐시
            int[,] arr = new int[10000, 10000];

            {
                long now = DateTime.Now.Ticks;
                for (int y = 0; y < 10000; y++)
                    for (int x = 0; x < 10000; x++)
                        arr[y, x] = 1;
                long end = DateTime.Now.Ticks;
                Console.WriteLine($"(y,x) 순서 걸린 시간 {end - now}");
            }
            {
                long now = DateTime.Now.Ticks;
                for (int y = 0; y < 10000; y++)
                    for (int x = 0; x < 10000; x++)
                        arr[x, y] = 1;
                long end = DateTime.Now.Ticks;
                Console.WriteLine($"(x,y) 순서 걸린 시간 {end - now}");
            }
            #endregion
        }


        #region 메모리 베리어
        static int x = 0;
        static int y = 0;
        static int r1 = 0;
        static int r2 = 0;

        static void Thread_1()
        {
            y = 1;  // Store y 
            Thread.MemoryBarrier();
            r1 = x;  // Load x
        }

        static void Thread_2()
        {
            x = 1;  // Store x
            Thread.MemoryBarrier();
            r2 = y;  // Load y
        }
        private static void MemoryBarryer()
        {
            int count = 0;
            while (true)
            {
                count++;
                x = y = r1 = r2 = 0;
                Task t1 = new Task(Thread_1);
                Task t2 = new Task(Thread_2);
                t1.Start();
                t2.Start();
                // 메인쓰레드가 끝날때까지 대기
                Task.WaitAll(t1, t2);

                if (r1 == 0 && r2 == 0)
                    break;
            }
            Console.WriteLine("{0} 번만에 빠져나옴", count);
        }
        #endregion

        #region Interlocked
        static int number = 0 ;

        static void Interlock_Thread_1()
        {
            /*  레이스컨디션 발생
            for (int i = 0; i < 10000 ; i++)
            {
                number++;
            }
            */
            for (int i = 0; i < 10000; i++)
            {
                Interlocked.Increment(ref number);
            }

        }

        static void Interlock_Thread_2()
        {
            /*레이스컨디션 발생
            for (int i = 0; i < 10000; i++)
            {
                number--;
            }
            */
            for (int i = 0; i < 10000; i++)
            {
                Interlocked.Decrement(ref number);
            }
        }

        private static void InterLocked()
        {
            Task t1 = new Task(Interlock_Thread_1);
            Task t2 = new Task(Interlock_Thread_2);
            t1.Start();
            t2.Start();
            Task.WaitAll(t1, t2);
            Console.WriteLine(number);
        }
        #endregion

    }
}
