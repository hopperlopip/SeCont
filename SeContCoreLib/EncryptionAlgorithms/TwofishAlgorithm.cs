using Org.BouncyCastle.Crypto.Engines;

namespace SeContCoreLib.EncryptionAlgorithms
{
    [EncryptionAlgorithm(3, 3)]
    public class TwofishAlgorithm : EncryptionAlgorithm
    {
        private TwofishEngine _twofish = new TwofishEngine();
        public override string Name => "Twofish";

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
            return EncryptionAlgorithmUtils.SymmetricAlgorithmEncryptBC(_twofish, key, IV, data);
        }

        public override byte[] Decrypt(byte[] key, byte[] data)
        {
            return EncryptionAlgorithmUtils.SymmetricAlgorithmDecryptBC(_twofish, key, IV, data);
        }

        public override void GenerateIV()
        {
            _iv = EncryptionAlgorithmUtils.GenerateIVForBC(_twofish);
        }
    }
}
