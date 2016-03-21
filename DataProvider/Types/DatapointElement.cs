using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace WccPcm.DataProvider
{
    public class DatapointElement
    {
        public ElementType Type { get; private set; }
        public string RefDpType { get; private set; }
        public string Name { get; private set; }        
        //private readonly DatapointElement _value;
        private readonly List<DatapointElement> childElements = new List<DatapointElement>();

        public DatapointElement()
        {
            this.Type = ElementType.DPEL_STRUCT;
            this.Name = "root";
            this.RefDpType = "";
        }

        public DatapointElement(ElementType Type, string Name)
        {
            this.Type = Type;
            this.Name = Name;
            this.RefDpType = "";
        }

        public DatapointElement(ElementType Type, string Name, string RefDpType)
        {
            this.Type = Type;
            this.Name = Name;
            this.RefDpType = RefDpType;
        }

        public DatapointElement this[int i]
        {
            get { return childElements[i]; }
        }

        public ReadOnlyCollection<DatapointElement> ChildElements
        {
            get { return childElements.AsReadOnly(); }
        }

        public void Add(DatapointElement element)
        {
            childElements.Add(element);
        }

        public void Clear()
        {
            childElements.Clear();
        }

        public void Remove(DatapointElement element)
        {
            childElements.Remove(element);
        }

        public void RemoveAt(int index)
        {
            childElements.RemoveAt(index);
        }
    }
}
