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
    /// PlayerSwitcherTransform
    /// </summary>
    [SerializeField]
    private Transform playerSwitcherTransform;
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
    /// PlayerModelTypeText
    /// </summary>
    [Header("UI")]
    [SerializeField]
    private TextMeshProUGUI playerModelTypeText;
    /// <summary>
    /// Player selection number
    /// </summary>
    [SerializeField]
    private int playerSelectionNumber = 0;
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
        var playerSelectionProp = new ExitGames.Client.Photon.Hashtable
        {
            {
                MultiplayerARSpinnerTopGame.PLAYER_SELECTION_NUMBER, playerSelectionNumber
            }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerSelectionProp);
        // SceneManager.LoadScene("Scene_Lobby");
    }
    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialize this class
    /// </summary>
    private void Init()
    {

    }

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

    private void SetPlayerModelTypeText()
    {
        if (playerSelectionNumber == 0 || playerSelectionNumber == 1)
            // This means the player model type is Attack
            playerModelTypeText.text = "Attack";
        else
            // This means the player model type is Defend
            playerModelTypeText.text = "Defend";
    }
    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
