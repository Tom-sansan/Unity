using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    void Start()
    {
        
    }
    /// <summary>
    /// Screen tap call back
    /// </summary>
    public void OnScreenTap() =>
        SceneManager.LoadScene("MainScene");
}
