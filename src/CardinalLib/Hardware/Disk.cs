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
        public string NameWithoutExt => Path.GetFileNameWithoutExtension(File.Name);
        public string Drive { get; set; }
        public StorageInfo Info { get; set; }

        public Disk(string fileName, string drive)
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

                // TODO - Parse output
            }
            else
            {
                // Set the size and capacity to 0 if the disk isn't found
                Info = new StorageInfo(new ByteValue(0, ByteFormat.B), new ByteValue(0, ByteFormat.B));
            }
        }
    }
}
