using OpenGost.Security.Cryptography;
using System.Security.Cryptography;

namespace SeContCoreLib.EncryptionAlgorithms
{
    [EncryptionAlgorithm(1, 1)]
    public class GrasshopperAlgorithm : EncryptionAlgorithm
    {
        private Grasshopper _grasshopper = Grasshopper.Create();
        public override string Name => "Grasshopper (Кузнечик)";

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
            return EncryptionAlgorithmUtils.SymmetricAlgorithmEncrypt(_grasshopper, key, IV, data, CipherMode.CBC);
        }

        public override byte[] Decrypt(byte[] key, byte[] data)
        {
            return EncryptionAlgorithmUtils.SymmetricAlgorithmDecrypt(_grasshopper, key, IV, data, CipherMode.CBC);
        }

        public override void GenerateIV()
        {
            _grasshopper.GenerateIV();
            _iv = _grasshopper.IV;
        }
    }
}
