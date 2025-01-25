using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

/// <summary>
/// DebugInVR Class
/// VR 環境でのデバッグ用のログを画面内に表示する
/// </summary>
public class DebugInVR : MonoBehaviour
{
    /// <summary>
    /// ログを表示する TextMeshProUGUI
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI logText;
    /// <summary>
    /// ログの最大行数
    /// </summary>
    [SerializeField]
    private int maxLogLine = 10;
    /// <summary>
    /// フィルタリングするプレフィックス
    /// </summary>
    [SerializeField]
    private string filterPrefix = nameof(DebugInVR);
    /// <summary>
    /// ログキュー
    /// </summary>
    private Queue<string> logQueue = new Queue<string>();
    /// <summary>
    /// ログビルダー
    /// </summary>
    private StringBuilder logBuilder = new StringBuilder();
    /// <summary>
    /// 現在のログ
    /// </summary>
    private string currentLog = string.Empty;
    /// <summary>
    /// ログ表示を有効化
    /// </summary>
    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }
    /// <summary>
    /// ログ表示を無効化
    /// </summary>
    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    public void ClearLog()
    {
        currentLog = string.Empty;
        logText.text = currentLog;
    }
    /// <summary>
    /// ログを処理
    /// </summary>
    /// <param name="logString"></param>
    /// <param name="stackTrace"></param>
    /// <param name="type"></param>
    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (logString.StartsWith(filterPrefix))
        {
            // ログをキューに追加
            logQueue.Enqueue(logString);
            // (ログの最大行数制限処理)
            if (logQueue.Count > maxLogLine)
            {
                // キューが最大サイズを超えたら古いログを削除
                logQueue.Dequeue();
            }
            // StringBuilder をクリア
            logBuilder.Clear();
            foreach (var log in logQueue)
                logBuilder.AppendLine(log);
            // TextMeshProUGUI のテキストを更新
            logText.text = logBuilder.ToString();
        }
    }
}
