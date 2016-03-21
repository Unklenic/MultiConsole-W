namespace WccPcm.DataProvider
{
    public class Datapoint
    {
        //public int Id { get; private set; }
        public string Name { get; private set; }
        public DatapointType Type { get; private set; }

        public Datapoint(string name, DatapointType type)
        {
            Name = name;
            Type = type;
        }
    }

}