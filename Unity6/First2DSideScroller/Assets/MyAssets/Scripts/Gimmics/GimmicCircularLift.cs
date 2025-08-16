using UnityEngine;

/// <summary>
/// GimmicCircularLift Class
/// Stage gimmic:Circular lift
/// Circular lift that moves in a circle
/// </summary>
public class GimmicCircularLift : MonoBehaviour
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField
    /// <summary>
    /// Moving time
    /// </summary>
    [SerializeField]
    private float MovingTime;
    /// <summary>
    /// Radius value
    /// </summary>
    [SerializeField]
    private float Radius;
    /// <summary>
    /// Initial angle(0-360)
    /// </summary>
    [SerializeField]
    private float InitialAngle;
    /// <summary>
    /// 回転方向反転フラグ
    /// </summary>
    [SerializeField]
    private bool isReverse;
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
    /// Rigidbody2D
    /// </summary>
    private Rigidbody2D _rigidbody2D;
    /// <summary>
    /// Actor Transform(Get when actor is on)
    /// </summary>
    private Transform actorTransform;
    /// <summary>
    /// ActorGroundSensor
    /// </summary>
    private ActorGroundSensor actorGroundSensor;
    /// <summary>
    /// Default position
    /// </summary>
    private Vector3 defaultPos;
    /// <summary>
    /// Move vector
    /// </summary>
    private Vector3 moveVec;
    /// <summary>
    /// Elapsed time
    /// </summary>
    private float elapsedTime;
    /// <summary>
    /// True:Actor riding
    /// </summary>
    private bool isActorRiding;
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
    void FixedUpdate()
    {
        FixedUpdateActorMove();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        var componet = collision.gameObject.GetComponent<ActorGroundSensor>();
        // 接しているのがアクターの接地判定オブジェクトでない場合は終了
        if (componet == null) return;

        actorGroundSensor = componet;
        actorGroundSensor.IsGround = true;
        // Get Actor's transform
        actorTransform = collision.gameObject.transform.parent;
        // アクターを移動させる処理の予約
        isActorRiding = true;
    }
    #endregion Unity Methods

    #region Public Methods

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
        _rigidbody2D = GetComponent<Rigidbody2D>();
        // Disable rotation
        _rigidbody2D.freezeRotation = true;
        defaultPos = transform.position;
        moveVec = Vector3.zero;
        elapsedTime = 0.0f;
        // Avoid error
        if (MovingTime < 0.1f) MovingTime = 0.1f;
        // 初期の移動進行率反映
        elapsedTime = MovingTime * InitialAngle * Mathf.Deg2Rad;
    }
    /// <summary>
    /// Move Actor in FixedUpdate
    /// </summary>
    private void FixedUpdateActorMove()
    {
        // Elapsed time
        elapsedTime += Time.fixedDeltaTime;
        // Calculate movement vector
        float moveValue = elapsedTime / MovingTime;
        if (isReverse) moveValue *= -1.0f;
        Vector3 vec = new Vector3(Mathf.Sin(moveValue) * Radius, Mathf.Cos(moveValue) * Radius, 0.0f);
        // 前フレームからの位置変化量を取得（乗ったアクターを移動させる処理に使用する）
        moveVec = (defaultPos + vec) - transform.position;
        _rigidbody2D.MovePosition(defaultPos + vec);
        // アクターが乗っているならアクターを移動
        if (isActorRiding)
        {
            isActorRiding = false;
            actorTransform.Translate(moveVec);
            actorGroundSensor.IsGround = true;
        }
    }
    #endregion Private Methods

    #endregion Methods
}
