using UnityEngine;

/// <summary>
/// Sheep class
/// </summary>
public class Sheep : MonoBehaviour
{
    #region Variables
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
    #endregion

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
    /// <summary>
    /// Mouse over event
    /// </summary>
    private void OnMouseOver()
    {
        if (!Input.GetMouseButton(0)) return;
        Shaving();
    }
    /// <summary>
    /// Shaving process
    /// </summary>
    private void Shaving()
    {
        sheepRenderer.sprite = cutSheepSprite;
        var wool = Instantiate(woolPrefab, transform.position, transform.rotation);
    }
}
