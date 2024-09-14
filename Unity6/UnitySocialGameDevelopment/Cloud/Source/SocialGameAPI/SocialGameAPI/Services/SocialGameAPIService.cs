using Microsoft.Data.SqlClient;
using SocialGameAPI.DAO;
using SocialGameAPI.Models;
using System.Data;

namespace SocialGameAPI.Services
{
    /// <summary>
    /// SocialGameAPIService Class
    /// </summary>
    public class SocialGameAPIService : BaseDao
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connectionString"></param>
        public SocialGameAPIService(string connectionString) : base(connectionString) { }

        /// <summary>
        /// Get data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetData<T>(string id, bool useStoredProcedure = false) where T : class, new()
        {
            var commandText = string.Empty;
            var commandType = UseStoredProcedure(useStoredProcedure);
            var parameterList = new List<SqlParameter>();
            Func<SqlDataReader, T>? readFunc = null;
            switch (typeof(T).Name)
            {
                case nameof(UserProfile):
                    commandText = "SpGetUserProfile";
                    parameterList.Add(new SqlParameter($"@{nameof(UserProfile.UserId)}", id));
                    readFunc = reader => new UserProfile
                    {
                        UserId = reader[nameof(UserProfile.UserId)].ToString(),
                        UserName = reader[nameof(UserProfile.UserName)].ToString(),
                        Crystal = int.Parse(reader[nameof(UserProfile.Crystal)].ToString()),
                        CrystalFree = int.Parse(reader[nameof(UserProfile.CrystalFree)].ToString()),
                        FriendCoin = int.Parse(reader[nameof(UserProfile.FriendCoin)].ToString()),
                        TutorialProgress = int.Parse(reader[nameof(UserProfile.TutorialProgress)].ToString()),
                    } as T;
                    break;
            }
            var result = base.ExecuteReader(commandText, commandType, readFunc, parameterList);
            return result ?? new T();
        }
        /// <summary>
        /// Add data
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameterList"></param>
        /// <param name="useStoredProcedure"></param>
        /// <returns></returns>
        public int AddData<T>(List<SqlParameter> parameterList, bool useStoredProcedure = false) where T : class, new()
        {
            var commandText = string.Empty;
            var commandType = UseStoredProcedure(useStoredProcedure);
            switch (typeof(T).Name)
            {
                case nameof(UserProfile):
                    // sql = ResourcesData.GetResourcesData<TextAsset>(S.Instance.Data.InsertUserProfile)?.text;
                    break;
                default:
                    // sql = ResourcesData.GetResourcesData<TextAsset>(S.Instance.Data.InsertUserProfile)?.text;
                    break;
            }
            return base.ExecuteNonQuery(commandText, commandType, parameterList);
        }
        /// <summary>
        /// Returns CommandType
        /// </summary>
        /// <param name="isStoredProcedure"></param>
        /// <returns>true;CommandType.StoredProcedure, false;CommandType.Text</returns>
        private CommandType UseStoredProcedure(bool isStoredProcedure) =>
            isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;
    }
}