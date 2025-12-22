#region usings

using System;
using System.Security.Cryptography;

#endregion

namespace SharedScriptsApi.Utilities
{
    public class SaltusCryptoTransformProvider : CryptoTransformProviderBase
    {
        #region constants

        private const int BLOCK_SIZE = 128;
        private const CipherMode CIPHER_MODE = CipherMode.CBC;
        private const int ITERATIONS = 1;
        private const int KEY_SIZE = 256;

        #endregion

        #region .ctors

        public SaltusCryptoTransformProvider()
            : base("ds/$8WNt{cZkF?!C")
        { }

        #endregion

        #region methods

        protected override SymmetricAlgorithm CreateSymmetricAlgorithm()
        {
            return CreateSymmetricAlgorithm(BLOCK_SIZE, CIPHER_MODE);
        }

        public override byte[] GetKey(string password, string salt)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            if (password.Length == 0)
            {
                throw new ArgumentException("password");
            }
            if (salt == null)
            {
                throw new ArgumentNullException("salt");
            }
            if (salt.Length == 0)
            {
                throw new ArgumentException("salt");
            }
            return this.GetKey(this.Encoding.GetBytes(password), this.Encoding.GetBytes(salt));
        }

        public override byte[] GetKey(byte[] password, byte[] salt)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            if (password.Length == 0)
            {
                throw new ArgumentException("password");
            }
            if (salt == null)
            {
                throw new ArgumentNullException("salt");
            }
            if (salt.Length == 0)
            {
                throw new ArgumentException("salt");
            }
            return CreateRfc2898DeriveBytesKey(password, salt, ITERATIONS, KEY_SIZE);
        }

        #endregion

        #region static fields/properties

        private static CryptoTransformProviderBase _Default = new SaltusCryptoTransformProvider();
        public static CryptoTransformProviderBase Default { get { return _Default; } }

        #endregion

        #region static methods

        private static byte[] CreateRfc2898DeriveBytesKey(byte[] password, byte[] salt, byte passwordIterations, short keySize)
        {
            using (Rfc2898DeriveBytes derivedBytes = new Rfc2898DeriveBytes(password, salt, passwordIterations))
            {
                return derivedBytes.GetBytes(keySize / 8);
            }
        }

        private static SymmetricAlgorithm CreateSymmetricAlgorithm(int blockSize, CipherMode mode)
        {
            RijndaelManaged retVal = new RijndaelManaged();
            try
            {
                retVal.BlockSize = blockSize;
                retVal.Mode = mode;
            }
            catch
            {
                ((IDisposable)retVal).Dispose();
                throw;
            }

            return retVal;
        }

        #endregion
    }
}