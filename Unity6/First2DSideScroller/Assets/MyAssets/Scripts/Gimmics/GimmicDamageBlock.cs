using UnityEngine;

/// <summary>
/// GimmicDamageBlock Class
/// Stage gimmic:Damage block
/// Actor on block(or Actor who enters trigger) is damaged.
/// </summary>
public class GimmicDamageBlock : MonoBehaviour
{
    #region Variables

    #region SerializeField
    /// <summary>
    /// The amount of damage
    /// </summary>
    [SerializeField]
    private int damage;
    #endregion SerializeField

    #endregion Variables

    #region Methods

    #region Unity Methods
    private void OnTriggerStay2D(Collider2D collision)
    {
        DamageToActor(collision.gameObject);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        DamageToActor(collision.gameObject);
    }
    #endregion Unity Methods

    #region Private Methods
    /// <summary>
    /// Damage to Actor
    /// </summary>
    /// <param name="gameObj"></param>
    private void DamageToActor(GameObject gameObj)
    {
        string tag = gameObj.tag;
        if (tag.Equals("Actor")) gameObj.GetComponent<ActorController>().Damaged(damage);
    }
    #endregion Private Methods

    #endregion Methods
}
