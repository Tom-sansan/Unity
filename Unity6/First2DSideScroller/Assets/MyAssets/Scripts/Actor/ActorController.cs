using UnityEngine;

/// <summary>
/// ActorController Class
/// </summary>
public class ActorController : MonoBehaviour
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField
    /// <summary>
    /// Actor Speed
    /// </summary>
    [SerializeField]
    private float actorSpeed = 6.0f;
    /// <summary>
    /// Basic jump power
    /// </summary>
    [SerializeField]
    private float basicJumpPower = 10.0f;
    /// <summary>
    /// Added jump power
    /// </summary>
    [SerializeField]
    private float addedJumpPower = 30.0f;
    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables
    /// <summary>
    /// Facing direction(true: right, false: left)
    /// </summary>
    [HideInInspector]
    public bool rightFacing = true;
    /// <summary>
    /// Movement speed to X axis
    /// </summary>
    public float xSpeed;
    #region Public Properties

    #endregion Public Properties

    #endregion Public Variables

    #region Private Variables
    /// <summary>
    /// ActorGroundSensor class
    /// </summary>
    private ActorGroundSensor actorGroundSensor;
    /// <summary>
    /// ActorSprite class
    /// </summary>
    private ActorSprite actorSprite;
    /// <summary>
    /// Actor SpriteRenderer
    /// </summary>
    private SpriteRenderer spriteRenderer;
    /// <summary>
    /// Actor Rigidbody2D
    /// </summary>
    private Rigidbody2D rigidbody2D;
    /// <summary>
    /// Actor Position
    /// </summary>
    private Vector2 pos;
    /// <summary>
    /// Actor Velocity
    /// </summary>
    private Vector2 velocity;
    /// <summary>
    /// Remaining left time for jumping in the air
    /// </summary>
    private float remainJumpTime;
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
        UpdateInput();
    }
    void FixedUpdate()
    {
        velocity = rigidbody2D.linearVelocity;
        velocity.x = xSpeed;
        rigidbody2D.linearVelocity = velocity;
    }
    #endregion Unity Methods

    #region Public Methods

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialize this class
    /// </summary>
    private void Init()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        actorGroundSensor = GetComponentInChildren<ActorGroundSensor>();
        actorSprite = GetComponent<ActorSprite>();
        actorSprite.Init(this);
    }
    /// <summary>
    /// Update Input
    /// </summary>
    private void UpdateInput()
    {
        UpdateMove();
        UpdateJump();
    }
    /// <summary>
    /// Input Key
    /// </summary>
    private void UpdateMove()
    {
        // Move to right
        if (Input.GetKey(KeyCode.RightArrow))
        {
            xSpeed = actorSpeed;
            rightFacing = true;
            // スプライトを通常の向きで表示
            spriteRenderer.flipX = false;
        }
        // Move to left
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            xSpeed = -actorSpeed;
            rightFacing = false;
            // スプライトを左右反転した向きで表示
            spriteRenderer.flipX = true;
        }
        // Stop
        else xSpeed = 0.0f;
    }
    /// <summary>
    /// Update Jump
    /// </summary>
    private void UpdateJump()
    {
        // Reduce remaining jump time in the air
        if (remainJumpTime > 0.0f) remainJumpTime -= Time.deltaTime;
        // Jump
        if (Input.GetKeyDown(KeyCode.UpArrow) && actorGroundSensor.IsGround)
        {
            rigidbody2D.linearVelocity = new Vector2(rigidbody2D.linearVelocityX, basicJumpPower);
            // Set remain jump time in the air
            remainJumpTime = 0.25f;
        }
        // ジャンプ中（ジャンプ入力を長押しすると継続して上昇する処理）
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            // Return if remain jump time is over
            if (remainJumpTime <= 0.0f) return;
            // Return if acor is on the ground
            if (actorGroundSensor.IsGround) return;
            // Calculate jump power
            float jumpAddPower = addedJumpPower * Time.deltaTime;
            // Add jump power
            rigidbody2D.linearVelocity += new Vector2(0.0f, jumpAddPower);
        }
        // End jump input
        else if (Input.GetKeyUp(KeyCode.UpArrow)) remainJumpTime = -1.0f;
    }
    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
