namespace SeContCoreLib
{
    public abstract class EncryptionAlgorithm
    {
        public abstract string Name { get; }
        public abstract byte[] IV { get; internal set; }
        public long ID { get => EncryptionAlgorithmUtils.GetId(this); }
        public abstract byte[] Encrypt(byte[] key, byte[] data);
        public abstract byte[] Decrypt(byte[] key, byte[] data);
        public abstract void GenerateIV();

        public override string ToString() => Name;
    }
}
