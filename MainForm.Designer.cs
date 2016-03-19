namespace WccPcm
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.treeView = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.gbPmon = new System.Windows.Forms.GroupBox();
            this.gbProjectControl = new System.Windows.Forms.GroupBox();
            this.btnImage = new System.Windows.Forms.ImageList(this.components);
            this.lProjectName = new System.Windows.Forms.Label();
            this.txtProjectName = new System.Windows.Forms.TextBox();
            this.gbTools = new System.Windows.Forms.GroupBox();
            this.treeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.добавитьУзелToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.добавитьПроектToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.удалитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectNodeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.запуститьprojectNode = new System.Windows.Forms.ToolStripMenuItem();
            this.остановитьprojectNode = new System.Windows.Forms.ToolStripMenuItem();
            this.перезапуститьprojectNode = new System.Windows.Forms.ToolStripMenuItem();
            this.удалитьprojectNode = new System.Windows.Forms.ToolStripMenuItem();
            this.rootContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.rootAddNode = new System.Windows.Forms.ToolStripMenuItem();
            this.rootAddProject = new System.Windows.Forms.ToolStripMenuItem();
            this.debugViewer = new WccPcm.DebugViewer();
            this.btnSysManagment = new WccPcm.AppButton();
            this.btnLogViewer = new WccPcm.AppButton();
            this.btnStopProject = new WccPcm.AppButton();
            this.btnStartProject = new WccPcm.AppButton();
            this.btnStartObserve = new WccPcm.AppButton();
            this.btnSettingProject = new WccPcm.AppButton();
            this.btnKillManager = new WccPcm.AppButton();
            this.btnStopManager = new WccPcm.AppButton();
            this.btnStartManager = new WccPcm.AppButton();
            this.dataGridView = new WccPcm.AppDataGridView();
            this.gbPmon.SuspendLayout();
            this.gbProjectControl.SuspendLayout();
            this.gbTools.SuspendLayout();
            this.treeContextMenu.SuspendLayout();
            this.projectNodeContextMenu.SuspendLayout();
            this.rootContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeView.ImageIndex = 0;
            this.treeView.ImageList = this.imageList;
            this.treeView.Location = new System.Drawing.Point(12, 57);
            this.treeView.Name = "treeView";
            this.treeView.SelectedImageIndex = 0;
            this.treeView.Size = new System.Drawing.Size(193, 483);
            this.treeView.TabIndex = 0;
            this.treeView.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.treeView_AfterLabelEdit);
            this.treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_NodeMouseClick);
            this.treeView.DoubleClick += new System.EventHandler(this.treeView_DoubleClick);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "null.png");
            this.imageList.Images.SetKeyName(1, "disconnect.png");
            this.imageList.Images.SetKeyName(2, "warning.png");
            this.imageList.Images.SetKeyName(3, "connect.png");
            // 
            // gbPmon
            // 
            this.gbPmon.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbPmon.Controls.Add(this.btnKillManager);
            this.gbPmon.Controls.Add(this.btnStopManager);
            this.gbPmon.Controls.Add(this.btnStartManager);
            this.gbPmon.Controls.Add(this.dataGridView);
            this.gbPmon.Enabled = false;
            this.gbPmon.Location = new System.Drawing.Point(211, 124);
            this.gbPmon.Name = "gbPmon";
            this.gbPmon.Size = new System.Drawing.Size(659, 416);
            this.gbPmon.TabIndex = 2;
            this.gbPmon.TabStop = false;
            this.gbPmon.Text = "Мониторинг за проектом";
            this.gbPmon.Visible = false;
            // 
            // gbProjectControl
            // 
            this.gbProjectControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbProjectControl.Controls.Add(this.btnLogViewer);
            this.gbProjectControl.Controls.Add(this.btnStopProject);
            this.gbProjectControl.Controls.Add(this.btnStartProject);
            this.gbProjectControl.Controls.Add(this.btnStartObserve);
            this.gbProjectControl.Controls.Add(this.btnSettingProject);
            this.gbProjectControl.Controls.Add(this.lProjectName);
            this.gbProjectControl.Controls.Add(this.txtProjectName);
            this.gbProjectControl.Enabled = false;
            this.gbProjectControl.Location = new System.Drawing.Point(211, 57);
            this.gbProjectControl.Name = "gbProjectControl";
            this.gbProjectControl.Size = new System.Drawing.Size(659, 61);
            this.gbProjectControl.TabIndex = 3;
            this.gbProjectControl.TabStop = false;
            this.gbProjectControl.Text = "Управление проектом";
            this.gbProjectControl.Visible = false;
            // 
            // btnImage
            // 
            this.btnImage.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("btnImage.ImageStream")));
            this.btnImage.TransparentColor = System.Drawing.Color.Transparent;
            this.btnImage.Images.SetKeyName(0, "stop_30.png");
            this.btnImage.Images.SetKeyName(1, "play_20.png");
            // 
            // lProjectName
            // 
            this.lProjectName.AutoSize = true;
            this.lProjectName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lProjectName.Location = new System.Drawing.Point(81, 22);
            this.lProjectName.Name = "lProjectName";
            this.lProjectName.Size = new System.Drawing.Size(106, 20);
            this.lProjectName.TabIndex = 11;
            this.lProjectName.Text = "Имя проекта";
            // 
            // txtProjectName
            // 
            this.txtProjectName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProjectName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.txtProjectName.Location = new System.Drawing.Point(193, 19);
            this.txtProjectName.Name = "txtProjectName";
            this.txtProjectName.ReadOnly = true;
            this.txtProjectName.Size = new System.Drawing.Size(341, 26);
            this.txtProjectName.TabIndex = 9;
            // 
            // gbTools
            // 
            this.gbTools.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbTools.Controls.Add(this.btnSysManagment);
            this.gbTools.Location = new System.Drawing.Point(12, 5);
            this.gbTools.Name = "gbTools";
            this.gbTools.Size = new System.Drawing.Size(858, 46);
            this.gbTools.TabIndex = 4;
            this.gbTools.TabStop = false;
            // 
            // treeContextMenu
            // 
            this.treeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.добавитьУзелToolStripMenuItem,
            this.добавитьПроектToolStripMenuItem,
            this.удалитьToolStripMenuItem});
            this.treeContextMenu.Name = "treeContextMenu";
            this.treeContextMenu.Size = new System.Drawing.Size(168, 70);
            // 
            // добавитьУзелToolStripMenuItem
            // 
            this.добавитьУзелToolStripMenuItem.Enabled = false;
            this.добавитьУзелToolStripMenuItem.Name = "добавитьУзелToolStripMenuItem";
            this.добавитьУзелToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.добавитьУзелToolStripMenuItem.Text = "Добавить узел";
            this.добавитьУзелToolStripMenuItem.Click += new System.EventHandler(this.добавитьУзелToolStripMenuItem_Click);
            // 
            // добавитьПроектToolStripMenuItem
            // 
            this.добавитьПроектToolStripMenuItem.Name = "добавитьПроектToolStripMenuItem";
            this.добавитьПроектToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.добавитьПроектToolStripMenuItem.Text = "Добавить проект";
            this.добавитьПроектToolStripMenuItem.Click += new System.EventHandler(this.добавитьПроектToolStripMenuItem_Click);
            // 
            // удалитьToolStripMenuItem
            // 
            this.удалитьToolStripMenuItem.Name = "удалитьToolStripMenuItem";
            this.удалитьToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.удалитьToolStripMenuItem.Text = "Удалить";
            this.удалитьToolStripMenuItem.Click += new System.EventHandler(this.удалитьToolStripMenuItem_Click);
            // 
            // projectNodeContextMenu
            // 
            this.projectNodeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.запуститьprojectNode,
            this.остановитьprojectNode,
            this.перезапуститьprojectNode,
            this.удалитьprojectNode});
            this.projectNodeContextMenu.Name = "projectNodeContextMenu";
            this.projectNodeContextMenu.Size = new System.Drawing.Size(156, 92);
            // 
            // запуститьprojectNode
            // 
            this.запуститьprojectNode.Name = "запуститьprojectNode";
            this.запуститьprojectNode.Size = new System.Drawing.Size(155, 22);
            this.запуститьprojectNode.Text = "Запустить";
            // 
            // остановитьprojectNode
            // 
            this.остановитьprojectNode.Name = "остановитьprojectNode";
            this.остановитьprojectNode.Size = new System.Drawing.Size(155, 22);
            this.остановитьprojectNode.Text = "Остановить";
            // 
            // перезапуститьprojectNode
            // 
            this.перезапуститьprojectNode.Name = "перезапуститьprojectNode";
            this.перезапуститьprojectNode.Size = new System.Drawing.Size(155, 22);
            this.перезапуститьprojectNode.Text = "Перезапустить";
            // 
            // удалитьprojectNode
            // 
            this.удалитьprojectNode.Name = "удалитьprojectNode";
            this.удалитьprojectNode.Size = new System.Drawing.Size(155, 22);
            this.удалитьprojectNode.Text = "Удалить";
            this.удалитьprojectNode.Click += new System.EventHandler(this.удалитьprojectNode_Click);
            // 
            // rootContextMenu
            // 
            this.rootContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rootAddNode,
            this.rootAddProject});
            this.rootContextMenu.Name = "treeContextMenu";
            this.rootContextMenu.Size = new System.Drawing.Size(168, 70);
            // 
            // rootAddNode
            // 
            this.rootAddNode.Enabled = false;
            this.rootAddNode.Name = "rootAddNode";
            this.rootAddNode.Size = new System.Drawing.Size(167, 22);
            this.rootAddNode.Text = "Добавить узел";
            this.rootAddNode.Click += new System.EventHandler(this.rootAddNode_Click);
            // 
            // rootAddProject
            // 
            this.rootAddProject.Name = "rootAddProject";
            this.rootAddProject.Size = new System.Drawing.Size(167, 22);
            this.rootAddProject.Text = "Добавить проект";
            this.rootAddProject.Click += new System.EventHandler(this.rootAddProject_Click);
            // 
            // debugViewer
            // 
            this.debugViewer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.debugViewer.BackColor = System.Drawing.SystemColors.Window;
            this.debugViewer.Location = new System.Drawing.Point(12, 546);
            this.debugViewer.Multiline = true;
            this.debugViewer.Name = "debugViewer";
            this.debugViewer.Path = null;
            this.debugViewer.ReadOnly = true;
            this.debugViewer.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.debugViewer.Size = new System.Drawing.Size(858, 113);
            this.debugViewer.TabIndex = 0;
            this.debugViewer.WordWrap = false;
            // 
            // btnSysManagment
            // 
            this.btnSysManagment.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnSysManagment.Enabled = false;
            this.btnSysManagment.Image = ((System.Drawing.Image)(resources.GetObject("btnSysManagment.Image")));
            this.btnSysManagment.Location = new System.Drawing.Point(6, 10);
            this.btnSysManagment.Name = "btnSysManagment";
            this.btnSysManagment.Size = new System.Drawing.Size(30, 30);
            this.btnSysManagment.TabIndex = 17;
            this.btnSysManagment.UseVisualStyleBackColor = true;
            this.btnSysManagment.Visible = false;
            // 
            // btnLogViewer
            // 
            this.btnLogViewer.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnLogViewer.Enabled = false;
            this.btnLogViewer.Image = ((System.Drawing.Image)(resources.GetObject("btnLogViewer.Image")));
            this.btnLogViewer.Location = new System.Drawing.Point(623, 19);
            this.btnLogViewer.Name = "btnLogViewer";
            this.btnLogViewer.Size = new System.Drawing.Size(30, 30);
            this.btnLogViewer.TabIndex = 18;
            this.btnLogViewer.UseVisualStyleBackColor = true;
            this.btnLogViewer.Click += new System.EventHandler(this.btnLogViewer_Click);
            // 
            // btnStopProject
            // 
            this.btnStopProject.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnStopProject.Enabled = false;
            this.btnStopProject.Image = ((System.Drawing.Image)(resources.GetObject("btnStopProject.Image")));
            this.btnStopProject.Location = new System.Drawing.Point(587, 19);
            this.btnStopProject.Name = "btnStopProject";
            this.btnStopProject.Size = new System.Drawing.Size(30, 30);
            this.btnStopProject.TabIndex = 15;
            this.btnStopProject.UseVisualStyleBackColor = true;
            this.btnStopProject.Click += new System.EventHandler(this.btnStopProject_Click);
            // 
            // btnStartProject
            // 
            this.btnStartProject.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnStartProject.Enabled = false;
            this.btnStartProject.Image = ((System.Drawing.Image)(resources.GetObject("btnStartProject.Image")));
            this.btnStartProject.Location = new System.Drawing.Point(551, 19);
            this.btnStartProject.Name = "btnStartProject";
            this.btnStartProject.Size = new System.Drawing.Size(30, 30);
            this.btnStartProject.TabIndex = 14;
            this.btnStartProject.UseVisualStyleBackColor = true;
            this.btnStartProject.Click += new System.EventHandler(this.btnStartProject_Click);
            // 
            // btnStartObserve
            // 
            this.btnStartObserve.ImageIndex = 1;
            this.btnStartObserve.ImageList = this.btnImage;
            this.btnStartObserve.Location = new System.Drawing.Point(42, 18);
            this.btnStartObserve.Name = "btnStartObserve";
            this.btnStartObserve.Size = new System.Drawing.Size(30, 30);
            this.btnStartObserve.TabIndex = 13;
            this.btnStartObserve.UseVisualStyleBackColor = true;
            this.btnStartObserve.Click += new System.EventHandler(this.btnStartObserve_Click);
            // 
            // btnSettingProject
            // 
            this.btnSettingProject.Image = ((System.Drawing.Image)(resources.GetObject("btnSettingProject.Image")));
            this.btnSettingProject.Location = new System.Drawing.Point(6, 18);
            this.btnSettingProject.Name = "btnSettingProject";
            this.btnSettingProject.Size = new System.Drawing.Size(30, 30);
            this.btnSettingProject.TabIndex = 12;
            this.btnSettingProject.UseVisualStyleBackColor = true;
            this.btnSettingProject.Click += new System.EventHandler(this.btnSettingProject_Click);
            // 
            // btnKillManager
            // 
            this.btnKillManager.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnKillManager.Enabled = false;
            this.btnKillManager.Image = ((System.Drawing.Image)(resources.GetObject("btnKillManager.Image")));
            this.btnKillManager.Location = new System.Drawing.Point(623, 91);
            this.btnKillManager.Name = "btnKillManager";
            this.btnKillManager.Size = new System.Drawing.Size(30, 30);
            this.btnKillManager.TabIndex = 18;
            this.btnKillManager.UseVisualStyleBackColor = true;
            this.btnKillManager.Click += new System.EventHandler(this.btnKillManager_Click);
            // 
            // btnStopManager
            // 
            this.btnStopManager.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStopManager.Enabled = false;
            this.btnStopManager.Image = ((System.Drawing.Image)(resources.GetObject("btnStopManager.Image")));
            this.btnStopManager.Location = new System.Drawing.Point(623, 55);
            this.btnStopManager.Name = "btnStopManager";
            this.btnStopManager.Size = new System.Drawing.Size(30, 30);
            this.btnStopManager.TabIndex = 17;
            this.btnStopManager.UseVisualStyleBackColor = true;
            this.btnStopManager.Click += new System.EventHandler(this.btnStopManager_Click);
            // 
            // btnStartManager
            // 
            this.btnStartManager.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStartManager.Enabled = false;
            this.btnStartManager.Image = ((System.Drawing.Image)(resources.GetObject("btnStartManager.Image")));
            this.btnStartManager.Location = new System.Drawing.Point(623, 19);
            this.btnStartManager.Name = "btnStartManager";
            this.btnStartManager.Size = new System.Drawing.Size(30, 30);
            this.btnStartManager.TabIndex = 16;
            this.btnStartManager.UseVisualStyleBackColor = true;
            this.btnStartManager.Click += new System.EventHandler(this.btnStartManager_Click);
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToResizeRows = false;
            this.dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(6, 19);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.Size = new System.Drawing.Size(611, 391);
            this.dataGridView.TabIndex = 1;
            this.dataGridView.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_RowEnter);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 662);
            this.Controls.Add(this.debugViewer);
            this.Controls.Add(this.gbTools);
            this.Controls.Add(this.gbProjectControl);
            this.Controls.Add(this.gbPmon);
            this.Controls.Add(this.treeView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(900, 700);
            this.Name = "MainForm";
            this.Text = "Мульти консоль WinCC OA";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.gbPmon.ResumeLayout(false);
            this.gbProjectControl.ResumeLayout(false);
            this.gbProjectControl.PerformLayout();
            this.gbTools.ResumeLayout(false);
            this.treeContextMenu.ResumeLayout(false);
            this.projectNodeContextMenu.ResumeLayout(false);
            this.rootContextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView;
        //private System.Windows.Forms.DataGridView dataGridView;
        private WccPcm.AppDataGridView dataGridView;
        private System.Windows.Forms.GroupBox gbPmon;
        //private WccPcm.AppButton btnStopManager;
        //private WccPcm.AppButton btnStartManager;
        //private WccPcm.AppButton btnKillManager;
        private System.Windows.Forms.GroupBox gbProjectControl;
        //private WccPcm.AppButton btnSettingProject;
        //private WccPcm.AppButton btnAddManager;
        private System.Windows.Forms.GroupBox gbTools;
        //private WccPcm.AppButton btnDeleteManager;
        //private WccPcm.AppButton btnStopProject;
        //private WccPcm.AppButton btnStartProject;
        //private WccPcm.AppButton btnStartObserve;
        private System.Windows.Forms.TextBox txtProjectName;
        private System.Windows.Forms.Label lProjectName;
        private System.Windows.Forms.ContextMenuStrip treeContextMenu;
        private System.Windows.Forms.ToolStripMenuItem добавитьУзелToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem добавитьПроектToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem удалитьToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip projectNodeContextMenu;
        private System.Windows.Forms.ToolStripMenuItem запуститьprojectNode;
        private System.Windows.Forms.ToolStripMenuItem остановитьprojectNode;
        private System.Windows.Forms.ToolStripMenuItem перезапуститьprojectNode;
        private System.Windows.Forms.ToolStripMenuItem удалитьprojectNode;
        private System.Windows.Forms.ContextMenuStrip rootContextMenu;
        private System.Windows.Forms.ToolStripMenuItem rootAddNode;
        private System.Windows.Forms.ToolStripMenuItem rootAddProject;
        //private AppButton btnInsertManager;
        //private AppButton btnManagerSettings;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ImageList btnImage;
        //private LogViewer logViewer;
        private AppButton btnSettingProject;
        private AppButton btnStartObserve;
        private AppButton btnKillManager;
        private AppButton btnStopManager;
        private AppButton btnStartManager;
        private AppButton btnStopProject;
        private AppButton btnStartProject;
        private DebugViewer debugViewer;
        private AppButton btnSysManagment;
        private AppButton btnLogViewer;
    }
}

