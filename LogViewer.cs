using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace WccPcm
{
    public class LogViewer : RichTextBox
    {
        private LogReader reader;
        private FileSystemWatcher fileSystemWatcher;
        private Dictionary<string, Color> logColor;
        //[DllImport("user32")]
        //private static extern bool HideCaret(IntPtr hWnd);

        private struct TextColor
        {
            public static Color INFO { get { return Color.FromArgb(0, 128, 0); } }
            public static Color SEVERE { get { return Color.FromArgb(255, 0, 255); } }
            public static Color WARNING { get { return Color.FromArgb(255, 165, 58); } }
            public static Color FATAL { get { return Color.FromArgb(255, 0, 0); } }
        }

        public String Path { get; set; }

        public LogViewer()
            : base()
        {
            this.Multiline = true;
            this.TabIndex = 0;
            this.ReadOnly = true;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.WordWrap = false;
            //HideCaret(this.Handle);
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
            logColor = new Dictionary<string, Color>();
            Random r = new Random();
            //IEnumerable<string> files = Directory.GetFiles(this.Path, "*.log");
            /*foreach (var file in files)
            {
                logColor.Add(file, Color.FromArgb(r.Next(255), r.Next(255), r.Next(255)));
            }*/
            this.reader = new LogReader(Directory.GetFiles(this.Path,"*.log"));
            //this.reader = new LogReader(files);
        }

        public void StopWatch()
        {
            this.fileSystemWatcher.EnableRaisingEvents = false;
        }

        private void fileSystemWatcher_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            //this.AppendText(e.FullPath);
            string fileName = e.FullPath;
          
            foreach (string line in reader.Read(fileName))
            {
                var l = line.Split(',');
                Color color = Color.Black;
                var colored = true;
                if(l.Length > 3)
                {
                    if(l[3].Contains("INFO"))
                    {
                        color = TextColor.INFO;
                        colored = false;
                    }
                    else if (l[3].Contains("SEVERE"))
                    {
                        color = TextColor.SEVERE;
                    }
                    else if (l[3].Contains("WARNING"))
                    {
                        color = TextColor.WARNING;
                    }
                    else if (l[3].Contains("FATAL"))
                    {
                        color = TextColor.FATAL;
                    }
                    /*
                    switch (l[3])
                    {
                        case "INFO":
                            color = TextColor.INFO;
                            colored = false;
                            break;
                        case "SEVERE":
                            color = TextColor.SEVERE;
                            break;
                        case "WARNING":
                            color = TextColor.WARNING;
                            break;
                        case "FAULT":
                            color = TextColor.FAULT;
                            break;
                    }*/
                    string sep = ",";
                    if (!logColor.ContainsKey(l[0]))
                    {
                        Random r = new Random();
                        logColor.Add(l[0], Color.FromArgb(r.Next(255), r.Next(255), r.Next(255)));
                    }
                    Color stColor = logColor[l[0]];

                    this.AddText(l[0] + sep, stColor);
                    this.AddText(l[1] + sep, stColor);
                    this.AddText(l[2] + sep, stColor);

                    this.AddText(l[3] + sep, color);
                    
                    for (int i = 4; i < l.Length; i++ )
                    {
                        if(i == l.Length - 1)
                        {
                            sep = "";
                        }
                        if (colored)
                        {
                            this.AddText(l[i] + sep, color);
                        }
                        else
                        {
                            this.AddText(l[i] + sep, Color.Black);
                        }
                    }
                }
                else
                {
                    this.AppendText(line);
                }                
                //this.AddText(line, Color.AliceBlue);
            }
        }

        public void AddText(string text, Color color)
        {
            this.SelectionStart = this.TextLength;
            this.SelectionLength = 0;
            this.SelectionColor = color;
            this.AppendText(text);
            this.SelectionColor = this.ForeColor;
        }

        //protected override void OnEnter(EventArgs e)
        //{
        //    HideCaret(this.Handle);
        //}
    }
}
