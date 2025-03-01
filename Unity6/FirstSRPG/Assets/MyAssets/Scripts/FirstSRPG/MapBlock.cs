using UnityEngine;

/// <summary>
/// MapBlock Class
/// </summary>
public class MapBlock : MonoBehaviour
{
    #region Nested Class

    #endregion Nested Class

    #region Enum
    /// <summary>
    /// Define the highlighting mode of the block
    /// ブロックの強調表示モードを定義する
    /// </summary>
    public enum Highlight
    {
        // Not selected
        Off,
        // Character is selected
        Select,
        // Character can reach
        // キャラクターが到達可能
        Reachable,
        // Character can attack
        // キャラクターが攻撃可能
        Attackable,
    }
    #endregion Enum

    #region Variables

    #region SerializeField
    /// <summary>
    /// Highlighted material: Selected
    /// 強調表示マテリアル：選択時
    /// </summary>
    [Header("Highlighted material: Selected")]
    [SerializeField]
    private Material selMat_Select;
    /// <summary>
    /// Highlighted material: Reachable
    /// 強調表示マテリアル：到達可能
    /// キャラクターが到達可能
    /// </summary>
    [Header("Highlighted material: Reachable")]
    [SerializeField]
    private Material selMat_Reachable;
    /// <summary>
    /// Highlighted material: Reachable
    /// 強調表示マテリアル：攻撃可能
    /// キャラクターが攻撃可能
    /// </summary>
    [Header("Highlighted material: Attackable")]
    [SerializeField]
    private Material selMat_Attackable;
    /// <summary>
    /// Passable flag (true if passable)
    /// 通行可能フラグ（trueなら通行可能）
    /// </summary>
    [Header("Passable flag")]
    [SerializeField]
    public bool IsPassable;
    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables
    /// <summary>
    /// X-directional position
    /// X方向の位置
    /// </summary>
    [HideInInspector]
    public int posX;
    /// <summary>
    /// Z-directional position
    /// Z方向の位置
    /// </summary>
    [HideInInspector]
    public int posZ;
    #region Public Properties

    #endregion Public Properties

    #endregion Public Variables

    #region Private Variables
    /// <summary>
    /// highlighting object
    /// </summary>
    private GameObject selectionBlockObj;
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
    #endregion Unity Methods

    #region Public Methods
    /// <summary>
    /// Show or hide highlighted objects
    /// 強調表示オブジェクトの表示・非表示を設定する
    /// </summary>
    /// <param name="mode">Selection status (true: selected)</param>
    public void SetSelectionMode(Highlight mode)
    {
        // Show highlighted object if selected
        // 選択中(true)なら強調表示オブジェクトを表示
        // Hide highlighted object if not selected
        // 選択中でない(false)なら強調表示オブジェクトを非表示
        // selectionBlockObj.SetActive(mode);

        switch (mode)
        {
            // 強調表示なし
            case Highlight.Off:
                selectionBlockObj.SetActive(false);
                break;
            // 選択時
            case Highlight.Select:
                selectionBlockObj.GetComponent<Renderer>().material = selMat_Select;
                selectionBlockObj.SetActive(true);
                break;
            // キャラクターが到達可能
            case Highlight.Reachable:
                selectionBlockObj.GetComponent<Renderer>().material = selMat_Reachable;
                selectionBlockObj.SetActive(true);
                break;
            // キャラクターが攻撃可能
            case Highlight.Attackable:
                selectionBlockObj.GetComponent<Renderer>().material = selMat_Attackable;
                selectionBlockObj.SetActive(true);
                break;
            default:
                break;
        }
    }
    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialize this class
    /// </summary>
    private void Init()
    {
        // Get the highlighting object
        selectionBlockObj = transform.GetChild(0).gameObject;
        // Not highlighted in the initial display
        // 初期表示では強調表示しない
        SetSelectionMode(Highlight.Off);
    }
    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
