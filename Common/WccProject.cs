using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Management;
using System.IO;
using System.Xml.Serialization;


namespace WccPcm
{
    public delegate void WccProjectStateEventHandler(WccProject project);
    public delegate void ProjectSettingCallback(ProjectSettingArgs arg);

    public class ProjectSettingArgs
    {
        public string MachineName;
        public int PmonPort;
        public bool AutoConnect;
        public EncryptContainer Credential;
        public string Path;
    }

    /// <summary>
    ///  Предоставялет доступ к базовому функционалу мониторинга и управления WinCC OA.
    /// </summary>
    [Serializable]
    public class WccProject : IWccProjectInfo, IWccControl, IDisposable
    {
        public enum WccConnectionState
        {
            Non = 0,
            Disconnected = 1,
            Warning = 2,
            Connected = 3,
            Processing = 4,
        }

        /*private class StateObject
        {
            public NetworkStream workStream = null;
            public const int BufferSize = 2048;//Размер буфера сокета
            public byte[] buffer = new byte[BufferSize];
            public int Length = 0;
            public ManualResetEvent receiveDone = new ManualResetEvent(false);
        }*/

        public event WccProjectStateEventHandler ProjectStateChanged;

        private Thread thread;
        private bool shouldStop;
        private bool authorized;
        private bool autoconnect;
        private int pmonPort;
        private string machineName;
        private Socket handler;
        private int tcpTimeOut;

        #region IWccProjectInfo

        public string ProjectAlias { get; set; }

        public string MachineName { get { return machineName; } set { machineName = value; CheckReady(); } }

        [XmlIgnore]
        public string ProjectName { get; private set; }

        public string Path { get; set; }

        public int PmonPort { get { return pmonPort; } set { pmonPort = value; CheckReady(); } }

        public EncryptContainer Credential { get; set; }

        #endregion    

        [XmlIgnore]
        public int TimeOut { get; set; }

        [XmlIgnore]
        public int ManagersCount { get { return this.Managers.Count; } }

        [XmlIgnore]
        public bool IsReady { get; private set; }

        [XmlIgnore]
        public bool IsObserved { get; private set; }

        [XmlIgnore]
        public bool IsStarted { get; private set; }

        [XmlIgnore]
        public bool AutoConnect 
        { 
            get { return autoconnect; } 
            set 
            {                  
                if (this.IsReady && value) 
                {
                    this.autoconnect = value;
                    Observe(); 
                }                
            } 
        }

        [XmlIgnore]
        public WccManagerCollection Managers { get; private set; }

        [XmlIgnore]
        public WccConnectionState ConnectionState { get; private set; }
                

        #region ctor

        public WccProject()
        {
            Initialize();
        }

        public WccProject(string MachineName, int Port) 
        {
            this.MachineName = MachineName;
            this.PmonPort = Port;
            Initialize();
        }

        #endregion

        #region IWccControl

        public void StartProject()
        {
            string[] result = PmonQuery("##START_ALL: \n");
            if (result != null)
            {
                Debugger.Write(ProjectAlias + ". Старт проекта: " + result[0]);
            }
        }

        public void StopProject()
        {
            string[] result = PmonQuery("##STOP_ALL: \n");
            if (result != null)
            {
                Debugger.Write(ProjectAlias + ". Останов проекта: " + result[0]);
            }
        }

        public void RestartProject()
        {
            string[] result = PmonQuery("##RESTART_ALL: \n");
            if (result != null)
            {
                Debugger.Write(ProjectAlias + ". Перезапуск проекта: " + result[0]);
            }
        }

        public void StartManager(WccManager manager)
        {
            string[] result = PmonQuery("##SINGLE_MGR:START " + manager.PmonNumber + " \n");
            if (result != null)
            {
                Debugger.Write(ProjectAlias + ". Старт менеджера " + manager.ManagerName + " (" + manager.Number + "): " + result[0]);
            }
        }

        public void StopManager(WccManager manager)
        {
            string[] result = PmonQuery("##SINGLE_MGR:STOP " + manager.PmonNumber + " \n");
            if (result != null)
            {
                Debugger.Write(ProjectAlias + ". Останов менеджера " + manager.ManagerName + " (" + manager.Number + "): " + result[0]);
            }
        }

        public void KillManager(WccManager manager)
        {
            string[] result = PmonQuery("##SINGLE_MGR:KILL " + manager.PmonNumber + " \n");
            if (result != null)
            {
                Debugger.Write(ProjectAlias + ". Принудительный останов менеджера " + manager.ManagerName + " (" + manager.Number + "): " + result[0]);
            }
        }

        public void AppendManager(WccManager manager)
        {
            throw new NotImplementedException();
        }

        public void InsertManager(WccManager manager)
        {
            throw new NotImplementedException();
        }

        public void RemoveManager(WccManager manager)
        {
            string[] result = PmonQuery("##SINGLE_MGR:DEL " + manager.PmonNumber + " \n");
            if (result != null)
            {
                Debugger.Write(ProjectAlias + ". Удаление менеджера " + manager.ManagerName + ": " + result[0]);
            }
            RefreshProjectState();
        }

        #endregion

        public void Observe()
        {
            this.ConnectionState = WccConnectionState.Processing;
            if (this.IsReady && !this.IsObserved)
            {
                thread = new Thread(Worker);
                thread.Start();
            }
        }

        public void StopObserve()
        {
            this.ConnectionState = WccConnectionState.Processing;
            this.shouldStop = true;
            this.Managers.Clear();
            //this.managerCounter = -1;
            if (this.thread != null && this.IsObserved)
            {
                //this.thread.Abort();
                DeleteAutorize();
            }
            this.ConnectionState = WccConnectionState.Disconnected;
            this.IsObserved = false;
            //this.Managers.Clear();
            if (ProjectStateChanged != null)
            {
                ProjectStateChanged(this);
            }            
        }

        #region Private Methods

        private void Worker()
        {            
            Authorize();

            string host = "";
            if (this.MachineName == "localhost" ||
               this.MachineName == "127.0.0.1" ||
               this.MachineName == Dns.GetHostName())
            {
                tcpTimeOut = 250;
                host = "127.0.0.1";
            }
            else
            {
                tcpTimeOut = 2000;
                host = this.MachineName;
            }

            try
            {
                using (this.handler = WccTcp.tcpOpen(host, this.PmonPort))
                {
                    while (!this.shouldStop)
                    {
                        this.IsObserved = true;
                        RefreshInfo();
                        Thread.Sleep(this.TimeOut);
                    }
                    WccTcp.tcpClose(handler);
                } 
            }
            catch (SocketException e)
            {
                Debugger.Write(ProjectAlias + ". " + e.Message);
                StopObserve();
            }
           
            this.shouldStop = false;
        }

        private void Initialize()
        {
            this.Managers = new WccManagerCollection();
            this.shouldStop = false;
            this.IsObserved = false;
            this.IsReady = false;
            this.TimeOut = 5000;
            this.pmonPort = 0;
            this.machineName = "";
            this.autoconnect = false;
            this.ConnectionState = WccConnectionState.Non;
            //this.managerCounter = -1;
            this.Credential = new EncryptContainer();
            this.Credential.DefaultAutorization = true;
            this.Credential.Login = "";
            this.Credential.Password = "";
            this.authorized = false;
            this.IsStarted = false;
            //this.needReloadManagerList = false;
            this.ProjectName = "";
            this.tcpTimeOut = 250;
            this.Path = "";
            //this.receiveDone = new ManualResetEvent(false);
            //thread = new Thread(this.Worker);
        }

        private void Authorize()
        {
            /*TEMP*/
            if (Credential.DefaultAutorization)
            {
                return;
            }
            using (var scope = new Process())
            {
                var startInfo = new ProcessStartInfo()
                {
                    FileName = "cmd",
                    ///c
                    Arguments = @"/c net use \\" + this.MachineName + @"\c$ " + Credential.Password + @" /USER:" + Credential.Login + @" /persistent:yes",
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    StandardOutputEncoding = Encoding.GetEncoding(866),
                    StandardErrorEncoding = Encoding.GetEncoding(866),
                    WindowStyle = ProcessWindowStyle.Hidden
                };
                scope.StartInfo = startInfo;
                scope.Start();
                StreamReader reader = scope.StandardOutput;
                string output = reader.ReadToEnd();
                output = output.Replace("\r\n", " ");
                if (!String.IsNullOrEmpty(output))
                {
                    Debugger.Write(ProjectAlias + ". Авторизация. " + output);
                }
                /*
                byte[] bytes = new byte[output.Length * sizeof(char)];
                System.Buffer.BlockCopy(output.ToCharArray(), 0, bytes, 0, bytes.Length);
                Logger.Write(output.Length + ";" + bytes.Length);
                Logger.Write(BitConverter.ToString(bytes));*/

                this.authorized = true;
                reader = scope.StandardError;
                output = reader.ReadToEnd();
                output = output.Replace("\r\n", " ");
                if (!String.IsNullOrEmpty(output))
                {
                    Debugger.Write(ProjectAlias + ". Авторизация. " + output);
                    this.authorized = false;
                }

                scope.WaitForExit();
            }
            /*TEMP*/
        }

        private void DeleteAutorize()
        {
            if (!this.authorized)
            {
                return;
            }
            using (var scope = new Process())
            {
                var startInfo = new ProcessStartInfo()
                {
                    FileName = "cmd",
                    ///c 
                    Arguments = @"/c net use /DELETE \\" + this.MachineName + @"\c$",
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    StandardOutputEncoding = Encoding.GetEncoding(866),
                    StandardErrorEncoding = Encoding.GetEncoding(866),
                    WindowStyle = ProcessWindowStyle.Hidden
                };
                scope.StartInfo = startInfo;
                scope.Start();
                StreamReader reader = scope.StandardOutput;
                string output = reader.ReadToEnd();
                output = output.Replace("\r\n", " ");
                if (!String.IsNullOrEmpty(output))
                {
                    Debugger.Write(ProjectAlias + ". Авторизация. " + output);
                }

                reader = scope.StandardError;
                output = reader.ReadToEnd();
                output = output.Replace("\r\n", " ");
                if (!String.IsNullOrEmpty(output))
                {
                    Debugger.Write(ProjectAlias + ". Авторизация. " + output);
                }
                this.authorized = false;
            }
            /*TEMP*/
        }

        private void CheckReady()
        {
            if (this.PmonPort > 0 && this.MachineName != "")
            {
                this.IsReady = true;
            }
            else
            {
                this.IsReady = false;
            }
        }

        private void RefreshInfo()
        {
            /*
            int count = GetProcessCount();
            var query = from manager in this.Managers.Values where manager.State == 1 || manager.State == 3 select manager;

            if (managerCounter != count || query.Count() > 0 
                || this.needReloadManagerList 
                || this.ConnectionState == WccConnectionState.Warning
                || this.ConnectionState == WccConnectionState.Disconnected)
            {
                RefreshProjectState();
                this.managerCounter = count;
                this.needReloadManagerList = false;
            }
            else
            {
                RefreshManagersState();
            }
            */
            
            RefreshProjectState();
            RefreshManagersState();
            
        }

        private void RefreshProjectState()
        {
            int result = GetProjectInfo();
            switch (result)
            {
                case -1:
                    this.ConnectionState = WccConnectionState.Disconnected;
                    break;
                case 0:
                    this.ConnectionState = WccConnectionState.Warning;
                    break;
            }       

            if (ProjectStateChanged != null && this.IsObserved && !this.shouldStop)
            {
                RefreshManagersState();
                if(this.ConnectionState == WccConnectionState.Disconnected)
                {
                    this.Managers.Clear();
                }
                ProjectStateChanged(this);
            }
        }

        private void RefreshManagersState()
        {
            foreach (var manager in this.Managers.Values.ToArray())
            {
                try
                {
                    manager.Refresh();
                }
                catch(InvalidOperationException e)
                {
                    //this.needReloadManagerList = true;
                }
                
            }
        }

        private int GetProcessCount()
        {
            int counter = 0;
            try
            {
                Process[] process = Process.GetProcesses(this.MachineName);
                foreach (var proc in process)
                {
                    if (proc.ProcessName.Contains("WCC"))
                    {
                        counter++;
                    }
                }
            }
            catch(InvalidOperationException e)
            {
                //MessageBox.Show(e.Message);
                shouldStop = true;
                IsObserved = true;
                Debugger.Write(ProjectAlias + ". Ошибка при попытке получить список процессов проекта: " + e.Message);
            }
            catch (System.UnauthorizedAccessException unauthorizedErr)
            {
                shouldStop = true;
                IsObserved = true;
                Debugger.Write(ProjectAlias + ". Ошибка при попытке получить список процессов проекта: " + unauthorizedErr.Message);
            }
            return counter;
        }

        private int GetProjectInfo()
        {
            string[] resultSTATI = PmonQuery("##MGRLIST:STATI\n");
            Thread.Sleep(100);
            string[] resultLIST = PmonQuery("##MGRLIST:LIST\n");

            if (resultSTATI == null || resultLIST == null)
            {
                return -1;
            }

            int STATIcount = Convert.ToInt32(!string.IsNullOrEmpty(resultSTATI[0]) ? resultSTATI[0].Replace("LIST:", "") : "0");
            int LISTcount = Convert.ToInt32(!string.IsNullOrEmpty(resultLIST[0]) ? resultLIST[0].Replace("LIST:", "") : "0");

            if (LISTcount != STATIcount || resultSTATI.Length < STATIcount || resultLIST.Length < LISTcount)
            {
                return 0;
            }

            int lastActNum = 0;
            int startedManagersCount = 0;

            for (int i = 1; i <= LISTcount; i++)
            {
                string[] tempSTATI = resultSTATI[i].Split(';');
                string[] tempLIST = resultLIST[i].Split(';');

                WccManager manager = new WccManager();
                manager.MachineName = this.MachineName;
                manager.ProjectAlias = this.ProjectAlias;
                manager.ManagerName = tempLIST[0];
                manager.Id = Convert.ToInt32(tempSTATI[1]);
                manager.Options = tempLIST[5];
                manager.State = Convert.ToInt32(tempSTATI[0]);
                manager.Number = Convert.ToInt32(tempSTATI[4]);
                manager.Mode = (WccManager.ManagerMode)Convert.ToInt32(tempSTATI[2]);
                //Процесс с идентификатором -1 не выполняется.
                if (manager.Id > 0 && manager.State > 0)
                {
                    startedManagersCount++;
                }                
                manager.PmonNumber = i - 1;
                lastActNum = i;

                if(!this.Managers.ContainsKey(manager.PmonNumber))
                {
                    this.Managers.Add(manager.PmonNumber, manager);
                }
                else
                {
                    this.Managers[manager.PmonNumber].Replace(manager);
                }
            }

            if (startedManagersCount > 1)
            {
                this.IsStarted = true;
            }
            else
            {
                this.IsStarted = false;
            }

            if (this.Managers.Count > lastActNum)
            {
                for (int i = lastActNum; i <= this.Managers.Count; i++)
                {
                    this.Managers.Remove(lastActNum);
                }                    
            }

            if(this.ConnectionState == WccConnectionState.Connected)
            {
                string[] result = PmonQuery("##PROJECT: \n");
                if(result != null)
                {
                    this.ProjectName = result[0];
                }                
            }

            return 1;
        }

        /*
        private string[] PmonQuery(string message)
        {
            try
            {
                int iCount = 0;
                bool bAnswerFull = false;
                string result = "";
                string sTemp = "";
                List<string> ds1 = new List<string>();
                int tcpTimeOut;
                string host = "";
                if (this.MachineName == "localhost" ||
                   this.MachineName == "127.0.0.1" ||
                   this.MachineName == Dns.GetHostName())
                {
                    tcpTimeOut = 250;
                    host = "127.0.0.1";
                }
                else
                {
                    tcpTimeOut = 2000;
                    host = this.MachineName;
                }
                using (TcpHandler handler = ccTcp.tcpOpen(host, this.PmonPort))
                //using(TcpClient handler = new TcpClient())
                {
                    ccTcp.tcpWrite(handler, message);

                    result = "-1";

                    while(result != "" && !bAnswerFull)
                    {
                        result = "";
                        result = ccTcp.tcpRead(handler, tcpTimeOut);

                        if(message.Contains("#PROJECT"))
                        {
                            ccTcp.tcpClose(handler);
                            return new string[] { result };
                        }

                        sTemp += result;
                        ds1.Clear();
                        ds1.AddRange(sTemp.Split('\n'));
                        if(ds1.Count > 1 && ds1[0].Contains("LIST") && iCount == 0)
                        {
                            iCount = Convert.ToInt32(!string.IsNullOrEmpty(ds1[0]) ? ds1[0].Replace("LIST:", "") : "0");
                            if (message.Contains("#MGRLIST:STATI"))
                            {
                                iCount += 3;
                            }
                            else
                            {
                                iCount += 2;
                            }
                        }
                        else if (ds1.Count > 0 && iCount == 0)
                        {
                            iCount = 1;
                        }

                        while (ds1.Count > 0 && ds1[0].IndexOf("LIST:") == 0)
                        {
                            ds1.RemoveAt(0);
                        }

                        if (ds1.Count == iCount && iCount > 0)  //All pieces where get
                        {
                            bAnswerFull = true;
                            break;
                        }
                        else
                        {
                            Thread.Sleep(10);
                        }
                    }
                    this.ConnectionState = WccConnectionState.Connected;
                    ccTcp.tcpClose(handler);
                }
                return sTemp.Split('\n');
            }
            catch (ArgumentNullException e)
            {
                this.ConnectionState = WccConnectionState.Disconnected;
                Logger.Write(ProjectAlias + ". Ошибка при подключении к pmon: " + e.Message);
                // MessageBox.Show("ArgumentNullException: " + e); << В Лог
            }
            catch (SocketException e)
            {
                this.ConnectionState = WccConnectionState.Disconnected;
                Logger.Write(ProjectAlias + ". Ошибка при подключении к pmon: " + e.Message);
                // MessageBox.Show("SocketException: " + e); << В Лог
            }
            //this.ConnectionState = WccConnectionState.Warning;
            return null;//new string[] { string.Empty };
        }
        */
        
        private string[] PmonQuery(string message)
        {
            try
            {
                int iCount = 0;
                bool bAnswerFull = false;
                string result = "";
                string sTemp = "";
                List<string> ds1 = new List<string>();

                WccTcp.tcpWrite(handler, message);

                result = "-1";

                while (result != "" && !bAnswerFull)
                {
                    result = "";
                    result = WccTcp.tcpRead(handler, this.tcpTimeOut);

                    if (message.Contains("#PROJECT"))
                    {
                        //WccTcp.tcpClose(handler);
                        return new string[] { result };
                    }

                    sTemp += result;
                    ds1.Clear();
                    ds1.AddRange(sTemp.Split('\n'));
                    if (ds1.Count > 1 && ds1[0].Contains("LIST") && iCount == 0)
                    {
                        iCount = Convert.ToInt32(!string.IsNullOrEmpty(ds1[0]) ? ds1[0].Replace("LIST:", "") : "0");
                        if (message.Contains("#MGRLIST:STATI"))
                        {
                            iCount += 3;
                        }
                        else
                        {
                            iCount += 2;
                        }
                    }
                    else if (ds1.Count > 0 && iCount == 0)
                    {
                        iCount = 1;
                    }

                    while (ds1.Count > 0 && ds1[0].IndexOf("LIST:") == 0)
                    {
                        ds1.RemoveAt(0);
                    }

                    if (ds1.Count == iCount && iCount > 0)  //All pieces where get
                    {
                        bAnswerFull = true;
                        break;
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                    
                    this.ConnectionState = WccConnectionState.Connected;
                }
                return sTemp.Split('\n');
            }
            catch (ArgumentNullException e)
            {
                this.ConnectionState = WccConnectionState.Disconnected;
                Debugger.Write(ProjectAlias + ". Ошибка при подключении к pmon: " + e.Message);
                // MessageBox.Show("ArgumentNullException: " + e); << В Лог
            }
            catch (SocketException e)
            {
                this.ConnectionState = WccConnectionState.Disconnected;
                if(!this.handler.Connected)
                {
                    StopObserve();
                }
                Debugger.Write(ProjectAlias + ". Ошибка при подключении к pmon: " + e.Message);
                // MessageBox.Show("SocketException: " + e); << В Лог
            }
            //this.ConnectionState = WccConnectionState.Warning;
            return null;//new string[] { string.Empty };
        }

        /*
        private string[] PmonQuery(string message)
        {
            try
            {
                TcpClient client = new TcpClient(this.MachineName, this.PmonPort);
                var receiveDone = new ManualResetEvent(false);
                //client.ReceiveBufferSize = 10000;

                NetworkStream stream = client.GetStream();
                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                stream.Write(data, 0, data.Length);
                stream.ReadTimeout = 100;
                Thread.Sleep(250);
                data = new Byte[1024];
                Byte[] response = new Byte[2024];
                String responseData = String.Empty;

                Int32 bytes = stream.Read(response, 0, response.Length);

                responseData = System.Text.Encoding.ASCII.GetString(response, 0, bytes);
                //responseData = System.Text.Encoding.ASCII.GetString(state.buffer, 0, state.Length);
                //responseData = responseData.Substring(0, responseData.IndexOf("\0"));
                // Close everything.
                stream.Close();
                client.Close();
                this.ConnectionState = WccConnectionState.Connected;
                return responseData.Split('\n');
            }
            catch (ArgumentNullException e)
            {
                this.ConnectionState = WccConnectionState.Disconnected;
                Logger.Write(ProjectAlias + ". Ошибка при подключении к pmon: " + e.Message);
                // MessageBox.Show("ArgumentNullException: " + e); << В Лог
            }
            catch (SocketException e)
            {
                this.ConnectionState = WccConnectionState.Disconnected;
                Logger.Write(ProjectAlias + ". Ошибка при подключении к pmon: " + e.Message);
                // MessageBox.Show("SocketException: " + e); << В Лог
            }
            //this.ConnectionState = WccConnectionState.Warning;
            return null;//new string[] { string.Empty };
        }
        */
        /*
        public string[] PmonQuery(string message)
        {
            String responseData = String.Empty;
            Byte[] response = new Byte[2024];
            Int32 length = 0;

            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                try
                {
                    IPAddress[] addresslist = Dns.GetHostAddresses(this.MachineName);
                    IPEndPoint point = new IPEndPoint(addresslist[0], this.PmonPort);
                    socket.Connect(point);

                    Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                    socket.Send(data);

                    data = new Byte[1024];
                                        
                    //socket.ReceiveTimeout = 100;
                    Int32 bytes = 0;
                    while
                    do
                    {
                        bytes = socket.Receive(response, length, response.Length - length, SocketFlags.None);
                        length += bytes;
                    }
                    while (bytes > 0);

                    responseData = System.Text.Encoding.ASCII.GetString(response, 0, length);

                    // Close everything.
                    socket.Close();

                    return responseData.Split('\n');
                }
                catch (ArgumentNullException e)
                {
                    //this.ConnectionState = WccConnectionState.Disconnected;
                    //Logger.Write(ProjectAlias + ". Ошибка при подключении к pmon: " + e.Message);
                    // MessageBox.Show("ArgumentNullException: " + e); << В Лог
                    Console.WriteLine("Ошибка при подключении к pmon: " + e.Message);
                    //socket.Close();
                }
                catch (SocketException e)
                {
                    //this.ConnectionState = WccConnectionState.Disconnected;
                    //Logger.Write(ProjectAlias + ". Ошибка при подключении к pmon: " + e.Message);
                    // MessageBox.Show("SocketException: " + e); << В Лог
                    if (e.ErrorCode != 10060)
                    {
                        Console.WriteLine("Ошибка при подключении к pmon: " + e.Message);
                    }
                    //socket.Close();
                }
            }

            responseData = System.Text.Encoding.ASCII.GetString(response, 0, length);

            //this.ConnectionState = WccConnectionState.Warning;
            return responseData.Split('\n');//new string[] { string.Empty };
        }
        */
        /*
        private void ReadCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            NetworkStream stream = state.workStream;
            // Читаем принятые от клиента пакеты 
            //MessageBox.Show("1");
            try
            {
                int bytesRead = stream.EndRead(ar);
                state.Length += bytesRead;
                if (bytesRead > 0)
                {
                    IAsyncResult result = stream.BeginRead(state.buffer, state.Length, StateObject.BufferSize - state.Length,
                                                            new AsyncCallback(ReadCallback), state);
                    result.AsyncWaitHandle.WaitOne(2000, true);
                }
                int ir = Array.IndexOf(state.buffer,(byte)0);
                //ar.AsyncWaitHandle.WaitOne(2000, false);
                if (Array.IndexOf(state.buffer,(byte)0) < state.Length + 1 || bytesRead <= 0)
                {
                    //this.receiveDone.Set();
                    state.receiveDone.Set();
                }
            }
            catch(Exception e)
            {
                Debugger.Write(ProjectAlias + ". " + e.Message);
            }         
        }
        */
        #endregion

        #region IDisposable

        public void Dispose()
        {
            /*
            if(this.IsObserved)
            {
                StopObserve();
            }
             * */
        }

        #endregion

    }
}
