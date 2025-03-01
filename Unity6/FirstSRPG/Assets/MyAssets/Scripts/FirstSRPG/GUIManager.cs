using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// GUIManager Class
/// </summary>
public class GUIManager : MonoBehaviour
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField
    /// <summary>
    /// StatusWindow GameObject
    /// </summary>
    [SerializeField]
    private GameObject statusWindow;
    /// <summary>
    /// Name Text
    /// </summary>
    [SerializeField]
    private Text nameText;
    /// <summary>
    /// AttributeIcon Image
    /// </summary>
    [SerializeField]
    private Image attributeIcon;
    /// <summary>
    /// HPGage Image
    /// </summary>
    [SerializeField]
    private Image hpGageImage;
    /// <summary>
    /// HP Text
    /// </summary>
    [SerializeField]
    private Text hpText;
    /// <summary>
    /// Attack Text
    /// </summary>
    [SerializeField]
    private Text attackText;
    /// <summary>
    /// Defence Text
    /// </summary>
    [SerializeField]
    private Text defenceText;
    /// <summary>
    /// Water attribute icon image
    /// </summary>
    [SerializeField]
    private Sprite attributeWater;
    /// <summary>
    /// Fire attribute icon image
    /// </summary>
    [SerializeField]
    private Sprite attributeFire;
    /// <summary>
    /// Wind attribute icon image
    /// </summary>
    [SerializeField]
    private Sprite attributeWind;
    /// <summary>
    /// Soil attribute icon image
    /// </summary>
    [SerializeField]
    private Sprite attributeSoil;
    /// <summary>
    /// CommandButtons gameObject for character
    /// Parent object of all command buttons
    /// </summary>
    [SerializeField]
    private GameObject commandButtons;
    /// <summary>
    /// Move cancel button
    /// </summary>
    [SerializeField]
    private GameObject moveCancelButton;
    /// <summary>
    /// Skill command button
    /// </summary>
    [SerializeField]
    private Button skillCommandButtons;
    /// <summary>
    /// Info text for skill of sellected character
    /// </summary>
    [SerializeField]
    private Text skillText;
    /// <summary>
    /// DecideButtons
    /// </summary>
    [SerializeField]
    private GameObject decideButtons;
    /// <summary>
    /// Game clear image
    /// </summary>
    [SerializeField]
    private Image gameClearImage;
    /// <summary>
    /// Game over image
    /// </summary>
    [SerializeField]
    private Image gameOverImage;
    /// <summary>
    /// Fade in image
    /// </summary>
    [SerializeField]
    private Image fadeInImage;
    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables
    /// <summary>
    /// BattleWindowUI gameObject
    /// </summary>
    public BattleWindowUI battleWindowUI;
    /// <summary>
    /// PlayerTurnImage
    /// プレイヤーターン開始画像
    /// </summary>
    public Image playerTurnImage;
    /// <summary>
    /// PlayerTurnImage
    /// 敵ターン開始画像
    /// </summary>
    public Image enemyTurnImage;
    #region Public Properties

    #endregion Public Properties

    #endregion Public Variables

    #region Private Variables

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
    #endregion Unity Methods

    #region Public Methods
    /// <summary>
    /// Show StatusWindow
    /// </summary>
    /// <param name="character"></param>
    public void ShowStatusWindow(Character character)
    {
        statusWindow.SetActive(true);
        nameText.text = character.CharaName;
        attributeIcon.sprite = character.attribute switch
        {
            Character.Attribute.Water => attributeWater,
            Character.Attribute.Fire => attributeFire,
            Character.Attribute.Wind => attributeWind,
            Character.Attribute.Soil => attributeSoil,
            _ => null,
        };

        //switch (character.attribute)
        //{
        //    case Character.Attribute.Water:
        //        attributeIcon.sprite = attributeWater;
        //        break;
        //    case Character.Attribute.Fire:
        //        attributeIcon.sprite = attributeFire;
        //        break;
        //    case Character.Attribute.Wind:
        //        attributeIcon.sprite = attributeWind;
        //        break;
        //    case Character.Attribute.Soil:
        //        attributeIcon.sprite = attributeSoil;
        //        break;
        //}

        // Show HP Gage
        // Set the ratio of the current HP to the maximum value to the fillAmount of the gauge Image
        // 最大値に対する現在HPの割合をゲージImageのfillAmountに設定する
        var ratio = (float)character.NowHP / character.MaxHP;
        hpGageImage.fillAmount = ratio;
        // Show HP Text
        hpText.text = $"{character.NowHP}/{character.MaxHP}";
        // Show Attack Text
        attackText.text = character.Attack.ToString();
        // Show Defence Text
        defenceText.text = character.IsDefenceBreak ?
                            "<color=red>0</color>"          // 防御力0化している場合
                            : character.Defence.ToString(); // 防御力Text表示
    }
    /// <summary>
    /// Hide StatusWindow
    /// </summary>
    public void HideStatusWindow() =>
        statusWindow.SetActive(false);
    /// <summary>
    /// Show or hide CommandButtons
    /// </summary>
    /// <param name="isShow">true if CommandButtons is shown</param>
    /// <param name="selectChara">Character data of acting</param>
    public void ShowHideCommandButtons(bool isShow, Character selectChara = null)
    {
        commandButtons.SetActive(isShow);
        if (isShow)
        {
            // Disables pressing skill button if skill is disabled
            // 特技使用不可状態の場合、特技ボタンを押せなくする
            skillCommandButtons.interactable = !selectChara.IsSkillLock;
            // 特技使用不可状態の場合、特技テキストは非表示
            skillText.enabled = !selectChara.IsSkillLock;
            if (skillText.enabled)
            {
                // Show selected character's skill
                SkillDefine.Skill skill = selectChara.Skill;
                string skillName = SkillDefine.dicSkillName[skill];
                string skillInfo = SkillDefine.dicSkillInfo[skill];
                // Show selected character's skill
                skillText.text = $"<size=40>{skillName}</size>\n{skillInfo}";
            }
        }
    }
    /// <summary>
    /// Show or Hide moveCancelButton
    /// </summary>
    /// <param name="isShow"></param>
    public void ShowHideMoveCancelButton(bool isShow) =>
        moveCancelButton.SetActive(isShow);
    /// <summary>
    /// Show or Hide decideButtons
    /// </summary>
    /// <param name="isShow"></param>
    public void ShowHideDecideButtons(bool isShow) =>
        decideButtons.SetActive(isShow);
    /// <summary>
    /// Show PlayerTurnImage / EnemyTurnImage
    /// </summary>
    public void ShowLogoTurnImage(Image targetTurnImage)
    {
        targetTurnImage
            .DOFade(1.0f, 1.0f)             // 指定数値まで画像のalpha値を変化、アニメーション時間（秒）
            .SetEase(Ease.OutCubic)         // イージングの種類 Ease.OutCubic：終了時に速度が遅くなる
            .SetLoops(2, LoopType.Yoyo);    // ループ回数とループの種類を設定 LoopType.Yoyo：逆再生
    }
    /// <summary>
    /// Show logo image for game clear
    /// </summary>
    public void ShowLogoGameClear()
    {
        // 徐々に表示するアニメーション
        gameClearImage
            .DOFade(1.0f, 1.0f)             // 指定数値まで画像のalpha値を変化、アニメーション時間（秒）
            .SetEase(Ease.OutCubic)         // イージングの種類 Ease.OutCubic：終了時に速度が遅くなる
            .SetLoops(2, LoopType.Yoyo);     // ループ回数とループの種類を設定 LoopType.Yoyo：逆再生
        // 拡大->縮小を行うアニメーション
        gameClearImage.transform
            .DOScale(1.5f, 1.0f)            // 指定数値まで画像のalpha値を変化、アニメーション時間（秒）
            .SetEase(Ease.OutCubic)         // イージングの種類 Ease.OutCubic：終了時に速度が遅くなる
            .SetLoops(2, LoopType.Yoyo);    // ループ回数とループの種類を設定 LoopType.Yoyo：逆再生
    }
    /// <summary>
    /// Show logo image for game over
    /// </summary>
    public void ShowLogoGameOver()
    {
        // 徐々に常時するアニメーション
        gameOverImage
            .DOFade(1.0f, 1.0f)             // 指定数値まで画像のalpha値を変化、アニメーション時間（秒）
            .SetEase(Ease.OutCubic);        // イージングの種類 Ease.OutCubic：終了時に速度が遅くなる
    }

    public void StartFadeIn()
    {
        fadeInImage
            .DOFade(1.0f, 5.5f)             // 指定数値まで画像のalpha値を変化、アニメーション時間（秒）
            .SetEase(Ease.Linear);          // イージング(変化の度合)を設定
    }
    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialize this class
    /// </summary>
    private void Init()
    {
        HideStatusWindow();
        ShowHideCommandButtons(false);
        ShowHideMoveCancelButton(false);
        ShowHideDecideButtons(false);
    }
    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
