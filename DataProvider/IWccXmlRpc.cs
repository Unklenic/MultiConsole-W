using System;
using CookComputing.XmlRpc;

namespace WccPcm.DataProvider
{
    public interface IWccXmlRpc : IXmlRpcProxy
    {
        #region Setters

        [XmlRpcMethod]
        void dpSet(string dpName, dynamic value);

        //[XmlRpcMethod]
        //void dpSet(string dpName, string value);

        #endregion

        #region Getters

        [XmlRpcMethod]
        string[] dpTypes(string pattern);

        [XmlRpcMethod]
        string[] dpNames(string dpType, string dpPattern = "*");

        [XmlRpcMethod]
        string[][] dpTypeGet(string dpType);

        [XmlRpcMethod]
        string dpGet(string dpName);

        [XmlRpcMethod]
        string[][] dpGetPeriod(DateTime startTime, DateTime endTime, int count, string dpName);

        [XmlRpcMethod]
        string Echo(string arg);

        #endregion
    }
}
