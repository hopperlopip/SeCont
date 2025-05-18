using SeContCoreLib.Exceptions;

namespace SeContCoreLib
{
    public static class SeContFile
    {
        const string SIGNATURE = "SeCont";
        const long FILE_VERSION = 1;

        public static byte[] Serialize(byte[] key, SecurityContainer container)
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(memoryStream);

            writer.Write(SIGNATURE);
            writer.Write(FILE_VERSION);
            byte[] encryptedContainer = container.EncryptContainer(key);
            writer.Write(encryptedContainer.Length);
            writer.Write(encryptedContainer);

            return memoryStream.ToArray();
        }

        public static SecurityContainer Deserialize(byte[] key, byte[] fileData)
        {
            MemoryStream memoryStream = new MemoryStream(fileData);
            BinaryReader reader = new BinaryReader(memoryStream);

            string signature = reader.ReadString();
            if (signature != SIGNATURE)
                throw new FileIsNotValidException("The file is not valid.");
            long fileVersion = reader.ReadInt64();
            if (fileVersion != FILE_VERSION)
                throw new FileIsNotValidException($"Version mismatch. Expected version {FILE_VERSION}. Got version {fileVersion}.");
            int encryptedContainerLength = reader.ReadInt32();
            byte[] encryptedContainer = reader.ReadBytes(encryptedContainerLength);
            SecurityContainer container = SecurityContainer.DecryptContainer(key, encryptedContainer);

            return container;
        }
    }
}
