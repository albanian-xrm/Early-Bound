using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbanianXrm.EarlyBound.Helpers
{
    public class AsyncWorkExecutor
    {

        public class ProgressReport
        {
            public Exception Error { get; private set; }

            public object Result { get; private set; }
        }
    }
}
