using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeContCoreLib
{
    internal class EncryptionAlgorithmAttribute : Attribute
    {
        public long ID { get; }
        public int Index { get; } = -1;

        public EncryptionAlgorithmAttribute(long Id) => ID = Id;

        public EncryptionAlgorithmAttribute(long Id, int index)
        {
            ID = Id;
            Index = index;
        }
    }
}
