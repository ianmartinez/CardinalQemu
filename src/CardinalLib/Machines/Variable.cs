namespace CardinalLib.Machines
{
    /// <summary>
    /// Handle variables in machine.xml files,
    /// such as:
    /// <para>{$HOME} - the user's home directory</para>
    /// <para>{$MACHINES} - the user's ~/CardinalMachines directory</para>
    /// <para>{$DISKS} - the user's ~/CardinalMachines/Disks directory</para>
    /// <para>{$CURRENT} - the current machine's folder</para>
    /// </summary>
    public static class Variable
    {
        /// <summary>
        /// Replace all instances of a variable in a string
        /// </summary>
        /// 
        /// <param name="text">The text to parse</param>
        /// <param name="variableName">The variable to be replaced, excluding the brackets, so
        /// for {$VARIABLE_NAME} use VARIABLE_NAME</param>
        /// <param name="replacement">The value to replace the variable with</param>
        /// <param name="isDirectory">If the value that is being replaced represents a directory</param>
        /// 
        /// <returns>The string, with all instances of the variable replaced by the value</returns>
        public static string Replace(string text, string variableName, string replacement, bool isDirectory)
        {
            string formattedReplacement = replacement;

            // If it's a directory, remove the final backslash
            if (isDirectory && replacement.EndsWith("/"))
                formattedReplacement = formattedReplacement.Substring(0, formattedReplacement.Length - 2);

            return text.Replace("{$" + variableName + "}", formattedReplacement);
        }

        /// <summary>
        /// Replace all instances of matching values in a text with a variable
        /// counterpart
        /// </summary>
        /// 
        /// <param name="text">The text to searh</param>
        /// <param name="variableName">The variable name to replace the old value with</param>
        /// <param name="oldValue">The old value to replace</param>
        /// 
        /// <returns>The string, with all instances of the value replaced by the variable</returns>
        public static string Reverse(string text, string variableName, string oldValue)
        {
            return text.Replace(oldValue, "{$" + variableName + "}");
        }
    }
}
