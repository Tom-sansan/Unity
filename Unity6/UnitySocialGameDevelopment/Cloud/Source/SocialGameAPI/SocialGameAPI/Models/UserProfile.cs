using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SocialGameAPI.Models
{
    [Table("UserProfile")]
    public class UserProfile
    {
        /// <summary>
        /// User Id
        /// </summary>
        [Key]
        [JsonProperty("UserId")]
        public string UserId { get; set; }
        /// <summary>
        /// User Name
        /// </summary>
        [JsonProperty("UserName")]
        public string UserName { get; set; }
        /// <summary>
        /// Crystal
        /// </summary>
        [JsonProperty("Crystal")]
        public int Crystal { get; set; }
        /// <summary>
        /// CrystalFree
        /// </summary>
        [JsonProperty("CrystalFree")]
        public int CrystalFree { get; set; }
        /// <summary>
        /// FriendCoin
        /// </summary>
        [JsonProperty("FriendCoin")]
        public int FriendCoin { get; set; }
        /// <summary>
        /// TutorialProgress
        /// </summary>
        [JsonProperty("TutorialProgress")]
        public int TutorialProgress { get; set; }
        /// <summary>
        /// タイムスタンプを無効にする
        /// </summary>
        //[NotMapped]
        //public bool Timestamps { get; set; } = false;
    }
}
