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
        public async Task SheduleBackUp(CancellationToken token)
        {
          
            string pathbackup = configuration["backuppath"];
            while (!token.IsCancellationRequested)
            {
                this.WriteToFile("=================================MYSQL BACUP SERVICE STARTED==================================", true);
                this.WriteToFile("Started taking backup.......", true);
                bool isError = false;
                DateTime d = DateTime.Now;
                string dateString = d.ToString("yyyyMMddhhmmss");
                string filename = "Backup_" + dateString + ".sql";
                string fullpath = Path.Combine(pathbackup, filename);
                MySQLDBOperations dbbackup = new MySQLDBOperations();
                if (!File.Exists(fullpath))
                {
                    try
                    {
                        
                        dbbackup.CreateBackup(fullpath);
                        isError = false;
                    }
                    catch(Exception e)
                    {
                        isError = true;
                        this.WriteToFile("Error occurred while taking backup. Reason : "+e.Message, true);
                    }
                }
                else
                {
                    this.WriteToFile("File already existed...unable to take backup...trying again.", true);
                    DateTime d2 = DateTime.Now;
                    string dateString2 = d2.ToString("yyyyMMddhhmmss");
                    string filename2 = "Backup_" + dateString2 + ".sql";
                    string fullpath2 = Path.Combine(pathbackup, filename2);
                    try
                    {
                        dbbackup.CreateBackup(fullpath2);
                        isError = false;
                        this.WriteToFile("Backup generated suceesfully with filename :" + filename2, true);
                    }
                    catch (Exception e)
                    {
                        isError = true;
                        this.WriteToFile("Error occurred Second time while taking backup. Reason : " + e.Message, true);
                    }

                }
                if (!isError)
                {
                    this.WriteToFile("Backup generated suceesfully with filename :" + filename, true);
                   
                }
                this.WriteToFile("===============================================END=================================================");
                //await Task.Delay(1000 * 60 * 60 * 24 * 7);
               await Task.Delay(1000 * 10);
            }
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
        private void WriteToFile(string text,bool forBackup=false)
        {
            string path = "";
            if(!forBackup) path = configuration["logpath"];
            else path = configuration["backuplogpath"];
            //  string path = $"C:\Users\pdsadmin\ServiceLog.txt"; 
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                if (forBackup)
                {
                    if (!text.StartsWith("="))
                        writer.WriteLine(string.Format(text + " at {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt")));
                    else
                        writer.WriteLine(text);
                    writer.Close();
                }
                else
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
}
