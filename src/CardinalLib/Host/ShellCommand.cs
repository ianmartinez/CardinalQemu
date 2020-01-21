using System;
namespace CardinalLib.Host
{
    public class ShellCommand
    {
        /// <summary>
        /// The executable to invoke
        /// </summary>
        public string Executable { get; set; }

        /// <summary>
        /// The arguments to send to the executable
        /// </summary>
        public string[] Arguments { get; set; } = new string[0];

        /// <summary>
        /// If lines that are empty should be ignored in the ShellResult
        /// </summary>
        public bool IgnoreEmptyLines { get; set; } = true;

        /// <summary>
        /// The directory to execute in
        /// </summary>
        public string WorkingDirectory { get; set; } = "";

        /// <summary>
        /// If the directory to execute is has been changed from the default
        /// </summary>
        public bool ChangeDirectory => !string.IsNullOrEmpty(WorkingDirectory);

        /// <summary>
        /// Use for when you specify the values after
        /// </summary>
        public ShellCommand() { }

        /// <summary>
        /// Create a ShellCommand with a single argument
        /// </summary>
        /// 
        /// <param name="executable">The executable to run</param>
        /// <param name="argument">The argument to send it</param>
        public ShellCommand(string executable, string argument)
        {
            Executable = executable;
            Arguments = new[] { argument };
        }

        /// <summary>
        /// Create a ShellCommand with multiple arguments
        /// </summary>
        /// 
        /// <param name="executable">The executable to run</param>
        /// <param name="arguments">The arguments to send it</param>
        public ShellCommand(string executable, string[] arguments)
        {
            Executable = executable;
            Arguments = arguments;
        }
    }
}
