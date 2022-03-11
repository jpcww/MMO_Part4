using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    class Program
    {
        static int x = 0;
        static int y = 0;
        static int r1 = 0;
        static int r2 = 0;

        static void Thread_1()
        {
            y = 1;  // store a value into y

            Thread.MemoryBarrier();

            r1 = x; // load the value of x into r1
        }

        static void Thread_2()
        {
            x = 1;  // store a value into x

            Thread.MemoryBarrier();

            r2 = y; // load the value of y into r2
        }

        static void Main(string[] args)
        {
            int count = 0;

            while (true)
            {
                count++;

                x = y = r1 = r2 = 0;

                Task task1 = new Task(Thread_1);
                Task task2 = new Task(Thread_2);

                task1.Start();
                task2.Start();

                Task.WaitAll(task1, task2); // the main thread will wait for the 2 tasks

                if (r1 == 0 && r2 == 0)
                    break;
            }

            Console.WriteLine($"it takes {count} times to get out of the loop");
        }
    }
}
