using System;
using UnityEngine;

/// <summary>
/// PrintLogObserver
/// 受信したメッセージをログに出力する Observer
/// </summary>
/// <typeparam name="T"></typeparam>
public class PrintLogObserver<T> : IObserver<T>
{
    public void OnCompleted()
    {
        Debug.Log("PrintLogObserver::OnCompleted");
        Debug.Log("OnCompleted!!");
    }

    public void OnError(Exception error)
    {
        Debug.Log("PrintLogObserver::OnError");
        Debug.LogError(error);
    }

    public void OnNext(T value)
    {
        Debug.Log("PrintLogObserver::OnNext");
        Debug.Log(value);
    }
}
