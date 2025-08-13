using UnityEngine;

/// <summary>
/// EnemyCrow Class
/// Left-to-right movement in the air
/// </summary>
public class EnemyCrow : EnemyBase
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

    #region Public Const Variables

    #endregion Public Const Variables

    #region Public Properties

    #endregion Public Properties

    #endregion Public Variables

    #region Private Variables

    #region Private Const Variables

    #endregion Private Const Variables

    #region Private Properties

    #endregion Private Properties

    /// <summary>
    /// X coordinate of the previous frame
    /// </summary>
    private float previousPositionX;

    /// <summary>
    /// Frame of initial one time
    /// </summary>
    private bool initialFrame = true;
    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods

    #endregion Unity Methods

    #region Public Methods

    #endregion Public Methods

    #region Private Methods
    /// <summary>
    /// Update animation
    /// </summary>
    // public override void UpdateAnimation()
    // {
    //     // Don't move while disappearing
    //     if (isVanishing) return;
    //     // Walking animation time elapsed
    //     moveAnimationTime += Time.deltaTime;
    //     // Calculate the number of walking animation frames
    //     if (moveAnimationTime >= MoveAnimationSpan)
    //     {
    //         moveAnimationTime -= MoveAnimationSpan;
    //         // Add flyAnimationFrame
    //         moveAnimationFrame++;
    //         // If the number of frames exceeds the number of walking animation frames, reset to 0
    //         if (moveAnimationFrame >= spriteAnimation.Length) moveAnimationFrame = 0;
    //     }
    //     // Update fly animation
    //     spriteRenderer.sprite = spriteAnimation[moveAnimationFrame];
    // }

    /// <summary>
    /// Fixupdate move
    /// </summary>
    public override void FixedUpdateMove()
    {
        // Don't move at the time of the fist frame
        if (initialFrame)
        {
            initialFrame = false;
            // Keep the current x coordinate only
            previousPositionX = transform.position.x;
            return;
        }
        // Don't move while disappearing
        if (isVanishing)
        {
            _rigidbody2D.linearVelocity = Vector2.zero;
            return;
        }
        // Get the current x coordinate
        float currentPositionX = transform.position.x;
        // 
        if (Mathf.Approximately(currentPositionX, previousPositionX))
            SetFacingRight(!rightFacing);
        // Keep the current x coordinate as the previous x coordinate
        previousPositionX = currentPositionX;
        // Lateral(横) movement
        float xSpeed = movingSpeed;
        if (!rightFacing) xSpeed *= -1.0f;
        _rigidbody2D.linearVelocity = new Vector2(xSpeed, 0.0f);
    }
    #endregion Private Methods

    #endregion Methods
}
