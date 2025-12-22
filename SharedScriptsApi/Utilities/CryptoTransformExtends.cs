#region usings

using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;

#endregion

namespace SharedScriptsApi.Utilities
{
    public static class CryptoTransformExtends
    {
        #region static methods
        /*
        private static ICryptoTransform CreateTransform(IAssociatedItemProvider<CryptoStreamMode, ICryptoTransform> source, CryptoStreamMode mode)
        {
            ICryptoTransform transform;
            if (!source.TryGetItem(mode, out transform))
            {
                throw new InvalidOperationException("Unable to create " + (mode == CryptoStreamMode.Read
                                                        ? "decryptor"
                                                        : "encryptor"));
            }
            return transform;
        }
        public static void Decrypt( this IAssociatedItemProvider<CryptoStreamMode, ICryptoTransform> source,  Stream input,  Stream output)
        {
            Transform(source, CryptoStreamMode.Read, input, output);
        }

        public static byte[] Decrypt( this IAssociatedItemProvider<CryptoStreamMode, ICryptoTransform> source,  byte[] buffer, int offset, int count)
        {
            return Transform(source, CryptoStreamMode.Read, buffer, offset, count);
        }

        public static int Decrypt( this IAssociatedItemProvider<CryptoStreamMode, ICryptoTransform> source,  Stream input,  byte[] outBuffer, int outOffset, int outCount)
        {
            return Transform(source, CryptoStreamMode.Read, input, outBuffer, outOffset, outCount);
        }

        public static int Decrypt( this IAssociatedItemProvider<CryptoStreamMode, ICryptoTransform> source,  byte[] inBuffer, int inOffset, int inCount,  byte[] outBuffer, int outOffset, int outCount)
        {
            return Transform(source, CryptoStreamMode.Read, inBuffer, inOffset, inCount, outBuffer, outOffset, outCount);
        }

        public static void Decrypt( this IAssociatedItemProvider<CryptoStreamMode, ICryptoTransform> source,  byte[] buffer, int offset, int count,  Stream output)
        {
            Transform(source, CryptoStreamMode.Read, buffer, offset, count, output);
        }
        public static int Encrypt( this IAssociatedItemProvider<CryptoStreamMode, ICryptoTransform> source,  byte[] inBuffer, int inOffset, int inCount,  byte[] outBuffer, int outOffset, int outCount)
        {
            return Transform(source, CryptoStreamMode.Write, inBuffer, inOffset, inCount, outBuffer, outOffset, outCount);
        }

        public static void Encrypt( this IAssociatedItemProvider<CryptoStreamMode, ICryptoTransform> source,  Stream input,  Stream output)
        {
            Transform(source, CryptoStreamMode.Write, input, output);
        }

        public static byte[] Encrypt( this IAssociatedItemProvider<CryptoStreamMode, ICryptoTransform> source,  byte[] buffer, int offset, int count)
        {
            return Transform(source, CryptoStreamMode.Write, buffer, offset, count);
        }

        public static int Encrypt( this IAssociatedItemProvider<CryptoStreamMode, ICryptoTransform> source,  Stream input,  byte[] outBuffer, int outOffset, int outCount)
        {
            return Transform(source, CryptoStreamMode.Write, input, outBuffer, outOffset, outCount);
        }

        public static void Encrypt( this IAssociatedItemProvider<CryptoStreamMode, ICryptoTransform> source,  byte[] buffer, int offset, int count,  Stream output)
        {
            Transform(source, CryptoStreamMode.Write, buffer, offset, count, output);
        }
        private static void Transform( IAssociatedItemProvider<CryptoStreamMode, ICryptoTransform> source, CryptoStreamMode mode,  Stream input,  Stream output)
        {
            using (ICryptoTransform trans = CreateTransform(source, mode))
            {
                Transform(trans, input, output);
            }
        }

        private static byte[] Transform( IAssociatedItemProvider<CryptoStreamMode, ICryptoTransform> source, CryptoStreamMode mode,  byte[] buffer, int offset, int count)
        {
            byte[] retVal;
            using (ICryptoTransform trans = CreateTransform(source, mode))
            {
                retVal = Transform(trans, buffer, offset, count);
            }
            return retVal;
        }

        private static int Transform( IAssociatedItemProvider<CryptoStreamMode, ICryptoTransform> source, CryptoStreamMode mode,  Stream input,  byte[] outBuffer, int outOffset, int outCount)
        {
            int retVal;
            using (ICryptoTransform trans = CreateTransform(source, mode))
            {
                retVal = Transform(trans, input, outBuffer, outOffset, outCount);
            }
            return retVal;
        }

        private static int Transform( IAssociatedItemProvider<CryptoStreamMode, ICryptoTransform> source, CryptoStreamMode mode,  byte[] inBuffer, int inOffset, int inCount,  byte[] outBuffer, int outOffset, int outCount)
        {
            int retVal;
            using (ICryptoTransform trans = CreateTransform(source, mode))
            {
                retVal = Transform(trans, inBuffer, inOffset, inCount, outBuffer, outOffset, outCount);
            }
            return retVal;
        }

        private static void Transform( IAssociatedItemProvider<CryptoStreamMode, ICryptoTransform> source, CryptoStreamMode mode,  byte[] buffer, int offset, int count,  Stream output)
        {
            using (ICryptoTransform trans = CreateTransform(source, mode))
            {
                Transform(trans, buffer, offset, count, output);
            }
        }
        */
        private static ICryptoTransform CreateTransform(CryptoTransformProviderBase source, CryptoStreamMode mode, string password, string salt)
        {
            ICryptoTransform transform;
            if (!source.TryGetItem(mode, password, salt, out transform))
            {
                throw new InvalidOperationException("Unable to create " + (mode == CryptoStreamMode.Read
                                                        ? "decryptor"
                                                        : "encryptor"));
            }
            return transform;
        }

        private static ICryptoTransform CreateTransform(CryptoTransformProviderBase source, CryptoStreamMode mode, byte[] key)
        {
            ICryptoTransform transform;
            if (!source.TryGetItem(mode, key, out transform))
            {
                throw new InvalidOperationException("Unable to create " + (mode == CryptoStreamMode.Read
                                                        ? "decryptor"
                                                        : "encryptor"));
            }
            return transform;
        }



        public static void Decrypt(this CryptoTransformProviderBase source, string password, string salt, Stream input, Stream output)
        {
            Transform(source, CryptoStreamMode.Read, password, salt, input, output);
        }

        public static byte[] Decrypt(this CryptoTransformProviderBase source, string password, string salt, byte[] buffer, int offset, int count)
        {
            return Transform(source, CryptoStreamMode.Read, password, salt, buffer, offset, count);
        }

        public static int Decrypt(this CryptoTransformProviderBase source, string password, string salt, Stream input, byte[] outBuffer, int outOffset, int outCount)
        {
            return Transform(source, CryptoStreamMode.Read, password, salt, input, outBuffer, outOffset, outCount);
        }

        public static int Decrypt(this CryptoTransformProviderBase source, string password, string salt, byte[] inBuffer, int inOffset, int inCount, byte[] outBuffer, int outOffset, int outCount)
        {
            return Transform(source, CryptoStreamMode.Read, password, salt, inBuffer, inOffset, inCount, outBuffer, outOffset, outCount);
        }

        public static void Decrypt(this CryptoTransformProviderBase source, string password, string salt, byte[] buffer, int offset, int count, Stream output)
        {
            Transform(source, CryptoStreamMode.Read, password, salt, buffer, offset, count, output);
        }

        public static void Decrypt(this CryptoTransformProviderBase source, byte[] key, Stream input, Stream output)
        {
            Transform(source, CryptoStreamMode.Read, key, input, output);
        }

        public static byte[] Decrypt(this CryptoTransformProviderBase source, byte[] key, byte[] buffer, int offset, int count)
        {
            return Transform(source, CryptoStreamMode.Read, key, buffer, offset, count);
        }

        public static int Decrypt(this CryptoTransformProviderBase source, byte[] key, Stream input, byte[] outBuffer, int outOffset, int outCount)
        {
            return Transform(source, CryptoStreamMode.Read, key, input, outBuffer, outOffset, outCount);
        }

        public static int Decrypt(this CryptoTransformProviderBase source, byte[] key, byte[] inBuffer, int inOffset, int inCount, byte[] outBuffer, int outOffset, int outCount)
        {
            return Transform(source, CryptoStreamMode.Read, key, inBuffer, inOffset, inCount, outBuffer, outOffset, outCount);
        }

        public static void Decrypt(this CryptoTransformProviderBase source, byte[] key, byte[] buffer, int offset, int count, Stream output)
        {
            Transform(source, CryptoStreamMode.Read, key, buffer, offset, count, output);
        }


        public static int Encrypt(this CryptoTransformProviderBase source, string password, string salt, byte[] inBuffer, int inOffset, int inCount, byte[] outBuffer, int outOffset, int outCount)
        {
            return Transform(source, CryptoStreamMode.Write, password, salt, inBuffer, inOffset, inCount, outBuffer, outOffset, outCount);
        }
        public static void Encrypt(this CryptoTransformProviderBase source, string password, string salt, Stream input, Stream output)
        {
            Transform(source, CryptoStreamMode.Write, password, salt, input, output);
        }

        public static byte[] Encrypt(this CryptoTransformProviderBase source, string password, string salt, byte[] buffer, int offset, int count)
        {
            return Transform(source, CryptoStreamMode.Write, password, salt, buffer, offset, count);
        }

        public static int Encrypt(this CryptoTransformProviderBase source, string password, string salt, Stream input, byte[] outBuffer, int outOffset, int outCount)
        {
            return Transform(source, CryptoStreamMode.Write, password, salt, input, outBuffer, outOffset, outCount);
        }

        public static void Encrypt(this CryptoTransformProviderBase source, string password, string salt, byte[] buffer, int offset, int count, Stream output)
        {
            Transform(source, CryptoStreamMode.Write, password, salt, buffer, offset, count, output);
        }
        public static int Encrypt(this CryptoTransformProviderBase source, byte[] key, byte[] inBuffer, int inOffset, int inCount, byte[] outBuffer, int outOffset, int outCount)
        {
            return Transform(source, CryptoStreamMode.Write, key, inBuffer, inOffset, inCount, outBuffer, outOffset, outCount);
        }
        public static void Encrypt(this CryptoTransformProviderBase source, byte[] key, Stream input, Stream output)
        {
            Transform(source, CryptoStreamMode.Write, key, input, output);
        }

        public static byte[] Encrypt(this CryptoTransformProviderBase source, byte[] key, byte[] buffer, int offset, int count)
        {
            return Transform(source, CryptoStreamMode.Write, key, buffer, offset, count);
        }

        public static int Encrypt(this CryptoTransformProviderBase source, byte[] key, Stream input, byte[] outBuffer, int outOffset, int outCount)
        {
            return Transform(source, CryptoStreamMode.Write, key, input, outBuffer, outOffset, outCount);
        }

        public static void Encrypt(this CryptoTransformProviderBase source, byte[] key, byte[] buffer, int offset, int count, Stream output)
        {
            Transform(source, CryptoStreamMode.Write, key, buffer, offset, count, output);
        }



        private static void Transform(CryptoTransformProviderBase source, CryptoStreamMode mode, string password, string salt, Stream input, Stream output)
        {
            using (ICryptoTransform trans = CreateTransform(source, mode, password, salt))
            {
                Transform(trans, input, output);
            }
        }

        private static byte[] Transform(CryptoTransformProviderBase source, CryptoStreamMode mode, string password, string salt, byte[] buffer, int offset, int count)
        {
            byte[] retVal;
            using (ICryptoTransform trans = CreateTransform(source, mode, password, salt))
            {
                retVal = Transform(trans, buffer, offset, count);
            }
            return retVal;
        }

        private static int Transform(CryptoTransformProviderBase source, CryptoStreamMode mode, string password, string salt, Stream input, byte[] outBuffer, int outOffset, int outCount)
        {
            int retVal;
            using (ICryptoTransform trans = CreateTransform(source, mode, password, salt))
            {
                retVal = Transform(trans, input, outBuffer, outOffset, outCount);
            }
            return retVal;
        }

        private static void Transform(CryptoTransformProviderBase source, CryptoStreamMode mode, string password, string salt, byte[] buffer, int offset, int count, Stream output)
        {
            using (ICryptoTransform trans = CreateTransform(source, mode, password, salt))
            {
                Transform(trans, buffer, offset, count, output);
            }
        }

        private static int Transform(CryptoTransformProviderBase source, CryptoStreamMode mode, string password, string salt, byte[] inBuffer, int inOffset, int inCount, byte[] outBuffer, int outOffset, int outCount)
        {
            int retVal;
            using (ICryptoTransform trans = CreateTransform(source, mode, password, salt))
            {
                retVal = Transform(trans, inBuffer, inOffset, inCount, outBuffer, outOffset, outCount);
            }
            return retVal;
        }

        private static void Transform(CryptoTransformProviderBase source, CryptoStreamMode mode, byte[] key, Stream input, Stream output)
        {
            using (ICryptoTransform trans = CreateTransform(source, mode, key))
            {
                Transform(trans, input, output);
            }
        }

        private static byte[] Transform(CryptoTransformProviderBase source, CryptoStreamMode mode, byte[] key, byte[] buffer, int offset, int count)
        {
            byte[] retVal;
            using (ICryptoTransform trans = CreateTransform(source, mode, key))
            {
                retVal = Transform(trans, buffer, offset, count);
            }
            return retVal;
        }

        private static int Transform(CryptoTransformProviderBase source, CryptoStreamMode mode, byte[] key, Stream input, byte[] outBuffer, int outOffset, int outCount)
        {
            int retVal;
            using (ICryptoTransform trans = CreateTransform(source, mode, key))
            {
                retVal = Transform(trans, input, outBuffer, outOffset, outCount);
            }
            return retVal;
        }

        private static void Transform(CryptoTransformProviderBase source, CryptoStreamMode mode, byte[] key, byte[] buffer, int offset, int count, Stream output)
        {
            using (ICryptoTransform trans = CreateTransform(source, mode, key))
            {
                Transform(trans, buffer, offset, count, output);
            }
        }

        private static int Transform(CryptoTransformProviderBase source, CryptoStreamMode mode, byte[] key, byte[] inBuffer, int inOffset, int inCount, byte[] outBuffer, int outOffset, int outCount)
        {
            int retVal;
            using (ICryptoTransform trans = CreateTransform(source, mode, key))
            {
                retVal = Transform(trans, inBuffer, inOffset, inCount, outBuffer, outOffset, outCount);
            }
            return retVal;
        }

        public static byte[] Transform(this ICryptoTransform source, byte[] buffer, int offset, int count)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }
            int bufferLength = buffer.Length;
            if (offset < 0 ||
                offset >= bufferLength)
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if (count < 0 ||
                offset + count > bufferLength)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            int outputBlockSize = source.OutputBlockSize;
            byte[] retVal = new byte[count + outputBlockSize - count % outputBlockSize];
            int length = Transform(source, buffer, offset, count, retVal, 0, retVal.Length);
            if (length != retVal.Length)
            {
                Array.Resize(ref retVal, length);
            }
            return retVal;
        }

        public static int Transform(this ICryptoTransform source, byte[] inBuffer, int inOffset, int inCount, byte[] outBuffer, int outOffset, int outCount)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (inBuffer == null)
            {
                throw new ArgumentNullException("inBuffer");
            }
            int inBufferLength = inBuffer.Length;
            if (inOffset < 0 ||
                inOffset >= inBufferLength)
            {
                throw new ArgumentOutOfRangeException("inOffset");
            }
            if (inCount < 0 ||
                inOffset + inCount > inBufferLength)
            {
                throw new ArgumentOutOfRangeException("inCount");
            }
            if (outBuffer == null)
            {
                throw new ArgumentNullException("outBuffer");
            }
            int outBufferLength = outBuffer.Length;
            if (outOffset < 0 ||
                outOffset >= outBufferLength)
            {
                throw new ArgumentOutOfRangeException("outOffset");
            }
            if (outCount < 0 ||
                outOffset + outCount > outBufferLength)
            {
                throw new ArgumentOutOfRangeException("outCount");
            }
            int inputBlockSize = source.InputBlockSize;
            int outputBlockSize = source.OutputBlockSize;
            int inputBlocks = (inCount + inputBlockSize - 1) / inputBlockSize;
            int outputBlocks = (outCount + outputBlockSize - 1) / outputBlockSize;
            if (inputBlocks > outputBlocks)
            {
                throw new ArgumentOutOfRangeException("inCount");
            }

            int outBytesTransformed = 0;
            int inBytesTransformed = 0;
            if (source.CanTransformMultipleBlocks)
            {
                int wholeInputBlocks = inCount / inputBlockSize;
                if (wholeInputBlocks > 0)
                {
                    outBytesTransformed = source.TransformBlock(inBuffer, inOffset, wholeInputBlocks * inputBlockSize, outBuffer, outOffset);
                    inBytesTransformed = wholeInputBlocks * inputBlockSize;
                }
            }
            else
            {
                for (; inBytesTransformed <= inCount - inputBlockSize; inBytesTransformed += inputBlockSize)
                {
                    outBytesTransformed += source.TransformBlock(inBuffer, inOffset + inBytesTransformed, inputBlockSize, outBuffer, outOffset + outBytesTransformed);
                }
            }

            int finalBlockSize = inCount - inBytesTransformed;
            byte[] finalBlock = source.TransformFinalBlock(inBuffer, inOffset + inBytesTransformed, finalBlockSize);
            Buffer.BlockCopy(finalBlock, 0, outBuffer, outOffset + outBytesTransformed, finalBlock.Length);
            outBytesTransformed += finalBlock.Length;
            return outBytesTransformed;
        }

        public static void Transform(this ICryptoTransform source, Stream input, Stream output)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }
            if (!input.CanRead)
            {
                throw new IOException("Cannot read from input stream");
            }
            if (!output.CanWrite)
            {
                throw new IOException("Cannot read from input stream");
            }
            int inCount = (int)input.Length;

            int inputBlockSize = source.InputBlockSize;
            int outputBlockSize = source.OutputBlockSize;

            byte[] inBuffer = new byte[4096];//BufferPool.Default.Allocate();
            byte[] outBuffer = new byte[4096];//BufferPool.Default.Allocate();
            //try
            //{
            int inBytesTransformed = 0;
            int maxOutputBufferBlocks = outBuffer.Length / outputBlockSize;
            int maxInputBufferBlocks = Math.Max(inBuffer.Length / inputBlockSize, maxOutputBufferBlocks - 1);
            int maxBlocks = source.CanTransformMultipleBlocks
                ? maxInputBufferBlocks
                : 1;

            int inputStreamBlocks;
            while ((inputStreamBlocks = (inCount - inBytesTransformed) / inputBlockSize) > 0)
            {
                int inputBlocks = Math.Min(inputStreamBlocks, maxBlocks);
                int currentInputSize = inputBlocks * inputBlockSize;

                //read input to inBuffer
                int bytesRead = input.Read(inBuffer, 0, currentInputSize);
                Debug.Assert(bytesRead == currentInputSize);
                //transform inBuffer to outBuffer
                int currentOutputSize = source.TransformBlock(inBuffer, 0, currentInputSize, outBuffer, 0);
                //write outBuffer to output
                output.Write(outBuffer, 0, currentOutputSize);

                inBytesTransformed += currentInputSize;
                //outBytesTransformed += currentOutputSize;
            }

            int finalBlockSize = inCount - inBytesTransformed;
            if (finalBlockSize > 0)
            {
                //read input to inBuffer
                int finalBytesRead = input.Read(inBuffer, 0, finalBlockSize);
                Debug.Assert(finalBytesRead == finalBlockSize);
            }

            byte[] finalBlock = source.TransformFinalBlock(inBuffer, 0, finalBlockSize);
            //write finalBlock to output
            output.Write(finalBlock, 0, finalBlock.Length);
            output.Flush();
            //}
            //finally
            //{
            //    BufferPool.Default.Release(inBuffer);
            //    BufferPool.Default.Release(outBuffer);
            //}
        }

        public static int Transform(this ICryptoTransform source, Stream input, byte[] outBuffer, int outOffset, int outCount)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            if (!input.CanRead)
            {
                throw new ArgumentException("Cannot read from stream", "input");
            }

            int inCount = (int)input.Length;

            int inputBlockSize = source.InputBlockSize;
            int outputBlockSize = source.OutputBlockSize;

            int inputBlocks = (inCount + inputBlockSize - 1) / inputBlockSize;
            int outputBlocks = (outCount + outputBlockSize - 1) / outputBlockSize;

            if (inputBlocks >= outputBlocks)
            {
                throw new ArgumentOutOfRangeException("outCount");
            }

            byte[] inBuffer = new byte[4096];//BufferPool.Default.Allocate();
            int outBytesTransformed = 0;
            //try
            //{
            int inBytesTransformed = 0;
            int wholeInputBufferBlocks = inBuffer.Length / inputBlockSize;
            int maxBlocks = source.CanTransformMultipleBlocks
                ? wholeInputBufferBlocks
                : 1;

            int wholeInputStreamBlocks;
            while ((wholeInputStreamBlocks = (inCount - inBytesTransformed) / inputBlockSize) > 0)
            {
                int wholeInputBlocks = Math.Min(wholeInputStreamBlocks, maxBlocks);
                int currentInputSize = wholeInputBlocks * inputBlockSize;

                //read input to inBuffer
                int bytesRead = input.Read(inBuffer, 0, currentInputSize);
                Debug.Assert(bytesRead == currentInputSize);
                //transform inBuffer to outBuffer
                int currentOutputSize = source.TransformBlock(inBuffer, 0, currentInputSize, outBuffer, outBytesTransformed);

                inBytesTransformed += currentInputSize;
                outBytesTransformed += currentOutputSize;
            }

            int finalBlockSize = inCount - inBytesTransformed;
            if (finalBlockSize > 0)
            {
                //read input to inBuffer
                int finalBytesRead = input.Read(inBuffer, 0, finalBlockSize);
                Debug.Assert(finalBytesRead == finalBlockSize);
            }

            byte[] finalBlock = source.TransformFinalBlock(inBuffer, 0, finalBlockSize);
            //write finalBlock to outBuffer
            Buffer.BlockCopy(finalBlock, 0, outBuffer, outBytesTransformed, finalBlock.Length);
            outBytesTransformed += finalBlock.Length;
            //}
            //finally
            //{
            //    BufferPool.Default.Release(inBuffer);
            //}
            return outBytesTransformed;
        }

        public static void Transform(this ICryptoTransform source, byte[] inBuffer, int inOffset, int inCount, Stream output)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (inBuffer == null)
            {
                throw new ArgumentNullException("inBuffer");
            }
            int inBufferLength = inBuffer.Length;
            if (inOffset < 0 ||
                inOffset >= inBufferLength)
            {
                throw new ArgumentOutOfRangeException("inOffset");
            }
            if (inCount < 0 ||
                inOffset + inCount > inBufferLength)
            {
                throw new ArgumentOutOfRangeException("inCount");
            }

            if (!output.CanWrite)
            {
                throw new ArgumentException("Cannot write to stream", "output");
            }

            int inputBlockSize = source.InputBlockSize;
            int outputBlockSize = source.OutputBlockSize;

            byte[] outBuffer = new byte[4096];//BufferPool.Default.Allocate();
            //int outBytesTransformed = 0;
            //try
            //{
            int inBytesTransformed = 0;
            //only transform enough input blocks to fill the output buffer so we don't overflow the output buffer
            int targetInputBufferBlocks = (outBuffer.Length / outputBlockSize) - 1;

            int maxBlocks = source.CanTransformMultipleBlocks
                ? targetInputBufferBlocks
                : 1;

            int wholeInputBufferBlocks;
            while ((wholeInputBufferBlocks = (inCount - inBytesTransformed) / inputBlockSize) > 0)
            {
                int wholeInputBlocks = Math.Min(wholeInputBufferBlocks, maxBlocks);
                int currentInputSize = wholeInputBlocks * inputBlockSize;

                //transform inBuffer to outBuffer
                int currentOutputSize = source.TransformBlock(inBuffer, inBytesTransformed, currentInputSize, outBuffer, 0);

                //write outBuffer to output
                output.Write(outBuffer, 0, currentOutputSize);

                inBytesTransformed += currentInputSize;
                //outBytesTransformed += currentOutputSize;
            }

            int finalBlockSize = inCount - inBytesTransformed;
            byte[] finalBlock = source.TransformFinalBlock(inBuffer, 0, finalBlockSize);
            //write finalBlock to output
            output.Write(finalBlock, 0, finalBlock.Length);
            //outBytesTransformed += finalBlock.Length;
            output.Flush();
            //}
            //finally
            //{
            //    BufferPool.Default.Release(outBuffer);
            //}
        }

        #endregion
    }
}