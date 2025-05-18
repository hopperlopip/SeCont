using SeContCoreLib.FileStructure;

namespace SeContCoreLib
{
    public abstract class EncryptedObject
    {
        public EncryptionAlgorithm? EncryptionAlgorithm { get; set; } = null;

        public IEncryptedStorage? ParentStorage { get; set; } = null;

        public abstract EncryptedObjectType ObjectType { get; }

        private static Type GetEncryptedObjectType(EncryptedObjectType type)
        {
            switch (type)
            {
                case EncryptedObjectType.EncryptedFile:
                    return typeof(EncryptedFile);
                case EncryptedObjectType.EncryptedDirectory:
                    return typeof(EncryptedDirectory);
            }
            throw new NotImplementedException();
        }

        public abstract byte[] Serialize();

        public static byte[] Serialize(object encryptedObject, EncryptedObjectType encryptedObjectType)
        {
            Type type = GetEncryptedObjectType(encryptedObjectType);

            object? data = type.GetMethod("Serialize")?.Invoke(encryptedObject, null);
            if (data == null)
                throw new Exception("Couldn't serialize encrypted object.");
            return (byte[])data;
        }

        public static EncryptedObject Deserialize(byte[] data, EncryptedObjectType encryptedObjectType)
        {
            Type type = GetEncryptedObjectType(encryptedObjectType);

            object? encryptedObject = type.GetMethod("Deserialize")?.Invoke(null, new object[] { data });
            if (encryptedObject == null)
                throw new Exception("Couldn't deserialize encrypted object.");
            return (EncryptedObject)encryptedObject;
        }
    }

    public enum EncryptedObjectType
    {
        EncryptedFile,
        EncryptedDirectory
    }
}
