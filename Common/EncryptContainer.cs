using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WccPcm
{
    [Serializable]
    public class EncryptContainer
    {
        public string Login { get; set;}
        public bool DefaultAutorization { get; set; }
        public byte[] EncPassword
        {
            get
            {
                return Encrypt(Password);
            }
            set
            {
                try
                {
                    Password = System.Text.Encoding.Default.GetString(Decrypt(value));
                }
                catch (IndexOutOfRangeException e)
                {
                    Debugger.Write(e.Message);
                }
            }
        }

        [XmlIgnore]
        public string Password { get; set; }

        public EncryptContainer()
        {
            Login = "";
            DefaultAutorization = true;
            Password = "";
        }

        private static byte[] Encrypt(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            //int[] lBytes = new int[bytes.Length];
            byte[] bResult = new byte[sizeof(int) * bytes.Length];
            var j = 0;
            for (byte i = 0; i < bytes.Length; i++)
            {
                var temp = (int)(((bytes[i] + i) * 9999) ^ (0xf32 + i * 3));
                var source = BitConverter.GetBytes((int)(((bytes[i] * ((bytes.Length * (i == 0 ? 2 : bytes[i - 1])) ^ 0xba) + i) * 9999) ^ (0xf32 + i * 3)));
                Array.Copy(source, 0, bResult, j, source.Length);
                j += sizeof(int);
            }
            return bResult;
        }

        private static byte[] Decrypt(byte[] bytes)
        {
            byte[] lBytes = new byte[sizeof(int)];
            byte[] bResult = new byte[bytes.Length / sizeof(int)];
            var j = 0;
            var len = bytes.Length / sizeof(int);
            for (byte i = 0; i < len; i++)
            {
                if (j + lBytes.Length > bytes.Length)
                {
                    throw new IndexOutOfRangeException("Ошибка в зашифрованной строке");
                }

                Array.Copy(bytes, j, lBytes, 0, lBytes.Length);
                var temp = BitConverter.ToInt32(lBytes, 0);
                bResult[i] = (byte)((((BitConverter.ToInt32(lBytes, 0) ^ (0xf32 + i * 3)) / 9999) - i) / ((len * (i == 0 ? 2 : bResult[i - 1])) ^ 0xba));
                j += sizeof(int);
            }
            return bResult;
        }


        bool isEqual(string login, string password)
        {
            return true;
        }
    }
}