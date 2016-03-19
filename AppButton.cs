using System;
using System.Windows.Forms;

namespace WccPcm
{
    public class AppButton : Button
    {

        public AppButton() : base()
        {
            this.MouseEnter += new System.EventHandler(this.btnMouseEnter);
            this.MouseLeave += new System.EventHandler(this.btnMouseLeave);
        }

        private void btnMouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void btnMouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }
    }
}
