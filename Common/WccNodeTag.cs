using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WccPcm
{
    public class WccNodeTag
    {
        public enum WccNodeType
        {
            Root,
            CommonNode,
            Project
        }
        public string NodeName { get; set; }
        public WccNodeType Type { get; set; }
        public WccProject Project { get; set; }
    }
}
