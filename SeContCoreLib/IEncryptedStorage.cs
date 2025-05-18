using SeContCoreLib.FileStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeContCoreLib
{
    public interface IEncryptedStorage
    {
        public void AddEncryptedFile(EncryptedFile file);
        public void AddEncryptedFiles(IEnumerable<EncryptedFile> encryptedFiles);
        public void RemoveEncryptedFile(EncryptedFile file);
        public void RemoveEncryptedFiles(IEnumerable<EncryptedFile> encryptedFiles);

        public void AddEncryptedDirectory(EncryptedDirectory directory);
        public void AddEncryptedDirectories(IEnumerable<EncryptedDirectory> directories);
        public void RemoveEncryptedDirectory(EncryptedDirectory directory);
        public void RemoveEncryptedDirectories(IEnumerable<EncryptedDirectory> directories);
    }
}
