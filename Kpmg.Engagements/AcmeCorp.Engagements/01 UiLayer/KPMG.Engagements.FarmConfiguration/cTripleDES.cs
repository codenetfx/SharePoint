using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint;

namespace AcmeCorp.Engagements.FarmConfiguration
{
    public class cTripleDES
    {
        // define the triple des provider
        private TripleDESCryptoServiceProvider m_des =
                 new TripleDESCryptoServiceProvider();

        // define the string handler
        private UTF8Encoding m_utf8 = new UTF8Encoding();

        // define the local property arrays
        private byte[] m_key;
        private byte[] m_iv;

        public cTripleDES()
        {
            SPFarm farm;
            farm = SPFarm.Local;
            byte[] key = new byte[24];
            string[] sKey = farm.Properties["pbs_secret_key"].ToString().Split('-');
            for (int i = 0; i < sKey.Length-1; i++)
            {
                key[i] = Byte.Parse(sKey[i]);
            }

            byte[] iv = new byte[8];
            string[] sIV = farm.Properties["pbs_initialization_vector"].ToString().Split('-');
            for (int i = 0; i < sIV.Length - 1; i++)
            {
                iv[i] = Byte.Parse(sIV[i]);
            }
            
            this.m_key = key;
            this.m_iv = iv;
        }

        public static byte[] StrToByteArray(string str)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(str);
        }

        public byte[] Encrypt(byte[] input)
        {
            return Transform(input,
                   m_des.CreateEncryptor(m_key, m_iv));
        }

        public byte[] Decrypt(byte[] input)
        {
            return Transform(input,
                   m_des.CreateDecryptor(m_key, m_iv));
        }

        public string Encrypt(string text)
        {
            byte[] input = m_utf8.GetBytes(text);
            byte[] output = Transform(input,
                            m_des.CreateEncryptor(m_key, m_iv));
            return Convert.ToBase64String(output);
        }

        public string Decrypt(string text)
        {
            byte[] input = Convert.FromBase64String(text);
            byte[] output = Transform(input,
                            m_des.CreateDecryptor(m_key, m_iv));
            return m_utf8.GetString(output);
        }

        private byte[] Transform(byte[] input,
                       ICryptoTransform CryptoTransform)
        {
            // create the necessary streams
            MemoryStream memStream = new MemoryStream();
            CryptoStream cryptStream = new CryptoStream(memStream,
                         CryptoTransform, CryptoStreamMode.Write);
            // transform the bytes as requested
            cryptStream.Write(input, 0, input.Length);
            cryptStream.FlushFinalBlock();
            // Read the memory stream and
            // convert it back into byte array
            memStream.Position = 0;
            byte[] result = memStream.ToArray();
            // close and release the streams
            memStream.Close();
            cryptStream.Close();
            // hand back the encrypted buffer
            return result;
        }
    }

}
