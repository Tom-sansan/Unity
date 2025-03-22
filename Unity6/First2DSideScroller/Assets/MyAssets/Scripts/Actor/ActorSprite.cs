using System.Collections.Generic;
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
    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

    #region Public Properties

    #endregion Public Properties

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
