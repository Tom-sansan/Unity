using UnityEngine;

/// <summary>
/// Sheep class
/// </summary>
public class Sheep : MonoBehaviour
{
    #region Variables

    #region SerializeField
    /// <summary>
    /// Sheep renderer
    /// </summary>
    [SerializeField]
    private SpriteRenderer sheepRenderer;
    /// <summary>
    /// For sheep replacement
    /// </summary>
    [SerializeField]
    private Sprite cutSheepSprite;
    /// <summary>
    /// Wool prefab
    /// </summary>
    [SerializeField]
    private Wool woolPrefab;
    #endregion SerializeField

    #region Public Variables
    /// <summary>
    /// Initial data of sheep
    /// </summary>
    public SheepData sheepData;
    #endregion Public Variables

    #region Private Variables
    /// <summary>
    /// Image of the first sheep
    /// </summary>
    private Sprite defaultSprite;
    /// <summary>
    /// Moving speed
    /// </summary>
    private float moveSpeed;
    /// <summary>
    /// Wool amount
    /// </summary>
    private int woolAmount;
    #endregion Private Variables

    #endregion Variables

    #region Unity Methods
    private void Start()
    {
        defaultSprite = sheepRenderer.sprite;
        Initialize();
    }

    private void Update()
    {
        transform.position += new Vector3(moveSpeed, 0) * Time.deltaTime;
        if (transform.position.x < -5) Initialize();
    }
    /// <summary>
    /// Mouse over event
    /// </summary>
    private void OnMouseOver()
    {
        if (!Input.GetMouseButton(0)) return;
        Shaving();
    }
    #endregion Unity Methods

    #region Private Methods
    /// <summary>
    /// Initialization
    /// </summary>
    private void Initialize()
    {
        sheepRenderer.sprite = defaultSprite;
        transform.position = new Vector3(5, Random.Range(0.0f, 4.0f), 0);
        moveSpeed = -Random.Range(1.0f, 2.0f);
        sheepRenderer.color = sheepData.color;
        woolAmount = sheepData.woolAmount;
    }
    /// <summary>
    /// Shaving process
    /// </summary>
    private void Shaving()
    {
        // Do nothing as there is no more wool to shear
        if (woolAmount <= 0) return;
        // 30-40% of wool is sheared
        var shavingWool = (int)(sheepData.woolAmount * Random.Range(0.3f, 0.4f));
        // Set the upper limit as cannot remove more wool than is left in the sheep
        if (woolAmount < shavingWool) shavingWool = woolAmount;
        // var shavingWool = (int)Mathf.Min(woolAmount, sheepData.woolAmount * Random.Range(0.3f, 0.4f));
        // Reduce the amount of wool retained for this harvest
        woolAmount -= shavingWool;
        if (woolAmount <= 0)
        {
            // Replace image with cut one
            sheepRenderer.sprite = cutSheepSprite;
            // Back to white color as no more wool
            sheepRenderer.color = Color.white;
        }
        var wool = Instantiate(woolPrefab, transform.position, transform.rotation);
        // Set the Wool object the wool you have sheared this time
        wool.price = sheepData.basePrice;
        wool.woolColor = sheepData.color;
    }
    #endregion Private Methods
}
