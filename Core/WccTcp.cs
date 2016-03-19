using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace WccPcm
{
    public static class WccTcp
    {
        public static Socket tcpOpen(string host, int port)
        {
            Socket handler = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {                
                IPAddress[] addresslist = Dns.GetHostAddresses(host);
                IPEndPoint point = new IPEndPoint(addresslist[0], port);
                handler.Connect(point);
            }
            catch(SocketException e)
            {
                throw new SocketException(e.ErrorCode);
            }

            return handler;
        }

        public static void tcpClose(Socket handler)
        {
            try
            {
                if(handler != null)
                {
                    handler.Disconnect(false);
                    handler.Close();
                }                
            }
            catch (SocketException e)
            {
                throw new SocketException(e.ErrorCode);
            }
        }

        public static void tcpWrite(Socket handler, string data)
        {
            try
            {
                if (handler != null)
                {
                    Byte[] d = System.Text.Encoding.ASCII.GetBytes(data);
                    if(handler.Connected)
                    {
                        handler.Send(d);
                    }
                }
            }
            catch (SocketException e)
            {
                throw new SocketException(e.ErrorCode);
            }
        }

        public static void tcpWrite(Socket handler, Byte[] data)
        {
            try
            {
                if (handler != null)
                {
                    if (handler.Connected)
                    {
                        handler.Send(data);
                    }
                }
            }
            catch (SocketException e)
            {
                throw new SocketException(e.ErrorCode);
            }
        }

        public static string tcpRead(Socket handler, int timeOut)
        {
            try
            {
                if (handler != null)
                {
                    if (handler.Connected)
                    {
                        Byte[] buffer = new Byte[handler.ReceiveBufferSize];
                        handler.ReceiveTimeout = timeOut;
                        int bytes = handler.Receive(buffer);
                        return System.Text.Encoding.ASCII.GetString(buffer, 0, bytes);
                    }
                }
            }
            catch (SocketException e)
            {
                throw new SocketException(e.ErrorCode);
            }

            return string.Empty;
        }
    }

    public class TcpHandler : IDisposable
    {
        public TcpClient Client { get; set; }
        public NetworkStream Stream { get; private set; }

        public TcpHandler(TcpClient handler)
        {
            this.Client = handler;
            this.Stream = this.Client.GetStream();
        }

        public void Close()
        {
            this.Stream.Close();
            this.Client.Close();
        }

        public void Dispose()
        {
            this.Stream.Dispose();
            ((IDisposable)this.Client).Dispose();
        }
    }

    public static class ccTcp
    {
        public static TcpHandler tcpOpen(string host, int port)
        {
            TcpClient client = new TcpClient(host, port);
            TcpHandler handler = new TcpHandler(client);
            /*
            try
            {
                handler.Connect(point);
            }
            catch (SocketException e)
            {
                throw new SocketException(e.ErrorCode);
            }
            */
            return handler;
        }

        public static void tcpClose(TcpHandler handler)
        {
            try
            {
                if (handler != null)
                {
                    handler.Close();
                }
            }
            catch (SocketException e)
            {
                throw new SocketException(e.ErrorCode);
            }
        }

        public static void tcpWrite(TcpHandler handler, string data)
        {
            try
            {
                if (handler != null)
                {
                    Byte[] d = System.Text.Encoding.ASCII.GetBytes(data);
                    if (handler.Client.Connected)
                    {
                        handler.Stream.Write(d, 0, d.Length);
                    }
                }
            }
            catch (SocketException e)
            {
                throw new SocketException(e.ErrorCode);
            }
        }

        public static void tcpWrite(TcpHandler handler, Byte[] data)
        {
            try
            {
                if (handler != null)
                {
                    if (handler.Client.Connected)
                    {
                        handler.Stream.Write(data, 0, data.Length);
                    }
                }
            }
            catch (SocketException e)
            {
                throw new SocketException(e.ErrorCode);
            }
        }

        public static string tcpRead(TcpHandler handler, int timeOut)
        {
            try
            {
                if (handler != null)
                {
                    if (handler.Client.Connected)
                    {
                        Byte[] buffer = new Byte[2048];
                        handler.Stream.ReadTimeout = timeOut;
                        int bytes = handler.Stream.Read(buffer, 0, buffer.Length);
                        return System.Text.Encoding.ASCII.GetString(buffer, 0, bytes);
                    }
                }
            }
            catch (SocketException e)
            {
                throw new SocketException(e.ErrorCode);
            }

            return string.Empty;
        }
    }
}
