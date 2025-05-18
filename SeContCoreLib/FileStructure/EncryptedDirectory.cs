namespace SeContCoreLib.FileStructure
{
    public class EncryptedDirectory : EncryptedObject, IEncryptedStorage
    {
        public string Name { get; set; }

        public override EncryptedObjectType ObjectType => EncryptedObjectType.EncryptedDirectory;

        private List<EncryptedObject> _encryptedObjects = new();

        public IReadOnlyList<EncryptedObject> EncryptedObjects { get => _encryptedObjects; }

        public EncryptedDirectory(string name) => Name = name;

        private EncryptedDirectory(string name, EncryptedDirectory[] encryptedDirectories, EncryptedFile[] encryptedFiles) : this(name)
        {
            AddEncryptedDirectories(encryptedDirectories);
            AddEncryptedFiles(encryptedFiles);
        }

        #region Files
        public void AddEncryptedFile(EncryptedFile encryptedFile)
        {
            _encryptedObjects.Add(encryptedFile);
            encryptedFile.ParentStorage = this;
        }

        public void AddEncryptedFiles(IEnumerable<EncryptedFile> encryptedFiles)
        {
            foreach (var encryptedFile in encryptedFiles)
            {
                AddEncryptedFile(encryptedFile);
            }
        }

        public void RemoveEncryptedFile(EncryptedFile encryptedFile)
        {
            _encryptedObjects.Remove(encryptedFile);
        }

        public void RemoveEncryptedFiles(IEnumerable<EncryptedFile> encryptedFiles)
        {
            foreach (var encryptedFile in encryptedFiles)
            {
                RemoveEncryptedFile(encryptedFile);
            }
        }

        public EncryptedFile[] GetEncryptedFiles()
        {
            return (EncryptedFile[])(_encryptedObjects.Where((encryptedObject) => { return encryptedObject is EncryptedFile; }).Cast<EncryptedFile>().ToArray());
        }
        #endregion

        #region Directories
        public void AddEncryptedDirectory(EncryptedDirectory encryptedDirectory)
        {
            _encryptedObjects.Add(encryptedDirectory);
            encryptedDirectory.ParentStorage = this;
        }

        public void AddEncryptedDirectories(IEnumerable<EncryptedDirectory> encryptedDirectories)
        {
            foreach (var encryptedDirectory in encryptedDirectories)
            {
                AddEncryptedDirectory(encryptedDirectory);
            }
        }

        public void RemoveEncryptedDirectory(EncryptedDirectory encryptedDirectory)
        {
            _encryptedObjects.Remove(encryptedDirectory);
        }

        public void RemoveEncryptedDirectories(IEnumerable<EncryptedDirectory> encryptedDirectories)
        {
            foreach (var encryptedDirectory in encryptedDirectories)
            {
                RemoveEncryptedDirectory(encryptedDirectory);
            }
        }

        public EncryptedDirectory[] GetEncryptedDirectories()
        {
            return (EncryptedDirectory[])(_encryptedObjects.Where((encryptedObject) => { return encryptedObject is EncryptedDirectory; }).Cast<EncryptedDirectory>().ToArray());
        }
        #endregion

        /// <summary>
        /// Removes encrypted directory.
        /// </summary>
        public void Remove()
        {
            if (ParentStorage == null)
                return;
            ParentStorage.RemoveEncryptedDirectory(this);
        }

        #region Serialization/Deserialization

        public override byte[] Serialize()
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(memoryStream);

            writer.Write(Name);
            var directories = GetEncryptedDirectories();
            writer.Write(directories.Length);
            foreach (var directory in directories)
            {
                byte[] directoryData = directory.Serialize();
                writer.Write(directoryData.Length);
                writer.Write(directoryData);
            }
            var files = GetEncryptedFiles();
            writer.Write(files.Length);
            foreach (var file in files)
            {
                byte[] fileData = file.Serialize();
                writer.Write(fileData.Length);
                writer.Write(fileData);
            }

            return memoryStream.ToArray();
        }

        public static EncryptedDirectory Deserialize(byte[] data)
        {
            MemoryStream memoryStream = new MemoryStream(data);
            BinaryReader reader = new BinaryReader(memoryStream);

            string name = reader.ReadString();
            int directoriesLength = reader.ReadInt32();
            EncryptedDirectory[] directories = new EncryptedDirectory[directoriesLength];
            for (int i = 0; i < directoriesLength; i++)
            {
                int directoryDataLength = reader.ReadInt32();
                var directoryData = reader.ReadBytes(directoryDataLength);
                directories[i] = Deserialize(directoryData);
            }
            int filesLength = reader.ReadInt32();
            EncryptedFile[] files = new EncryptedFile[filesLength];
            for (int i = 0; i < filesLength; i++)
            {
                int fileDataLength = reader.ReadInt32();
                byte[] fileData = reader.ReadBytes(fileDataLength);
                files[i] = EncryptedFile.Deserialize(fileData);
            }

            return new EncryptedDirectory(name, directories, files);
        }

        #endregion
    }
}
