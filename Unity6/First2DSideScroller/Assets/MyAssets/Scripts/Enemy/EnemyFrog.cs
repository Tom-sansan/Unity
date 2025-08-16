using UnityEngine;

/// <summary>
/// EnemyFrog Class
/// Left-to-right movement
/// </summary>
public class EnemyFrog : EnemyBase
{
    #region Variables

    #region SerializeField
    /// <summary>
    /// Enemy's jump interval
    /// </summary>
    [SerializeField]
    private float jumpInterval;
    /// <summary>
    /// Enemy's jump power forward
    /// </summary>
    [SerializeField]
    private float jumpPowerForward;
    /// <summary>
    /// Enemy's jump power upward
    /// </summary>
    [SerializeField]
    private float jumpPowerUpward;
    #endregion SerializeField

    #region Private Variables
    /// <summary>
    /// Time count
    /// </summary>
    private float timeCount;
    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Public Methods
    /// <summary>
    /// Update animation
    /// </summary>
    public override void UpdateAnimation()
    {
        // Set sprite
        if (_rigidbody2D.linearVelocity.magnitude < 0.1f)
            spriteRenderer.sprite = spriteAnimationList[0];
        else
        {
            spriteRenderer.sprite = spriteAnimationList[1];
            return;
        }
        // 
        timeCount += Time.deltaTime;
        if (timeCount < jumpInterval) return;
        timeCount = 0.0f;

        // Determine orientation based on position relative to actor
        if (transform.position.x > actorTransform.position.x) SetFacingRight(false);
        else SetFacingRight(true);
        // Jump move
        Vector2 jumpVec = new Vector2(jumpPowerForward, jumpPowerUpward);
        if (!rightFacing) jumpVec.x *= -1.0f;
        _rigidbody2D.linearVelocity += jumpVec;
    }
    /// <summary>
    /// Fixupdate move
    /// </summary>
    public override void FixedUpdateMove()
    {
        // Don't move while disappearing
        if (isVanishing)
        {
            _rigidbody2D.linearVelocity = Vector2.zero;
            return;
        }
    }
    #endregion Public Methods

    #endregion Methods
}
