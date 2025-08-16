using UnityEngine;

/// <summary>
/// EnemyWolf Class
/// Left-to-right movement
/// </summary>
public class EnemyWolf : EnemyBase
{
    #region Methods

    #region Public Methods
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
        // Change direction when the enemy hits a wall
        if (rightFacing && _rigidbody2D.linearVelocity.x <= 0.0f)
            SetFacingRight(false);
        else if (!rightFacing && _rigidbody2D.linearVelocity.x >= 0.0f)
            SetFacingRight(true);
        // Lateral(横) movement
        float xSpeed = movingSpeed;
        if (!rightFacing) xSpeed *= -1.0f;
        _rigidbody2D.linearVelocity = new Vector2(xSpeed, _rigidbody2D.linearVelocity.y);
    }
    #endregion Public Methods

    #endregion Methods
}
