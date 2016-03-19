namespace WccPcm
{
    partial class SystemManagment
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SystemManagment));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnShowLogView = new WccPcm.AppButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnShowLogView);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(510, 138);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // btnShowLogView
            // 
            this.btnShowLogView.Image = ((System.Drawing.Image)(resources.GetObject("btnShowLogView.Image")));
            this.btnShowLogView.Location = new System.Drawing.Point(6, 19);
            this.btnShowLogView.Name = "btnShowLogView";
            this.btnShowLogView.Size = new System.Drawing.Size(55, 40);
            this.btnShowLogView.TabIndex = 0;
            this.btnShowLogView.UseVisualStyleBackColor = true;
            this.btnShowLogView.Click += new System.EventHandler(this.btnShowLogView_Click);
            // 
            // SystemManagment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 162);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SystemManagment";
            this.Text = "System Managment";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private AppButton btnShowLogView;
    }
}