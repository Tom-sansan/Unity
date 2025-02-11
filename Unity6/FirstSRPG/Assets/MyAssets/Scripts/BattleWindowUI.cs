using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// BattleWindowUI Class
/// </summary>
public class BattleWindowUI : MonoBehaviour
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField
    /// <summary>
    /// Name text
    /// </summary>
    [SerializeField]
    private Text nameText;
    /// <summary>
    /// HP gage image
    /// </summary>
    [SerializeField]
    private Image hpGageImage;
    /// <summary>
    /// HP text
    /// </summary>
    [SerializeField]
    private Text hpText;
    /// <summary>
    /// Damage text
    /// </summary>
    [SerializeField]
    private Text damageText;
    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

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
    void Update()
    {

    }
    #endregion Unity Methods

    #region Public Methods
    /// <summary>
    /// 
    /// </summary>
    /// <param name="character"></param>
    /// <param name="damageValue"></param>
    public void ShowWindow(Character character, int damageValue)
    {
        gameObject.SetActive(true);
        nameText.text = character.CharaName;
        // Get the remaining HP after damage calculation
        // (The HP of the target character data is not changed here)
        // ダメージ計算後の残りHPを取得
        // (ここでは対象キャラクターデータのHPは変化させない)
        int nowHP = character.NowHP - damageValue;
        // HP corrected to fall in the range of 0 to max
        // HPが0～最大値の範囲に収まるように補正
        nowHP = Mathf.Clamp(nowHP, 0, character.MaxHP);

        // Show HP gage
        // float ratio = (float)nowHP / character.MaxHP;
        // Set the ratio of the current HP to the maximum value to the fillAmount of the gauge Image
        // 最大値に対する現在のHPの割合をゲージImageのfillAmountにセットする
        // hpGageImage.fillAmount = ratio;

        // FillAmount on display (initial value before HP reduction)
        // 表示中のFillAmount(初期値はHP減少前のもの)
        float amount = (float)character.NowHP / character.MaxHP;
        // FillAmount after animation
        // アニメーション後のFillAmount
        float endAmount = (float)nowHP / character.MaxHP;
        DOTween.To
        (
            () => amount,       // 現在のHPゲージの表示割合を取得する(getter)
            (x) => amount = x,  // HPゲージの表示割合を更新する(setter)
            endAmount,          // 変化先の数値(endValue)
            1.0f                // アニメーション時間(duration)
        )
        // Process performed each time the animation is updated
        // アニメーションが更新されるたびに実行される処理
        .OnUpdate(() =>
        {
            // Update the display of the HP gauge
            // HPゲージの表示を更新
            hpGageImage.fillAmount = amount;
        });

        // Show HP text
        hpText.text = nowHP + "/" + character.MaxHP;
        // Show damage text
        damageText.text = damageValue + " ダメージ！";
    }
    /// <summary>
    /// Hide BattleWindowUI
    /// </summary>
    public void HideWindow() =>
        gameObject.SetActive(false);
    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialize this class
    /// </summary>
    private void Init()
    {
        HideWindow();
    }

    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
