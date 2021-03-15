using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using pdstest.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace pdstest.DAL
{
    public class MySqlBackupService : IHostedService,IDisposable//BackgroundService
    {
        private readonly IWorker _worker;
        private readonly ILogger _logger;
        private Timer timer;
        public MySqlBackupService(IWorker worker,ILogger<MySqlBackupService> logger)
        {
            this._worker = worker;
            this._logger = logger;
        }

        public void Dispose()
        {
            // throw new NotImplementedException();
            timer?.Dispose();
        }

        public  Task StartAsync(CancellationToken cancellationToken)
        {
            // throw new NotImplementedException();
            timer = new Timer(x=>this.DoWork(cancellationToken), null, TimeSpan.Zero, TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }

        public  Task StopAsync(CancellationToken cancellationToken)
        {
            //   throw new NotImplementedException();
            return Task.CompletedTask;
        }
        public void DoWork(CancellationToken stoppingToken)
        {
            _worker.SheduleBackUp(stoppingToken);
        }

        //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //{

        //    await _worker.SheduleBackUp(stoppingToken);
        //}
    }
}
