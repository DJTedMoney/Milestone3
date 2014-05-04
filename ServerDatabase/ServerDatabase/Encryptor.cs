using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Security.Cryptography;

namespace SQLiteTest
{
    public static class Encryptor
    {
        public static string encryptString(string token)
        {
            MD5 md5_encryptor = new MD5CryptoServiceProvider();

            md5_encryptor.ComputeHash(ASCIIEncoding.ASCII.GetBytes(token) );

            StringBuilder encryptedToken = new StringBuilder();

            return encryptedToken.ToString();
        }
    }
}
