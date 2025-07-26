using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// ActorSprite Class
/// </summary>
public class ActorSprite : MonoBehaviour
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField
    /// <summary>
    /// Walk animation list
    /// </summary>
    [SerializeField]
    private List<Sprite> walkAnimationList;
    /// <summary>
    /// Stuck sprite list
    /// </summary>
    [SerializeField]
    private List<Sprite> stuckSpriteRes;
    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

    #region Public Properties

    #endregion Public Properties
    /// <summary>
    /// Stuck image display mode
    /// </summary>
    public bool stuckMode;
    #endregion Public Variables

    #region Private Variables

    #region Private Const Variables
    /// <summary>
    /// Sprite switching time for walking animation
    /// </summary>
    private const float WalkAnimationSpan = 0.3f;
    #endregion Private Const Variables

    /// <summary>
    /// ActorController class
    /// </summary>
    private ActorController actorController;
    /// <summary>
    /// SpriteRenderer class
    /// </summary>
    private SpriteRenderer spriteRenderer;
    /// <summary>
    /// Tween for blinking
    /// </summary>
    private Tween blinkTween;
    /// <summary>
    /// Current frame number of the walk animation
    /// </summary>
    private int walkAnimationFrame;
    /// <summary>
    /// Walk animation elapsed time
    /// </summary>
    private float walkAnimationTime;
    /// <summary>
    /// Number of pieces per type of walking animation
    /// </summary>
    private int walkAnimationNumber;
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
        // 被撃破中なら終了
        if (actorController.isDefeat) return;
        // スタン画像表示モード中ならスタン画像を表示
        if (stuckMode)
        {
            spriteRenderer.sprite = stuckSpriteRes[0];
            return;
        }
        ProcessWalkAnimation();
    }
    #endregion Unity Methods

    #region Public Methods
    /// <summary>
    /// Initialize from ActorController.cs)
    /// </summary>
    /// <param name="actorController"></param>
    public void Init(ActorController actorController)
    {
        this.actorController = actorController;
        this.spriteRenderer = this.actorController.GetComponent<SpriteRenderer>();
    }
    /// <summary>
    /// Start blinking animation
    /// </summary>
    public void StartBlinking()
    {
        // Process blinking using DoTween
        blinkTween = spriteRenderer.DOFade(0.0f, 0.15f) // 1回分の再生時間：0.15秒
            .SetDelay(0.3f)                 // 遅延時間：0.3秒
            .SetEase(Ease.Linear)           // Ease: 線形補間
            .SetLoops(-1, LoopType.Yoyo);   // 無限ループ再生（偶数回は逆再生）
    }
    /// <summary>
    /// Start defeat animation
    /// </summary>
    public void StartDefeatAnimation()
    {
        // 被撃破スプライト表示
        spriteRenderer.sprite = stuckSpriteRes[0];
        // 点滅演出終了
        if (blinkTween != null) blinkTween.Kill();
        // スプライト非表示化アニメーション
        // 2.0秒かけてスプライトの非透明度を0.0fにする
        spriteRenderer.DOFade(0.0f, 2.0f);
    }
    /// <summary>
    /// End blinking animation
    /// </summary>
    public void EndBlinking()
    {
        // DoTweenの点滅処理を終了させる
        if (blinkTween != null)
        {
            // DoTweenの終了
            blinkTween.Kill();
            // 色を元に戻す
            spriteRenderer.color = Color.white;
            // Tween情報を初期化
            blinkTween = null;
        }
    }
    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialize this class
    /// </summary>
    private void Init()
    {
        walkAnimationNumber = walkAnimationList.Count;
    }
    /// <summary>
    /// Process walk animation
    /// </summary>
    private void ProcessWalkAnimation()
    {
        // Walking animation time elapsed (only while moving sideways)
        if (Mathf.Abs(actorController.xSpeed) > 0.0f) walkAnimationTime += Time.deltaTime;
        // Calculate the number of walking animation frames
        if (walkAnimationTime >= WalkAnimationSpan)
        {
            // Deduct walking animation time by WalkAnimationSpan
            walkAnimationTime -= WalkAnimationSpan;
            // Add frames
            walkAnimationFrame++;
            // If the number of frames exceeds the number of walking animations, set it back to 0.
            // コマ数が歩行アニメーション枚数を越えているなら0に戻す
            if (walkAnimationFrame >= walkAnimationNumber) walkAnimationFrame = 0;
        }
        // Update walking animation
        spriteRenderer.sprite = walkAnimationList[walkAnimationFrame];
    }
    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
