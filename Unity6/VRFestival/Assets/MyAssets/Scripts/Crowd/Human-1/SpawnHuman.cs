using UnityEngine;

/// <summary>
/// SpawnHuman Class
/// Spawn worshippers
/// </summary>
public class SpawnHuman : MonoBehaviour
{
    #region Variables

    #region SerializeField

    /// <summary>
    /// Prefab for worshipper
    /// </summary>
    [SerializeField]
    private GameObject worshipperPrefab;
    /// <summary>
    /// The number of worshippers at the first time
    /// </summary>
    [SerializeField]
    private int firstNumberWorshipper;

    #endregion SerializeField

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {
        Init();
    }
    #endregion Unity Methods

    /// <summary>
    /// Initialize this class
    /// </summary>
    private void Init()
    {
        // Spawn worshippers designated by firstNumberWorshipper
        for (int i = 0; i < firstNumberWorshipper; i++)
            InstantiateWorshipperPrefab();
        InvokeSpawnWorshipper();
    }
    /// <summary>
    /// Spaw worshipper
    /// </summary>
    private void SpawnWorshipper()
    {
        InstantiateWorshipperPrefab();
        InvokeSpawnWorshipper();
    }
    /// <summary>
    /// Instantiate WorshipperPrefab
    /// </summary>
    private void InstantiateWorshipperPrefab() =>
        Instantiate(worshipperPrefab, transform.position, transform.rotation);
    /// <summary>
    /// Invoke SpawnWorshipper
    /// </summary>
    private void InvokeSpawnWorshipper() =>
        Invoke(nameof(SpawnWorshipper), Random.Range(3, 5));

    #endregion Methods
}
