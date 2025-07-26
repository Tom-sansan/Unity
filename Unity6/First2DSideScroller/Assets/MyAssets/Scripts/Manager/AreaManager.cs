using UnityEngine;

/// <summary>
/// AreaManager Class
/// </summary>
public class AreaManager : MonoBehaviour
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
    /// <summary>
    /// StageManager class
    /// </summary>
    [HideInInspector]
    public StageManager stageManager;
    #endregion Public Properties

    #endregion Public Variables

    #region Private Variables
    /// <summary>
    /// CameraMovingLimitter class
    /// </summary>
    private CameraMovingLimitter cameraMovingLimitter;
    /// <summary>
    /// Array of all enemies in this area
    /// </summary>
    private EnemyBase[] inAreaEnemies;
    #region Private Const Variables

    #endregion Private Const Variables

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
    #endregion Unity Methods

    #region Public Methods
    /// <summary>
    /// Initialize this class (called from StageManager.cs)
    /// </summary>
    /// <param name="_stageManager"></param>
    public void Init(StageManager _stageManager)
    {
        this.stageManager = _stageManager;
        cameraMovingLimitter = GetComponentInChildren<CameraMovingLimitter>();
        // Get all enemies in this area
        inAreaEnemies = GetComponentsInChildren<EnemyBase>();
        foreach (var targetEnemyBase in inAreaEnemies)
            targetEnemyBase.Init(this);
        // Deactivate this area until when actor enters
        gameObject.SetActive(false);
    }
    /// <summary>
    /// Activate this area
    /// </summary>
    public void ActivateArea()
    {
        // Deactivate all areas
        stageManager.DeactivateAllAreas();
        // Activate this area
        gameObject.SetActive(true);
        // Activate all enemies in this area
        foreach (var targetEnemyBase in inAreaEnemies)
            targetEnemyBase.OnAreaActivated();
        // Change camera moving limitter
        stageManager.cameraController.ChangeCameraMovingLimitter(cameraMovingLimitter);
    }
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

    }

    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
