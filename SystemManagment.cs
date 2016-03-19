using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WccPcm
{
    public partial class SystemManagment : Form
    {
        private IWccProjectInfo projectInfo;
        
        public SystemManagment(IWccProjectInfo ProjectInfo)
        {
            InitializeComponent();
            this.projectInfo = ProjectInfo;
        }

        private void btnShowLogView_Click(object sender, EventArgs e)
        {
            LoggerForm form = new LoggerForm(this.projectInfo);
            form.Show();
        }
    }
}
