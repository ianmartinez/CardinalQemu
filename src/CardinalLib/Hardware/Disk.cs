using System.Collections.Generic;
using System.IO;
using CardinalLib.Core;
using CardinalLib.Qemu;

namespace CardinalLib.Hardware
{
    public class Disk
    {
        private const string imageInfoApp = "qemu-img";

        public FileInfo File { get; }
        public bool Exists => File.Exists;
        public string Name => File.Name;
        public string AbsolutePath => File.FullName;
        public string NameWithoutExt => Path.GetFileNameWithoutExtension(File.Name);
        public string Drive { get; set; }
        public StorageInfo Info { get; }

        public Disk(string fileName, string drive = "")
        { 
            Drive = drive;

            // Get the disk info app
            var diskInfo = QemuApps.Get(imageInfoApp);

            // Throw exception if the qemu-img app can't be found
            if (diskInfo == null)
                throw new FileNotFoundException("Cannot find the qemu-img app", imageInfoApp);

            // Assign the file object
            File = new FileInfo(fileName);

            // Check if the disk exists
            if (Exists)
            {
                // Get the results from the app
                var results = diskInfo.Run(new[] {
                    "info",
                    string.Format("\"{0}\"", File.FullName)
                });

                if(results.IsValid)
                {
                    var infoDictionary = new QemuDictionaryResponse(results.Output);
                    var diskSize = infoDictionary["disk size"];
                    var virtualSize = infoDictionary["virtual size"];

                    Info = new StorageInfo(ByteValue.Parse(diskSize), ByteValue.Parse(virtualSize));
                }
            }
            else
            {
                // Set the size and capacity to 0 if the disk isn't found
                Info = new StorageInfo(new ByteValue(0, ByteFormat.B), new ByteValue(0, ByteFormat.B));
            }
        }

        /// <summary>
        /// Get an array of disks in the ~/CardinalMachines/Disks directory
        /// </summary>
        /// 
        /// <returns>An array of disks in the disks directory</returns>
        public static Disk[] GetAll()
        {
            List<Disk> disks = new List<Disk>();

            // Create a new disk for each file
            foreach (var file in Directory.EnumerateFiles(Directories.Disks))
            {
                var fileInfo = new FileInfo(file);

                // Ignore hidden files like ".DS_STORE"
                if(fileInfo.Attributes != FileAttributes.Hidden)
                {
                    var newDisk = new Disk(file);
                    disks.Add(newDisk);
                }
            }

            // Sort alphabetically
            disks.Sort((diskA, diskB) => diskA.Name.CompareTo(diskB.Name));

            return disks.ToArray();
        }
    }
}