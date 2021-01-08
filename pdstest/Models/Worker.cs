using Microsoft.Extensions.Logging;
using pdstest.services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace pdstest.Models
{
    public class Worker : IWorker
    {
        private readonly ILogger<Worker> logger;
        public Worker(ILogger<Worker> logger)
        {
            this.logger = logger;
        }
        public async Task DoWork(CancellationToken token)
        {
            //string fileName = @"C:\Users\Public\Documents\logs.txt";
            //if(!File.Exists(fileName))
            //{
            //    File.Create(fileName).Dispose();
            //}
           // FileInfo fi = new FileInfo(fileName);

            while (!token.IsCancellationRequested)
            {
                //using (StreamWriter writer = new StreamWriter(fileName))
                //{

                //    writer.WriteLine("test");

                //}
              //  logger.LogInformation("Testing purpose.");
                //StringDateTimeFormatExtensions.WriteCustomString(fileName, "New file created: " + DateTime.Now.ToString());
                  //  StringDateTimeFormatExtensions.WriteCustomString(fileName, "Ram Test Purpose");
                    //StringDateTimeFormatExtensions.WriteCustomString(fileName, "Done!!!");
                    await Task.Delay(1000 * 10);
                
                //if (File.Exists(fileName))
                //{
                //    File.Delete(fileName);
                //}

                // Create a new file     




            }

        }
    }
}
