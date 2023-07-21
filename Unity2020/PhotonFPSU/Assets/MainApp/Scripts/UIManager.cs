using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI Manager Class
/// </summary>
public class UIManager : MonoBehaviour
{
    #region Variables

    #region Public
    public PlayerInformation info;
    /// <summary>
    /// Death Panel
    /// </summary>
    public GameObject deathPanel;
    /// <summary>
    /// Score Board
    /// </summary>
    public GameObject scoreboard;
    /// <summary>
    /// End Panel
    /// </summary>
    public GameObject endPanel;
    /// <summary>
    /// HP Slider
    /// </summary>
    public Slider hpSlider;
    /// <summary>
    /// Ammunition Text
    /// </summary>
    public Text ammoText;
    /// <summary>
    /// Death Text
    /// </summary>
    public Text deathText;
    #endregion

    #region Private

    #endregion

    #endregion

    #region Methods

    #region Public Methods
    /// <summary>
    /// Update magasin text
    /// </summary>
    /// <param name="ammoClip"></param>
    /// <param name="ammunition"></param>
    public void SetBulletsText(int ammoClip, int ammunition)
    {
        // Number of bullets in magasin / number of bullets in possession
        ammoText.text = ammoClip + "/" + ammunition;
    }
    /// <summary>
    /// Update HP
    /// </summary>
    public void UpdateHP(int maxHp, int currentHp)
    {
        hpSlider.maxValue = maxHp;
        hpSlider.value = currentHp;
    }
    /// <summary>
    /// Update and open Death Panel
    /// </summary>
    /// <param name="playerName"></param>
    public void UpdateDeathUI(string playerName)
    {
        deathPanel.SetActive(true);
        deathText.text = $"Defeated by {playerName}";
        // Close deathPanel
        Invoke("CloseDeathUI", 5f);
    }
    /// <summary>
    /// Close deathPanel
    /// </summary>
    public void CloseDeathUI() =>
        deathPanel.SetActive(false);
    /// <summary>
    /// Display Score Board
    /// </summary>
    public void ChangeScoreUI() =>
        scoreboard.SetActive(!scoreboard.activeInHierarchy);
    /// <summary>
    /// Display game end panel
    /// </summary>
    public void OpenEndPanel() =>
        endPanel.SetActive(true);

    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #endregion Methods
}
