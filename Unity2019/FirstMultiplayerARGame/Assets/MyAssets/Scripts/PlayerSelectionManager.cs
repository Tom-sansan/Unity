using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// PlayerSelectionManager Class
/// </summary>
public class PlayerSelectionManager : MonoBehaviour
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField
    /// <summary>
    /// SpinnerTopModels GameObjects
    /// </summary>
    [SerializeField]
    private GameObject[] spinnerTopModels;
    /// <summary>
    /// Player selection number
    /// </summary>
    [SerializeField]
    private int playerSelectionNumber = 0;
    /// <summary>
    /// PlayerSwitcherTransform
    /// </summary>
    [SerializeField]
    private Transform playerSwitcherTransform;
    /// <summary>
    /// PlayerModelTypeText
    /// </summary>
    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI playerModelTypeText;
    /// <summary>
    /// NextButton
    /// </summary>
    [SerializeField]
    private Button nextButton;
    /// <summary>
    /// PreviousButton
    /// </summary>
    [SerializeField]
    private Button previousButton;
    /// <summary>
    /// UISelection GameObject
    /// </summary>
    [SerializeField]
    private GameObject uISelection;
    /// <summary>
    /// UIAfterSelection GameObject
    /// </summary>
    [SerializeField]
    private GameObject uIAfterSelection;
    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

    #region Public Properties

    #endregion Public Properties

    #endregion Public Variables

    #region Private Variables

    #region Private Const Variables

    #endregion Private Const Variables

    #region Private Properties

    #endregion Private Properties

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {
        Init();
    }
    void Update()
    {

    }
    #endregion Unity Methods

    #region Public Methods
    /// <summary>
    /// NextPlayer
    /// </summary>
    public void NextPlayer()
    {
        playerSelectionNumber++;
        if (playerSelectionNumber >= spinnerTopModels.Length) playerSelectionNumber = 0;
        Debug.Log(playerSelectionNumber);
        SetButtonEnabled(false);
        StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform, 90, 1.0f));
        SetPlayerModelTypeText();
    }
    /// <summary>
    /// PreviousPlayer
    /// </summary>
    public void PreviousPlayer()
    {
        playerSelectionNumber--;
        if (playerSelectionNumber < 0) playerSelectionNumber = spinnerTopModels.Length - 1;
        Debug.Log(playerSelectionNumber);
        SetButtonEnabled(false);
        StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform, -90, 1.0f));
        SetPlayerModelTypeText();
    }
    /// <summary>
    /// OnSelectButtonClicked
    /// </summary>
    public void OnSelectButtonClicked()
    {
        uISelection.SetActive(false);
        uIAfterSelection.SetActive(true);
        var playerSelectionProp = new ExitGames.Client.Photon.Hashtable
        {
            {
                MultiplayerARSpinnerTopGame.PLAYER_SELECTION_NUMBER, playerSelectionNumber
            }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerSelectionProp);
    }
    /// <summary>
    /// OnReSelectButtonClicked
    /// </summary>
    public void OnReSelectButtonClicked()
    {
        SetUISelection(true);
    }
    /// <summary>
    /// OnBattleButtonClicked
    /// </summary>
    public void OnBattleButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Scene_Gameplay");
    }
    /// <summary>
    /// 
    /// </summary>
    public void OnBackButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Scene_Lobby");
    }
    #endregion Public Methods

    #region Private Methods
    /// <summary>
    /// Initialize this class
    /// </summary>
    private void Init()
    {
        uISelection.SetActive(true);
        uIAfterSelection.SetActive(false);
    }
    /// <summary>
    /// Rotate
    /// </summary>
    /// <param name="axis"></param>
    /// <param name="transformToRotate"></param>
    /// <param name="angle"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    private IEnumerator Rotate(Vector3 axis, Transform transformToRotate, float angle, float duration = 1f)
    {
        Quaternion originalRotation = transformToRotate.rotation;
        Quaternion finalRotation = transformToRotate.rotation * Quaternion.Euler(axis * angle);
        float elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            transformToRotate.rotation = Quaternion.Slerp(originalRotation, finalRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transformToRotate.rotation = finalRotation;
        SetButtonEnabled(true);
    }
    /// <summary>
    /// Set button enabled or disabled
    /// </summary>
    /// <param name="isEnabled"></param>
    private void SetButtonEnabled(bool isEnabled)
    {
        nextButton.enabled = isEnabled;
        previousButton.enabled = isEnabled;
    }
    /// <summary>
    /// Set player model type text
    /// </summary>
    private void SetPlayerModelTypeText()
    {
        if (playerSelectionNumber == 0 || playerSelectionNumber == 1)
            // This means the player model type is Attack
            playerModelTypeText.text = "ATTACK";
        else
            // This means the player model type is Defend
            playerModelTypeText.text = "DEFEND";
    }
    /// <summary>
    /// Set UI selection
    /// </summary>
    /// <param name="isUISelectionActive"></param>
    private void SetUISelection(bool isUISelectionActive)
    {
        uISelection.SetActive(isUISelectionActive);
        uIAfterSelection.SetActive(!isUISelectionActive);
    }
    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
