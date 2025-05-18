using OpenGost.Security.Cryptography;
using SeContCoreLib.EncryptionAlgorithms;
using SeContCoreLib.Exceptions;
using SeContCoreLib.FileStructure;
using System.Security.Cryptography;

namespace SeContCoreLib
{
    public class SecurityContainer : IEncryptedStorage
    {
        private static EncryptionAlgorithm _encryptionAlgorithm = new GrasshopperAlgorithm();
        private static HashAlgorithm _hashAlgorithm = Streebog512.Create();
        private List<EncryptedObject> _encryptedObjects = new();
        public IReadOnlyList<EncryptedObject> EncryptedObjects { get => _encryptedObjects; }

        public SecurityContainer(List<EncryptedObject> encryptedObjects)
        {
            this._encryptedObjects.AddRange(encryptedObjects);
        }

        public SecurityContainer() : this(new List<EncryptedObject>()) { }

        private SecurityContainer(ref List<EncryptedObject> encryptedObjects)
        {
            this._encryptedObjects = encryptedObjects;
        }

        private void AddEncryptedObject(EncryptedObject encryptedObject)
        {
            _encryptedObjects.Add(encryptedObject);
            encryptedObject.ParentStorage = this;
        }

        private void AddEncryptedObjects(IEnumerable<EncryptedObject> encryptedObjects)
        {
            foreach (var encryptedObject in encryptedObjects)
            {
                AddEncryptedObject(encryptedObject);
            }
        }

        private void RemoveEncryptedObject(EncryptedObject encryptedObject) => _encryptedObjects.Remove(encryptedObject);

        private void RemoveEncryptedObjects(IEnumerable<EncryptedObject> encryptedObjects)
        {
            foreach (var encryptedObject in encryptedObjects)
            {
                RemoveEncryptedObject(encryptedObject);
            }
        }

        #region Files

        public void AddEncryptedFile(EncryptedFile encryptedFile) => AddEncryptedObject(encryptedFile);

        public void AddEncryptedFiles(IEnumerable<EncryptedFile> encryptedFiles) => AddEncryptedObjects(encryptedFiles);

        public void RemoveEncryptedFile(EncryptedFile encryptedFile) => RemoveEncryptedObject(encryptedFile);

        public void RemoveEncryptedFiles(IEnumerable<EncryptedFile> encryptedFiles) => RemoveEncryptedObjects(encryptedFiles);

        #endregion

        #region Directories

        public void AddEncryptedDirectory(EncryptedDirectory encryptedDirectory) => AddEncryptedObject(encryptedDirectory);

        public void AddEncryptedDirectories(IEnumerable<EncryptedDirectory> encryptedDirectories) => AddEncryptedObjects(encryptedDirectories);

        public void RemoveEncryptedDirectory(EncryptedDirectory encryptedDirectory) => RemoveEncryptedObject(encryptedDirectory);

        public void RemoveEncryptedDirectories(IEnumerable<EncryptedDirectory> encryptedDirectories) => RemoveEncryptedObjects(encryptedDirectories);

        #endregion

        internal static byte[] GetSecureKeyHash(byte[] key)
        {
            List<byte> secureHash = new List<byte>();

            //Blake3 (512)
            byte[] blakeHashArray = new byte[64];
            Blake3.Hasher.Hash(key, blakeHashArray);
            secureHash.AddRange(blakeHashArray);

            //Streebog512
            Streebog512 streebogHash = Streebog512.Create();
            byte[] streebogHashArray = streebogHash.ComputeHash(key);
            secureHash.AddRange(streebogHashArray);

            return secureHash.ToArray();
        }

        private static byte[] EncryptContainer(byte[] key, SecurityContainer securityContainer, EncryptionAlgorithm encryptionAlgorithm)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(memoryStream);

            byte[] keyHash = GetSecureKeyHash(key);
            byte[] encryptedContainer = encryptionAlgorithm.Encrypt(key, securityContainer.Serialize());
            byte[] encryptionAlgorithmIV = encryptionAlgorithm.IV;

            writer.Write(keyHash.Length);
            writer.Write(keyHash);
            writer.Write(encryptionAlgorithmIV.Length);
            writer.Write(encryptionAlgorithmIV);
            writer.Write(encryptedContainer.Length);
            writer.Write(encryptedContainer);

            return memoryStream.ToArray();
        }

        public byte[] EncryptContainer(byte[] key)
        {
            return EncryptContainer(key, this, _encryptionAlgorithm);
        }

        private static SecurityContainer DecryptContainer(byte[] key, byte[] data, EncryptionAlgorithm encryptionAlgorithm)
        {
            MemoryStream memoryStream = new MemoryStream(data);
            BinaryReader reader = new BinaryReader(memoryStream);

            int storedKeyHashLength = reader.ReadInt32();
            byte[] storedKeyHash = reader.ReadBytes(storedKeyHashLength);
            byte[] keyHash = GetSecureKeyHash(key);
            bool isKeyValid = storedKeyHash.SequenceEqual(keyHash);
            if (!isKeyValid)
                throw new KeyValidationException("The password for the container isn't correct.");

            int encryptionAlgorithmIVLength = reader.ReadInt32();
            byte[] encryptionAlgorithmIV = reader.ReadBytes(encryptionAlgorithmIVLength);
            int encryptedContainerLength = reader.ReadInt32();
            byte[] encryptedContainer = reader.ReadBytes(encryptedContainerLength);

            encryptionAlgorithm.IV = encryptionAlgorithmIV;

            return Deserialize(encryptionAlgorithm.Decrypt(key, encryptedContainer));
        }

        public static SecurityContainer DecryptContainer(byte[] key, byte[] data)
        {
            return DecryptContainer(key, data, _encryptionAlgorithm);
        }

        #region Serialization/Deserialization

        private byte[] Serialize()
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(memoryStream);

            writer.Write(_encryptedObjects.Count);
            foreach (EncryptedObject encryptedObject in _encryptedObjects)
            {
                byte[] encryptedObjectData = Array.Empty<byte>();

                writer.Write((int)encryptedObject.ObjectType);
                encryptedObjectData = encryptedObject.Serialize();

                writer.Write(encryptedObjectData.Length);
                writer.Write(encryptedObjectData);
            }

            byte[] hash = _hashAlgorithm.ComputeHash(memoryStream.ToArray());
            writer.Write(_hashAlgorithm.HashSize / 8);
            writer.Write(hash);

            return memoryStream.ToArray();
        }

        private static SecurityContainer Deserialize(byte[] data)
        {
            MemoryStream memoryStream = new MemoryStream(data);
            BinaryReader reader = new BinaryReader(memoryStream);

            int encryptedObjectsCount = reader.ReadInt32();
            List<EncryptedObject> encryptedObjects = new();
            for (int i = 0; i < encryptedObjectsCount; i++)
            {
                int serializedEncryptedObjectType = reader.ReadInt32();
                EncryptedObjectType encryptedObjectType = (EncryptedObjectType)serializedEncryptedObjectType;

                int encryptedObjectDataLength = reader.ReadInt32();
                byte[] encryptedObjectData = reader.ReadBytes(encryptedObjectDataLength);

                EncryptedObject encryptedObject = EncryptedObject.Deserialize(encryptedObjectData, encryptedObjectType);

                encryptedObjects.Add(encryptedObject);
            }

            ArraySegment<byte> containerBytes = new ArraySegment<byte>(data, 0, (int)reader.BaseStream.Position);
            int hashSizeInBytes = reader.ReadInt32();
            byte[] storedHash = reader.ReadBytes(hashSizeInBytes);
            byte[] hash = _hashAlgorithm.ComputeHash(containerBytes.ToArray());
            bool isHashValid = storedHash.SequenceEqual(hash);
            if (!isHashValid)
                throw new HashValidationException("The container data is corrupted.");

            return new SecurityContainer(ref encryptedObjects);
        }

        #endregion
    }
}
