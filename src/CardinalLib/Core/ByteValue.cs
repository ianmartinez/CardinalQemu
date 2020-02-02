using System;

namespace CardinalLib.Core
{
    /// <summary>
    /// Handles the conversion between bytes, kilobytes, megabytes, and 
    /// so on... Can be any type of base (1024/1000)
    /// </summary>
    public class ByteValue
    {
        /// <summary>
        /// Create new ByteValue, but default to base 1024
        /// </summary>
        /// 
        /// <param name="value">The number value in the specified format</param>
        /// <param name="initialFormat">The initial format to parse the value as</param>
        /// <param name="numBase">he base for calculations, i.e. a megabyte is base 1000 but a mebibyte is 1024</param>
        public ByteValue(double value, ByteFormat initialFormat, int numBase = 1024)
        {
            Base = numBase;

            // Convert everything to bytes for ease of calculation
            switch (initialFormat)
            {
                case ByteFormat.B:
                    Bytes = value;
                    break;
                case ByteFormat.KB:
                    Bytes = value * Math.Pow(numBase, 1);
                    break;
                case ByteFormat.MB:
                    Bytes = value * Math.Pow(numBase, 2);
                    break;
                case ByteFormat.GB:
                    Bytes = value * Math.Pow(numBase, 3);
                    break;
                case ByteFormat.TB:
                    Bytes = value * Math.Pow(numBase, 4);
                    break;
            }
        }

        /// <summary>
        /// The number base (1000/1024)
        /// </summary>
        public int Base { get; set; }

        /// <summary>
        /// Get the value as bytes
        /// </summary>
        public double Bytes { get; }

        /// <summary>
        /// Get value represented as kilobytes(base 1000)/kibibytes(base 1024)
        /// </summary>
        public double Kilobytes => Bytes / Math.Pow(Base, 1);

        /// <summary>
        /// Get the value represented as megabytes(base 1000)/mebibytes(base 1024)
        /// </summary>
        public double Megabytes => Bytes / Math.Pow(Base, 2);

        /// <summary>
        /// Get the value represented as gigabytes(base 1000)/gibibytes(base 1024)
        /// </summary>
        public double Gigabytes => Bytes / Math.Pow(Base, 3);

        /// <summary>
        /// Get the value represented as terabytes(base 1000)/tebibytes(base 1024)
        /// </summary>
        public double Terabytes => Bytes / Math.Pow(Base, 4);

        /// <summary>
        ///  Gets the largest format needed to represent the current
        ///  ByteValue as a number equal to or greater than 1.
        ///  
        ///  So a ByteValue for 0.97 GB would return MB because the largest
        ///  whole format is megabytes (i.e. 970 MB).
        /// </summary>
        public ByteFormat LargestFormat
        {
            get
            {
                if (Terabytes >= 1)
                    return ByteFormat.TB;
                else if (Gigabytes >= 1)
                    return ByteFormat.GB;
                else if (Megabytes >= 1)
                    return ByteFormat.MB;
                else if (Kilobytes >= 1)
                    return ByteFormat.KB;

                return ByteFormat.B;
            }
        }

        /// <summary>
        /// Parse a string, such as "2 MB" and return a ByteValue that
        /// represents that value
        /// </summary>
        /// 
        /// <param name="value">The string to be parsed</param>
        /// 
        /// <returns>A ByteValue created from the parsed string, null if unparsable</returns>
        public static ByteValue Parse(string value)
        {
            string[] values = value.Split(' ');

            if (values.Length >= 2)
            {
                string number = values[0].ToLower();
                double numberValue = Double.Parse(number);
                string format = values[1].ToLower();
                ByteFormat formatValue = ByteFormat.B;
                int baseNumber = 1000;

                // Something like KiB or GiB
                if (format[1] == 'i')
                    baseNumber = 1024;

                switch (format)
                {
                    case "kb":
                    case "kib":
                        formatValue = ByteFormat.KB;
                        break;
                    case "mb":
                    case "mib":
                        formatValue = ByteFormat.MB;
                        break;
                    case "gb":
                    case "gib":
                        formatValue = ByteFormat.GB;
                        break;
                    case "tb":
                    case "tib":
                        formatValue = ByteFormat.TB;
                        break;
                }

                return new ByteValue(numberValue, formatValue, baseNumber);
            }

            return null;
        }

        /// <summary>
        /// Get the value in a given ByteFormat
        /// </summary>
        /// 
        /// <param name="format">The target format</param>
        /// 
        /// <returns>The value in the given ByteFormat</returns>
        public double Get(ByteFormat format)
        {
            return format switch
            {
                ByteFormat.B => Bytes,
                ByteFormat.KB => Kilobytes,
                ByteFormat.MB => Megabytes,
                ByteFormat.GB => Gigabytes,
                ByteFormat.TB => Terabytes,
                _ => -1,
            };
        }

        /// <summary>
        /// Same as the get() function, but round the value to a
        /// given number of digits
        /// </summary>
        /// 
        /// <param name="format">The target ByteFormat</param>
        /// <param name="digits">The number of digits to round to</param>
        /// 
        /// <returns>The rounded value in the given byte format</returns>
        public double GetRounded(ByteFormat format, int digits)
        {
            return Math.Round(Get(format), digits);
        }

        /// <summary>
        /// Get the percentage rounded to the nearest int
        /// </summary>
        /// 
        /// <param name="otherValue">The other value to compare to</param>
        /// 
        /// <returns>The percentage rounded to the nearest int</returns>
        public int GetPercentInt(ByteValue otherValue)
        {
            return (int)Math.Round(GetPercentageOf(otherValue), MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Get the larger format between the two ByteValues
        /// that will make it easier to compare them with
        /// </summary>
        /// 
        /// <param name="other">The other value to compare to</param>
        /// 
        /// <returns>The common ByteFormat</returns>
        public ByteFormat GetCommomFormatWith(ByteValue other)
        {
            return (other.Bytes > Bytes) ? other.LargestFormat : LargestFormat;
        }

        /// <summary>
        /// Get the percentage, rounded to two decimal points of the current
        /// ByteValue compared to another ByteValue
        /// </summary>
        /// 
        /// <param name="otherValue">The other value to compare to</param>
        /// <param name="digits">The number of digits to round to</param>
        /// 
        /// <returns>The percentage that this value is of the other value, rounded</returns>
        public double GetPercentageOf(ByteValue otherValue, int digits = 2)
        {
            return Math.Round(Bytes / otherValue.Bytes * 100, digits);
        }

        /// <summary>
        /// Calls the static Format() function, passing this.Bytes as the variable
        /// for value and defaulting the numBase to the Base for this ByteValue.
        /// </summary>
        /// 
        /// <param name="format">The format needed</param>
        /// <param name="numBase">Optional, defaults to the current Base</param>
        /// 
        /// <returns>The formatted value</returns>
        public string Format(ByteFormat format, int numBase = -1)
        {
            return Format(Get(format), format, (numBase == -1) ? Base : numBase);
        }

        /// <summary>
        /// Formats a value to a string, such as 10 MB
        /// </summary>
        /// 
        /// <param name="value">The value</param>
        /// <param name="numBase">The base for calculations</param>
        /// <param name="format">The format needed for calculations</param>
        /// 
        /// <returns>The formatted value</returns>
        public static string Format(double value, ByteFormat format, int numBase = 1024)
        {
            string formatted = value.ToString() + " ";

            switch (format)
            {
                case ByteFormat.B:
                    formatted += "B";
                    break;
                case ByteFormat.KB:
                    formatted += "K";
                    break;
                case ByteFormat.MB:
                    formatted += "M";
                    break;
                case ByteFormat.GB:
                    formatted += "G";
                    break;
                case ByteFormat.TB:
                    formatted += "T";
                    break;
            }

            return formatted + ((format != ByteFormat.B && numBase == 1024) ? "iB" : "B");
        }

        /// <summary>
        /// Add two ByteValues together
        /// </summary>
        /// 
        /// <returns>A new ByteValue with the two values added together</returns>
        public static ByteValue operator +(ByteValue a, ByteValue b)
        {
            return new ByteValue(a.Bytes + b.Bytes, ByteFormat.B);
        }

        /// <summary>
        /// Subtract one ByteValue from another
        /// </summary>
        /// 
        /// <returns>A new ByteValue with ByteValue b subtracted from ByteValue a</returns>
        public static ByteValue operator -(ByteValue a, ByteValue b)
        {
            return new ByteValue(a.Bytes - b.Bytes, ByteFormat.B);
        }
    }
}