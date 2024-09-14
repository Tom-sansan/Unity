/// <summary>
/// UserProfile Class
/// </summary>
/// <remarks>
/// Due to JsonUtility.FromJson function, the first letter of field name must be lowercase.
/// </remarks>
public class UserProfile
{
    public string userId;
    public string userName;
    public int crystal;
    public int crystalFree;
    public int friendCoin;
    public int tutorialProgress;
}
