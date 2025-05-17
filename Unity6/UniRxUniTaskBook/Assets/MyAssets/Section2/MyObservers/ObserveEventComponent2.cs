using System;
using UniRx;
using UnityEngine;

/// <summary>
/// ObserveEventComponent Class
/// </summary>
public class ObserveEventComponent2 : MonoBehaviour
{
    [SerializeField]
    private CountDownEventProvider _countDownEventProvider;
    [SerializeField]
    private PrintLogObserver<int> _printLogObserver;
    private IDisposable _disposable;
    void Start()
    {
        _disposable = _countDownEventProvider
            .CountDownObservable
            .Subscribe(
                x => Debug.Log(x), // OnNext
                ex => Debug.LogError(ex), // OnError
                () => Debug.Log("OnCompleted") // OnCompleted
            );

    }
    private void OnDestroy()
    {
        _disposable?.Dispose();
    }
}
