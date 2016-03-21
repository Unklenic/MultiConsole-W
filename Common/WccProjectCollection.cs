using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WccPcm
{
    [Serializable]
    class WccProjectCollection : Dictionary<string, WccProject>
    {
        public void AddRange(WccProjectCollection Projects)
        {
            foreach (var project in Projects)
            {
                this.Add(project.Key, project.Value);
            }
        }

        public void AddRange(Dictionary<string, WccProject> Projects)
        {
            foreach (var project in Projects)
            {
                this.Add(project.Key, project.Value);
            }
        }

        public void Serialize()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Item[]));

            using (FileStream fs = new FileStream("AppData.dat", FileMode.Create))
            {
                //formatter.Serialize(fs, this);
                serializer.Serialize(fs, this.Select(kv => new Item() { path = kv.Key, projectInfo = kv.Value }).ToArray());
            }
        }

        public void Deserialize()
        {
            this.Clear();
            if (File.Exists("AppData.dat"))
            {
                using (FileStream fs = new FileStream("AppData.dat", FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Item[]));
                    var projects = ((Item[])serializer.Deserialize(fs))
                                                      .ToDictionary(i => i.path, i => i.projectInfo);
                    //WccProjectCollection projects = (WccProjectCollection)formatter.Deserialize(fs);

                    this.AddRange(projects);
                }
            }
            /*else
            {
                //FileStream fs = new FileStream("AppData.dat", FileMode.OpenOrCreate);
                FileStream fs  = File.Create("AppData.dat");
                fs.Close();
                //fs.Close();
            }*/

        }
    }

    [Serializable]
    public class Item
    {
        //[XmlAttribute]
        public string path;
        //[XmlAttribute]
        public WccProject projectInfo;

        public Item() 
        {
            path = "";
            projectInfo = null;
        }
    }
}
