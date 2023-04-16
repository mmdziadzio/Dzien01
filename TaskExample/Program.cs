﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskExample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Task task1 = new Task(TestTask);
            //task1.Start();
            //task1.Wait();

            //Task task2 = Task.Factory.StartNew(TestTask);

            //Task task3 = Task.Run(() =>
            //{
            //    TestTask();
            //});

            //Task.WaitAll(new Task[] { task1,task2,task3});
            //Task.WaitAny( new Task[] { task1,task2,task3});
            //var task4 = Task.Run( () => Add(10, 20));
            ////Task.WaitAll(new Task[] { task4 });

            //task4.ContinueWith(t1 => 
            //{ 
            //    var task5 = Task.Run(() => Average(task4.Result, 2));
            //    task5.ContinueWith(t2 =>
            //    {
            //        Console.WriteLine(t2.Result);
            //    });
            //});

            // anulowanie zadania

            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            cts.CancelAfter(1000);
            Task taskCancel = Task.Run(() =>
            {
                try
                {
                    for (int i = 0; i < 10; i++)
                    {
                        Thread.Sleep(250);
                        Console.WriteLine($"Task iteracja {i}");
                        if (token.IsCancellationRequested)
                        {
                            token.ThrowIfCancellationRequested();
                        }
                    }
                } catch (OperationCanceledException exc)
                {
                    return; 
                }
            }, token);

            Console.WriteLine("Trwa wykonywanie zadania #1");
            Console.ReadKey();
        }

        static private void TestTask()
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] count value = {i}");
                Thread.Sleep(500);
            }
        }

        static int Add(int a, int b)
        {
            Console.WriteLine($"Liczenie sumy dla {a} oraz {b}");
            Thread.Sleep(500);
            return a + b;
        }

        static float Average(int sum, int n)
        {
            Console.WriteLine($"Liczenie średniej dla sumy {sum} oraz {n} liczb");
            Thread.Sleep(500);
            return sum / n;
        }

    }
}
