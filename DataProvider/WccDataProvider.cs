using System;
using System.Collections.Generic;
using System.Linq;
using CookComputing.XmlRpc;

namespace WccPcm.DataProvider
{
    public class WccDataProvider : IWccDataProvider
    {
        private readonly IWccXmlRpc wccXmlRpcProxy;

        public string ConnectionString { get; private set; }

        public WccDataProvider(string connectionString, int timeoutSec = 300)
        {
            ConnectionString = connectionString + "/RPC2";
            wccXmlRpcProxy = XmlRpcProxyGen.Create<IWccXmlRpc>();
            wccXmlRpcProxy.Timeout = timeoutSec * 1000;
            wccXmlRpcProxy.Url = ConnectionString;
        }

        public IEnumerable<DatapointType> dpTypes(string pattern)
        {
            //var result = new Dictionary<int, DatapointType>();
            var result = new List<DatapointType>();

            try
            {
                var wResult = wccXmlRpcProxy.dpTypes(pattern);
                foreach (var r in wResult)
                {
                    //var id = int.Parse(r[elementIdIndex]);
                    //var parentId = int.Parse(r[parentElementIdIndex]);
                    //var elementName = r[elementNameIndex];
                    //var elementType = int.Parse(r[elementTypeIndex]);
                    var dpType = new DatapointType(r);
                    //result.Add(id, dpType);
                    result.Add(dpType);
                    //DatapointType parent;
                    /*if (result.TryGetValue(parentId, out parent))
                    {
                        parent.Items.Add(dpType);
                        dpType.Parent = parent;
                    }*/
                }
            }
            catch (Exception e)
            {
                Debugger.Write(e.Message);
            }

            return result;
        }

        //string[][] dpTypeGet(string dpType);

        public DatapointElement dpTypeGet(DatapointType dpType)
        {
            DatapointElement result = null;

            try
            {
                result = new DatapointElement();
                var wResult = wccXmlRpcProxy.dpTypeGet(dpType.Name);
                ParseTreeStruct(result, wResult, 0, 0);
            }
            catch (Exception e)
            {
                Debugger.Write(e.Message);
            }

            return result;
        }

        private void ParseTreeStruct(DatapointElement element, string[][] source, int nodeLevel, int startIndex)
        {
            var lvl = 0;
            for (int i = startIndex; i < source.Length; i++ )
            {
                for (int j = 0; j < source[i].Length; j++)
                //foreach (var l in source[i])
                {
                    var l = source[i][j];
                    var splited = l.Split(';');
                    var Name = splited[0];
                    var Type = (ElementType)int.Parse(splited[1]);
                    if (Type == ElementType.DPEL_NONE)
                    {
                        lvl++;
                    }
                    else if(lvl < nodeLevel)
                    {
                        return;
                    }
                    else if (lvl == 0 && nodeLevel == 0)
                    {
                        //element = new DatapointElement(Type, Name);
                        ParseTreeStruct(element, source, nodeLevel + 1, i + 1);
                    }
                    else if (lvl == nodeLevel && lvl != 0)
                    {
                        DatapointElement childElement;
                        
                        if(Type == ElementType.DPEL_TYPEREF)
                        {
                            j++;
                            childElement = new DatapointElement(Type, Name, source[i][j].Split(';')[0]);
                        }
                        else
                        {
                            childElement = new DatapointElement(Type, Name);
                            ParseTreeStruct(childElement, source, nodeLevel + 1, i + 1);
                        }
                        element.Add(childElement);
                    }
                }
                lvl = 0;
            }
        }

        public IEnumerable<Datapoint> dpNames(DatapointType dpType, string dpPattern = "*")
        {
            var result = new List<Datapoint>();

            try
            {
                var wResult = wccXmlRpcProxy.dpNames(dpType.Name, dpPattern);
                foreach (var r in wResult)
                {
                    var dp = new Datapoint(r.Split(':')[1], dpType);
                    result.Add(dp);
                }
            }
            catch (Exception e)
            {
                Debugger.Write(e.Message);
            }

            return result;
        }

        public dynamic dpGet(Datapoint dp)
        {
            var fullDpName = dp.Type.BuildFullName(dp);
            return wccXmlRpcProxy.dpGet(fullDpName);
        }

        public IEnumerable<WccTimedValue> dpGetPeriod(Datapoint dp, DateTime startTime, DateTime endTime)
        {
            var fullDpName = dp.Type.BuildFullName(dp);
            var wResult = wccXmlRpcProxy.dpGetPeriod(startTime, endTime, 1, fullDpName);

            WccTimedValue[] result = new WccTimedValue[wResult.Length];
            var i = 0;
            foreach( var r in wResult)
            {
                result[i++] = new WccTimedValue(DateTime.Parse(r[0]), double.Parse(r[1]));
            }
            return result;
        }

    }
}
