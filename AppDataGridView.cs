using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace WccPcm
{
    [Description("Data Grid View с установленым свойством DoubleBuffered = true")]
    public class AppDataGridView : DataGridView
    {
        public AppDataGridView() : base()
        {
            // и устанавливаем значение true при создании экземпляра класса
            this.DoubleBuffered = true;
            // или с помощью метода SetStyle
            //this.SetStyle(ControlStyles.DoubleBuffer |
            //    ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            //this.UpdateStyles();
        }
    }
}
