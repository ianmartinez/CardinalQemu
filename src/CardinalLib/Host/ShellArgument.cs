using System.Collections.Generic;

namespace CardinalLib.Host
{
    /// <summary>
    /// Provide more fine-grained control of an argument than just raw strings
    /// </summary>
    public class ShellArgument
    {
        /// <summary>
        /// The argument name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// An array of values to come after the argument
        /// </summary>
        public List<string> Values { get; set; } = new List<string>();

        /// <summary>
        /// If the values are surrounded by quotes
        /// </summary>
        public bool HasQuotes { get; set; } = false;

        /// <summary>
        /// If the argument starts with a dash: "-arg1"
        /// </summary>
        public bool HasDash { get; set; } = true;

        /// <summary>
        /// Create an argument with an optional value string
        /// </summary>
        /// 
        /// <param name="name">The argument name</param>
        /// <param name="value">The value string</param>
        public ShellArgument(string name, string value = null)
        {
            Name = name;

            if(value != null)
                Values.Add(value);
        }

        /// <summary>
        /// Create an argument with an optional array of values
        /// </summary>
        /// 
        /// <param name="name">The argument name</param>
        /// <param name="values">The array of values</param>
        public ShellArgument(string name, IEnumerable<string> values)
        {
            Name = name;

            if (values != null)
                Values.AddRange(values);
        }

        /// <summary>
        /// Convert a set of ShellArguments to a string array that can
        /// be used by ShellCommand
        /// </summary>
        /// 
        /// <param name="args">The array of arguments</param>
        /// 
        /// <returns>A string array of arguments</returns>
        public static string[] ToArray(params ShellArgument[] args)
        {
            var argStrings = new List<string>();

            foreach (var arg in args)
            {
                var quote = (arg.HasQuotes) ? "\"" : "";
                var dash = (arg.HasDash) ? "-" : "";

                // Add the argument
                argStrings.Add(string.Format("{0}{1}", dash, arg.Name));

                // Add its values
                if (arg.Values.Count > 0)
                    argStrings.Add(string.Format("{0}{1}{0}", quote, string.Join(",", arg.Values)));
            }

            return argStrings.ToArray();
        }
    }
}