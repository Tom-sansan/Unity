using UnityEngine;

/// <summary>
/// EnemyBee Class
/// Up and down movement in the air
/// </summary>
public class EnemyBee : EnemyBase
{
    #region Variables

    #region SerializeField
    /// <summary>
    /// Enemy's movement time
    /// </summary>
    [SerializeField]
    private float MovingTime;
    #endregion SerializeField

    #region Private Variables
    /// <summary>
    /// Default position
    /// </summary>
    private Vector3 defaultPos;
    /// <summary>
    /// Movement vector
    /// </summary>
    private Vector3 moveVec;
    /// <summary>
    /// Elapsed time
    /// </summary>
    private float elapsedTime;
    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Public Methods
    /// <summary>
    /// Initialize Start()
    /// </summary>
    public override void InitStart()
    {
        defaultPos = transform.position;
        moveVec = Vector3.zero;
        elapsedTime = 0.0f;
        // Avoid error
        if (MovingTime < 0.1f) MovingTime = 0.1f;
    }
    /// <summary>
    /// Fixupdate move
    /// </summary>
    public override void FixedUpdateMove()
    {
        // Up and down movement
        // Elapsed time
        elapsedTime += Time.deltaTime;
        // Calculate movement vector
        Vector3 vec = new Vector3((Mathf.Sin(elapsedTime / MovingTime) + 1.0f) * movingSpeed, 0.0f, 0.0f);
        vec = Quaternion.Euler(0, 0, 90) * vec;
        // Movement
        _rigidbody2D.MovePosition(defaultPos + vec);
    }
    #endregion Public Methods

    #endregion Methods
}
