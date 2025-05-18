using System.Security.Cryptography;

namespace SeContCoreLib.EncryptionAlgorithms
{
    [EncryptionAlgorithm(0, 0)]
    public class AesAlgorithm : EncryptionAlgorithm
    {
        private Aes _aes = Aes.Create();
        public override string Name => "AES";

        private byte[] _iv = Array.Empty<byte>();
        public override byte[] IV
        {
            get
            {
                if (_iv.Length < 1)
                    GenerateIV();
                return _iv;
            }
            internal set
            {
                _iv = value;
            }
        }

        public override byte[] Encrypt(byte[] key, byte[] data)
        {
            return EncryptionAlgorithmUtils.SymmetricAlgorithmEncrypt(_aes, key, IV, data, CipherMode.CBC);
        }

        public override byte[] Decrypt(byte[] key, byte[] data)
        {
            return EncryptionAlgorithmUtils.SymmetricAlgorithmDecrypt(_aes, key, IV, data, CipherMode.CBC);
        }

        public override void GenerateIV()
        {
            _aes.GenerateIV();
            _iv = _aes.IV;
        }
    }
}
