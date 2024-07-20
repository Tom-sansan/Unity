using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// Game View class
/// </summary>
public class GameView : ViewBase
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    /// <summary>
    /// TakDataType in spreadsheet
    /// </summary>
    public enum TalkDataType
    {
        TalkData002_0,
        TalkData002_1,
        TalkData002_2
    }

    #endregion Enum

    #region Variables

    #region SerializeField

    /// <summary>
    /// Talk window
    /// </summary>
    [SerializeField]
    private TalkWindow talkWindow = null;
    /// <summary>
    /// SpreadSheetReader
    /// </summary>
    [SerializeField]
    private SpreadSheetReader spreadSheetReader = null;

    #endregion SerializeField

    #region Private Variables

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {
        // TestSaveData();
    }
    #endregion Unity Methods

    #region Public Methods

    /// <summary>
    /// Call at view open
    /// </summary>
    public override async void OnViewOpened()
    {
        base.OnViewOpened();
        // var data = talkWindow.Talks;
        var saveData = new SaveData();
        var loadedData = saveData.Load();
        string _sheetId = "1ysWc8fYia9XN_lmjtA9SQkJVDTeBgoL104dM7RiKMnM";
        string _sheetName = string.Empty;
        try
        {
            if (loadedData.StoryNumber == 0)
            {
                _sheetName = "TalkData001";
                var response = await LoadSpreadSheetData(_sheetId, _sheetName);
                await talkWindow.Close();

                // Save game result
                loadedData.StoryNumber = 1;
                loadedData.TestString = response[0].ToString();
                saveData.Save(loadedData);

                _sheetName = GetTalkData002SheetName(response[0]);
                await LoadSpreadSheetData(_sheetId, _sheetName);
                await talkWindow.Close();
            }
            else if (loadedData.StoryNumber == 1)
            {
                var num = Convert.ToInt32(loadedData.TestString);
                _sheetName = GetTalkData002SheetName(num);
                await LoadSpreadSheetData(_sheetId, _sheetName);
                await talkWindow.Close();
            }
            else Debug.Log("Error number: " + saveData.StoryNumber);
        }
        catch (OperationCanceledException e)
        {
            Debug.Log($"OperationCanceledException: {e}");
        }
        catch (Exception e)
        {
            Debug.Log($"Exception: {e}");
        }
    }
    /// <summary>
    /// Call at view close
    /// </summary>
    public override void OnViewClosed()
    {
        base.OnViewClosed();
    }
    /// <summary>
    /// Back to "01_Home" scene
    /// </summary>
    public void OnBackToHomeButtonClicked() =>
        Scene.ChangeScene("01_Home").Forget();

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Load spread sheet data
    /// </summary>
    /// <param name="sheedId"></param>
    /// <param name="sheetName"></param>
    private async UniTask<List<int>> LoadSpreadSheetData(string sheedId, string sheetName)
    {
        var data = await spreadSheetReader.LoadSpreadSheet(sheedId, sheetName);
        await talkWindow.SetBg(data[0].Place, true);
        await talkWindow.Open();
        return await talkWindow.StartTalk(data);
    }
    /// <summary>
    /// Get sheet name of TalkData002_X
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    private string GetTalkData002SheetName(int number)
    {
        string talkData002SheetName = string.Empty;
        switch (number)
        {
            case 0:
                talkData002SheetName = nameof(TalkDataType.TalkData002_0);
                break;
            case 1:
                talkData002SheetName = nameof(TalkDataType.TalkData002_1);
                break;
            case 2:
                talkData002SheetName = nameof(TalkDataType.TalkData002_2);
                break;
        }
        return talkData002SheetName;
    }
    /// <summary>
    /// Test save data
    /// </summary>
    private void TestSaveData()
    {
        //var data = new SaveData();
        //data.StoryNumber = 100;
        //data.TestString = "Over written.";
        //var json = JsonUtility.ToJson(data);
        //Debug.Log(json);
        //var _data = JsonUtility.FromJson<SaveData>(json);
        //Debug.Log(_data);

        var saveData = new SaveData();
        var data = saveData.Load();
        saveData.TestString = DateTime.Now.ToString();
        Debug.Log(saveData.TestString);
        saveData.Save(data);
    }

    #endregion Private Methods

    #endregion Methods
}
