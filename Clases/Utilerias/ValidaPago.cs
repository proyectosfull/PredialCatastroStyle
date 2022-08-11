using System;
using System.Security.Cryptography;

namespace Clases.Utilerias
{
    public class ValidaPago
    {

        public string checkHMAC(string cadena)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes("W2844aB7e1f18de2Df7aece57280eebcdEed9de55f4889203a44Cee85c566A255");

            HMACSHA256 hmacsha256 = new HMACSHA256(keyByte);
            byte[] messageBytes = encoding.GetBytes(cadena);
            byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
            return BitConverter.ToString(hashmessage).Replace("-", "").ToLower();
        }
    }
}
