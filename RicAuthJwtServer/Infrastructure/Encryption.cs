using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace RicAuthJwtServer.Infrastructure
{
    //--- Wrap the Encryption/Decrytion functions
    //--- TripleDES Encryption and Decryption Implementation
    public class Encryption
    {
        private static byte[] key = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
        private static byte[] iv = { 65, 110, 68, 26, 69, 178, 200, 219 };

        /// <summary>
        /// This routine handle the RSA TripleDES Encryption
        /// </summary>
        public static string Encrypt(string plainText)
        {
            //Byte()
            //--- Declare a UTF8Encoding object so we may use the GetByte 
            //---    method to transform the plainText into a Byte array. 
            UTF8Encoding utf8encoder = new UTF8Encoding();
            byte[] inputInBytes = utf8encoder.GetBytes(plainText);

            //--- Create a new TripleDES service provider 
            TripleDESCryptoServiceProvider tdesProvider = new TripleDESCryptoServiceProvider();

            //--- The ICryptTransform interface uses the TripleDES 
            //---    crypt provider along with encryption key and init vector 
            //---    information 
            ICryptoTransform cryptoTransform = tdesProvider.CreateEncryptor(key, iv);

            //--- All cryptographic functions need a stream to output the 
            //---    encrypted information. Here we declare a memory stream 
            //---    for this purpose. 
            MemoryStream encryptedStream = new MemoryStream();
            CryptoStream cryptStream = new CryptoStream(encryptedStream, cryptoTransform, CryptoStreamMode.Write);

            //--- Write the encrypted information to the stream. Flush the information 
            //---    when done to ensure everything is out of the buffer. 
            cryptStream.Write(inputInBytes, 0, inputInBytes.Length);
            cryptStream.FlushFinalBlock();
            encryptedStream.Position = 0;

            //--- Read the stream back into a Byte array and return it to the calling 
            //--- method. 
            byte[] result = new byte[(int)encryptedStream.Length - 1 + 1];
            encryptedStream.Read(result, 0, (int)encryptedStream.Length);
            cryptStream.Close();
            return Convert.ToBase64String(result);
        }

        /// <summary>
        /// This routine handle the RSA TripleDES Decryption
        /// </summary>
        public static string Decrypt(string inputEncryptedString)
        {
            byte[] inputInBytes = null;
            inputInBytes = Convert.FromBase64String(inputEncryptedString);

            //--- UTFEncoding is used to transform the decrypted Byte Array 
            //---    information back into a string. 
            UTF8Encoding utf8encoder = new UTF8Encoding();
            TripleDESCryptoServiceProvider tdesProvider = new TripleDESCryptoServiceProvider();

            //--- As before we must provide the encryption/decryption key along with 
            //---    the init vector. 
            ICryptoTransform cryptoTransform = tdesProvider.CreateDecryptor(key, iv);

            //--- Provide a memory stream to decrypt information into 
            MemoryStream decryptedStream = new MemoryStream();
            CryptoStream cryptStream = new CryptoStream(decryptedStream, cryptoTransform, CryptoStreamMode.Write);
            cryptStream.Write(inputInBytes, 0, inputInBytes.Length);
            cryptStream.FlushFinalBlock();
            decryptedStream.Position = 0;

            //--- Read the memory stream and convert it back into a string 
            byte[] result = new byte[(int)decryptedStream.Length - 1 + 1];
            decryptedStream.Read(result, 0, (int)decryptedStream.Length);
            cryptStream.Close();
            UTF8Encoding myutf = new UTF8Encoding();
            return myutf.GetString(result);
        }

        public static string GetHash(string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();

            byte[] byteValue = Encoding.UTF8.GetBytes(input);

            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }
    }
}
