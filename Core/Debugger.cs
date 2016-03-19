using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace WccPcm
{
    public static class Debugger
    {
        private static int currentLine;
        const string fileName = "Console.log";

        /*
        public Logger()
        {
            _currentLine = getLastLineCounter();
        }
        */

        public static void Write(string Message)
        {
            bool repit = true;
            InitializeCounter();
            if (File.Exists(fileName))
            {
                while (repit)
                {
                    try
                    {
                        using (StreamWriter stream = new StreamWriter(fileName, true))
                        {
                            stream.WriteLine("[" + Convert.ToString(DateTime.Now) + "]: " + Message);
                            stream.Close();
                            stream.Dispose();
                            repit = false;
                        }
                    }
                    catch (IOException e)
                    {
                        //Write(Message);
                        repit = true;
                    }
                    if (repit)
                    {
                        Thread.Sleep(100);
                    }
                }
            }
            else
            {
                CreateLogFile();
            }
        }

        public static IEnumerable<string> Read()
        {
            List<string> lines = new List<string>();
            bool repit = true;
            InitializeCounter();
            if (File.Exists(fileName))
            {
                while (repit)
                {
                    try
                    {
                        using (StreamReader stream = new StreamReader(fileName))
                        {
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
                    }
                    catch (IOException e)
                    {
                        //lines = (List<string>)Read();
                        repit = true;
                    }
                    if (repit)
                    {
                        Thread.Sleep(100);
                    }
                }

            }
            return lines;
        }

        private static void CreateLogFile()
        {
            using (StreamWriter stream = new StreamWriter(fileName))
            {
                stream.WriteLine("## Console logger 1B505662 ##");
                stream.Close();
                stream.Dispose();
            }
        }

        private static int getLastLineCounter()
        {
            int count = 1;
            if (File.Exists(fileName))
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    while ((sr.ReadLine()) != null)
                    {
                        count++;
                    }
                }
            }
            else
            {
                CreateLogFile();
            }
            return count;
        }

        private static void InitializeCounter()
        {
            if (currentLine < 1)
            {
                currentLine = getLastLineCounter();
            }
        }
    }

}
