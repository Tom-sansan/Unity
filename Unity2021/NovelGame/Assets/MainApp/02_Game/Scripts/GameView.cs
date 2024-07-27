using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GameView class
/// </summary>
public class GameView : ViewBase
{
    #region Nested Class

    #endregion Nested Class

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
        var storyDataList = new List<StoryData>();
        var responseList = new List<int>();
        string _sheetId = SingletonData.Instance.Data.SheetId;
        string _sheetName = string.Empty;
        try
        {
            if (loadedData.StoryNumber == 0)
            {
                _sheetName = SingletonData.Instance.Data.TalkData001;
                storyDataList = await LoadSpreadSheetData(_sheetId, _sheetName);
                responseList = await StartTalk(storyDataList);
                await talkWindow.Close();

                // Save game result
                loadedData.StoryNumber = 1;
                loadedData.TestString = responseList[0].ToString();
                saveData.Save(loadedData);

                _sheetName = GetTalkData002SheetName(responseList[0]);
                storyDataList = await LoadSpreadSheetData(_sheetId, _sheetName);
                await StartTalk(storyDataList);
                await talkWindow.Close();
            }
            else if (loadedData.StoryNumber == 1)
            {
                var num = Convert.ToInt32(loadedData.TestString);
                _sheetName = GetTalkData002SheetName(num);
                storyDataList = await LoadSpreadSheetData(_sheetId, _sheetName);
                await StartTalk(storyDataList);
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
        Scene.ChangeScene(SingletonData.Instance.Data.S01_Home).Forget();

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Load spread sheet data
    /// </summary>
    /// <param name="sheedId"></param>
    /// <param name="sheetName"></param>
    /// <returns></returns>
    private async UniTask<List<StoryData>> LoadSpreadSheetData(string sheedId, string sheetName) =>
        await spreadSheetReader.LoadSpreadSheet(sheedId, sheetName);
    /// <summary>
    /// Start talk to open talk window
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private async UniTask<List<int>> StartTalk(List<StoryData> data)
    {
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
                talkData002SheetName = SingletonData.Instance.Data.TalkData002_0;
                break;
            case 1:
                talkData002SheetName = SingletonData.Instance.Data.TalkData002_1;
                break;
            case 2:
                talkData002SheetName = SingletonData.Instance.Data.TalkData002_2;
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
