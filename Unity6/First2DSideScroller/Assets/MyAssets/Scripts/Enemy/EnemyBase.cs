using UnityEngine;

/// <summary>
/// EnemyBase Class
/// </summary>
public class EnemyBase : MonoBehaviour
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField
    /// <summary>
    /// Enemy's max HP
    /// </summary>
    [SerializeField]
    private int maxHP;
    /// <summary>
    /// Touch damage
    /// </summary>
    [SerializeField]
    private int touchDamage;
    #endregion SerializeField

    #region Protected Variables
    /// <summary>
    /// RigidBody2D class
    /// </summary>
    protected Rigidbody2D _rigidbody2D;
    /// <summary>
    /// Enemy sprite
    /// </summary>
    protected SpriteRenderer spriteRenderer;
    /// <summary>
    /// Actor's transform
    /// </summary>
    protected Transform actorTransform;
    #endregion Protected Variables

    #region Public Variables
    /// <summary>
    /// AreaManager class
    /// </summary>
    [HideInInspector]
    public AreaManager areaManager;
    /// <summary>
    /// Enemy's current HP
    /// </summary>
    [HideInInspector]
    public int nowHP;
    /// <summary>
    /// Invincible(無敵) mode
    /// </summary>
    [HideInInspector]
    public bool isInvis;
    /// <summary>
    /// Facing direction(true: right, false: left)
    /// </summary>
    [HideInInspector]
    public bool rightFacing;
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

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Awake()
    {
        InitAwake();
    }
    void Start()
    {
        InitStart();
    }
    void Update()
    {

    }
    #endregion Unity Methods

    #region Public Methods
    public void Init(AreaManager areaManager)
    {
        this.areaManager = areaManager;
        this.actorTransform = this.areaManager.stageManager.actorController.transform;
        this._rigidbody2D = GetComponent<Rigidbody2D>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.nowHP = this.maxHP;
        if (transform.localScale.x > 0.0f) rightFacing = true;
        // エリアがアクティブになるまで何も処理せず待機
        gameObject.SetActive(false);
    }
    /// <summary>
    /// Activate this monster when actor enters the monster's area
    /// </summary>
    public virtual void OnAreaActivated()
    {
        gameObject.SetActive(true);
    }
    /// <summary>
    /// Damage to enemy
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    public bool Damaged(int damage)
    {
        // Damage to enemy
        nowHP -= damage;
        if (nowHP <= 0.0) Vanish();
        else { }
        return true;
    }
    /// <summary>
    /// Attack actor
    /// </summary>
    public void AttackBody(GameObject actorObject)
    {
        ActorController actorController = actorObject.GetComponent<ActorController>();
        if (actorController == null) return;
        // Damage to actor
        actorController.Damaged(touchDamage);
    }
    /// <summary>
    /// Determine the orientation of the object from left to right
    /// </summary>
    /// <param name="isRight"></param>
    public void SetFacingRight(bool isRight)
    {
        // Left facing
        if (!isRight)
        {
            // Display sprites in their normal orientation
            spriteRenderer.flipX = false;
            // Set right facing direction off
            rightFacing = false;
        }
        // Right facing
        else
        {
            // Display sprites in their flipped orientation
            spriteRenderer.flipX = true;
            // Set right facing direction on
            rightFacing = true;
        }
    }
    #endregion Public Methods

    #region Private Methods
    /// <summary>
    /// Initialize Awake()
    /// </summary>
    private void InitAwake()
    {

    }
    /// <summary>
    /// Initialize Start()
    /// </summary>
    private void InitStart()
    {

    }
    /// <summary>
    /// Called when enemy is beaten
    /// </summary>
    private void Vanish() =>
        Destroy(gameObject);
    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
