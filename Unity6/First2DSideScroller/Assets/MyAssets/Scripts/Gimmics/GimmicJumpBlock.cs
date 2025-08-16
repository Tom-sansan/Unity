using UnityEngine;

/// <summary>
/// GimmicJumpBlock Class
/// Stage gimmic:Jump stage
/// </summary>
public class GimmicJumpBlock : MonoBehaviour
{
    #region Variables

    #region SerializeField
    /// <summary>
    /// Jump power
    /// </summary>
    [SerializeField]
    private float jumpPower;
    #endregion SerializeField

    #endregion Variables

    #region Methods

    #region Unity Methods
    private void OnTriggerStay2D(Collider2D collision)
    {
        // 接しているのがアクターの接地判定オブジェクトでない場合は終了
        var actorGroundSensor = collision.gameObject.GetComponent<ActorGroundSensor>();
        if (actorGroundSensor == null) return;
        // アクターを移動させる
        var rigidbody2D = collision.gameObject.GetComponentInParent<Rigidbody2D>();
        rigidbody2D.linearVelocity += new Vector2(0.0f, jumpPower);
    }
    #endregion Unity Methods

    #endregion Methods
}
