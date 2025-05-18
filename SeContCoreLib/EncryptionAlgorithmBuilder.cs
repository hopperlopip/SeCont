namespace SeContCoreLib
{
    public class EncryptionAlgorithmBuilder
    {
        private readonly Type _encryptionAlgorithmType;

        internal EncryptionAlgorithmBuilder(Type encryptionAlgorithmType)
        {
            _encryptionAlgorithmType = encryptionAlgorithmType;
        }

        public EncryptionAlgorithm? CreateEncryptionAlgorithm()
        {
            return (EncryptionAlgorithm?)Activator.CreateInstance(_encryptionAlgorithmType);
        }
    }
}
