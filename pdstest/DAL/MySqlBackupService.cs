using Microsoft.Extensions.Hosting;
using pdstest.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace pdstest.DAL
{
    public class MySqlBackupService : BackgroundService
    {
        private readonly IWorker _worker;
        public MySqlBackupService(IWorker worker)
        {
            this._worker = worker;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            await _worker.SheduleBackUp(stoppingToken);
        }
    }
}
