using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WccPcm
{
    public interface IWccControl
    {
        void StartProject();
        void StopProject();
        void RestartProject();
        void StartManager(WccManager manager);
        void StopManager(WccManager manager);
        void KillManager(WccManager manager);
        void AppendManager(WccManager manager);
        void InsertManager(WccManager manager);
        void RemoveManager(WccManager manager);
    }
}
