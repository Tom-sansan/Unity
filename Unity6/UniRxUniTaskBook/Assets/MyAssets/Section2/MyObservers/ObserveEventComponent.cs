using System;
using UnityEngine;

/// <summary>
/// ObserveEventComponent Class
/// </summary>
public class ObserveEventComponent : MonoBehaviour
{
    /// <summary>
    /// CountDownEventProviderのインスタンス
    /// </summary>
    [SerializeField]
    private CountDownEventProvider _countDownEventProvider;
    /// <summary>
    /// Observerのインスタンス
    /// </summary>
    private PrintLogObserver<int> _printLogObserver;
    /// <summary>
    /// IDisposable
    /// </summary>
    private IDisposable _disposable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("ObserveEventComponent::Start");
        // PrintLogObserverインスタンスを作成
        _printLogObserver = new PrintLogObserver<int>();
        // SubjectのSubscribeを呼び出して、observerを登録する
        _disposable = _countDownEventProvider
            .CountDownObservable
            .Subscribe(_printLogObserver);
    }

    private void OnDestroy()
    {
        Debug.Log("ObserveEventComponent::OnDestroy");
        // GameObject破棄時にイベント購読を中断する
        _disposable?.Dispose();
    }
}
