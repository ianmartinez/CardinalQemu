using System;
using System.IO;

namespace CardinalLib
{
    /// <summary>
    /// Get special directories that are used throughout the program
    /// </summary>
    public static class Directories
    {
        /// <summary>
        /// The user's home folder. "~/" on Unix systems
        /// </summary>
        public static string UserHome => Environment.GetFolderPath(Environment.SpecialFolder.Personal);

        /// <summary>
        /// The folder the app is running in
        /// </summary>
        public static string App => AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// The ~/CardinalMachines folder that holds the disks folder as well as all the machines
        /// </summary>
        public static string Machines => Path.Combine(UserHome, "CardinalMachines");

        /// <summary>
        /// The disks folder that holds all the *.qcow/*.qcow2 files
        /// </summary>
        public static string Disks => Path.Combine(Machines, "Disks");
    }
}
