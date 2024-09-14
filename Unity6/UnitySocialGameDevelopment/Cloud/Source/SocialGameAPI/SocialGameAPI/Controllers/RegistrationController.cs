using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using SocialGameAPI.Models; // UserProfileモデルの名前空間

namespace SocialGameAPI.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IConfiguration _configuration;

        public RegistrationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Registration()
        {
            // ユーザーIDの決定
            var userId = Guid.NewGuid().ToString(); // 例: 4b3403665fea6

            // 初期データの設定
            var userProfile = new UserProfile
            {
                UserId = userId,
                UserName = "user name",
                Crystal = _configuration.GetValue<int>("Constants:CRYSTAL_DEFAULT"),
                CrystalFree = _configuration.GetValue<int>("Constants:CRYSTAL_FREE_DEFAULT"),
                FriendCoin = _configuration.GetValue<int>("Constants:FRIEND_COIN_DEFAULT"),
                TutorialProgress = _configuration.GetValue<int>("Constants:TUTORIAL_START")
            };

            // データの書き込み
            try
            {
                //using (var context = new YourDbContext())
                //{
                //    context.UserProfiles.Add(userProfile);
                //    context.SaveChanges();
                //}
            }
            catch (Exception e)
            {
                // ログの記録
                Console.WriteLine(e.Message);
                return StatusCode(500, _configuration.GetValue<string>("Error:ERROR_DB_UPDATE"));
            }

            // クライアントへのレスポンス
            //using (var context = new YourDbContext())
            //{
            //    userProfile = context.UserProfiles.FirstOrDefault(up => up.UserId == userId);
            //}

            var response = new
            {
                UserProfile = userProfile
            };

            return Json(response);
        }
    }
}