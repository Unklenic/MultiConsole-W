using System;
using System.Text;

namespace WccPcm.DataProvider
{
    public class DatapointType
    {
        #region Fields

        //public int Id { get; private set; }
        public string Name { get; private set; }
        //public int ElementType { get; private set; }
        
        public DatapointType Parent { get; set; }
        public DatapointElement TypeStruct { get; set; }
        //public ICollection<DatapointType> Items { get; private set; }
        
        #endregion

        #region Ctor

        public DatapointType(string name, int elementType)
        {
            //Id = IdGenerator.NewId();
            Name = name;
            //ElementType = elementType;
            //Items = new List<DatapointType>();
        }

        public DatapointType(string name)
        {
            //Id = IdGenerator.NewId();
            Name = name;
            //ElementType = elementType;
            //Items = new List<DatapointType>();
        }

        #endregion

        /*public DatapointType FindElement(int elementId)
        {
            if (Id == elementId)
                return this;

            return Items.Select(item => item.FindElement(elementId)).FirstOrDefault(result => result != null);
        }*/

        /*public DatapointType ItemByName(string itemName)
        {
            return Items.SingleOrDefault(i => i.Name == itemName);
        }*/

        public string BuildFullName(Datapoint dp)
        {
            var sb = new StringBuilder();

            if (Parent == null)
                sb.Append(".");

            var currDpt = this;
            while (currDpt.Parent != null)
            {
                sb.Insert(0, currDpt.Name);
                sb.Insert(0, ".");

                currDpt = currDpt.Parent;
            }

            sb.Insert(0, dp.Name);

            return sb.ToString();
        }
    }
}