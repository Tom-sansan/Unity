using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Character image object management class
/// </summary>
public class EnemyPicture : MonoBehaviour
{
    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    [SerializeField]
    private RectTransform rectTransform = null;

    [SerializeField]
    private Image image = null;

    #endregion SerializeField

    #region Public Variables

    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// Destination Y relative coordinate
    /// </summary>
    private const float TargetPositionYRelative = 200.0f;
    /// <summary>
    /// Performance time
    /// </summary>
    private const float AnimTime1 = 1.0f;
    /// <summary>
    /// X-direction range of destination
    /// </summary>
    private const float JumpPosXWidth = 100.0f;
    /// <summary>
    /// Y-direction range of destination
    /// </summary>
    private const float JumpPosYHeight = 100.0f;
    /// <summary>
    /// Moving time
    /// </summary>
    private const float AnimTimeMove = 0.1f;
    /// <summary>
    /// Moving time for animation to return to original position
    /// </summary>
    private const float AnimTimeBack = 1.2f;
    /// <summary>
    /// X relative coordinate of jump destination
    /// </summary>
    private const float JumpPosXRelative = 100.0f;
    /// <summary>
    /// Y relative coordinate of jump destination
    /// </summary>
    private const float JumpPosYRelative = 50.0f;
    /// <summary>
    /// Jump power
    /// </summary>
    private const float JumpPower = 30.0f;
    /// <summary>
    /// Performance time at 0.7f
    /// </summary>
    private const float AnimTime07 = 0.7f;

    /// <summary>
    /// Enemy image initial coordinates
    /// </summary>
    private Vector2 basePosition;
    /// <summary>
    /// Random Movement Sequence when damaged
    /// </summary>
    private Sequence randomMoveSequence;
    
    #endregion Private Variables

    /// <summary>
    /// Character manager
    /// </summary>
    private CharacterManager characterManager;
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
    /// Initialization and enemy appearance processing (called from CharacterManager.cs)
    /// </summary>
    /// <param name="characterManager"></param>
    /// <param name="enemySprite"></param>
    public void Init(CharacterManager characterManager, Sprite enemySprite)
    {
        this.characterManager = characterManager;
        // Save initial coordinates
        basePosition = rectTransform.anchoredPosition;
        // Enemy Image Display
        image.sprite = enemySprite;
        // Fit the size of the object to the size of the screen
        image.SetNativeSize();
        // Animation of an enemy coming down from the top of the screen
        // Set initial position
        Vector2 pos = basePosition;
        pos.y += TargetPositionYRelative;
        rectTransform.anchoredPosition = pos;
        // Y-direction movement animation (tween)
        rectTransform
            .DOAnchorPosY(-TargetPositionYRelative, AnimTime1)
            .SetRelative();
    }
    /// <summary>
    /// Play damage animation
    /// </summary>
    public void DamageAnimation()
    {
        // Initialize Sequence
        if (randomMoveSequence != null) randomMoveSequence.Kill();
        randomMoveSequence = DOTween.Sequence();
        // Enemy random movement animation
        // Set random destinations
        Vector2 pos = rectTransform.anchoredPosition;
        pos.x += Random.Range(-JumpPosXWidth / 2.0f, JumpPosXWidth / 2.0f);
        pos.y += Random.Range(-JumpPosYHeight / 2.0f, JumpPosYHeight / 2.0f);
        // Jump movement animation (tween)
        randomMoveSequence.Append(rectTransform.DOAnchorPos(pos, AnimTimeMove));
        // Animation to return to original position
        // Jump movement animation (tween)
        randomMoveSequence.Append(rectTransform.DOAnchorPos(basePosition, AnimTimeBack));
    }
    /// <summary>
    /// Play hide animation when destroyed
    /// </summary>
    public void DefeatAnimation()
    {
        // Stop playing Sequence
        if (randomMoveSequence != null) randomMoveSequence.Kill();
        // Initialization of staging sequence at the time of destruction
        var defeatSequence = DOTween.Sequence();
        // Enemy bouncing (jumping) animation
        // Set jump destination
        Vector2 pos = rectTransform.anchoredPosition;
        pos.x += JumpPosXRelative;
        pos.y += JumpPosYRelative;
        // Jump movement animation (tween)
        defeatSequence.Append(rectTransform
            .DOJumpAnchorPos(pos, JumpPower, 1, AnimTime07)
            .SetEase(Ease.Linear)); // Specify how to change
        // Scale reduction (Tween)
        defeatSequence.Join(rectTransform
            .DOScale(0.0f, AnimTime07)
            .SetEase(Ease.Linear));
        // Delete objects when animation is complete
        defeatSequence.OnComplete(() =>
        {
            characterManager.DeleteEnemy();
        });
    }
    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #endregion Methods
}
