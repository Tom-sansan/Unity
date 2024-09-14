using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SocialGameAPI.Models;
using SocialGameAPI.Services;

namespace SocialGameAPI.Controllers
{
    /// <summary>
    /// UserProfileController Class
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfileController : ControllerBase
    {
        private readonly SocialGameAPIService _service;

        public UserProfileController(SocialGameAPIService service)
        {
            _service = service;
        }
        /// <summary>
        /// Get User Profile data
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetUserProfile/{id}")]
        // [HttpGet("{id}")]
        public IActionResult GetUserProfile(string id)
        {
            var userProfile = _service.GetData<UserProfile>(id, true);
            if (userProfile == null) return NotFound();
            return Ok(userProfile);
        }
        /// <summary>
        /// Add User Profile data
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddUserProfile([FromBody]UserProfile userProfile)
        {
            var parameterList = new List<SqlParameter>
            {
                new SqlParameter($"@{nameof(userProfile.UserId)}", userProfile.UserId),
                new SqlParameter($"@{nameof(userProfile.UserName)}", userProfile.UserName),
                new SqlParameter($"@{nameof(userProfile.Crystal)}", userProfile.Crystal),
                new SqlParameter($"@{nameof(userProfile.CrystalFree)}", userProfile.CrystalFree),
                new SqlParameter($"@{nameof(userProfile.FriendCoin)}", userProfile.FriendCoin),
                new SqlParameter($"@{nameof(userProfile.TutorialProgress)}", userProfile.TutorialProgress),
            };
            var result = _service.AddData<UserProfile>(parameterList);
            if (result > 0) return CreatedAtAction(nameof(GetUserProfile), new { id = userProfile.UserId });
            return BadRequest();
        }
    }
}
