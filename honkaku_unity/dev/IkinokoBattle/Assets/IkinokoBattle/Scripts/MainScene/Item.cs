using DG.Tweening;
using UnityEngine;

/// <summary>
/// Generate item from enemy
/// </summary>
[RequireComponent(typeof(Collider))]
public class Item : MonoBehaviour
{
    public enum ItemType
    {
        Wood,
        Stone,
        ThrowAxe
    }

    [SerializeField] private ItemType type;

    public void Initialize()
    {
        // Disable collider until animation is ended.
        var colliderCache = GetComponent<Collider>();
        colliderCache.enabled = false;
        //
        var transformCache = transform;
        var dropPosition = transform.localPosition + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        transformCache.DOLocalMove(dropPosition, 0.5f);
        var defaultScale = transformCache.localScale;
        transformCache.localScale = Vector3.zero;
        transformCache.DOScale(defaultScale, 0.5f)
            .SetEase(Ease.OutBounce)
            .OnComplete(() =>
            {
                // Enable collider once animation is finished
                colliderCache.enabled = true;
            });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Add item as Player's possession
        OwnedItemsData.Instance.Add(type);
        OwnedItemsData.Instance.Save();

        // Owned Item Log
        foreach (var item in OwnedItemsData.Instance.OwnedItems)
        {
            Debug.Log("Owned Item Type: " + item.Type + ", Owned number: " + item.Number);
        }

        // Destroy object
        Destroy(gameObject);
    }
}
