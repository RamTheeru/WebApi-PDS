using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace pdstest.services
{
   public interface IWorker
    {
         Task DoWork(CancellationToken token);
        void SheduleBackUp(CancellationToken token);
            // Task SheduleBackUp(CancellationToken token);
    }
}
