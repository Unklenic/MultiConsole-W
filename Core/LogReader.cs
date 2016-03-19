using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace WccPcm
{
    public class LogReader
    {
        private Dictionary<string, int> logMap;

        public LogReader(IEnumerable<string> Files)
        {
            logMap = new Dictionary<string, int>();
            foreach(var file in Files)
            {
                logMap.Add(file, GetLastLineCounter(file));
            }
        }

        public IEnumerable<string> Read(string FileName)
        {
            List<string> lines = new List<string>();
            bool repit = true;
            
            //InitializeCounter();
            if (File.Exists(FileName))
            {
                while (repit)
                {
                    try
                    {
                        //using (StreamReader stream = new StreamReader(FileName))
                        if(!this.logMap.ContainsKey(FileName))
                        {
                            logMap.Add(FileName, GetLastLineCounter(FileName));
                        }

                        var currentLine = this.logMap[FileName];
                        using (FileStream FS = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            StreamReader stream = new StreamReader(FS);
                            for (int i = 1; i < currentLine; i++)
                            {
                                stream.ReadLine();
                            }
                            string line;
                            while ((line = stream.ReadLine()) != null)
                            {
                                lines.Add(line + "\n");
                                currentLine++;
                            }
                            stream.Close();
                            stream.Dispose();
                            repit = false;
                        }
                        this.logMap[FileName] = currentLine;
                    }
                    catch (IOException e)
                    {
                        //lines = (List<string>)Read();
                        repit = true;
                    }
                    /*catch(KeyNotFoundException e)
                    {
                        logMap.Add(FileName, GetLastLineCounter(FileName));
                    }*/
                    if (repit)
                    {
                        Thread.Sleep(100);
                    }                    
                }

            }            
            return lines;
        }

        private int GetLastLineCounter(string FileName)
        {
            int count = 0;

            try
            {
                //using (StreamReader sr = new StreamReader(FileName))
                using(FileStream FS = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    StreamReader sr = new StreamReader(FS);
                    while ((sr.ReadLine()) != null)
                    {
                        count++;
                    }
                    sr.Close();
                    sr.Dispose();
                }
            }
            catch (IOException e)
            {
                //lines = (List<string>)Read();
                Debugger.Write(e.Message);
            }           
            
            return count;
        }

    }
}
