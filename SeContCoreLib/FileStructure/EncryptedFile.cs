using SeContCoreLib.EncryptionAlgorithms;
using SeContCoreLib.Exceptions;
using System.Text;

namespace SeContCoreLib.FileStructure
{
    public class EncryptedFile : EncryptedObject
    {
        public string Name { get; set; }

        public override EncryptedObjectType ObjectType => EncryptedObjectType.EncryptedFile;

        private byte[] _fileData = Array.Empty<byte>();

        private byte[] _storedKeyHash = Array.Empty<byte>();

        /// <summary>
        /// Not encrypted file data.
        /// </summary>
        public byte[] FileData
        {
            get
            {
                if (EncryptedFileData.Length > 0)
                    throw new ArgumentException("Encryption Algorithm isn't null. The data is encrypted.");
                return _fileData;
            }
            set
            {
                if (EncryptedFileData.Length > 0)
                    throw new ArgumentException("Encryption Algorithm isn't null. The data should be encrypted.");
                _fileData = value;
            }
        }

        /// <summary>
        /// Encrypted file data.
        /// </summary>
        public byte[] EncryptedFileData { get; private set; } = Array.Empty<byte>();
        public bool IsDataEncrypted { get => EncryptionAlgorithm != null; }

        public EncryptedFile(string name, EncryptionAlgorithm? encryptionAlgorithm)
        {
            Name = name;
            this.EncryptionAlgorithm = encryptionAlgorithm;
        }

        private EncryptedFile(string name, EncryptionAlgorithm? encryptionAlgorithm, byte[] storedKeyHash) : this(name, encryptionAlgorithm)
        {
            _storedKeyHash = storedKeyHash;
        }

        /// <summary>
        /// Encrypts the not encrypted file data and stores the encrypted data in the <see cref="EncryptedFileData"/>.
        /// </summary>
        /// <param name="key">Encryption key</param>
        /// <param name="data">Not encrypted file data</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void EncryptFileData(byte[] key, byte[] data)
        {
            if (EncryptionAlgorithm == null)
                throw new ArgumentNullException("Encryption Algorithm is null. The data should not be encrypted.");
            EncryptedFileData = EncryptionAlgorithm.Encrypt(key, data);
            _storedKeyHash = SecurityContainer.GetSecureKeyHash(key);
        }

        /// <summary>
        /// Encrypts the not encrypted file data, which is stored in the <see cref="FileData"/>, and stores the encrypted data in the <see cref="EncryptedFileData"/>.
        /// </summary>
        /// <param name="key">Encryption key</param>
        /// <param name="encryptionAlgorithm">Encryption algorithm</param>
        /// <exception cref="InvalidOperationException"></exception>
        public void EncryptFileDataInside(byte[] key, EncryptionAlgorithm encryptionAlgorithm)
        {
            if (IsDataEncrypted)
                throw new InvalidOperationException("There is not need to encrypt this file because it was already encrypted.");

            this.EncryptionAlgorithm = encryptionAlgorithm;
            EncryptFileData(key, FileData);
            _fileData = Array.Empty<byte>();
        }

        /// <summary>
        /// Decrypts the encrypted file data, which is stored in the <see cref="EncryptedFileData"/>, and returns the decrypted data.
        /// </summary>
        /// <param name="key">Encryption key</param>
        /// <returns>Decrypted file data</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public byte[] DecryptFileData(byte[] key)
        {
            if (EncryptionAlgorithm == null)
                throw new ArgumentNullException("Encryption Algorithm is null. The data isn't encrypted.");

            byte[] keyHash = SecurityContainer.GetSecureKeyHash(key);
            bool isKeyValid = _storedKeyHash.SequenceEqual(keyHash);
            if (!isKeyValid)
                throw new KeyValidationException("The password for the file isn't correct.");

            return EncryptionAlgorithm.Decrypt(key, EncryptedFileData);
        }

        /// <summary>
        /// Decrypts the encrypted file data, which is stored in the <see cref="EncryptedFileData"/>, and stores the decrypted data in the <see cref="FileData"/>.
        /// </summary>
        /// <param name="key">Encryption key</param>
        public void DecryptFileDataInside(byte[] key)
        {
            _fileData = DecryptFileData(key);
            this.EncryptionAlgorithm = null;
            EncryptedFileData = Array.Empty<byte>();
        }

        /// <summary>
        /// Removes encrypted file.
        /// </summary>
        public void Remove()
        {
            if (ParentStorage == null)
                return;
            ParentStorage.RemoveEncryptedFile(this);
        }

        #region Serialization/Deserialization

        public override byte[] Serialize()
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(memoryStream);

            writer.Write(Name);
            writer.Write(EncryptionAlgorithm != null ? EncryptionAlgorithm.ID : -1);
            byte[] encryptionAlgorithmIV = EncryptionAlgorithm != null ? EncryptionAlgorithm.IV : Array.Empty<byte>();
            writer.Write(encryptionAlgorithmIV.Length);
            writer.Write(encryptionAlgorithmIV);
            writer.Write(_storedKeyHash.Length);
            writer.Write(_storedKeyHash);
            writer.Write(_fileData.Length);
            writer.Write(_fileData);
            writer.Write(EncryptedFileData.Length);
            writer.Write(EncryptedFileData);

            return memoryStream.ToArray();
        }

        public static EncryptedFile Deserialize(byte[] data)
        {
            MemoryStream memoryStream = new MemoryStream(data);
            BinaryReader reader = new BinaryReader(memoryStream);

            string name = reader.ReadString();
            long encryptionAlgorithmId = reader.ReadInt64();
            int encryptionAlgorithmIVLength = reader.ReadInt32();
            byte[] encryptionAlgorithmIV = reader.ReadBytes(encryptionAlgorithmIVLength);
            int storedKeyHashLength = reader.ReadInt32();
            byte[] storedKeyHash = reader.ReadBytes(storedKeyHashLength);
            int fileDataLength = reader.ReadInt32();
            byte[] fileData = reader.ReadBytes(fileDataLength);
            int encryptedFileDataLength = reader.ReadInt32();
            byte[] encryptedFileData = reader.ReadBytes(encryptedFileDataLength);

            EncryptionAlgorithm? encryptionAlgorithm = EncryptionAlgorithmUtils.GetAlgorithm(encryptionAlgorithmId);
            if (encryptionAlgorithm != null)
                encryptionAlgorithm.IV = encryptionAlgorithmIV;

            EncryptedFile encryptedFile = new EncryptedFile(name, encryptionAlgorithm, storedKeyHash);
            if (!encryptedFile.IsDataEncrypted)
                encryptedFile.FileData = fileData;
            else
                encryptedFile.EncryptedFileData = encryptedFileData;

            return encryptedFile;
        }

        #endregion
    }
}
