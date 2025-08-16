using UnityEngine;

/// <summary>
/// EnemyShot Class
/// </summary>
public class EnemyShot : MonoBehaviour
{
    #region Variables

    #region Private Variables
    /// <summary>
    /// Move vector
    /// </summary>
    // private Vector3 vec;
    /// <summary>
    /// Shot speed
    /// </summary>
    private float speed;
    /// <summary>
    /// Angle(角度(0-360)0で右・90で上)
    /// </summary>
    private float angle;
    /// <summary>
    /// Existence time (seconds). Disappears after this time has elapsed.
    /// </summary>
    private float limitTime;
    /// <summary>
    /// Shot damage
    /// </summary>
    private int damage;
    /// <summary>
    /// True: Disappear when hit on ground or wall
    /// </summary>
    private bool isDestroyHitToGround;
    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Update()
    {
        UpdateShot();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag == "Actor")
        {
            // Hit actor
            var actorController = collision.gameObject.GetComponent<ActorController>();
            if (actorController != null) actorController.Damaged(damage);
        }
        else if (tag == "Ground")
        {
            // Hit ground/wall
            if (isDestroyHitToGround) Destroy(gameObject);
        }
    }
    #endregion Unity Methods

    #region Public Methods
    /// <summary>
    /// Initialize
    /// </summary>
    /// <param name="speed"></param>
    /// <param name="angle"></param>
    /// <param name="limitTime"></param>
    /// <param name="damage"></param>
    /// <param name="isDestroyHitToGround"></param>
    public void Init(float speed, float angle, float limitTime, int damage, bool isDestroyHitToGround)
    {
        this.speed = speed;
        this.angle = angle;
        this.limitTime = limitTime;
        this.damage = damage;
        this.isDestroyHitToGround = isDestroyHitToGround;
    }
    #endregion Public Methods

    #region Private Methods
    private void UpdateShot()
    {
        // Calculate move vector
        Vector3 vec = new Vector3(speed * Time.deltaTime, 0.0f, 0.0f);
        // Vector rotation
        vec = Quaternion.Euler(0, 0, angle) * vec;
        // Move
        transform.Translate(vec);
        // elimination judgment
        limitTime -= Time.deltaTime;
        if (limitTime <= 0.0f) Destroy(gameObject);
    }
    #endregion Private Methods

    #endregion Methods
}
