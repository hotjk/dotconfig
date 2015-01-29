using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Utility.Security
{
    public class RijndaelManager
    {
        public RijndaelManager(
            byte[] key,
            byte[] iv,
            CipherMode cipherMode = CipherMode.CBC, int saltSize = 8)
        {
            _key = key;
            _iv = iv;
            _cipherMode = cipherMode;
            _saltSize = saltSize;
        }
        private byte[] _key;
        private byte[] _iv;
        private CipherMode _cipherMode;
        private int _saltSize;

        public static void GenerateKeyAndIV(out string Key, out string IV)
        {
            using (RijndaelManaged AES = new RijndaelManaged())
            {
                Key = Convert.ToBase64String(AES.Key);
                IV = Convert.ToBase64String(AES.IV);
            }
        }

        public static void GenerateKeyAndIV(out byte[] Key, out byte[] IV)
        {
            using (RijndaelManaged AES = new RijndaelManaged())
            {
                Key = AES.Key;
                IV = AES.IV;
            }
        }

        public byte[] Encrypt(params byte[][] bytesToBeEncrypted)
        {
            byte[] encrypted;
            using (RijndaelManaged AES = new RijndaelManaged())
            {
                AES.Mode = _cipherMode;
                AES.Key = _key;
                AES.IV = _iv;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms,
                        AES.CreateEncryptor(),
                        CryptoStreamMode.Write))
                    {
                        foreach (byte[] bytes in bytesToBeEncrypted)
                        {
                            cs.Write(bytes, 0, bytes.Length);
                        }
                        cs.Close();
                    }
                    encrypted = ms.ToArray();
                }
            }
            return encrypted;
        }

        public byte[] Decrypt(byte[] bytesToBeDecrypted)
        {
            byte[] decryptedBytes = null;
            using (RijndaelManaged AES = new RijndaelManaged())
            {
                AES.Mode = _cipherMode;
                AES.Key = _key;
                AES.IV = _iv;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms,
                        AES.CreateDecryptor(),
                        CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }
            return decryptedBytes;
        }

        public string Encrypt(string text)
        {
            byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(text);
            byte[] encryptedBytes;
            if (_saltSize > 0)
            {
                byte[] saltBytes = GetRandomBytes(_saltSize);
                encryptedBytes = Encrypt(saltBytes, bytesToBeEncrypted);
            }
            else
            {
                encryptedBytes = Encrypt(bytesToBeEncrypted);
            }
            return Convert.ToBase64String(encryptedBytes);
        }

        public string Decrypt(string text)
        {
            byte[] bytesToBeDecrypted = Convert.FromBase64String(text);
            byte[] decryptedBytes = Decrypt(bytesToBeDecrypted);
            byte[] originalBytes;
            if (_saltSize > 0)
            {
                originalBytes = new byte[decryptedBytes.Length - _saltSize];
                Buffer.BlockCopy(decryptedBytes, _saltSize, originalBytes, 0, originalBytes.Length);
            }
            else
            {
                originalBytes = decryptedBytes;
            }
            return Encoding.UTF8.GetString(originalBytes);
        }


        public static byte[] GetRandomBytes(int length)
        {
            byte[] ba = new byte[length];
            RNGCryptoServiceProvider.Create().GetBytes(ba);
            return ba;
        }
    }
}
