using Org.BouncyCastle.Crypto.Engines;

namespace SeContCoreLib.EncryptionAlgorithms
{
    [EncryptionAlgorithm(4, 4)]
    public class ThreefishAlgorithm : EncryptionAlgorithm
    {
        private ThreefishEngine _threefish = new ThreefishEngine(256);
        public override string Name => "Threefish";

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
            return EncryptionAlgorithmUtils.SymmetricAlgorithmEncryptBC(_threefish, key, IV, data);
        }

        public override byte[] Decrypt(byte[] key, byte[] data)
        {
            return EncryptionAlgorithmUtils.SymmetricAlgorithmDecryptBC(_threefish, key, IV, data);
        }

        public override void GenerateIV()
        {
            _iv = EncryptionAlgorithmUtils.GenerateIVForBC(_threefish);
        }
    }
}
