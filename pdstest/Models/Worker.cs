using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using pdstest.DAL;
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
        private readonly IConfiguration configuration;
        public Worker(ILogger<Worker> logger, IConfiguration config)
        {
            this.logger = logger;
            this.configuration = config;
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
                MySQLDBOperations dbOps = new MySQLDBOperations();
                int count = 0;
                string msg = "";
                try
                {
                    int checkCount = 0;
                    checkCount = dbOps.ClearInactiveSessions("c");
                    if (checkCount > 0)
                    {
                        this.WriteToFile("==========================================================================================");
                         msg = string.Format("Found {0} inactive sessions and if they dont signed out, will delete after ten minutes", checkCount);
                        this.WriteToFile(msg);
                        await Task.Delay(1000 * 60 * 10);
                        count = dbOps.ClearInactiveSessions("d");
                        if(count == 0)
                        {
                            msg = string.Format("All sessions signed out before removing");
                            this.WriteToFile(msg);
                            this.WriteToFile("==========================================================================================");
                        }

                    }
                }
                catch(Exception e)
                {
                     msg = string.Format("Something went wrong while removing Sessions. Reason : {0}", e.Message);
                    this.WriteToFile(msg);
                    count = 0;
                    this.WriteToFile("==========================================================================================");
                }
              
                if(count > 0)
                {
                     msg = string.Format("Cleared {0} inactive sessions", count);
                    this.WriteToFile(msg);
                    this.WriteToFile("==========================================================================================");
                }
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
        private void WriteToFile(string text)
        {
            string path = configuration["logpath"];
          //  string path = $"C:\Users\pdsadmin\ServiceLog.txt"; 
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                if (!text.StartsWith("="))
                    writer.WriteLine(string.Format(text + " at {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt")));
                else
                    writer.WriteLine(text);
                writer.Close();
            }
        }
    }
}
