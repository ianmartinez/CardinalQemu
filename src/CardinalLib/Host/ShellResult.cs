using System.Linq;

namespace CardinalLib.Host
{
    public class ShellResult
    {
        /// <summary>
        /// The stderror output of a shell command
        /// </summary>
        public string[] Errors { get; set; }

        /// <summary>
        /// The stdout output of a shell command;
        /// </summary>
        public string[] Output { get; set; }

        /// <summary>
        /// If the stdError output is anything but empty
        /// </summary>
        public bool HasErrors => (from error in Errors
                                  where !string.IsNullOrEmpty(error)
                                  select error).Count() > 0;

        /// <summary>
        /// If the stdOut output is anything but empty
        /// </summary>
        public bool HasOutput => (from output in Output
                                  where !string.IsNullOrEmpty(output)
                                  select output).Count() > 0;
        /// <summary>
        /// If there are no errors, but there is output
        /// </summary>
        public bool IsValid => !HasErrors && HasOutput;

        /// <summary>
        /// The executable that the command was sent to
        /// </summary>
        public string TargetExecutable { get; set; }

        /// <summary>
        /// The arguments that were sent to the command
        /// </summary>
        public string SentArguments { get; set; }
    }
}
