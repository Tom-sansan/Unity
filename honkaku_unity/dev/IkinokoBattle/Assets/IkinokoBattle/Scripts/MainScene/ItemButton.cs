using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Button to show item info
/// </summary>
[RequireComponent(typeof(Button))]
public class ItemButton : MonoBehaviour
{
    // Image of each item
    [SerializeField] private ItemTypeSpriteMap[] itemSprites;
    [SerializeField] private Image image;
    [SerializeField] private Text number;
    private Button _button;

    private OwnedItemsData.OwnedItem _ownedItem;
    public OwnedItemsData.OwnedItem OwnedItem
    {
        get => _ownedItem;
        set
        {
            _ownedItem = value;
            // アイテムが割り当てられたかどうかでアイテム画像や所持個数の表示を切り替える
            var isEmpty = null == _ownedItem;
            image.gameObject.SetActive(!isEmpty);
            number.gameObject.SetActive(!isEmpty);
            _button.interactable = !isEmpty;
            if (!isEmpty)
            {
                image.sprite = itemSprites.First(x => x.itemType == _ownedItem.Type).sprite;
                number.text = "" + _ownedItem.Number;
            }
        }
    }

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        // TODO
    }

    /// <summary>
    /// Class for relate item type and Sprite by Inspector
    /// </summary>
    [Serializable]
    public class ItemTypeSpriteMap
    {
        public Item.ItemType itemType;
        public Sprite sprite;
    }
}
