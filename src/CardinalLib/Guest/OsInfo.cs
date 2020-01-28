using System;
namespace CardinalLib.Guest
{
    public class OsInfo
    {
        public string Family { get; set; } = "Linux";
        public string Name { get; set; } = "Other";
        public string Version { get; set; } = "Generic Linux";

        public OsInfo(string family, string name, string version)
        {
            Family = family;
            Name = name;
            Version = version;
        }

        public OsInfo() { }
    }
}
