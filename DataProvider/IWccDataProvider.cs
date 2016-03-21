using System;
using System.Collections.Generic;

namespace WccPcm.DataProvider
{
    public interface IWccDataProvider
    {
        string ConnectionString { get; }

        dynamic dpGet(Datapoint dp);

        IEnumerable<WccTimedValue> dpGetPeriod(Datapoint dp, DateTime startTime, DateTime endTime);
    }
}
