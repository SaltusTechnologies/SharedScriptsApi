#region usings

using System;
using System.Security.Cryptography;
using System.Text;

#endregion

namespace SharedScriptsApi.Utilities
{
    public abstract class CryptoTransformProviderBase : IDisposable
    {
        #region fields/properties

        protected Encoding Encoding { get; private set; }
        private string _InitializationVector;
        private SymmetricAlgorithm _SymmetricAlgorithm;
        private object _SyncRoot = new object();
        private SymmetricAlgorithm SymmetricAlgorithm
        {
            get
            {
                this.EnsureSymmetricAlgorithm();
                return this._SymmetricAlgorithm;
            }
        }
        protected object SyncRoot { get { return this._SyncRoot; } }
        private byte[] _Iv;

        #endregion

        #region .ctor

        protected CryptoTransformProviderBase(string initializationVector, Encoding encoding)
        {
            if (initializationVector == null)
            {
                throw new ArgumentNullException("initializationVector");
            }

            if (initializationVector == string.Empty)
            {
                throw new ArgumentException("initializationVector");
            }
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }
            this._Iv = encoding.GetBytes(initializationVector);
            this._InitializationVector = initializationVector;
            this.Encoding = encoding;
        }

        protected CryptoTransformProviderBase(string initializationVector)
            : this(initializationVector, new UTF8Encoding(false))
        { }

        #endregion

        public bool TryGetItem(CryptoStreamMode mode, byte[] key, out ICryptoTransform result)
        {
            result = null;
            if (key != null &&
                key.Length > 0)
            {
                switch (mode)
                {
                    case CryptoStreamMode.Read:
                        result = this.SymmetricAlgorithm.CreateDecryptor(key, this._Iv);
                        break;
                    case CryptoStreamMode.Write:
                        result = this.SymmetricAlgorithm.CreateEncryptor(key, this._Iv);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("mode");
                }
            }
            return result != null;
        }

        public bool TryGetItem(CryptoStreamMode mode, string password, string salt, out ICryptoTransform result)
        {
            bool retVal = false;
            result = null;
            if (!string.IsNullOrEmpty(password) &&
                !string.IsNullOrEmpty(salt))
            {
                retVal = this.TryGetItem(mode, this.GetKey(password, salt), out result);
            }
            return retVal;
        }

        #region IDisposable Members

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region methods

        protected abstract SymmetricAlgorithm CreateSymmetricAlgorithm();

        private void EnsureSymmetricAlgorithm()
        {
            if (this._SymmetricAlgorithm == null)
            {
                lock (this._SyncRoot)
                {
                    if (this._SymmetricAlgorithm == null)
                    {
                        this._SymmetricAlgorithm = this.CreateSymmetricAlgorithm();
                    }
                }
            }
        }

        public abstract byte[] GetKey(byte[] password, byte[] salt);
        public abstract byte[] GetKey(string password, string salt);

        ~CryptoTransformProviderBase()
        {
            this.Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this._SymmetricAlgorithm != null)
            {
                this._SymmetricAlgorithm.Clear();
                this._SymmetricAlgorithm = null;
                Array.Clear(this._Iv, 0, this._Iv.Length);
            }
        }

        #endregion
    }
}