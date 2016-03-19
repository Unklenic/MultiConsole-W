using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace WccPcm
{
    public partial class LoggerForm : Form
    {
        //[DllImport("user32")]
        //private static extern bool HideCaret(IntPtr hWnd);

        public LoggerForm(IWccProjectInfo ProjectInfo)
        {
            InitializeComponent();
            this.Text = "Log Viewer: " + ProjectInfo.ProjectAlias;
            logViewer.Path = ProjectInfo.Path + "\\log";
            logViewer.StartWatch();
            //HideCaret();
        }

        private void LoggerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            logViewer.StopWatch();
        }

        //private void HideCaret()
        //{
        //    HideCaret(logViewer.Handle);
        //}

    }
}
