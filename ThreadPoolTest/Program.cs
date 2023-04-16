using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPoolTest
{
    internal class Program
    {
        static int totalUrls;
        static void Main(string[] args)
        {

            List<string> urls = new List<string>
            {
                "https://onet.pl",
                "https://wp.pl",
                "https://alx.pl",
                "https://gazeta.pl"
            };
            totalUrls = urls.Count;

            ThreadPool.SetMinThreads(1, 1);
            ThreadPool.SetMaxThreads(15, 15);
          //  for (int i = 0; i < 10; i++)
          //  {
           //     ThreadPool.QueueUserWorkItem(new WaitCallback(CustomMrthod));
          //  }

            foreach (var url in urls)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(DownloadData), url);
            }
       
            Console.ReadKey();

        }

        static void DownloadData(object urlObject)
        {
            string url = (string)urlObject;
            Console.WriteLine($"Pobieram date z {url}");
            WebClient webClient = new WebClient();
            string data = webClient.DownloadString(url);
            Console.WriteLine($"Zakońćzono pobieranie {url}, liczba danych {data.Length}");
            Interlocked.Decrement(ref totalUrls);
        }

        static void CustomMrthod(object obj)
        {
            Thread thread = Thread.CurrentThread;
            Console.WriteLine($"ThreadID: {thread.ManagedThreadId}");
            Thread.Sleep(5000);
        }
    }
}
