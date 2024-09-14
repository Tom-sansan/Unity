using SocialGameAPI.Models;

namespace SocialGameAPI.Services
{
    /// <summary>
    /// MasterDataService class
    /// </summary>
    public class MasterDataService
    {
        public static void GenerateMasterData(string version)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, version);
            File.Create(path).Close();
            File.SetAttributes(path, FileAttributes.Normal);
            var masterDataList = new Dictionary<string, object>
            {
                { "MasterLoginItem", MasterLoginItem.GetAll() },
                { "MasterQuest", MasterQuest.GetAll() },
                { "MasterCharacter", MasterCharacter.GetAll() },
                { "MasterGacha", MasterGacha.GetAll() },
                { "MasterGachaCharacter", MasterGachaCharacter.GetAll() },
                { "MasterShop", MasterShop.GetAll() }
            };
        }
    }
}
