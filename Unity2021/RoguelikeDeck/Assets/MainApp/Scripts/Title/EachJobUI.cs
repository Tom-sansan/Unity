using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Individual job UI processing class in job selection window
/// </summary>
public class EachJobUI : MonoBehaviour
{
    #region Class

    #endregion Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    /// <summary>
    /// Job icon
    /// </summary>
    [SerializeField]
    private Image iconImage = null;
    /// <summary>
    /// Background image for job name text
    /// </summary>
    [SerializeField]
    private Image nameBackImage = null;
    /// <summary>
    /// Job name text
    /// </summary>
    [SerializeField]
    private Text nameText = null;

    #endregion SerializeField

    #region Public Variables

    #endregion Public Variables

    #region Private Variables

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
    /// Display job data on each UI
    /// </summary>
    /// <param name="jobTypeID"></param>
    public void SetJobUI(int jobTypeID)
    {
        var jobData = JobDataDefine.DicJobData[JobDataDefine.GetJobTypeByInt(jobTypeID)];
        // Job icon image
        Sprite jobIcon = Data.instance.jobIcons[jobTypeID];
        if (jobIcon == null) iconImage.color = Color.clear;
        else
        {
            iconImage.color = Color.white;
            iconImage.sprite = jobIcon;
        }
        // Background image of job name
        nameBackImage.color = jobData.themeColor;
        // Text for job name
        if (Data.nowLanguage == SystemLanguage.Japanese) nameText.text = jobData.nameJP;
        else if (Data.nowLanguage == SystemLanguage.English) nameText.text = jobData.nameEN;

    }
    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #endregion Methods
}
