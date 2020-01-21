using System;
using System.IO;
using CardinalLib.Core;

namespace CardinalLib.Hardware
{
    public class Disk
    {
        FileInfo File { get; }
        string Drive { get; set; }
        StorageInfo Info { get; set; }

        public Disk(string drive, string fileName)
        {
            
        }
    }
}
