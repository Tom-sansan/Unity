using UnityEngine;

/// <summary>
/// DogBoss Class
/// </summary>
public class DogBoss : EnemyBase
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField
    /// <summary>
    /// Wait sprite
    /// </summary>
    [SerializeField]
    private Sprite spriteWait;
    /// <summary>
    /// Move sprite
    /// </summary>
    [SerializeField]
    private Sprite spriteMove;
    /// <summary>
    /// Jump sprite
    /// </summary>
    [SerializeField]
    private Sprite spriteJump;
    /// <summary>
    /// Attack interval
    /// </summary>
    [SerializeField]
    private float attackInterval;
    /// <summary>
    /// Moving speed
    /// </summary>
    // [SerializeField]
    // private float movingSpeed;
    /// <summary>
    /// Jump speed
    /// </summary>
    [SerializeField]
    private float jumpSpeed;
    /// <summary>
    /// Jump power min
    /// </summary>
    [SerializeField]
    private float jumpPowerMin;
    /// <summary>
    /// Jump power max
    /// </summary>
    [SerializeField]
    private float jumpPowerMax;
    /// <summary>
    /// Jump ratio(0 to 100)
    /// </summary>
    [SerializeField]
    private int jumpRatio;
    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

    #region Public Const/Readonly Variables

    #endregion Public Const/Readonly Variables

    #region Public Properties

    #endregion Public Properties

    #endregion Public Variables

    #region Private Variables

    #region Private Const/Readonly Variables

    #endregion Private Const/Readonly Variables

    #region Private Properties

    #endregion Private Properties
    /// <summary>
    /// Time remaining until the next attack
    /// </summary>
    private float nextAttackTime;
    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    //void Awake()
    //{
    //    InitAwake();
    //}
    //void Start()
    //{
    //    InitStart();
    //}
    public override void Update()
    {
        if (isVanishing) return;
        // Change displayed sprite
        UpdateAnimation();
        // Start attack
        UpdateAttack();
    }
    //void FixedUpdate()
    //{

    //}
    #endregion Unity Methods

    #region Public Methods
    /// <summary>
    /// Processing when an actor enters the area where this monster resides (Area activation processing)
    /// </summary>
    public override void OnAreaActivated()
    {
        base.OnAreaActivated();
    }
    #endregion Public Methods

    #region Private Methods
    /// <summary>
    /// Initialize Awake()
    /// </summary>
    //private void InitAwake()
    //{

    //}
    /// <summary>
    /// Initialize Start()
    /// </summary>
    public override void InitStart()
    {
        nextAttackTime = attackInterval / 2.0f;
    }
    /// <summary>
    /// Update sprite of boss
    /// </summary>
    private new void UpdateAnimation()
    {
        // Get grounding determination
        ContactPoint2D[] contactPoints = new ContactPoint2D[2];
        _rigidbody2D.GetContacts(contactPoints);
        bool isGround = contactPoints[1].enabled;
        if (!isGround)
            // Jumping
            spriteRenderer.sprite = spriteJump;
        else if (Mathf.Abs(_rigidbody2D.linearVelocityX) >= 0.1f)
            // Moving horizontally
            spriteRenderer.sprite = spriteMove;
        else
            // Waiting
            spriteRenderer.sprite = spriteWait;
    }
    /// <summary>
    /// Process attack interval
    /// </summary>
    private bool IsUpdateAttackInterval()
    {

        return true;
    }
    /// <summary>
    /// Update attack
    /// </summary>
    private void UpdateAttack()
    {
        // Process attack interval
        nextAttackTime -= Time.deltaTime;
        if (nextAttackTime > 0.0f) return;
        nextAttackTime = attackInterval;
        // Once you attack, reduce the gravitational acceleration
        // 一度でも攻撃したら重力加速度を下げる
        _rigidbody2D.gravityScale = 0.5f;

        // Velocity
        Vector2 velocity = new Vector2();
        if (Random.Range(0, 100) < jumpRatio)
        {
            // Jump attack
            velocity.x = jumpSpeed;
            velocity.y = Random.Range(jumpPowerMin, jumpPowerMax);
        }
        else
        {
            // Normal movement
            velocity.x = movingSpeed;
        }
        // Update direction
        UpdateDirection(velocity);
    }
    /// <summary>
    /// Update direction of boss
    /// </summary>
    private void UpdateDirection(Vector2 velocity)
    {
        // Determine orientation based on positional relationship with the actor
        if (transform.position.x > actorTransform.position.x)
        {
            // Face left
            SetFacingRight(false);
            velocity.x *= -1.0f;
        }
        else
        {
            // Face right
            SetFacingRight(true);
        }
        // Apply velocity
        _rigidbody2D.linearVelocity = velocity;
    }
    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
