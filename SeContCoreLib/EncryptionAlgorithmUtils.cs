using OpenGost.Security.Cryptography;
using Org.BouncyCastle.Asn1.Misc;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System.Reflection;
using System.Security.Cryptography;

namespace SeContCoreLib
{
    public static class EncryptionAlgorithmUtils
    {
        private static Dictionary<long, Type> EncryptionAlgorithmsDictionary { get; } = new Dictionary<long, Type>();
        //public static IReadOnlyDictionary<long, Type> EncryptionAlgorithmsReadOnlyDictionary { get => EncryptionAlgorithmsDictionary; }
        private static List<EncryptionAlgorithmBuilder> _encryptionAlgorithms = new();
        public static IReadOnlyList<EncryptionAlgorithmBuilder> EncryptionAlgorithms { get => _encryptionAlgorithms; }

        static EncryptionAlgorithmUtils()
        {
            AddEncryptionAlgorithms();
        }

        /// <summary>
        /// Gets encryption algorithm from specified ID.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>The <see cref="EncryptionAlgorithm"/> object.</returns>
        public static EncryptionAlgorithm? GetAlgorithm(long Id)
        {
            if (Id < 0)
                return null;
            if (!EncryptionAlgorithmsDictionary.ContainsKey(Id))
                return null;

            Type encryptionAlgorithmType = EncryptionAlgorithmsDictionary[Id];
            EncryptionAlgorithm? encryptionAlgorithm = (EncryptionAlgorithm?)Activator.CreateInstance(encryptionAlgorithmType);
            return encryptionAlgorithm;
        }

        /// <summary>
        /// Gets ID from specified <see cref="EncryptionAlgorithm"/> object.
        /// </summary>
        /// <param name="encryptionAlgorithm"></param>
        /// <returns>The algorithm ID. If ID is not found then -1.</returns>
        public static long GetId(EncryptionAlgorithm encryptionAlgorithm)
        {
            Type type = encryptionAlgorithm.GetType();
            foreach (object attribute in type.GetCustomAttributes(false))
            {
                if (attribute is EncryptionAlgorithmAttribute encryptionAlgorithmAttribute)
                {
                    return encryptionAlgorithmAttribute.ID;
                }
            }
            return -1;
        }

        private static void AddEncryptionAlgorithms()
        {
            List<EncryptionAlgorithmBuilder> encryptionAlgorithmBuilders = new();
            Dictionary<EncryptionAlgorithmBuilder, int> indexDictionary = new();

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    foreach (object attribute in type.GetCustomAttributes(false))
                    {
                        if (attribute is EncryptionAlgorithmAttribute encryptionAlgorithmAttribute)
                        {
                            int encryptionAlgorithmIndex = encryptionAlgorithmAttribute.Index;
                            EncryptionAlgorithmBuilder encryptionAlgorithmBuilder = new EncryptionAlgorithmBuilder(type);
                            encryptionAlgorithmBuilders.Add(encryptionAlgorithmBuilder);
                            indexDictionary.Add(encryptionAlgorithmBuilder, encryptionAlgorithmIndex);

                            EncryptionAlgorithmsDictionary.Add(encryptionAlgorithmAttribute.ID, type);
                        }
                    }
                }
            }

            EncryptionAlgorithmBuilder[] orderedEncryptionAlgorithmBuilders = new EncryptionAlgorithmBuilder[encryptionAlgorithmBuilders.Count];
            List<EncryptionAlgorithmBuilder> otherEncryptionAlgorithmBuilders = new();
            foreach (EncryptionAlgorithmBuilder encryptionAlgorithmBuilder in encryptionAlgorithmBuilders)
            {
                int encryptionAlgorithmIndex = indexDictionary[encryptionAlgorithmBuilder];

                if (encryptionAlgorithmIndex >= 0 && encryptionAlgorithmIndex < encryptionAlgorithmBuilders.Count)
                {
                    orderedEncryptionAlgorithmBuilders[encryptionAlgorithmIndex] = encryptionAlgorithmBuilder;
                }
                else
                {
                    otherEncryptionAlgorithmBuilders.Add(encryptionAlgorithmBuilder);
                }
            }
            List<EncryptionAlgorithmBuilder> finalEncryptionAlgorithmBuilders = new();
            finalEncryptionAlgorithmBuilders.AddRange(orderedEncryptionAlgorithmBuilders);
            finalEncryptionAlgorithmBuilders.RemoveAll(builder => builder == null);
            finalEncryptionAlgorithmBuilders.AddRange(otherEncryptionAlgorithmBuilders);

            _encryptionAlgorithms.AddRange(finalEncryptionAlgorithmBuilders);
        }

        #region Encryption/Decryption methods

        /// <summary>
        /// Encrypts data using basic .Net <see cref="SymmetricAlgorithm"/> class.
        /// </summary>
        /// <param name="encryptionAlgorithm"></param>
        /// <param name="key"></param>
        /// <param name="IV"></param>
        /// <param name="data"></param>
        /// <param name="cipherMode"></param>
        /// <returns></returns>
        public static byte[] SymmetricAlgorithmEncrypt(SymmetricAlgorithm encryptionAlgorithm, byte[] key, byte[] IV, byte[] data, CipherMode cipherMode = CipherMode.CBC)
        {
            encryptionAlgorithm.IV = IV;
            encryptionAlgorithm.Key = key;
            encryptionAlgorithm.Mode = cipherMode;


            ICryptoTransform encryptor = encryptionAlgorithm.CreateEncryptor(encryptionAlgorithm.Key, encryptionAlgorithm.IV);

            byte[] encryptedData = encryptor.TransformFinalBlock(data, 0, data.Length);

            encryptionAlgorithm.Dispose();

            return encryptedData;
        }

        /// <summary>
        /// Decrypts data using basic .Net <see cref="SymmetricAlgorithm"/> class.
        /// </summary>
        /// <param name="encryptionAlgorithm"></param>
        /// <param name="key"></param>
        /// <param name="IV"></param>
        /// <param name="encryptedData"></param>
        /// <param name="cipherMode"></param>
        /// <returns></returns>
        public static byte[] SymmetricAlgorithmDecrypt(SymmetricAlgorithm encryptionAlgorithm, byte[] key, byte[] IV, byte[] encryptedData, CipherMode cipherMode = CipherMode.CBC)
        {
            encryptionAlgorithm.IV = IV;
            encryptionAlgorithm.Key = key;
            encryptionAlgorithm.Mode = cipherMode;


            ICryptoTransform decryptor = encryptionAlgorithm.CreateDecryptor(encryptionAlgorithm.Key, encryptionAlgorithm.IV);

            byte[] decryptedData = decryptor.TransformFinalBlock(encryptedData, 0, encryptedData.Length);

            encryptionAlgorithm.Dispose();

            return decryptedData;
        }

        /// <summary>
        /// Encrypts data using BouncyCastle <see cref="IBlockCipher"/> interface in CBC mode.
        /// </summary>
        /// <param name="symmetricBlockCipher"></param>
        /// <param name="key"></param>
        /// <param name="IV"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] SymmetricAlgorithmEncryptBC(IBlockCipher symmetricBlockCipher, byte[] key, byte[] IV, byte[] data) // CBC Mode
        {
            KeyParameter keyParameter = new KeyParameter(key);
            ParametersWithIV keyParamWithIV = new ParametersWithIV(keyParameter, IV);

            IBlockCipherMode symmetricBlockMode = new CbcBlockCipher(symmetricBlockCipher);
            IBlockCipherPadding padding = new Pkcs7Padding();

            PaddedBufferedBlockCipher cbcCipher = new PaddedBufferedBlockCipher(symmetricBlockMode, padding);

            cbcCipher.Init(true, keyParamWithIV);
            int blockSize = cbcCipher.GetBlockSize();
            byte[] cipherData = new byte[cbcCipher.GetOutputSize(data.Length)];
            int processLength = cbcCipher.ProcessBytes(data, 0, data.Length, cipherData, 0);
            int finalLength = cbcCipher.DoFinal(cipherData, processLength);

            //byte[] finalCipherData = new byte[cipherData.Length - (blockSize - finalLength)];
            byte[] finalCipherData = new byte[cipherData.Length];
            Array.Copy(cipherData, 0, finalCipherData, 0, finalCipherData.Length);

            return finalCipherData;
        }

        /// <summary>
        /// Decrypts data using BouncyCastle <see cref="IBlockCipher"/> interface in CBC mode.
        /// </summary>
        /// <param name="symmetricBlockCipher"></param>
        /// <param name="key"></param>
        /// <param name="IV"></param>
        /// <param name="encryptedData"></param>
        /// <returns></returns>
        public static byte[] SymmetricAlgorithmDecryptBC(IBlockCipher symmetricBlockCipher, byte[] key, byte[] IV, byte[] encryptedData) // CBC Mode
        {
            KeyParameter keyParameter = new KeyParameter(key);
            ParametersWithIV keyParamWithIV = new ParametersWithIV(keyParameter, IV);

            IBlockCipherMode symmetricBlockMode = new CbcBlockCipher(symmetricBlockCipher);
            IBlockCipherPadding padding = new Pkcs7Padding();

            PaddedBufferedBlockCipher cbcCipher = new PaddedBufferedBlockCipher(symmetricBlockMode, padding);

            cbcCipher.Init(false, keyParamWithIV);
            int blockSize = cbcCipher.GetBlockSize();
            byte[] decryptedData = new byte[cbcCipher.GetOutputSize(encryptedData.Length)];
            int processLength = cbcCipher.ProcessBytes(encryptedData, 0, encryptedData.Length, decryptedData, 0);
            int finalLength = cbcCipher.DoFinal(decryptedData, processLength);
            byte[] finalDecryptedData = new byte[decryptedData.Length - (blockSize - finalLength)];
            Array.Copy(decryptedData, 0, finalDecryptedData, 0, finalDecryptedData.Length);

            return finalDecryptedData;
        }

        /// <summary>
        /// Generates IV for BouncyCastle cipher.
        /// </summary>
        /// <param name="symmetricBlockCipher"></param>
        /// <returns></returns>
        public static byte[] GenerateIVForBC(IBlockCipher symmetricBlockCipher)
        {
            int blockSize = symmetricBlockCipher.GetBlockSize();
            SecureRandom secureRandom = new SecureRandom();
            byte[] iv = new byte[blockSize];
            secureRandom.NextBytes(iv);
            return iv;
        }

        #endregion


    }
}
