using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// EnemyBase Class
/// </summary>
public class EnemyBase : MonoBehaviour
{
    #region Variables

    #region SerializeField
    /// <summary>
    /// Sprite list for animation
    /// </summary>
    [SerializeField]
    protected List<Sprite> spriteAnimationList = null;
    /// <summary>
    /// Enemy's moving speed
    /// </summary>
    [SerializeField]
    protected float movingSpeed;
    /// <summary>
    /// Enemy's max speed
    /// </summary>
    [SerializeField]
    protected float maxSpeed;
    /// <summary>
    /// Enemy's sprite of defeat
    /// </summary>
    [SerializeField]
    private Sprite spriteDefeat;
    /// <summary>
    /// Prefab for Boss Defeat Particle
    /// </summary>
    [SerializeField]
    private GameObject bossDefeatParticlePrefab;
    /// <summary>
    /// Flag for boss
    /// </summary>
    [SerializeField]
    private bool isBoss;
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
    /// <summary>
    /// Flag for vanish
    /// </summary>
    [HideInInspector]
    public bool isVanishing;
    #region Public Const Variables

    #endregion Public Const Variables

    #region Public Properties

    #endregion Public Properties
    /// <summary>
    /// The current frame number of move animation
    /// </summary>
    [HideInInspector]
    public int moveAnimationFrame;
    #endregion Public Variables

    #region Private Variables

    #region Private Const/Readonly Variables
    /// <summary>
    /// Default color
    /// </summary>
    private readonly Color COL_DEFAUL = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    /// <summary>
    /// Damaged color
    /// </summary>
    private readonly Color COL_DAMAGED = new Color(1.0f, 0.1f, 0.1f, 1.0f);
    /// <summary>
    /// Knockback value in X direction when damaged
    /// </summary>
    private const float KNOCKBACK_X = 1.8f;
    /// <summary>
    /// Knockback value in Y direction when damaged
    /// </summary>
    private const float KNOCKBACK_Y = 0.3f;
    /// <summary>
    /// Sprite change time of fly animation
    /// </summary>
    private const float MoveAnimationSpan = 0.3f;
    #endregion Private Const/Readonly Variables
    /// <summary>
    /// Enemy's damage tween
    /// </summary>
    private Tween damageTween;
    #region Private Properties

    #endregion Private Properties
    /// <summary>
    /// Elapsed time of move animation
    /// </summary>
    private float moveAnimationTime;
    /// <summary>
    /// Enemy destroyed flag
    /// </summary>
    private bool isDestroyed = false;
    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    public virtual void Awake()
    {
        InitAwake();
    }
    public virtual void Start()
    {
        InitStart();
    }
    public virtual void Update()
    {
        UpdateMove();
        UpdateAnimation();
    }
    public virtual void FixedUpdate()
    {
        FixedUpdateMove();
    }
    #endregion Unity Methods

    #region Public Methods
    /// <summary>
    /// Initialize Awake()
    /// </summary>
    public virtual void InitAwake() { }
    /// <summary>
    /// Initialize Start()
    /// </summary>
    public virtual void InitStart() { }
    public virtual void UpdateMove() { }
    public virtual void UpdateAnimation()
    {
        // Don't move while disappearing
        if (isVanishing) return;
        // Animation time elapsed
        moveAnimationTime += Time.deltaTime;
        // Calculate the number of animation frames
        if (moveAnimationTime >= MoveAnimationSpan)
        {
            moveAnimationTime -= MoveAnimationSpan;
            // Add flyAnimationFrame
            moveAnimationFrame++;
            // If the number of frames exceeds the number of animation frames, reset to 0
            if (moveAnimationFrame >= spriteAnimationList.Count) moveAnimationFrame = 0;
        }
        // Update animation
        spriteRenderer.sprite = spriteAnimationList[moveAnimationFrame];
    }
    public virtual void FixedUpdateMove() { }
    public virtual void OnAreaActivated()
    {
        if (!isDestroyed)
            gameObject.SetActive(true);
        // For Boss
        if (isBoss)
            // Play Boss BGM
            areaManager.stageManager.PlayBossBGM();
    }
    public void Init(AreaManager areaManager)
    {
        this.areaManager = areaManager;
        this.actorTransform = this.areaManager.stageManager.actorController.transform;
        this._rigidbody2D = GetComponent<Rigidbody2D>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this._rigidbody2D.freezeRotation = true;
        this.nowHP = this.maxHP;
        if (transform.localScale.x > 0.0f) rightFacing = true;
        // エリアがアクティブになるまで何も処理せず待機
        gameObject.SetActive(false);
    }
    /// <summary>
    /// Activate this monster when actor enters the monster's area
    /// </summary>
    /// <summary>
    /// Damage to enemy
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    public bool Damaged(int damage)
    {
        // Damage to enemy
        nowHP -= damage;
        // Initialize damagedTween
        if (damageTween != null) damageTween.Kill();
        damageTween = null;

        if (nowHP <= 0)
        {   // HP0
            // Vanishing
            isVanishing = true;
            _rigidbody2D.linearVelocity = Vector2.zero;
            _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
            // 点滅後に消滅処理をCall
            this.spriteRenderer
                .DOFade(0.0f, 0.15f)        // 0.15秒ループ回数分の再生時間
                .SetEase(Ease.Linear)       // 変化の仕方
                .SetLoops(7, LoopType.Yoyo) // 7回ループ再生（偶数回は逆再生）
                .OnComplete(Vanish);        // 再生終了後 Vanish() を実行
            if (spriteDefeat != null)
                spriteRenderer.sprite = spriteDefeat;
            // When defeating the boss
            if (isBoss)
            {
                // Generate boss defeat particles
                var obj = Instantiate(bossDefeatParticlePrefab);
                obj.transform.position = transform.position;
                // Stage clear
                areaManager.stageManager.StageClear();
            }
            else { }
        }
        else
        {   // HP > 0
            // 被ダメージ演出処理（一瞬だけスプライトを赤色にする）
            if (!isInvis)
            {
                // Change to Red
                spriteRenderer.color = COL_DAMAGED;
                // Return to default color gradually
                damageTween = spriteRenderer.DOColor(COL_DEFAUL, 1.0f);
            }
        }
        return true;
    }
    /// <summary>
    /// Attack actor
    /// </summary>
    public void AttackBody(GameObject actorObject)
    {
        // 自身が消滅中なら無効
        if (isVanishing) return;
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
    /// Called when enemy is beaten
    /// </summary>
    private void Vanish()
    {
        isDestroyed = true;
        Destroy(gameObject);
    }
    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
