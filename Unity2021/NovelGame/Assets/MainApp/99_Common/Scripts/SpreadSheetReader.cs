using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// SpreadSheetReader Class
/// </summary>
public class SpreadSheetReader : MonoBehaviour
{
    #region Variables

    #region Private Variables

    /// <summary>
    /// Google docs link
    /// </summary>
    private string link = string.Empty;

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods

    private void Awake()
    {
        link = SingletonData.Instance.Data.Link;
    }
    void Start()
    {
        // ExecTestCall();
    }

    #endregion Unity Methods

    #region Public Methods

    /// <summary>
    /// Load spread sheet
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public async UniTask<List<StoryData>> LoadSpreadSheet(string id, string name)
    {
        Debug.Log("Start loading");
        UnityWebRequest request = UnityWebRequest.Get(string.Format(link, id, name));
        await request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ProtocolError || request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
            return null;
        }
        else
        {
            var text = request.downloadHandler.text;
            Debug.Log("<<Download Handler>>\n" + text);
            return GetSheetData(text);
        }
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Get sheet data from Request.downloadHandler.text
    /// </summary>
    /// <param name="text"></param>
    private List<StoryData> GetSheetData(string text)
    {
        StringReader reader = new(text);
        List<string[]> cells = new List<string[]>();
        string line;
        string result;
        int count;
        bool isCell;;
        // Read one line to exclude the first line (title row)
        reader.ReadLine();
        while (reader.Peek() != -1)
        {
            line = reader.ReadLine();
            result = string.Empty;
            while (true)
            {
                count = 0;
                isCell = false;
                foreach (var c in line)
                {
                    // If a comma is used in a cell, replace it with <c>. Otherwise, the comma is replaced by a string.
                    if (c.ToString().Equals(",") && isCell) result += "<c>";
                    else result += c.ToString();
                    // Odd " is the start of cell, even " is end.
                    if (c.ToString().Equals("\""))
                    {
                        count++;
                        isCell = !isCell;
                    }
                }
                // If a line ends with an odd number, the line is not complete, so the result is reset and the next line is read into the line.
                if (count == 0 || count % 2 == 1)
                {
                    line += "<br>";
                    line += reader.ReadLine();
                    result = string.Empty;
                }
                else break;
            }
            Debug.Log(result);
            // Cells in a row are separated by ,. Separated by cell and arrayed
            string[] elements = result.Split(',');
            cells.Add(elements);
        }

        string log = string.Empty;
        var stories = new List<StoryData>();
        // Set each cell data to StoryData class
        foreach (var cell in cells)
        {
            var data = new StoryData();
            data.Place = getCellData(cell[0]);
            int.TryParse(getCellData(cell[1]), out data.CharacterNumber);
            data.Talk = getCellData(cell[2]);
            data.Left = getCellData(cell[3]);
            data.Center = getCellData(cell[4]);
            data.Right = getCellData(cell[5]);
            log += $"<場所> {data.Place}, <キャラNo> {data.CharacterNumber}, <内容> {data.Talk}, <左> {data.Left}, <中> {data.Center}, <右> {data.Right}\n";
            stories.Add(data);
        }
        Debug.Log(log);
        return stories;

        // Get cell data
        string getCellData(string _cell) =>
            // Erase the first and last enclosures and restore commas, line breaks.
            _cell.Trim('"').Replace("<br>", "\n").Replace("<c>", ",");
    }

    #endregion Private Methods

    /// <summary>
    /// Test use
    /// </summary>
    private void ExecTestCall()
    {
        string _sheetId = SingletonData.Instance.Data.SheetId;
        string _sheetName = "TalkData001";
        LoadSpreadSheet(_sheetId, _sheetName).Forget();
    }

    #endregion Methods
}
