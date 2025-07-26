using UnityEngine;

/// <summary>
/// ActorNormalShot Class
/// </summary>
public class ActorNormalShot : MonoBehaviour
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

    #region Proteced Variables
    /// <summary>
    /// Shot speed
    /// </summary>
    protected float speed;
    /// <summary>
    /// Shot angle
    /// </summary>
    protected float angle;
    /// <summary>
    /// Existence time (seconds) Disappears after this time has elapsed
    /// </summary>
    protected float limitTime;
    /// <summary>
    /// Damage upon hitting
    /// </summary>
    protected int damage;
    #endregion Proteced Variables

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
        UpdateShot();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        // Hit enemy
        if (tag == "Enemy")
        {
            EnemyBase enemyBase = collision.gameObject.GetComponent<EnemyBase>();
            if (enemyBase != null && !enemyBase.isInvis)
            {
                bool result = enemyBase.Damaged(damage);
                // Delete bullet object if damaged
                if (result) Destroy(gameObject);
            }
            enemyBase.Damaged(damage);
            Destroy(gameObject);
        }
        // Hit ground or wall
        else if (tag == "Ground") Destroy(gameObject);
    }
    #endregion Unity Methods

    #region Public Methods
    /// <summary>
    /// Initialize from ActorController.cs
    /// </summary>
    /// <param name="speed">Bullet velocity</param>
    /// <param name="angle">Bullet angle</param>
    /// <param name="limitTime">Bullet life time</param>
    /// <param name="damage">Bullet damage</param>
    public void Init(float speed, float angle, float limitTime, int damage)
    {
        this.speed = speed;
        this.angle = angle;
        this.limitTime = limitTime;
        this.damage = damage;
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
    /// Update Shot
    /// </summary>
    private void UpdateShot()
    {
        // 移動ベクトル計算（1フレーム分の進行方向と距離を取得）
        Vector3 vec = new Vector3(speed * Time.deltaTime, 0.0f, 0.0f);
        vec = Quaternion.Euler(0, 0, angle) * vec;
        // 移動ベクトルをもとに移動
        transform.Translate(vec);
        // 消滅判定
        limitTime -= Time.deltaTime;
        // 存在時間が0になったら消滅
        if (limitTime <= 0.0f) Destroy(gameObject);
    }
    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
