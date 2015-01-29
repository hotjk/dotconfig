using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Utility.Security
{
    public class RSAManager
    {
        public RSAManager(string key)
        {
            _key = key;
        }
        private string _key;

        public static void GenerateKeyAndIV(out string publicKey, out string privateKey)
        {
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                publicKey = RSA.ToXmlString(false);
                privateKey = RSA.ToXmlString(true);
            }
        }

        public byte[] Encrypt(byte[] bytesToBeEncrypted)
        {
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.FromXmlString(_key);
                var encryptedBytes = RSA.Encrypt(bytesToBeEncrypted, false);
                return encryptedBytes;
            }
        }

        public byte[] Decrypt(byte[] bytesToBeDecrypted)
        {
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.FromXmlString(_key);
                var decryptedBytes = RSA.Decrypt(bytesToBeDecrypted, false);
                return decryptedBytes;
            }
        }

        public byte[] PrivateEncrypt(byte[] bytesToBeEncrypted)
        {
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.FromXmlString(_key);
                byte[] encryptedBytes = RSA.PrivareEncryption(bytesToBeEncrypted);
                return encryptedBytes;
            }
        }

        public byte[] PublicDecrypt(byte[] bytesToBeDecrypted)
        {
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.FromXmlString(_key);
                byte[] decryptedBytes = RSA.PublicDecryption(bytesToBeDecrypted);
                return decryptedBytes;
            }
        }
    }
}
