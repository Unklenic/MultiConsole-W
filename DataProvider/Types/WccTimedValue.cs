using System;

namespace WccPcm.DataProvider
{
    public class WccTimedValue
    {
        public DateTime Timestamp { get; private set; }
        public dynamic Value { get; private set; }

        public WccTimedValue(DateTime timestamp, double value)
        {
            Timestamp = timestamp;
            Value = value;            
        }
    }
}