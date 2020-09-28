using UnityEngine;

/// <summary>
/// Item List
/// </summary>
public class ItemsDialog : MonoBehaviour
{
    [SerializeField] private int buttonNumber = 15;
    [SerializeField] private ItemButton itemButton;
    private ItemButton[] _itemButtons;

    void Start()
    {
        // Disable to show Item Dialog
        gameObject.SetActive(false);

        // Copy necessary item rows
        for (var i = 0; i < buttonNumber - 1; i++)
        {
            Instantiate(itemButton, transform);
        }

        _itemButtons = GetComponentsInChildren<ItemButton>();
    }

    /// <summary>
    /// Toggle of item list
    /// </summary>
    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        if (gameObject.activeSelf)
        {
            // Refresh item list if displayed
            for (var i = 0; i < buttonNumber; i++)
            {
                // Set owned item info to each item
                _itemButtons[i].OwnedItem = OwnedItemsData.Instance.OwnedItems.Length > i ?
                    OwnedItemsData.Instance.OwnedItems[i] :
                    null;
            }
        }
    }
}
