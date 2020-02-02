using System;
namespace CardinalLib.Hardware
{
    /// <summary>
    /// Represents a temporary ISO file chosen by the user
    /// </summary>
    public class DiskImage
    {
        public bool IsBootDisk { get; set; }
        public string DiskFile { get; set; }
        public bool IsFloppy { get; set; }
        public string DriveLetter => IsFloppy ? "a" : "d";
    }
}
