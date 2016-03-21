using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WccPcm
{    
    public interface IWccProjectInfo
    {
        string ProjectAlias { get; set; }
        string MachineName { get; set; }        
        string ProjectName { get; }
        string Path { get; set; }
        EncryptContainer Credential { get; set; }
        int PmonPort { get; set; }
        int ManagersCount { get; }
    }
}
