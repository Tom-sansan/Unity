using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// EnemyScorpion Class
/// Tornado attack without movement
/// </summary>
public class EnemyScorpion : EnemyBase
{
    #region Variables

    #region SerializeField
    /// <summary>
    /// Tornado bullet prefab
    /// </summary>
    [SerializeField]
    private GameObject tornadoBulletPrefab;
    /// <summary>
    /// Attack interval
    /// </summary>
    [SerializeField]
    private float attackInterval;
    #endregion SerializeField

    #region Private Variables
    /// <summary>
    /// Time count
    /// </summary>
    private float timeCount;
    /// <summary>
    /// Next action time
    /// </summary>
    private float nextActionTime;
    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods

    #endregion Unity Methods

    #region Public Methods
    public override void InitStart()
    {
        timeCount = -1.0f;
        nextActionTime = attackInterval;
    }
    public override void UpdateAnimation()
    {
        //  減速中なら処理しない
        if (isVanishing) return;
        // アクターとの位置関係から向きを決定
        if (transform.position.x > actorTransform.position.x)
            // Face left
            SetFacingRight(false);
        else
            // Face right
            SetFacingRight(true);
        // Elapsed time
        timeCount += Time.deltaTime;
        // Set sprite
        moveAnimationFrame = (int)(timeCount * spriteAnimationList.Count / attackInterval);
        moveAnimationFrame = Mathf.Clamp(moveAnimationFrame, 0, spriteAnimationList.Count - 1);
        spriteRenderer.sprite = spriteAnimationList[moveAnimationFrame];
        // End process if this moment is not move timing
        if (timeCount < attackInterval) return;
        // Start move
        timeCount = 0.0f;
        // Tornado attack
        ShotTornadoBullet();
    }
    #endregion Public Methods

    #region Private Methods
    /// <summary>
    /// Shoot tornado bullet
    /// </summary>
    private void ShotTornadoBullet()
    {
        // Create bullet object
        Vector2 startPos = transform.position;
        // Omnidirectional launch prcessing(全方位発射処理)
        GameObject obj = Instantiate(tornadoBulletPrefab, startPos, Quaternion.identity);
        obj.GetComponent<EnemyShot>().Init
        (
            2.4f,                   // speed
            rightFacing ? 0 : 180,  // angle
            4.2f,                   // limit time
            3,                      // damage
            true                    // Disappear if this is hit on ground
        );
    }
    #endregion Private Methods

    #endregion Methods
}
