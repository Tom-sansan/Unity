using UnityEngine;

/// <summary>
/// GimmicLift Class
/// Up and downn or left and right movement
/// </summary>
public class GimmicLift : MonoBehaviour
{
    #region Variables

    #region SerializeField
    /// <summary>
    /// Moving time
    /// </summary>
    [SerializeField]
    private float MovingTime;
    /// <summary>
    /// Moving speed
    /// </summary>
    [SerializeField]
    private float MovingSpeed;
    /// <summary>
    /// Moving angle(0-360:right, 90:up)
    /// </summary>
    [SerializeField]
    private float MovingAngle;
    #endregion SerializeField

    #region Private Variables
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
    private Vector2 moveVec;
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
    void Start()
    {
        InitStart();
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

    #region Private Methods
    /// <summary>
    /// Initialize Start()
    /// </summary>
    private void InitStart()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        // Disable rotation
        _rigidbody2D.freezeRotation = true;
        actorTransform = transform;
        defaultPos = transform.position;
        moveVec = Vector2.zero;
        elapsedTime = 0.0f;
        // Avoid error
        if (MovingTime < 0.1f) MovingTime = 0.1f;
    }
    /// <summary>
    /// Move Actor in FixedUpdate
    /// </summary>
    private void FixedUpdateActorMove()
    {
        // Elapsed time
        elapsedTime += Time.fixedDeltaTime;
        // Calculate movement vector
        Vector3 vec = new Vector3((Mathf.Sin(elapsedTime / MovingTime) + 1.0f) * MovingSpeed, 0.0f, 0.0f);
        vec = Quaternion.Euler(0, 0, MovingAngle) * vec;
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
