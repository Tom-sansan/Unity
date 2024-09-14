namespace SocialGameAPI.Models
{
    /// <summary>
    /// MasterLoginItem Class
    /// </summary>
    public class MasterLoginItem
    {
        public int LoginDay { get; set; }
        public int ItemType { get; set; }
        public int ItemCount { get; set; }

        /// <summary>
        /// Get all data
        /// </summary>
        /// <returns></returns>
        public static List<object> GetAll() =>
            new List<object>();
    }
}
