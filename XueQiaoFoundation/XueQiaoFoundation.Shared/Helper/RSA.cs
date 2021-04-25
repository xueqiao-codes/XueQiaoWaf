using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.Shared.Helper
{
    public static class RSA
    {
        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="publickey"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static byte[] RSAEncrypt(string xmlPublickey, byte[] content)
        {
            byte[] cipherbytes;
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(xmlPublickey);
                cipherbytes = rsa.Encrypt(content, false);
            }
            return cipherbytes;
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="xmlPrivatekey"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static byte[] RSADecrypt(string xmlPrivatekey, byte[] content)
        {
            var rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(xmlPrivatekey);
            cipherbytes = rsa.Decrypt(content, false);
            return cipherbytes;
        }
    }
}
