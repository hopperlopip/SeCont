using OpenGost.Security.Cryptography;
using System.Security.Cryptography;

namespace SeContCoreLib.EncryptionAlgorithms
{
    [EncryptionAlgorithm(2, 2)]
    public class MagmaAlgorithm : EncryptionAlgorithm
    {
        private Magma _magma = Magma.Create();
        public override string Name => "Magma (Магма)";

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
            return EncryptionAlgorithmUtils.SymmetricAlgorithmEncrypt(_magma, key, IV, data, CipherMode.CBC);
        }

        public override byte[] Decrypt(byte[] key, byte[] data)
        {
            return EncryptionAlgorithmUtils.SymmetricAlgorithmDecrypt(_magma, key, IV, data, CipherMode.CBC);
        }

        public override void GenerateIV()
        {
            _magma.GenerateIV();
            _iv = _magma.IV;
        }
    }
}
