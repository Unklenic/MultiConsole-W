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
    public partial class ProjectSettings : Form
    {
        private ProjectSettingCallback setDel;

        public ProjectSettings(IWccProjectInfo SelectedProject, ProjectSettingCallback callback)
        {
            InitializeComponent();
            txtMachineName.Text = SelectedProject.MachineName;
            txtPmonPort.Text = Convert.ToString(SelectedProject.PmonPort);
            cbDefault.Checked = SelectedProject.Credential.DefaultAutorization;
            txtPassword.Text = SelectedProject.Credential.Password;
            txtLoginName.Text = SelectedProject.Credential.Login;
            txtPath.Text = SelectedProject.Path;
            setDel = callback;
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            ProjectSettingArgs args = new ProjectSettingArgs();
            args.MachineName = txtMachineName.Text;
            args.PmonPort = Convert.ToInt32(txtPmonPort.Text);
            args.AutoConnect = cbAutoConnect.Checked;
            EncryptContainer Credential = new EncryptContainer();
            Credential.Login = txtLoginName.Text;
            Credential.Password = txtPassword.Text;
            Credential.DefaultAutorization = cbDefault.Checked;
            args.Credential = Credential;
            args.Path = txtPath.Text;
            setDel(args);
            this.Close();
        }

        private void cbDefault_CheckedChanged(object sender, EventArgs e)
        {
            if (cbDefault.Checked)
            {
                gbCredential.Enabled = false;
            }
            else
            {
                gbCredential.Enabled = true;
            }
        }
    }
}
