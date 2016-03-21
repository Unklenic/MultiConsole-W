using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WccPcm.DataProvider;


namespace WccPcm
{
    public partial class para : Form
    {

        private List<DatapointType> types;
        private List<Datapoint> dps = new List<Datapoint>();
        private String MachineName;

        public para(String MachineName)
        {
            this.MachineName = MachineName;
            InitializeComponent();
        }

        private void LoadDp()
        {
            var client = new WccDataProvider("http://" + MachineName + ":84");
            var dpt = client.dpTypes("*");
            types = client.dpTypes("*").ToList<DatapointType>();
            foreach (var type in types)
            {
                type.TypeStruct = client.dpTypeGet(type);
                dps.AddRange(client.dpNames(type).ToList<Datapoint>());
            }
        }

        private void para_Load(object sender, EventArgs e)
        {
            //Прогружаем все точки
            LoadDp();

            //Далее имея полный набор всех dp делаем что хотим, к примеру строим дерево по типу para
            //Для сортировки очень пригодится Linq
            treeView.Nodes.Clear();
            foreach (var item in types)
            {
                TreeNode typeNode = new TreeNode(item.Name);
                TreeNode[] typeNodes = DatapointElementToTree(item.TypeStruct);
                foreach (var dp in dps.Where( s => String.Equals(s.Type.Name, item.Name)))
                {
                    TreeNode dpNode = new TreeNode(dp.Name);
                    dpNode.Tag = "dp";
                    dpNode.Nodes.AddRange(typeNodes);
                    typeNode.Nodes.Add((TreeNode)dpNode.Clone());
                }
                treeView.Nodes.Add(typeNode);
            }
        }

        private TreeNode[] DatapointElementToTree(DatapointElement element)
        {
            if (element.Type == ElementType.DPEL_TYPEREF)
            {
                return DatapointElementToTree(types.Where(s => String.Equals(s.Name, element.RefDpType)).FirstOrDefault().TypeStruct);
            }

            int count = element.ChildElements.Count;
            TreeNode[] treeCollection = new TreeNode[count];
            
            ReadOnlyCollection<DatapointElement> childsElement = element.ChildElements;
            for (int i = 0; i < count; i++ )
            {
                treeCollection[i] = new TreeNode(childsElement[i].Name);
                treeCollection[i].Tag = childsElement[i];
                treeCollection[i].Nodes.AddRange(DatapointElementToTree(childsElement[i]));
            }
            return treeCollection;
        }
    }
}
