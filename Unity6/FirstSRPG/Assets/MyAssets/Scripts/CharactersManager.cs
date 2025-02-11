using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// CharactersManager Class
/// </summary>
public class CharactersManager : MonoBehaviour
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField
    /// <summary>
    /// Parent object Transform of all character objects
    /// 全キャラクターオブジェクトの親オブジェクトTransform
    /// </summary>
    [SerializeField]
    private Transform charactersParent;
    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables
    /// <summary>
    /// All character data
    /// 全キャラクターデータ
    /// </summary>
    [HideInInspector]
    public List<Character> characters;
    #region Public Properties

    #endregion Public Properties

    #endregion Public Variables

    #region Private Variables

    #region Private Properties

    #endregion Private Properties

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {
        Init();
    }
    void Update()
    {

    }
    #endregion Unity Methods

    #region Public Methods
    /// <summary>
    /// Searches for and returns character data existing at the specified position
    /// 指定した位置に存在するキャラクターデータを検索して返す
    /// </summary>
    /// <param name="xPos">x Position</param>
    /// <param name="zPos">z Position</param>
    /// <returns></returns>
    public Character GetCharacterDataByPos(int xPos, int zPos)
    {
        // The same process is applied to all character data in the map, one by one
        // マップ内の全キャラクターデータ１体１体ずつに同じ処理を行う
        foreach (Character character in characters)
            // Check if the character's position matches the specified position
            // キャラクターの位置が指定された位置と一致しているかチェック
            if (character.currentPosX == xPos && character.CurrentPosZ == zPos) return character;
        // If the data is not found, return null
        // データが見つからなければnullを返す
        return null;
    }

    public void DeleteCharacter(Character character)
    {
        // Remove the specified character data from the list
        characters.Remove(character);
        // Destroy the specified character object
        DOVirtual.DelayedCall(0.5f, () =>
        {
            Destroy(character.gameObject);
        });
    }
    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialize this class
    /// </summary>
    private void Init()
    {
        // マップ上の全キャラクターデータを取得
        // (charactersParent以下の全Characterコンポーネントを検索しリストに格納)
        //characters = new List<Character>();
        //foreach (Transform child in charactersParent)
        //{
        //    Character character = child.GetComponent<Character>();
        //    if (character != null)
        //    {
        //        characters.Add(character);
        //    }
        //}
        characters = new List<Character>(charactersParent.GetComponentsInChildren<Character>());
    }

    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
