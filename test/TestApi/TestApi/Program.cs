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
                var stopwatch = new Stopwatch();
                int countFailed = 0;
                stopwatch.Start();
                for (int i = 0; i < 10; i++)
                {
                    var response = httpClient.GetAsync(path).Result;
                    if (!response.IsSuccessStatusCode)
                    {
                        countFailed += 1;
                    }
                }
                stopwatch.Stop();
                Console.WriteLine($"average duration: {stopwatch.ElapsedMilliseconds / 1000} seconds.");
                Console.WriteLine($"failed responses: {countFailed}");
            }
        }
    }
}
