using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace XueQiaoFoundation.Shared.Helper
{
    /// <summary>
    /// AES 加解密辅助。
    /// 加解密由 4 个参数参与运作：明文、密文、密钥、向量。为了初学者容易理解，可以把4个参数的关系写成：密文=明文+密钥+向量；明文=密文-密钥-向量。
    /// </summary>
    public static class AES
    {
        /// <summary>
        /// AES 加密
        /// </summary>
        /// <param name="content">要进行加密的原始内容</param>
        /// <param name="key">秘钥（必须为 128 bit 以上，16 字节）</param>
        /// <param name="IV">向量（必须为 128 bit，16 字节）</param>
        /// <returns>已加密的内容</returns>
        public static byte[] Encrypt(byte[] content, byte[] Key, byte[] IV)
        {
            SymmetricAlgorithm aes = Rijndael.Create();
            //设置密钥及密钥向量
            aes.Key = Key;
            aes.IV = IV;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.ECB;//设置为ECB
            aes.Padding = PaddingMode.PKCS7;//设置为PKCS7，否则解密后字符串结尾会出现多余字符

            ICryptoTransform cTransform = aes.CreateEncryptor();
            var s = cTransform.TransformFinalBlock(content, 0, content.Length);
            aes.Clear();
            return s;
        }

        /// <summary>
        /// AES 解密
        /// </summary>
        /// <param name="content">要进行解密的原始内容</param>
        /// <param name="key">秘钥（必须为 128 bit 以上，16 字节）</param>
        /// <param name="IV">向量（必须为 128 bit，16 字节）</param>
        /// <returns>已解密的内容</returns>
        public static byte[] Decrypt(byte[] content, byte[] Key, byte[] IV)
        {
            SymmetricAlgorithm aes = Rijndael.Create();
            aes.Key = Key;
            aes.IV = IV;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.ECB;//必须设置为ECB
            aes.Padding = PaddingMode.PKCS7;//设置为PKCS7，否则解密后字符串结尾会出现多余字符

            ICryptoTransform cTransform = aes.CreateDecryptor();
            var s = cTransform.TransformFinalBlock(content, 0, content.Length);
            aes.Clear();
            return s;
        }
    }
}
