using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WccPcm
{
    class DebugViewer : TextBox
    {
        private FileSystemWatcher fileSystemWatcher;

        /*public String Filter
        {
            get { return this.fileSystemWatcher.Filter; }
            set { this.fileSystemWatcher.Filter = value; }
        }*/

        public String Path
        {
            /*
            get { return this.fileSystemWatcher.Path; }
            set { this.fileSystemWatcher.Path = value; }
             */
            get;
            set;
        }

        public DebugViewer()
            : base()
        {
            this.Multiline = true;
            this.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TabIndex = 0;
            this.ReadOnly = true;
            this.BackColor = System.Drawing.SystemColors.Window;
        }

        public void StartWatch()
        {
            this.fileSystemWatcher = new FileSystemWatcher();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher)).BeginInit();
            this.fileSystemWatcher.EnableRaisingEvents = true;
            this.fileSystemWatcher.Filter = "*.log";
            this.fileSystemWatcher.NotifyFilter = System.IO.NotifyFilters.LastWrite;
            this.fileSystemWatcher.SynchronizingObject = this;
            this.fileSystemWatcher.Path = /*Application.StartupPath*/this.Path;
            this.fileSystemWatcher.Changed += new System.IO.FileSystemEventHandler(this.fileSystemWatcher_Changed);
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher)).EndInit();
            this.AppendText(this.Path + "\n");
        }

        public void StopWatch()
        {
            this.fileSystemWatcher.EnableRaisingEvents = false;
        }

        private void fileSystemWatcher_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            //this.AppendText(e.FullPath);
            foreach (string line in Debugger.Read())
            {
                this.AppendText(line);
            }
        }
    }
}
