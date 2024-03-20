using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Select Button Dialog Class
/// </summary>
public class SelectButtonDialog : MonoBehaviour
{
    #region Class

    #endregion Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    /// <summary>
    /// Background transition
    /// </summary>
    [SerializeField]
    private UITransition bgTransition = null;
    /// <summary>
    /// Parent of button
    /// </summary>
    [SerializeField]
    private Transform buttonParent = null;
    /// <summary>
    /// Prefab of button
    /// </summary>
    [SerializeField]
    private SelectButton buttonPrefab = null;

    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// Button list
    /// </summary>
    private List<SelectButton> buttons = new List<SelectButton>();
    /// <summary>
    /// Response
    /// </summary>
    private int response = -1;

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {

    }
    void Update()
    {

    }
    #endregion Unity Methods

    #region Public Methods

    /// <summary>
    /// Button creation
    /// </summary>
    /// <param name="bgOpen"></param>
    /// <param name="selectParams"></param>
    /// <returns></returns>
    public async UniTask<int> CreateButtons(bool bgOpen, string[] selectParams)
    {
        var res = response;
        // Close
        // await Close();
        return res;
    }

    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #endregion Methods
}
