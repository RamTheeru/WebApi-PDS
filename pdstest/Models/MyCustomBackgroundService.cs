using Microsoft.Extensions.Hosting;
using pdstest.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace pdstest.Models
{
    public class MyCustomBackgroundService : BackgroundService
    {
        private readonly IWorker _worker;
        public MyCustomBackgroundService(IWorker worker)
        {
            this._worker = worker;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

           // await _worker.DoWork(stoppingToken);
        }
    }
}
