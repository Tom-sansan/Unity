using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Save or Retrieve Owned Items
/// </summary>
[Serializable]
public class OwnedItemsData
{
    // Save key for PlaylerPrefs
    private const string PlayerPrefsKey = "OWNED_ITEMS_DATA";

    // Constructor
    // Note: Make constructor private not to create an instance from outside for Singleton
    private OwnedItemsData() { }

    private static OwnedItemsData _instance;
    public static OwnedItemsData Instance
    {
        get
        {
            if (null == _instance)
            {
                _instance = PlayerPrefs.HasKey(PlayerPrefsKey)
                    ? JsonUtility.FromJson<OwnedItemsData>(PlayerPrefs.GetString(PlayerPrefsKey))
                    : new OwnedItemsData();
            }
            return _instance;
        }
    }

    [SerializeField] private List<OwnedItem> ownedItems = new List<OwnedItem>();
    public OwnedItem[] OwnedItems => ownedItems.ToArray();
    

    /// <summary>
    /// Save in PlayerPrefs by JSON
    /// </summary>
    public void Save()
    {
        var jsonString = JsonUtility.ToJson(this);
        PlayerPrefs.SetString(PlayerPrefsKey, jsonString);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Add item
    /// </summary>
    /// <param name="type">Item Type</param>
    /// <param name="number">Number</param>
    public void Add(Item.ItemType type, int number = 1)
    {
        var item = GetItem(type);
        if (null == item)
        {
            item = new OwnedItem(type);
            ownedItems.Add(item);
        }
        item.Add(number);
    }

    /// <summary>
    /// Use item
    /// </summary>
    /// <param name="type"></param>
    /// <param name="number"></param>
    public void Use(Item.ItemType type, int number = 1)
    {
        var item = GetItem(type);
        if (null == item || item.Number < number) throw new Exception("You don't have your items.");
        item.Use(number);
    }

    /// <summary>
    /// Get item of target type
    /// </summary>
    /// <param name="type">Item type</param>
    /// <returns></returns>
    public OwnedItem GetItem(Item.ItemType type)
    {
        return ownedItems.FirstOrDefault(x => x.Type == type);
    }

    /// <summary>
    /// Owned item model
    /// </summary>
    [Serializable]
    public class OwnedItem
    {
        /// <summary>
        /// Item type
        /// </summary>
        [SerializeField] private Item.ItemType type;
        public Item.ItemType Type => type;

        /// <summary>
        /// The number of owned items
        /// </summary>
        [SerializeField] private int number;
        public int Number => number;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type"></param>
        public OwnedItem(Item.ItemType type)
        {
            this.type = type;
        }

        public void Add(int number = 1)
        {
            this.number += number;
        }

        public void Use(int number = 1)
        {
            this.number -= number;
        }
    }
}
