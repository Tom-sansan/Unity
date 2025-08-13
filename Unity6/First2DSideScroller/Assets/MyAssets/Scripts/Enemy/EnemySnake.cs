using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// EnemySnake Class
/// Aproach to actor when the actor is close to the snake.
/// Snake doesn't attach but do body attack.
/// </summary>
public class EnemySnake : EnemyBase
{
    #region Variables

    #region SerializeField
    /// <summary>
    /// Movement condition(it moves less than or equal to the value)
    /// </summary>
    [SerializeField]
    private float awakeDistance;
    /// <summary>
    /// 非移動時減速率
    /// </summary>
    [SerializeField]
    private float brakeRatio;
    #endregion SerializeField

    #region Private Variables
    /// <summary>
    /// Flag for breaking(Slow down when true)
    /// </summary>
    private bool isBreaking;
    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    public override void FixedUpdate()
    {
        Breaking();
    }
    #endregion Unity Methods

    #region Public Methods

    public override void OnAreaActivated()
    {
        base.OnAreaActivated();
        Debug.Log("OnAreaActivated()");
    }
    #endregion Public Methods

    #region Private Methods
    public override void UpdateMove()
    {
        //  減速中なら処理しない
        if (isVanishing) return;
        // Approching if actor is close
        // x方向移動速度
        var speed = 0.0f;
        // Enemy's coordinates
        Vector2 ePos = transform.position;
        // Actor's coordinates
        Vector2 aPos = actorTransform.position;
        // アクターとの距離が離れている場合はブレーキフラグを立て終了（移動しない）
        if (Vector2.Distance(ePos, aPos) > awakeDistance)
        {
            isBreaking = true;
            return;
        }
        // 離れていないならブレーキフラグfalse
        isBreaking = false;
        // アクターとの位置関係から向きを決定
        if (ePos.x > aPos.x)
        {
            // Face left
            speed -= movingSpeed;
            SetFacingRight(false);
        }
        else
        {
            // Face right
            speed = movingSpeed;
            SetFacingRight(true);
        }
        // Movement
        Vector2 vec = _rigidbody2D.linearVelocity;
        vec.x += speed * Time.deltaTime;
        // Set maximum speed to x direction
        // Right direction
        if (vec.x > 0.0f) vec.x = Mathf.Clamp(vec.x, 0.0f, maxSpeed);
        // Left direction
        else vec.x = Mathf.Clamp(vec.x, -maxSpeed, 0.0f);
        // 速度ベクトルをセット
        _rigidbody2D.linearVelocity = vec;
    }
    /// <summary>
    /// Breaking process
    /// </summary>
    private void Breaking()
    {
        // Break when actor is not close to the enemy
        if (isBreaking)
        {
            Vector2 vec = _rigidbody2D.linearVelocity;
            // Slow down only for x direction
            vec.x *= brakeRatio;
            _rigidbody2D.linearVelocity = vec;
        }
    }
    #endregion Private Methods

    #endregion Methods
}
