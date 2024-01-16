using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Job define class
/// </summary>
public class JobDataDefine : MonoBehaviour
{
    #region Class

    /// <summary>
    /// Data retention class for each occupation name, etc.
    /// </summary>
    public class JobData
    {
        /// <summary>
        /// Name(Japanese)
        /// </summary>
        public string nameJP;
        /// <summary>
        /// Name(English)
        /// </summary>
        public string nameEN;
        /// <summary>
        /// Explanation(Japanese)
        /// </summary>
        public string explainJP;
        /// <summary>
        /// Explanation(English)
        /// </summary>
        public string explainEN;
        /// <summary>
        /// Theme color
        /// </summary>
        public Color themeColor;
        /// <summary>
        /// EXP required to open in the training area
        /// </summary>
        public int requireEXP;
    }

    #endregion Class

    #region Enum

    /// <summary>
    /// Job Type
    /// </summary>
    public enum JobType
    {
        None,       // なし
        Fighter,    // 闘士
        FengShui,   // 風水師
        Healer,     // 僧侶
        Sage,       // 賢者
        Samurai,    // 侍
        Sorcerer,   // 呪術師
        _Max,
    }
    #endregion Enum

    #region Variables

    #region SerializeField

    #endregion SerializeField

    #region Public Variables

    public static readonly Dictionary<JobType, JobData> DicJobData = new Dictionary<JobType, JobData>()
    {
        {
            JobType.None,
            new JobData()
            {
                nameJP = "職業なし",
                nameEN = "No Job",
                explainJP = string.Empty,
                explainEN = string.Empty,
                themeColor = new Color(0.3f, 0.3f, 0.3f),
                requireEXP = 0,
                
            }
        },
        {
            JobType.Fighter,
            new JobData()
            {
                nameJP = "闘士",
                nameEN = "Fighter",
                explainJP = "武器ダメージを与える時に+2追加",
                explainEN = "Additional +2 when dealing weapon damage",
                themeColor = new Color (0.5f, 0.15f, 0.1f),
                requireEXP = 200,

            }
        },
        {
            JobType.FengShui,
            new JobData
            {
                nameJP = "風水師",
                nameEN = "FengShui",
                explainJP = "自身の悪性状態異常の減少量+1\n敵に悪性状態異常を与える時+1",
                explainEN = "Reduction of own debuff +1\n+1 when inflicting debuff on an enemy.",
                themeColor = new Color (0.6f, 0.64f, 0.0f),
                requireEXP = 800,
            }
        },
        {
            JobType.Healer,
            new JobData
            {
                nameJP = "僧侶",
                nameEN = "Healer",
                explainJP = "自身の体力を回復する度に最大体力+2",
                explainEN = "Max HP +2 every time his HP is restored.",
                themeColor = new Color (0.0f, 0.6f, 0.64f),
                requireEXP = 1000,
            }
        },
        {
            JobType.Sage,
            new JobData
            {
                nameJP = "賢者",
                nameEN = "Sage",
                explainJP = "ターン開始時の手札１枚追加",
                explainEN = "1 additional card in hand at start of turn.",
                themeColor = new Color (0.6f, 0.0f, 0.5f),
                requireEXP = 6000,
            }
        },
        {
            JobType.Samurai,
            new JobData
            {
                nameJP = "侍",
                nameEN = "Samurai",
                explainJP = "左端のカードが与えるダメージ２倍\n（敵のカードならダメージ５減少）",
                explainEN = "Double the damage dealt by the leftmost card.\n(5 less damage if it is an enemy card)",
                themeColor = new Color (0.15f, 0.3f, 0.4f),
                requireEXP = 8000,
            }
        },
        {
            JobType.Sorcerer,
            new JobData
            {
                nameJP = "呪術師",
                nameEN = "Sorcerer",
                explainJP = "右端で発動するカードの\n効果量が全て３倍",
                explainEN = "Triple the effect amount of all cards\nactivated at the right end.",
                themeColor = new Color (0.3f, 0.18f, 0.45f),
                requireEXP = 20000,
            }
        },
    };

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
    /// Return corresponding JobType from int number
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static JobType GetJobTypeByInt(int id) =>
        (JobType)Enum.ToObject(typeof(JobType), id);

    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #endregion Methods
}
