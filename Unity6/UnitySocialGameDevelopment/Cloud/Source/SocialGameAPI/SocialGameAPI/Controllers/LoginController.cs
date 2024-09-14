using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace SocialGameAPI.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var clientMasterVersion = request.ClientMasterVersion;
            var userId = request.UserId;

            // マスターデータチェック
            //if (!MasterDataService.CheckMasterDataVersion(clientMasterVersion))
            //{
            //    return StatusCode(400, _configuration.GetValue<string>("Error:ERROR_MASTER_DATA_UPDATE"));
            //}

            // user_profileテーブルのレコードを取得
            //var userProfile = GetUserProfile(userId);
            //if (userProfile == null)
            //{
            //    return StatusCode(400, _configuration.GetValue<string>("Error:ERROR_INVALID_DATA"));
            //}

            // ログインボーナステーブルのレコードを取得
            //var userLogin = GetUserLogin(userId);
            //if (userLogin == null)
            //{
            //    //userLogin = new UserLogin
            //    //{
            //    //    UserId = userId,
            //    //    LoginDay = 0,
            //    //    LastLoginAt = new DateTime(2000, 1, 1)
            //    //};
            //}

            // 日付の比較
            var today = DateTime.UtcNow.Date;
            //var lastLoginDay = userLogin.LastLoginAt.Date;

            //if (today != lastLoginDay)
            //{
            //    userLogin.LoginDay += 1;
                // var masterLoginItem = MasterLoginItem.GetMasterLoginItemByLoginDay(userLogin.LoginDay);

                // アイテムデータがあるか確認
                //if (masterLoginItem != null)
                //{
                //    // アイテム付与
                //    switch (masterLoginItem.ItemType)
                //    {
                //        case Constants.ITEM_TYPE_CRYSTAL:
                //            userProfile.Crystal += masterLoginItem.ItemCount;
                //            break;
                //        case Constants.ITEM_TYPE_CRYSTAL_FREE:
                //            userProfile.CrystalFree += masterLoginItem.ItemCount;
                //            break;
                //        case Constants.ITEM_TYPE_FRIEND_COIN:
                //            userProfile.FriendCoin += masterLoginItem.ItemCount;
                //            break;
                //        default:
                //            break;
                //    }
                //}
            //}

            // ログイン時刻の更新
            // userLogin.LastLoginAt = DateTime.UtcNow;

            // データの書き込み
            try
            {
                //SaveUserProfile(userProfile);
                //SaveUserLogin(userLogin);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return StatusCode(500, _configuration.GetValue<string>("Error:ERROR_DB_UPDATE"));
            }

            // クライアントへのレスポンス
            //userProfile = GetUserProfile(userId);
            //userLogin = GetUserLogin(userId);

            var response = new
            {
                //UserProfile = userProfile,
                //UserLogin = userLogin
            };

            return Json(response);
        }

        //private UserProfile GetUserProfile(string userId)
        //{
        //    using (var context = new YourDbContext())
        //    {
        //        return context.UserProfiles.FirstOrDefault(up => up.UserId == userId);
        //    }
        //}

        //private UserLogin GetUserLogin(string userId)
        //{
        //    using (var context = new YourDbContext())
        //    {
        //        return context.UserLogins.FirstOrDefault(ul => ul.UserId == userId);
        //    }
        //}

        //private void SaveUserProfile(UserProfile userProfile)
        //{
        //    using (var context = new YourDbContext())
        //    {
        //        context.UserProfiles.Update(userProfile);
        //        context.SaveChanges();
        //    }
        //}

        //private void SaveUserLogin(UserLogin userLogin)
        //{
        //    using (var context = new YourDbContext())
        //    {
        //        context.UserLogins.Update(userLogin);
        //        context.SaveChanges();
        //    }
        //}
    }
    public class LoginRequest
    {
        public string ClientMasterVersion { get; set; }
        public string UserId { get; set; }
    }
}
