using System;
using System.Diagnostics;
using System.Net.Http;

namespace TestApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //string path = "http://172.31.31.16/api/articles";
            string path = "https://localhost:44337/api/articles";
            using (var httpClient = new HttpClient())
            {
                long time = 0;
                int countFailed = 0;
                
                for (int i = 0; i < 10; i++)
                {
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    var response = httpClient.GetAsync(path).Result;
                    stopwatch.Stop();
                    time += stopwatch.ElapsedMilliseconds;
                    if (!response.IsSuccessStatusCode)
                    {
                        countFailed += 1;
                    }
                }
                
                Console.WriteLine($"average duration: { time/10 } miliseconds.");
                Console.WriteLine($"failed responses: {countFailed}");
            }
        }
    }
}
