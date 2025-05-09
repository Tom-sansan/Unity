using System;
using System.Collections;
using UniRx;
using UnityEngine;

/// <summary>
/// CountDownEventProvider Class
/// 指定秒数カウントしてイベント通知する
/// </summary>
public class CountDownEventProvider : MonoBehaviour
{
    /// <summary>
    /// カウントする秒数
    /// </summary>
    [SerializeField]
    private int _countSeconds = 10;
    /// <summary>
    /// Subjectのインスタンス
    /// </summary>
    private Subject<int> _subject;
    /// <summary>
    /// SubjectのIObservableインタフェース部分のみ公開する
    /// </summary>
    public IObservable<int> CountDownObservable => _subject;

    private void Awake()
    {
        Debug.Log("CountDownEventProvider::Awake");
        // Subject生成
        _subject = new Subject<int>();
        // カウントダウンするコルーチン起動
        StartCoroutine(CountCoroutine());
    }
    /// <summary>
    /// 指定秒数カウントし、その都度メッセージを発行するコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator CountCoroutine()
    {
        Debug.Log("CountDownEventProvider::CountCoroutine");
        var current = _countSeconds;

        while (current > 0)
        {
            // 現在の値を発行する
            _subject.OnNext(current);
            current--;
            yield return new WaitForSeconds(1.0f);
        }

        // 最後に0とOnCompletedメッセージを発行する
        _subject.OnNext(0);
        _subject.OnCompleted();
    }
    /// <summary>
    /// GameObjectが破棄されたらSubjectも解放する
    /// </summary>
    private void OnDestroy()
    {
        Debug.Log("CountDownEventProvider::OnDestroy");
        // GameObjectが破棄されたらSubjectも解放する
        _subject.Dispose();
    }
}
