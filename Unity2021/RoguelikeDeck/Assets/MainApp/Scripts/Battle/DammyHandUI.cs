using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DammyHandUI : MonoBehaviour
{
    #region Variables

    #region SerializeField

    /// <summary>
    /// HorizontalLayoutGroup for dummy hand alignment
    /// </summary>
    [SerializeField]
    private HorizontalLayoutGroup layoutGroup = null;
    /// <summary>
    /// Dummy Hand Prefab
    /// </summary>
    [SerializeField]
    private GameObject dammyHandPrefab = null;

    #endregion SerializeField

    #region Public Variables

    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// List of dummy hands generated
    /// </summary>
    private List<Transform> dammyHandList;

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {

    }
    void Update()
    {

    }
    #endregion Unity Methods

    #region Public Methods

    /// <summary>
    /// Create or delete a dummy hand to reach a specified number of cards
    /// </summary>
    /// <param name="value"></param>
    public void SetHandNum(int value)
    {
        if (dammyHandList == null)
        {
            // Initial run time
            // Initialize list
            dammyHandList = new List<Transform>();
            // AddhandObj(value);
        }
        else
        {
            // Calculate the number of pieces that change from the current
            int differentNum = value - dammyHandList.Count;
            // Dummy hand creation/deletion
            if (differentNum > 0)
                // Dummy hand creation if more cards are in hand
                AddHandObj(differentNum);
            else if (differentNum < 0)
                // Dummy hand deletion if the hand is reduced
                RemoveHandObj(differentNum);
        }
    }
    /// <summary>
    /// Returns the coordinates of the dummy hand with the corresponding number
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Vector2 GetHandPos(int index)
    {
        if (index < 0 || index >= dammyHandList.Count) return Vector2.zero;
        // Return the coordinates of the dummy hand
        return dammyHandList[index].position;
    }
    /// <summary>
    /// Immediately apply the auto-align feature of the layout
    /// </summary>
    public void ApplyLayout()
    {
        layoutGroup.CalculateLayoutInputHorizontal();
        layoutGroup.SetLayoutHorizontal();
    }
    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    /// <summary>
    /// Adds a specified number of dummy cards to the hand
    /// </summary>
    /// <param name="value"></param>
    private void AddHandObj(int value)
    {
        // Create objects for additional number of sheets
        for (int i = 0; i < value; i++)
        {
            // Create a object
            var obj = Instantiate(dammyHandPrefab, transform);
            // Add to list
            dammyHandList.Add(obj.transform);
        }
    }
    /// <summary>
    /// Remove specified number of dummy cards from hand
    /// </summary>
    /// <param name="value"></param>
    private void RemoveHandObj(int value)
    {
        // Get the number of sheets deleted as a positive number
        value = Mathf.Abs(value);
        // Delete objects for the number of sheets to be deleted
        for (int i = 0; i < value; i++)
        {
            if (dammyHandList.Count <= 0) break;
            // Delete object
            Destroy(dammyHandList[0].gameObject);
            // Delete from list
            dammyHandList.RemoveAt(0);
        }
    }

    #endregion Methods
}
