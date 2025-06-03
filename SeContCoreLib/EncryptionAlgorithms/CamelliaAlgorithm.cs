using Org.BouncyCastle.Crypto.Engines;

namespace SeContCoreLib.EncryptionAlgorithms
{
    [EncryptionAlgorithm(6, 6)]
    public class CamelliaAlgorithm : EncryptionAlgorithm
    {
        private CamelliaEngine _camellia = new CamelliaEngine();
        public override string Name => "Camellia";

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
            return EncryptionAlgorithmUtils.SymmetricAlgorithmEncryptBC(_camellia, key, IV, data);
        }

        public override byte[] Decrypt(byte[] key, byte[] data)
        {
            return EncryptionAlgorithmUtils.SymmetricAlgorithmDecryptBC(_camellia, key, IV, data);
        }

        public override void GenerateIV()
        {
            _iv = EncryptionAlgorithmUtils.GenerateIVForBC(_camellia);
        }
    }
}
