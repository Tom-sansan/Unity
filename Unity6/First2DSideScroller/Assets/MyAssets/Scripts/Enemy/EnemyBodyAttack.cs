using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// EnemyBodyAttack Class
/// 全敵共通：エネミーとアクターが接触した時アクターにダメージを発生させる処理
/// </summary>
public class EnemyBodyAttack : MonoBehaviour
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

    #region Private Variables

    #region Private Const Variables

    #endregion Private Const Variables
    /// <summary>
    /// EnemyBase class
    /// </summary>
    private EnemyBase enemyBase;
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
    private void OnTriggerStay2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag == "Actor")
            // アクターに接触ダメージを与える
            enemyBase.AttackBody(collision.gameObject);
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
        enemyBase = GetComponentInParent<EnemyBase>();
        var Coll_TouchArea = GetComponent<BoxCollider2D>();
        var Coll_Body = enemyBase.gameObject.GetComponent<BoxCollider2D>();
        Coll_TouchArea.offset = Coll_Body.offset;
        Coll_TouchArea.size = Coll_Body.size;
        Coll_TouchArea.size *= 0.8f;
    }

    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
