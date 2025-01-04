using UnityEngine;

/// <summary>
/// ArrowStopper Class
/// </summary>
public class ArrowStopper : MonoBehaviour
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

    #region Public Properties

    #endregion Public Properties

    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// When the arrow is stopped, the arrow's rigidbody is retained
    /// 矢を止めたら、その矢の Rigidbody を保持
    /// </summary>
    private Rigidbody holdingArrow;

    #region Private Properties

    #endregion Private Properties

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {
        Init();
    }
    void Update()
    {

    }

    /// <summary>
    /// Trigger when the arrow hits the stopper
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        // If the arrow hits, take out the arrow's Rigidbody and stop the fall
        // 矢が当たった場合、矢の Rigidbody を取り出し落下を止める
        if (other.CompareTag("WoodenArrow"))
        {
            holdingArrow = other.GetComponent<Rigidbody>();
            holdingArrow.isKinematic = true;
            holdingArrow.useGravity = false;
        }
    }
    #endregion Unity Methods

    #region Public Methods

    /// <summary>
    /// If the Rigidbody of the arrow is retained, resume the fall
    /// もし矢の Rigidbody が保持されていたら、落下を再開させる
    /// </summary>
    public void ReleaseArrow()
    {
        if (holdingArrow != null)
        {
            holdingArrow.isKinematic = false;
            holdingArrow.useGravity = true;
            // The arrow has been released, so disable the Rigidbody retention
            // 矢は放したので Rigidbody の保持を無効にする
            holdingArrow = null;
        }
    }

    #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Initialize this class
        /// </summary>
    private void Init()
    {

    }

    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
