using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace WccPcm
{
    public partial class MainForm : Form
    {
        private BindingSource dataSource;
        private WccProject selectedWccProject;
        //private IAppDataBase appDataBase;
        private WccProjectCollection projectCollection;
        private delegate void ProjectStateEventHandler(TreeNode node, int state);

        private WccProject SelectedProject
        {
            get { return selectedWccProject; }
            set 
            {
                if(!ReferenceEquals(selectedWccProject,value))
                {
                    if (selectedWccProject != null)
                    {
                        foreach (WccManager manager in dataSource.List)
                        {
                            manager.ManagerStateChanged -= OnManagerStateChanged;
                        }
                    }
                   dataSource.Clear();
                   selectedWccProject = value;
                   if (selectedWccProject != null)
                   {
                       OnProjectStateChanged(selectedWccProject);
                       EnableWorkArea(true);
                   }
                   else
                   {
                       EnableWorkArea(false);
                   }
                }
            }
        }
        
        public MainForm()
        {
            InitializeComponent();
            debugViewer.Path = Application.StartupPath;
            debugViewer.StartWatch();
            InitializeDataGrid();
            selectedWccProject = null;
            InitializeTreeView();            
        }

        private void InitializeTreeView()
        {
            treeView.HideSelection = false;
            TreeNode treeNode = new TreeNode("Windows");
            treeNode.Name = "Windows";
            WccNodeTag tag = new WccNodeTag();
            tag.NodeName = treeNode.Name;
            tag.Type = WccNodeTag.WccNodeType.Root;
            treeNode.Tag = tag;
            treeNode.ImageIndex = 0;
            ImageList imageList = new ImageList();

            projectCollection = new WccProjectCollection();
            projectCollection.Deserialize();
            //appDataBase = new AppDataBase();
            //List<Dictionary<string, object>> configMap = appDataBase.ReadConfig();
            LoadTreeNodes(treeNode, projectCollection);
            treeView.Nodes.Add(treeNode);
        }

        private void LoadTreeNodes(TreeNode parentNode, WccProjectCollection projectCollection)
        {
            foreach (var project in projectCollection)
            {
                string nodeName = project.Key;
                WccNodeTag.WccNodeType nodeType = WccNodeTag.WccNodeType.Project;
                string[] nodes = nodeName.Split('.');
                if (nodes.Length > 1)
                {
                    if(nodes[nodes.Length - 2].Equals(parentNode.Name))
                    {
                        TreeNode node = new TreeNode(nodes[nodes.Length - 1]);
                        node.Name = nodes[nodes.Length - 1];
                        WccNodeTag _tag = new WccNodeTag();
                        _tag.Type = nodeType;
                        _tag.NodeName = nodeName;
                        if (nodeType == WccNodeTag.WccNodeType.Project)
                        {
                            WccProject proj = project.Value;
                            //project.ProjectAlias = Convert.ToString(map["projectalias"]);
                            //project.MachineName = Convert.ToString(map["machinename"]);
                            //project.PmonPort = Convert.ToInt32(map["pmonport"]);

                            //project.Credential.Login = Convert.ToString(map["login"]);
                            //project.Credential.Password = Convert.ToString(map["password"]);
                            //project.Credential.DefaultAutorization = Convert.ToBoolean(map["defaultAutorization"]);
                            //project.Path = Convert.ToString(map["path"]);
                            proj.ProjectStateChanged += OnProjectStateChanged;
                            _tag.Project = proj;
                        }
                        else
                        {
                            LoadTreeNodes(node, projectCollection);
                        }
                        node.Tag = _tag;
                        parentNode.Nodes.Add(node);
                    }
                }                
            }
        }
        
        private void InitializeDataGrid()
        {
            dataSource = new BindingSource();
            dataGridView.AutoGenerateColumns = false;
            //dataGridView.AutoSize = true;
            dataGridView.DataSource = dataSource;

            DataGridViewColumn column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "State";
            column.Name = "St";
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Number";
            column.Name = "No";
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Id";
            column.Name = "ИД процесса";
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Memory";
            column.Name = "Память";
            column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "ManagerName";
            column.Name = "Процесс";
            dataGridView.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Options";
            column.Name = "Опции";
            dataGridView.Columns.Add(column);
        }

        #region tree view

        private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeView.SelectedNode = e.Node;
            TreeNode selectedNode = treeView.SelectedNode;
            WccNodeTag nodeTag = null;
            if (selectedNode == null)
            {
                return;
            }
            nodeTag = (WccNodeTag)selectedNode.Tag;
            if (e.Button == System.Windows.Forms.MouseButtons.Right 
                /*&& (nodeTag.Type == WccNodeTag.WccNodeType.CommonNode || nodeTag.Type == WccNodeTag.WccNodeType.Root)*/)
            {
                treeView.SelectedNode = e.Node;
                //CreatePvssProjectForm.CreatePvssProjectForm form = new CreatePvssProjectForm();
                //form.Show();
                Point p = new Point(e.X, e.Y);

                // Get the node that the user has clicked.
                TreeNode node = treeView.GetNodeAt(p);
                if (node != null)
                {

                    treeView.SelectedNode = node;

                    // Find the appropriate ContextMenu depending on the selected node.
                    WccNodeTag tag = (WccNodeTag)node.Tag;
                    switch (tag.Type)
                    {
                        case WccNodeTag.WccNodeType.Root:
                            rootContextMenu.Show(treeView, p);
                            break;
                        case WccNodeTag.WccNodeType.CommonNode:
                            treeContextMenu.Show(treeView, p);
                            break;
                        case WccNodeTag.WccNodeType.Project:
                            projectNodeContextMenu.Show(treeView, p);
                            break;
                    }
                }
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Left /*&& nodeTag.Type == WccNodeTag.WccNodeType.Project*/)
            {
                this.btnStartManager.Enabled = false;
                this.btnStopManager.Enabled = false;
                this.btnKillManager.Enabled = false;
                this.btnStartProject.Enabled = false;
                this.btnStopProject.Enabled = false;
                this.btnLogViewer.Enabled = false;
                SelectedProject = nodeTag.Project;
            }
        }

        private void добавитьУзелToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string name = "Новый узел";
            TreeNode treeNode = new TreeNode(name);
            treeNode.Name = name;
            WccNodeTag tag = new WccNodeTag();
            tag.NodeName = ((WccNodeTag)treeView.SelectedNode.Tag).NodeName + "." + name;
            tag.Type = WccNodeTag.WccNodeType.CommonNode;
            treeNode.Tag = tag;
            treeView.SelectedNode.Nodes.Add(treeNode);
            treeView.SelectedNode.Expand();
            projectCollection.Serialize();
            //appDataBase.AddNode(tag);
        }

        private void treeView_DoubleClick(object sender, EventArgs e)
        {
            if( ((WccNodeTag)treeView.SelectedNode.Tag).Type != WccNodeTag.WccNodeType.Root)
            {
                treeView.LabelEdit = true;
                if (!treeView.SelectedNode.IsEditing)
                {
                    treeView.SelectedNode.BeginEdit();
                }
            }
        }

        private void treeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label != null)
            {
                if (e.Label.Length > 0)
                {
                    if (e.Label.IndexOfAny(new char[] { '@', '.', ',', '!' }) == -1)
                    {
                        RenameNodes((WccNodeTag)e.Node.Tag, e.Label);
                        e.Node.EndEdit(false);
                    }
                    else
                    {
                        /* Cancel the label edit action, inform the user, and 
                           place the node in edit mode again. */
                        e.CancelEdit = true;
                        MessageBox.Show("Invalid tree node label.\n" +
                           "The invalid characters are: '@','.', ',', '!'",
                           "Node Label Edit");
                        e.Node.BeginEdit();
                    }
                }
                else
                {
                    /* Cancel the label edit action, inform the user, and 
                       place the node in edit mode again. */
                    e.CancelEdit = true;
                    MessageBox.Show("Invalid tree node label.\nThe label cannot be blank",
                       "Node Label Edit");
                    e.Node.BeginEdit();
                }
            }
        }

        private void RenameNodes(WccNodeTag tag, string newName)
        {
            string[] path = tag.NodeName.Split('.');
            string oldName = tag.NodeName;
            path[path.Length - 1] = newName;
            if(tag.Type == WccNodeTag.WccNodeType.Project)
            {
                if(tag.Project != null)
                {
                    tag.Project.ProjectAlias = newName;
                }
            }
            string NodeName = "";
            for(int i = 0; i < path.Length; i++)
            {
                NodeName += path[i];
                if(i != path.Length - 1)
                {
                    NodeName += ".";
                }
            }
            tag.NodeName = NodeName;
            WccProject project = projectCollection[oldName];
            projectCollection.Remove(oldName);
            projectCollection.Add(NodeName, project);
            projectCollection.Serialize();
            //appDataBase.UpdateNode(tag, oldName);
        }

        private void добавитьПроектToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string name = "Новый проект";
            TreeNode treeNode = new TreeNode(name);
            treeNode.Name = name;
            WccNodeTag tag = new WccNodeTag();
            tag.NodeName = ((WccNodeTag)treeView.SelectedNode.Tag).NodeName + "." + name;
            tag.Type = WccNodeTag.WccNodeType.Project;
            tag.Project = new WccProject();
            /*TEMP*/
            tag.Project.ProjectAlias = name;
            tag.Project.ProjectStateChanged += OnProjectStateChanged;
            //tag.Project.PmonPort = 4999;
            //tag.Project.MachineName = "sms156419snv";
            /*TEMP*/
            treeNode.Tag = tag;
            treeNode.ImageIndex = 0;
            treeNode.SelectedImageIndex = 0;
            
            if (!projectCollection.ContainsKey(tag.NodeName))
            {
                projectCollection.Add(tag.NodeName, tag.Project);
                treeView.SelectedNode.Nodes.Add(treeNode);
                treeView.SelectedNode.Expand();
            }
            else
            {
                Debugger.Write("Проект с таким именем уже существует.");
            }
            
            projectCollection.Serialize();
            //appDataBase.AddNode(tag);
            //arg.RefreshInfo();
            //treeStruct.Add(treeNode.Name, arg);
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            deleteNode();
        }

        private void deleteNode()
        {
            TreeNode selectedNode = treeView.SelectedNode;
            WccNodeTag nodeTag = (WccNodeTag)selectedNode.Tag;
            if (nodeTag.Type == WccNodeTag.WccNodeType.Project)
            {
                if (ReferenceEquals(SelectedProject, nodeTag.Project))
                {
                    SelectedProject.StopObserve();
                    SelectedProject.ProjectStateChanged -= OnProjectStateChanged;
                    SelectedProject = null;
                }
            }
            else
            {
                deleteChildProjectNodes(selectedNode.Nodes);
            }
            treeView.Nodes.Remove(selectedNode);
            projectCollection.Remove(nodeTag.NodeName);
            projectCollection.Serialize();
            //appDataBase.RemoveNode(nodeTag);
        }

        private void deleteChildProjectNodes(TreeNodeCollection nodes)
        {
            if (nodes.Count != 0)
            {
                foreach (TreeNode node in nodes)
                {
                    WccNodeTag nodeTag = (WccNodeTag)node.Tag;
                    if (nodeTag.Type == WccNodeTag.WccNodeType.Project)
                    {
                        nodeTag.Project.StopObserve();
                        nodeTag.Project.ProjectStateChanged -= OnProjectStateChanged;
                    }
                    else
                    {
                        deleteChildProjectNodes(node.Nodes);
                    }
                }
            }
        }

        private void удалитьprojectNode_Click(object sender, EventArgs e)
        {
            deleteNode();
        }

        private void rootAddNode_Click(object sender, EventArgs e)
        {
            добавитьУзелToolStripMenuItem_Click(sender, e);
        }

        private void rootAddProject_Click(object sender, EventArgs e)
        {
            добавитьПроектToolStripMenuItem_Click(sender, e);
        }

        private void OnManagerStateChanged(WccManager manager)
        {
            if (dataGridView.InvokeRequired)
            {
                ManagerEventHandler d = new ManagerEventHandler(OnManagerStateChanged);
                dataGridView.Invoke(d, new object[] { manager });
            }
            else
            {                    
                if (dataGridView.Rows.Count > 0)
                {
                    DataGridViewRow row = dataGridView.Rows[manager.PmonNumber];
                    //row.Cells[0].Value = manager.Id;
                    row.Cells["Память"].Value = manager.Memory.ToString();
                    //row.Cells[2].Value = manager.ManagerName;
                    //row.Cells[3].Value = manager.Options;
                }
                dataGridView.Update();
            }
        }

        #endregion

        #region gbProjectControl

        private void btnStartObserve_Click(object sender, EventArgs e)
        {
            if(SelectedProject != null)
            {
                gbProjectControl.Enabled = false;
                if(!SelectedProject.IsObserved)
                {
                    SelectedProject.Observe();
                    this.btnStartObserve.ImageIndex = 0;
                }
                else
                {
                    SelectedProject.StopObserve();
                    this.btnStartObserve.ImageIndex = 1;
                }
            }
        }

        private void OnProjectStateChanged(WccProject project)
        {
            if (ReferenceEquals(project, SelectedProject))
            {
                if(dataGridView.InvokeRequired)
                {
                    WccProjectStateEventHandler d = new WccProjectStateEventHandler(OnProjectStateChanged);
                    dataGridView.Invoke(d, new object[] { project });
                }
                else
                {
                    //dataSource.Clear();
                    if (dataSource.Count > SelectedProject.Managers.Count || 
                        SelectedProject.ConnectionState == WccProject.WccConnectionState.Disconnected)
                    {
                        dataSource.Clear();
                    }

                    foreach (var manager in SelectedProject.Managers.Values)
                    {
                        //dataSource.Add(manager);
                        if (dataSource.Count > manager.PmonNumber)
                        {
                            ((WccManager)dataSource[manager.PmonNumber]).Replace(manager);
                        }
                        else
                        {
                            dataSource.Add(manager);
                        }
                        
                        SelectedProject.Managers[manager.PmonNumber].ManagerStateChanged -= OnManagerStateChanged;
                        manager.ManagerStateChanged += new ManagerEventHandler(OnManagerStateChanged);
                    }

                    dataGridView.Update();

                    for (int i = 0; i < dataGridView.RowCount; i++)
                    {
                        dataGridView["St", i].Style.BackColor
                            = Color.White;
                        if (dataGridView["St", i].FormattedValue.ToString().Contains("2"))
                        {
                            dataGridView["St", i].Style.BackColor = Color.FromArgb(0,255,0);
                        }
                        else if (dataGridView["St", i].FormattedValue.ToString().Contains("3"))
                        {
                            dataGridView["St", i].Style.BackColor = Color.Pink;
                        }
                        else if (dataGridView["St", i].FormattedValue.ToString().Contains("1"))
                        {
                            dataGridView["St", i].Style.BackColor = Color.Yellow;
                        }
                        else if(dataGridView["St", i].FormattedValue.ToString().Contains("0") &&
                                dataGridView["ИД процесса", i].FormattedValue.ToString().Contains("-2"))
                        {
                            dataGridView["St", i].Style.BackColor = Color.Blue;
                        }
                        else if (dataGridView["St", i].FormattedValue.ToString().Contains("0") &&
                                (((WccManager)dataSource[i]).Mode == WccManager.ManagerMode.alwayes ||
                                 ((WccManager)dataSource[i]).Mode == WccManager.ManagerMode.once))
                        {
                            dataGridView["St", i].Style.BackColor = Color.Red;
                        }
                    }  

                    txtProjectName.Text = SelectedProject.ProjectName;

                    if(project.IsObserved)
                    {
                        btnStartObserve.ImageIndex = 0;
                    }
                    else
                    {
                        btnStartObserve.ImageIndex = 1;
                    }

                    if (project.ConnectionState != WccProject.WccConnectionState.Processing)
                    {
                        gbProjectControl.Enabled = true;
                    }

                    if (project.IsObserved)
                    {
                        if (project.IsStarted && project.IsObserved)
                        {
                            this.btnStartProject.Enabled = false;
                            this.btnStopProject.Enabled = true;
                        }
                        else if (!project.IsStarted && project.IsObserved)
                        {
                            this.btnStartProject.Enabled = true;
                            this.btnStopProject.Enabled = false;                            
                        }
                        this.btnLogViewer.Enabled = true;
                    }
                    else
                    {
                        this.btnStartProject.Enabled = false;
                        this.btnStopProject.Enabled = false;
                        this.btnLogViewer.Enabled = false;
                    }

                    if (dataGridView.RowCount > 0)
                    {
                        ManagerControlPanelStateChange((WccManager)dataSource[dataGridView.CurrentRow.Index]);
                    }                    
                }                
            }
            else if(SelectedProject == null)
            {
                if(txtProjectName.InvokeRequired)
                {
                    WccProjectStateEventHandler d = new WccProjectStateEventHandler(OnProjectStateChanged);
                    txtProjectName.Invoke(d, new object[] { project });
                }
                else
                {
                    txtProjectName.Text = "";
                }                
            }
            ChangeProjectNodeState(treeView.Nodes, project);
        }

        private void ChangeProjectNodeState(TreeNodeCollection nodes, WccProject project)
        {
            foreach(TreeNode node in nodes)
            {
                WccNodeTag tag = (WccNodeTag)node.Tag;
                if(tag.Type == WccNodeTag.WccNodeType.Root || tag.Type == WccNodeTag.WccNodeType.CommonNode)
                {
                    ChangeProjectNodeState(node.Nodes, project);
                }
                else 
                {
                    if (tag.Project == project && tag.Project != null)
                    {
                        int state = (int)project.ConnectionState;
                        OnProjectNodeStateChanged(node, state);
                    }
                }
            }
        }
        
        private void OnProjectNodeStateChanged(TreeNode node, int state)
        {
            if (treeView.InvokeRequired)
            {
                ProjectStateEventHandler d = new ProjectStateEventHandler(OnProjectNodeStateChanged);
                dataGridView.Invoke(d, new object[] { node, state });
            }
            else
            {
                node.ImageIndex = state;
                node.SelectedImageIndex = state;
            }
        }

        private void btnSettingProject_Click(object sender, EventArgs e)
        {
            if(SelectedProject != null)
            {
                ProjectSettings form = new ProjectSettings(SelectedProject, new ProjectSettingCallback(SetProjectSettings));
                form.ShowDialog();
            }
        }

        private void SetProjectSettings(ProjectSettingArgs args)
        {
            SelectedProject.MachineName = args.MachineName;
            SelectedProject.PmonPort = args.PmonPort;
            SelectedProject.AutoConnect = args.AutoConnect;
            SelectedProject.Credential = args.Credential;
            SelectedProject.Path = args.Path;
            projectCollection.Serialize();
            //appDataBase.UpdateNode((WccNodeTag)treeView.SelectedNode.Tag, ((WccNodeTag)treeView.SelectedNode.Tag).NodeName);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            stopingObserve(treeView.Nodes);
            debugViewer.StopWatch();
        }

        private void stopingObserve(TreeNodeCollection nodes)
        {
            foreach(TreeNode node in nodes)
            {
                WccNodeTag tag = (WccNodeTag)node.Tag;
                if(tag.Type == WccNodeTag.WccNodeType.Project)
                {
                    if (tag.Project != null)
                    {
                        tag.Project.StopObserve();
                    }                    
                }
                else
                {
                    stopingObserve(node.Nodes);
                }
            }
        }
        #endregion

        #region Функции работы с рабочей областью GUI

        private void EnableWorkArea(bool block)
        {
            if(block)
            {
                gbProjectControl.Visible = true;
                gbPmon.Visible = true;
                gbProjectControl.Enabled = true;
                gbPmon.Enabled = true;
                //txtProjectName.Text = SelectedProject.ProjectName;
            }
            else
            {
                gbProjectControl.Visible = false;
                gbPmon.Visible = false;
                gbProjectControl.Enabled = false;
                gbPmon.Enabled = false;
                txtProjectName.Text = "";
            }
        }
        #endregion

        private void MainForm_Load(object sender, EventArgs e)
        {
            //logViewer.StartWatch();
        }

        private void btnStartManager_Click(object sender, EventArgs e)
        {
            if(this.SelectedProject != null)
            {
                this.SelectedProject.StartManager(((WccManager)dataSource[dataGridView.CurrentRow.Index]));
            }
            this.dataGridView.ClearSelection();
        }

        private void btnStopManager_Click(object sender, EventArgs e)
        {
            if (this.SelectedProject != null)
            {
                this.SelectedProject.StopManager(((WccManager)dataSource[dataGridView.CurrentRow.Index]));
            }
            this.dataGridView.ClearSelection();
        }

        private void btnKillManager_Click(object sender, EventArgs e)
        {
            if (this.SelectedProject != null)
            {
                this.SelectedProject.KillManager(((WccManager)dataSource[dataGridView.CurrentRow.Index]));
            }
            this.dataGridView.ClearSelection();
        }

        private void btnDeleteManager_Click(object sender, EventArgs e)
        {
            if (this.SelectedProject != null)
            {
                this.SelectedProject.RemoveManager(((WccManager)dataSource[dataGridView.CurrentRow.Index]));
            }
            this.dataGridView.ClearSelection();
        }

        private void btnStartProject_Click(object sender, EventArgs e)
        {
            if (this.SelectedProject != null)
            {
                this.SelectedProject.StartProject();
            }
        }

        private void dataGridView_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            WccManager manager = (WccManager)dataSource[e.RowIndex];
            ManagerControlPanelStateChange(manager);
        }

        private void ManagerControlPanelStateChange(WccManager manager)
        {
            if (this.SelectedProject.IsStarted && manager.PmonNumber != 0)
            {
                if (manager.State > 0)
                {
                    this.btnStartManager.Enabled = false;
                    this.btnStopManager.Enabled = true;
                    this.btnKillManager.Enabled = true;
                }
                else
                {
                    this.btnStartManager.Enabled = true;
                    this.btnStopManager.Enabled = false;
                    this.btnKillManager.Enabled = false;
                }
            }
            else
            {
                this.btnStartManager.Enabled = false;
                this.btnStopManager.Enabled = false;
                this.btnKillManager.Enabled = false;
            }
        }

        private void btnStopProject_Click(object sender, EventArgs e)
        {
            if (this.SelectedProject != null)
            {
                this.SelectedProject.StopProject();
            }
        }

        private void appButton2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(((WccManager)dataSource[dataGridView.CurrentRow.Index]).Number.ToString());
        }

        private void btnSysManagment_Click(object sender, EventArgs e)
        {
            if (SelectedProject != null)
            {
                SystemManagment form = new SystemManagment(SelectedProject);
                form.Show();
            }
        }

        private void btnLogViewer_Click(object sender, EventArgs e)
        {
            if(this.SelectedProject != null)
            {
                LoggerForm form = new LoggerForm(this.SelectedProject);
                form.Show();
            }            
        }

    }
}
