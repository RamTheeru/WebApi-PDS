﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace pdstest.services
{
   public interface IWorker
    {
         Task DoWork(CancellationToken token);
    }
}