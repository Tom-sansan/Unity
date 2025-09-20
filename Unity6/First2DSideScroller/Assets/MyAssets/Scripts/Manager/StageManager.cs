using UnityEngine;

/// <summary>
/// StageManager Class
/// </summary>
public class StageManager : MonoBehaviour
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField
    /// <summary>
    /// Initial area in stage
    /// </summary>
    [Header("AreaManager of Initial Area")]
    [SerializeField]
    private AreaManager initAreaManager;
    /// <summary>
    /// AudioClip for Boss Battle BGM
    /// </summary>
    [SerializeField]
    private AudioClip bossBGMClip;
    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables
    /// <summary>
    /// CameraController class
    /// </summary>
    [HideInInspector]
    public CameraController cameraController;
    /// <summary>
    /// ActorController class
    /// </summary>
    [HideInInspector]
    public ActorController actorController;
    #region Public Const Variables

    #endregion Public Const Variables

    #region Public Properties

    #endregion Public Properties

    #endregion Public Variables

    #region Private Variables
    /// <summary>
    /// Array of all areas in the stage
    /// </summary>
    private AreaManager[] inStageAreas;
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
    /// Deactivate all areas in stage
    /// </summary>
    public void DeactivateAllAreas()
    {
        foreach (var targetAreaManager in inStageAreas)
            targetAreaManager.gameObject.SetActive(false);
    }
    /// <summary>
    /// Play boss battle BGM
    /// </summary>
    public void PlayBossBGM()
    {
        // Change BGM
        GetComponent<AudioSource>().clip = bossBGMClip;
        GetComponent<AudioSource>().Play();
    }
    /// <summary>
    /// Stage clear
    /// </summary>
    public void StageClear()
    {

    }
    /// <summary>
    /// Game over
    /// </summary>
    public void GameOver()
    {

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
        actorController = GetComponentInChildren<ActorController>();
        cameraController = GetComponentInChildren<CameraController>();
        inStageAreas = GetComponentsInChildren<AreaManager>();
        foreach (var targetAreaManager in inStageAreas)
            targetAreaManager.Init(this);
        // Activate initial area
        initAreaManager.ActivateArea();

    }

    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
