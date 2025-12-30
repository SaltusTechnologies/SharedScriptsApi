using System;
using System.Text;
using SharedScriptsApi.Interfaces;

namespace SharedScriptsApi.Utilities
{
    public class CryptographyProvider : ICryptographyProvider
    {
        #region constants

        private const string ENCRYPT_PASSWORD_PASSWORD = "pL6hOeB#?h9uP_OE6_LUJlA9laylA18aY$U4iuVi";
        private const string ENCRYPT_PASSWORD_SALT = "*0luw8e8hl@x@ej7eglE9iUvl-Di+p+@PH1e9=uK";

        #endregion

        public byte[] Decrypt(string password, string salt, byte[] buffer, int offset, int count)
        {
            return SaltusCryptoTransformProvider.Default.Decrypt(password, salt, buffer, offset, count);
        }

        public byte[] Decrypt(byte[] key, byte[] buffer, int offset, int count)
        {
            return SaltusCryptoTransformProvider.Default.Decrypt(key, buffer, offset, count);
        }

        public byte[] GetKey(string password, string salt)
        {
            return SaltusCryptoTransformProvider.Default.GetKey(password, salt);
        }

        string? ICryptographyProvider.Decrypt(string value)
        {
            string? retVal = null;

            if (!string.IsNullOrEmpty(value))
            {
                byte[] base64Password = Convert.FromBase64String(value);
                byte[] decryptedValue = SaltusCryptoTransformProvider.Default.Decrypt(ENCRYPT_PASSWORD_PASSWORD, ENCRYPT_PASSWORD_SALT, base64Password, 0, base64Password.Length);
                retVal = new UTF8Encoding(false).GetString(decryptedValue, 0, decryptedValue.Length);
            }

            return retVal;

        }
        string? ICryptographyProvider.Decrypt(string value, string password, string salt)
        {
            string? retVal = null;

            if (!string.IsNullOrEmpty(value))
            {
                byte[] base64Password = Convert.FromBase64String(value);
                byte[] decryptedValue = SaltusCryptoTransformProvider.Default.Decrypt(password, salt, base64Password, 0, base64Password.Length);
                retVal = new UTF8Encoding(false).GetString(decryptedValue, 0, decryptedValue.Length);
            }

            return retVal;

        }

        string? ICryptographyProvider.Encrypt(string value)
        {
            string? newPassword = value != null
                                     ? Convert.ToBase64String(SaltusCryptoTransformProvider.Default.Encrypt(ENCRYPT_PASSWORD_PASSWORD, ENCRYPT_PASSWORD_SALT, new UTF8Encoding(false).GetBytes(value), 0, new UTF8Encoding(false).GetByteCount(value)))
                                     : null;
            return newPassword;
        }

        string? ICryptographyProvider.Encrypt(string value, string password, string salt)
        {
            string? encryptedString = value != null
                                     ? Convert.ToBase64String(SaltusCryptoTransformProvider.Default.Encrypt(password, salt, new UTF8Encoding(false).GetBytes(value), 0, new UTF8Encoding(false).GetByteCount(value)))
                                     : null;
            return encryptedString;
        }
    }
}
