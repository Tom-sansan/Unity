using UnityEngine;

/// <summary>
/// Generate item when enemy is down.
/// </summary>
[RequireComponent(typeof(MobStatus))]
public class MobItemDropper : MonoBehaviour
{
    // Rate of item generation
    [SerializeField] [Range(0, 1)] private float dropRate = 0.1f;
    [SerializeField] private Item itemPrefab;
    // Number of item generation
    [SerializeField] private int number = 1;

    private MobStatus _status;
    private bool _isDropInvoked;

    // Start is called before the first frame update
    void Start()
    {
        _status = GetComponent<MobStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_status.Life <= 0) DropIfNeeded();
    }

    /// <summary>
    /// Generate item if necessary
    /// </summary>
    private void DropIfNeeded()
    {
        if (_isDropInvoked) return;

        _isDropInvoked = true;

        if (Random.Range(0, 1f) >= dropRate) return;

        // Generate items of designated numbers
        for (int i = 0; i < number; i++)
        {
            var item = Instantiate(itemPrefab, transform.position, Quaternion.identity);
            item.Initialize();
        }
    }
}
