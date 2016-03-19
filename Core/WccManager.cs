using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace WccPcm
{
    public delegate void ManagerEventHandler(WccManager sender);

    public class WccManager
    {
        public event ManagerEventHandler ManagerStateChanged;

        public enum ManagerMode
        {
            manual = 0,
            once = 1,
            alwayes = 2
        }

        private int id;
        private int state;

        public int Id 
        {
            get { return this.id; }
            set 
            {
                if (this.id != value && value > 0)
                {                    
                    //SetProcess(); 
                }
                else if (this.id > 0 && value <= 0)
                {
                    if (this.ManagerProcess != null)
                    {
                        this.ManagerProcess.Dispose();
                        this.ManagerProcess = null;
                    }
                }
                this.id = value;
            }
        }

        public long Memory { get; set; }
        public string ManagerName { get; set; }
        public string Description { get; set; }
        public string Options { get; set; }
        public string MachineName { get; set; }
        public string ProjectAlias { get; set; }
        public ManagerMode Mode { get; set; }
        public int State
        {
            get { return this.state; }
            set 
            {
                if (this.state == 0 && value > 0 && this.id > 0)
                {
                    SetProcess();
                }
                else if (value == 0 && this.Id > 0)
                {
                    this.Id = -1;
                }
                this.state = value;
            } 
        }
        public int Number { get; set; }
        public int RestartCount { get; set; }
        public int ResetMin { get; set; }
        public int SecKill { get; set; }
        public int PmonNumber { get; set; }
        public Process ManagerProcess { get; private set; }

        public WccManager()
        {
            this.id = -1;
            Memory = 0;
            ManagerName = "n/a";
            Description = "n/a";
            Options = "n/a";
            this.state = 0;
            Number = - 1;
            MachineName = "";
        }

        public void Refresh()
        {
            if(ManagerProcess != null)
            {
                try
                {
                    ManagerProcess.Refresh();
                    this.Memory = ManagerProcess.WorkingSet64 / 1024;
                }
                catch(InvalidOperationException e)
                {
                    throw new InvalidOperationException(e.Message);
                }
                if (ManagerStateChanged != null)
                {
                    ManagerStateChanged(this);
                }                
            }
        }

        public void Replace(WccManager manager)
        {
            /*if (this.Id != manager.Id)
            {
                this.ManagerProcess = manager.ManagerProcess;
            } */
            this.ProjectAlias = manager.ProjectAlias;
            this.MachineName = manager.MachineName;
            this.Id = manager.Id;
            this.Memory = manager.Memory;
            this.ManagerName = manager.ManagerName;
            this.Description = manager.Description;
            this.Options = manager.Options;
            this.State = manager.State;
            this.Number = manager.Number;
            this.PmonNumber = manager.PmonNumber;
        }

        private void SetProcess()
        {
            try
            {
                this.ManagerProcess = Process.GetProcessById(this.Id, this.MachineName);
            }
            catch (ArgumentException e)
            {
                this.ManagerProcess = null;
                this.Id = -1;
                Debugger.Write(this.ProjectAlias + ". Ошибка при получении данных процесса: " + e.Message);
            }
            catch (InvalidOperationException e)
            {
                this.ManagerProcess = null;
                this.Id = -1;
                Debugger.Write(this.ProjectAlias + ". Ошибка при получении данных процесса: " + e.Message);
            }
        }

    }
}
