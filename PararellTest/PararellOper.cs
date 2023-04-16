using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PararellTest
{
    internal class PararellOper
    {
        Random random = new Random();
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public void LoopParallelCancel()
        {
            new Thread( () =>
            {
                Thread.Sleep(300);
                cancellationTokenSource.Cancel();
            }).Start();

            Stopwatch sw = new Stopwatch();
            sw.Start();
            ParallelOptions parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = 8;
            parallelOptions.CancellationToken = cancellationTokenSource.Token;

            try
            {
                Parallel.For(0, 10, parallelOptions, i =>
                {
                    long total = Longoperation();
                    Console.WriteLine($"{i} - {total}");
                    Thread.Sleep(1000);
                });
            } catch (OperationCanceledException exc)
            {
                Console.WriteLine(exc.Message);
            }
            sw.Stop();
            Console.WriteLine($"CancellationTokenSource - {sw.Elapsed.TotalMilliseconds}");
        }

        public void LoopNoPararell()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 10; i++)
            {
                long total = Longoperation();
                Console.WriteLine($"{i} - {total}");
            }
            sw.Stop();
            Console.WriteLine($"LoopNoPararell - {sw.Elapsed.TotalMilliseconds}");
        }

        public void LoopWithPararell()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            ParallelOptions parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = 8;
            Parallel.For(0, 10, parallelOptions, i =>
            {
                long total = Longoperation();
                Console.WriteLine($"{i} - {total}");
            });
            sw.Stop();
            Console.WriteLine($"LoopWithPararell - {sw.Elapsed.TotalMilliseconds}");
        }
        public void LoopWithPararellBreakStop()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            ParallelOptions parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = 5;
            Parallel.For(0, 10, parallelOptions, (i, loopState) =>
            {
                long total = Longoperation();
                if (i >= 5)
                {
                    // loopState.Break();
                    loopState.Stop();
                }
                Console.WriteLine($"{i} - {total}");
            });
            sw.Stop();
            Console.WriteLine($"LoopWithPararell - {sw.Elapsed.TotalMilliseconds}");
        }

        public void LoopWithPararellForEach()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            ParallelOptions parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = 8;
            List<int> integersList = Enumerable.Range(0, 10).ToList();
            Parallel.ForEach(integersList, parallelOptions, i =>
            {
                long total = Longoperation();
                Console.WriteLine($"{i} - {total}");
            });
            sw.Stop();
            Console.WriteLine($"LoopWithPararellForEach - {sw.Elapsed.TotalMilliseconds}");
        }

        public void ParallelInvoke()
        {
            ParallelOptions options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 4,
            };
            Parallel.Invoke(options,
                () => TestTask(1),
                () => TestTask(2),
                () => TestTask(3),
                () => TestTask(4)
                );
        }

        private long Longoperation()
        {
            long total = 0;
            for (int i = 0; i < 100_000_000; i++)
            {
                total += i;
            }
            return total;
        }

        private void TestTask(int nr)
        {
            Console.WriteLine($"Start zadania [{nr}]");
            Thread.Sleep(random.Next(500, 1200));
            Console.WriteLine($"Koniec Zadania [{nr}]");
        }
    }
}
