namespace CardinalLib.Core
{
    /// <summary>
    /// Represents info regarding a disk, cd, floppy, or any storage medium,
    /// including its current size, total capacity, and the relationship
    /// between the two values
    /// </summary>
    public class StorageInfo
    {
        /// <summary>
        /// Create a new StorageInfo with a given size (the space used on a medium)
        /// and capacity (the total space available)
        /// </summary>
        /// 
        /// <param name="size"></param>
        /// <param name="capacity"></param>
        public StorageInfo(ByteValue size, ByteValue capacity)
        {
            Size = size;
            Capacity = capacity;
        }

        /// <summary>
        /// The current size of the disk
        /// </summary>
        public ByteValue Size { get; set; }

        /// <summary>
        /// The total max size of the disk
        /// </summary>
        public ByteValue Capacity { get; set; }

        /// <summary>
        /// The available space left on the disk
        /// </summary>
        public ByteValue Available => Capacity - Size;

        /// <summary>
        /// The percentage of space used out of the total capacity
        /// </summary>
        public double Percentage => Size.GetPercentageOf(Capacity);

        /// <summary>
        /// The percentage of space used out of the total capacity, rounded
        /// to the nearest int
        /// </summary>
        public int PercentageInt => Size.GetPercentInt(Capacity);

        /// <summary>
        /// Get the common format (i.e. the largest of the two formats Size and
        /// Capacity)
        /// </summary>
        public ByteFormat CommonFormat => Size.GetCommomFormatWith(Capacity);

        /// <summary>
        /// Format the size, rounded to two decimals using the common format
        /// </summary>
        public string FormattedSize =>
            ByteValue.Format(Size.GetRounded(CommonFormat, 2), CommonFormat);

        /// <summary>
        /// Format the capacity, rounded to two decimals using the common format
        /// </summary>
        public string FormattedCapacity =>
            ByteValue.Format(Capacity.GetRounded(CommonFormat, 2), CommonFormat);

        /// <summary>
        /// Format the percentage
        /// </summary>
        public string FormattedPercentage => string.Format("{0} %", Percentage);

        /// <summary>
        /// The formatted text for displaying on the machine info panel
        /// </summary>
        public string FormattedText => string.Format("{0} of {1} ({2})", FormattedSize, FormattedCapacity, FormattedPercentage);
    }
}