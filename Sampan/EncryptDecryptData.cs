using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Sampan
{
    class EncryptDecryptData
    {
        //Prevois key string
        public static string keyString = "E546C8DF278CD5931069B522E695D4F2";
        public static int keySize = 256;
        //string completeEncodedKey = model.GenerateKey(keySize);
        public static string completeEncodedKey = "dzJJT0xCNWtEUkE4VFh4NHZ3dHIxUT09LGtqTUdhWXY2azJLY1ZKaXdocjBjaEVXZnlHT3d0Q3pCSnFnckZzT0lVckU9";

        //Prevoius
        //public string EncryptString(string text)
        //{
        //    try
        //    {
        //        var key = Encoding.UTF8.GetBytes(keyString);

        //        using (var aesAlg = Aes.Create())
        //        {
        //            using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
        //            {
        //                using (var msEncrypt = new MemoryStream())
        //                {
        //                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
        //                    using (var swEncrypt = new StreamWriter(csEncrypt))
        //                    {
        //                        swEncrypt.Write(text);
        //                    }

        //                    var iv = aesAlg.IV;

        //                    var decryptedContent = msEncrypt.ToArray();

        //                    var result = new byte[iv.Length + decryptedContent.Length];

        //                    Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
        //                    Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

        //                    return Convert.ToBase64String(result);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}

        //// Previous
        //public string DecryptString(string cipherText)
        //{
        //    try
        //    {
        //        var fullCipher = Convert.FromBase64String(cipherText);

        //        var iv = new byte[16];
        //        var cipher = new byte[16];

        //        Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
        //        Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);
        //        var key = Encoding.UTF8.GetBytes(keyString);

        //        using (var aesAlg = Aes.Create())
        //        {
        //            using (var decryptor = aesAlg.CreateDecryptor(key, iv))
        //            {
        //                string result;
        //                using (var msDecrypt = new MemoryStream(cipher))
        //                {
        //                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
        //                    {
        //                        using (var srDecrypt = new StreamReader(csDecrypt))
        //                        {
        //                            result = srDecrypt.ReadToEnd();
        //                        }
        //                    }
        //                }

        //                return result;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;
        //    }
        //}

        public string GenerateKey(int iKeySize)
        {
            RijndaelManaged aesEncryption = new RijndaelManaged();
            aesEncryption.KeySize = iKeySize;
            aesEncryption.BlockSize = 128;
            aesEncryption.Mode = CipherMode.CBC;
            //aesEncryption.Padding = PaddingMode.PKCS7;
            aesEncryption.Padding = PaddingMode.ISO10126;
            aesEncryption.GenerateIV();
            string ivStr = Convert.ToBase64String(aesEncryption.IV);
            aesEncryption.GenerateKey();
            string keyStr = Convert.ToBase64String(aesEncryption.Key);
            string completeKey = ivStr + "," + keyStr;
            return Convert.ToBase64String(ASCIIEncoding.UTF8.GetBytes(completeKey));
        }

        /// <summary>
        /// Encrypt
        /// From : www.chapleau.info/blog/2011/01/06/usingsimplestringkeywithaes256encryptioninc.html
        /// </summary>
        public string EncryptString(string iPlainStr, string iCompleteEncodedKey="", int iKeySize=0)
        {
            RijndaelManaged aesEncryption = new RijndaelManaged();
            aesEncryption.KeySize = keySize;
            aesEncryption.BlockSize = 128;
            aesEncryption.Mode = CipherMode.CBC;
            //aesEncryption.Padding = PaddingMode.PKCS7;
            aesEncryption.Padding = PaddingMode.ISO10126;
            aesEncryption.IV = Convert.FromBase64String(ASCIIEncoding.UTF8.GetString(Convert.FromBase64String(completeEncodedKey)).Split(',')[0]);
            aesEncryption.Key = Convert.FromBase64String(ASCIIEncoding.UTF8.GetString(Convert.FromBase64String(completeEncodedKey)).Split(',')[1]);
            byte[] plainText = ASCIIEncoding.UTF8.GetBytes(iPlainStr);
            ICryptoTransform crypto = aesEncryption.CreateEncryptor();
            byte[] cipherText = crypto.TransformFinalBlock(plainText, 0, plainText.Length);
            return Convert.ToBase64String(cipherText);
        }

        /// <summary>
        /// Decrypt
        /// From : www.chapleau.info/blog/2011/01/06/usingsimplestringkeywithaes256encryptioninc.html
        /// </summary>
        public string DecryptString(string iEncryptedText, string iCompleteEncodedKey="", int iKeySize=0)
        {
            RijndaelManaged aesEncryption = new RijndaelManaged();
            aesEncryption.KeySize = keySize;
            aesEncryption.BlockSize = 128;
            aesEncryption.Mode = CipherMode.CBC;
            //aesEncryption.Padding = PaddingMode.PKCS7;
            aesEncryption.Padding = PaddingMode.ISO10126;
            aesEncryption.IV = Convert.FromBase64String(ASCIIEncoding.UTF8.GetString(Convert.FromBase64String(completeEncodedKey)).Split(',')[0]);
            aesEncryption.Key = Convert.FromBase64String(ASCIIEncoding.UTF8.GetString(Convert.FromBase64String(completeEncodedKey)).Split(',')[1]);
            ICryptoTransform decrypto = aesEncryption.CreateDecryptor();
            byte[] encryptedBytes = Convert.FromBase64CharArray(iEncryptedText.ToCharArray(), 0, iEncryptedText.Length);
            return ASCIIEncoding.UTF8.GetString(decrypto.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length));
        }
        
    }
}

