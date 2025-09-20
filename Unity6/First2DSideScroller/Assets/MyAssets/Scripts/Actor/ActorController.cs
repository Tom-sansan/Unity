using UnityEngine;

/// <summary>
/// ActorController Class
/// </summary>
public class ActorController : MonoBehaviour
{
    #region Variables

    #region SerializeField
    /// <summary>
    /// CameraController class
    /// </summary>
    [SerializeField]
    private CameraController cameraController;
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
    /// <summary>
    /// Weapon bullet prefab
    /// </summary>
    [SerializeField]
    private GameObject weaponBulletPrefab;
    /// <summary>
    /// Is icy ground mode
    /// </summary>
    [SerializeField]
    private bool isIcyGroundMode;
    #endregion SerializeField

    #region Public Variables
    /// <summary>
    /// Facing direction(true: right, false: left)
    /// </summary>
    [HideInInspector]
    public bool rightFacing = true;
    /// <summary>
    /// Enemy's current HP
    /// </summary>
    [HideInInspector]
    public int nowHP;
    /// <summary>
    /// Enemy's max HP
    /// </summary>
    [HideInInspector]
    public int maxHP;
    /// <summary>
    /// True: Defeat (Game Over)
    /// </summary>
    [HideInInspector]
    public bool isDefeat;
    /// <summary>
    /// True: Water mode
    /// </summary>
    [HideInInspector]
    public bool isWaterMode;
    /// <summary>
    /// Movement speed to X axis
    /// </summary>
    public float xSpeed;
    #endregion Public Variables

    #region Private Variables

    #region Private Const Variables
    /// <summary>
    /// Initial HP
    /// </summary>
    private const int InitialHP = 20;
    /// <summary>
    /// Invincible time(Second) after damaged
    /// 被ダメージ直後の無敵時間（秒）
    /// </summary>
    private const float InvincibleTime = 2.0f;
    /// <summary>
    /// Stuck time after damaged
    /// 被ダメージ直後の硬直時間（秒）
    /// </summary>
    private const float StuckTime = 0.5f;
    /// <summary>
    /// Knock back power after damaged(x axis)
    /// 被ダメージ時ノックバック力（x方向）
    /// </summary>
    private const float KnockBack_X = 2.5f;
    /// <summary>
    /// X-direction velocity multiplier in water
    /// 水中でのX方向速度倍率
    /// </summary>
    private const float WaterModeDecelerate_X = 0.8f;
    /// <summary>
    /// Y-direction velocity multiplier in water
    /// 水中でのY方向速度倍率
    /// </summary>
    private const float WaterModeDecelerate_Y = 0.92f;
    #endregion Private Const Variables
    /// <summary>
    /// ActorGroundSensor class for Actor
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
    private Rigidbody2D _rigidbody2D;
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
    /// <summary>
    /// Remain stuck time
    /// 残り硬直時間（0以上だと行動できない）
    /// </summary>
    private float remainStuckTime;
    /// <summary>
    /// Remain invincible time
    /// 残り無敵時間（秒）
    /// </summary>
    private float invincileTime;
    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {
        InitStart();
    }
    void Update()
    {
        // Game over if defeated
        if (isDefeat) return;
        UpdateInvincibleTime();
        UpdateStuckTime();
        UpdateInput();
        PreventSlippingOnSlopes();
    }
    void FixedUpdate()
    {
        FixedUpdateCalculateActorVelocity();
    }
    #endregion Unity Methods

    #region Public Methods
    /// <summary>
    /// Set water mode
    /// </summary>
    /// <param name="waterMode">True: in water</param>
    public void SetWaterMode(bool waterMode)
    {
        isWaterMode = waterMode;
        // Gravity in water
        if (isWaterMode) _rigidbody2D.gravityScale = 0.3f;
        else _rigidbody2D.gravityScale = 1.0f;

    }
    /// <summary)
    /// <summary>
    /// Process attack button input
    /// </summary>
    public void StartShotAction()
    {
        // 攻撃ボタンが入力されていない場合終了
        if (!Input.GetKeyDown(KeyCode.Z)) return;
        // このメソッド内で選択別武器メソッドの呼び分けやエネルギー消費処理を行う
        ShotAction_Normal();
    }
    /// <summary>
    /// Called when actor is damaged
    /// </summary>
    /// <param name="damage"></param>
    public void Damaged(int damage)
    {
        if (isDefeat) return;
        // No damage if invincible
        if (invincileTime > 0.0f) return;
        nowHP -= damage;
        // Game over if HP is 0
        if (nowHP <= 0)
        {
            isDefeat = true;
            // 被撃破演出開始
            actorSprite.StartDefeatAnimation();
            // 物理演算を停止
            _rigidbody2D.linearVelocity = Vector2.zero;
            _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            xSpeed = 0.0f;
            return;
        }
        // Process stuck
        remainStuckTime = StuckTime;
        actorSprite.stuckMode = true;
        // Process knock back
        float knockBackPower = KnockBack_X;
        if (rightFacing) knockBackPower *= -1.0f;
        // Apply knockback
        xSpeed = knockBackPower;
        // Process invincible
        invincileTime = InvincibleTime;
        if (invincileTime > 0.0f)
            actorSprite.StartBlinking();
    }
    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialize Start()
    /// </summary>
    private void InitStart()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        actorGroundSensor = GetComponentInChildren<ActorGroundSensor>();
        actorSprite = GetComponent<ActorSprite>();
        actorSprite.Init(this);
        // Initialize camera position
        cameraController.SetPosition(transform.position);
        // Initial HP
        nowHP = maxHP = InitialHP;
    }
    /// <summary>
    /// Update Input
    /// </summary>
    private void UpdateInput()
    {
        UpdateMove();
        UpdateJump();
        UpdateCameraPosition();
        StartShotAction();
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
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // If not grounded, stop (continue if in water)
            if (!actorGroundSensor.IsGround && !isWaterMode) return;
            _rigidbody2D.linearVelocity = new Vector2(_rigidbody2D.linearVelocityX, basicJumpPower);
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
            _rigidbody2D.linearVelocity += new Vector2(0.0f, jumpAddPower);
        }
        // End jump input
        else if (Input.GetKeyUp(KeyCode.UpArrow)) remainJumpTime = -1.0f;
    }
    /// <summary>
    /// Prevent slipping on slopes
    /// </summary>
    private void PreventSlippingOnSlopes()
    {
        // Rotation is frozen in Rigidbody
        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (actorGroundSensor.IsGround && !Input.GetKey(KeyCode.UpArrow))
        {
            // Prevent upward force from working when climbing a slope
            if (_rigidbody2D.linearVelocity.y > 0.0f)
                _rigidbody2D.linearVelocity = new Vector2(_rigidbody2D.linearVelocity.x, 0.0f);
            // Avoid slipping and falling when standing on a slope
            if (Mathf.Abs(xSpeed) < 0.1f && !isIcyGroundMode)
                // Stops movement and rotation among Rigidbody functions
                _rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX
                                        | RigidbodyConstraints2D.FreezePositionY
                                        | RigidbodyConstraints2D.FreezeRotation;
        }
    }
    /// <summary>
    /// Calculate actor velocity in FixedUpdate
    /// </summary>
    private void FixedUpdateCalculateActorVelocity()
    {
        velocity = _rigidbody2D.linearVelocity;
        velocity.x = xSpeed;
        // In case of icy ground mode, set xSpeed like slipping on ground
        if (isIcyGroundMode && actorGroundSensor.IsGround)
            velocity.x = Mathf.Lerp(xSpeed, _rigidbody2D.linearVelocityX, 0.99f);
        // Speed in water
        if (isWaterMode)
        {
            velocity.x *= WaterModeDecelerate_X;
            velocity.y *= WaterModeDecelerate_Y;
        }
        _rigidbody2D.linearVelocity = velocity;
    }
    /// <summary>
    /// Update camera position
    /// </summary>
    private void UpdateCameraPosition() =>
        cameraController.SetPosition(transform.position);
    /// <summary>
    /// Shot action (Normal)
    /// </summary>
    private void ShotAction_Normal()
    {
        // Get bullet direction
        float bulletAngle = 0.0f; // Facing right
        // If the actor is facing left, the bullet will also move to the left
        if (!rightFacing) bulletAngle = 180.0f;
        // Create bullet object
        GameObject bullet = Instantiate(    // Create bullet object
            weaponBulletPrefab,             // Bullet prefab
            transform.position,             // Bullet init position
            Quaternion.identity);           // Bullet init rotation
        // Set bullet
        bullet.GetComponent<ActorNormalShot>().Init
        (
            12.0f,      // Bullet velocity
            bulletAngle,// Bullet angle
            5.0f,       // Bullet life time
            1           // Bullet damage
        );
    }
    /// <summary>
    /// Update Invincible(無敵) Time
    /// </summary>
    private void UpdateInvincibleTime()
    {
        if (invincileTime > 0.0f)
        {
            invincileTime -= Time.deltaTime;
            if (invincileTime <= 0.0f)
                // Process end of invincible time
                actorSprite.EndBlinking();
        }
    }
    /// <summary>
    /// Update Stuck Time
    /// </summary>
    private void UpdateStuckTime()
    {
        if (remainStuckTime > 0.0f)
        {
            remainStuckTime -= Time.deltaTime;
            if (remainStuckTime <= 0.0f)
                // Process end of stuck time
                actorSprite.stuckMode = false;
        }
    }
    #endregion Private Methods

    #endregion Methods
}
