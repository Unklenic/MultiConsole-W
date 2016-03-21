using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WccPcm
{
    public class WccManagerCollection : Dictionary<int, WccManager>
    {
        public void AddRange(WccManagerCollection managers)
        {
            foreach(var manager in managers)
            {
                this.Add(manager.Key, manager.Value);
            }
        }
    }
}
