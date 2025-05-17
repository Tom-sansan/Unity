using System;
using System.Collections.Generic;
using UniRx;

/// <summary>
/// MySubject Class
/// </summary>
/// <typeparam name="T"></typeparam>
public class MySubject<T> : ISubject<T>, IDisposable
{
    /// <summary>
    /// ストップフラグ
    /// </summary>
    public bool IsStopped { get; } = false;
    /// <summary>
    /// Dispose フラグ
    /// </summary>
    public bool IsDisposed { get; } = false;
    /// <summary>
    /// Lock オブジェクト
    /// </summary>
    private readonly object _lockObject = new object();
    /// <summary>
    /// 途中で発生した例外
    /// </summary>
    private Exception error;
    /// <summary>
    /// 自身を購読している Observer のリスト
    /// </summary>
    private List<IObserver<T>> _observers;
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public MySubject()
    {
        _observers = new List<IObserver<T>>();
    }
    /// <summary>
    /// ISubject.OnNext のオーバーライド
    /// </summary>
    /// <param name="value"></param>
    public void OnNext(T value)
    {
        if (IsStopped) return;
        lock (_lockObject)
        {
            ThrowIfDisposed();
            // 自身を行動している Observer 全員へメッセージを発行する
            foreach (var observer in _observers)
            {
                observer.OnNext(value);
            }
        }
    }
    /// <summary>
    /// ISubject.OnError のオーバーライド
    /// </summary>
    /// <param name="error"></param>
    public void OnError(Exception error)
    {
        lock (_lockObject)
        {
            ThrowIfDisposed();
            if (IsStopped) return;
            this.error = error;
            try
            {
                foreach (var observer in _observers)
                {
                    observer.OnError(error);
                }
            }
            finally
            {
                Dispose();
            }
        }
    }
    /// <summary>
    /// ISubject.OnCompleted のオーバーライド
    /// </summary>
    public void OnCompleted()
    {
        lock (_lockObject)
        {
            ThrowIfDisposed();
            if (IsStopped) return;
            try
            {
                foreach (var observer in _observers)
                {
                    observer.OnCompleted();
                }
            }
            finally
            {
                Dispose();
            }
        }

    }
    /// <summary>
    /// IObservable.Subscribe のオーバーライド
    /// </summary>
    /// <param name="observer"></param>
    /// <returns></returns>
    public IDisposable Subscribe(IObserver<T> observer)
    {
        lock (_lockObject)
        {
            if (IsStopped)
            {
                // 既に動作を終了しているなら OnError メッセージ、
                // または OnCompleted メッセージを発行する
                if (error != null)
                {
                    observer.OnError(error);
                }
                else
                {
                    observer.OnCompleted();
                }
                return Disposable.Empty;
            }
            _observers.Add(observer);
            return new Subscription(this, observer);
        }
    }
    /// <summary>
    /// Throw ObjectDisposedException if disposed
    /// </summary>
    /// <exception cref="ObjectDisposedException"></exception>
    private void ThrowIfDisposed()
    {
        if (IsDisposed) throw new ObjectDisposedException("MySubject");
    }
    /// <summary>
    /// 購読クラス
    /// Subscribe の Dispose を実現するために用いる inner class
    /// </summary>
    private sealed class Subscription : IDisposable
    {
        private readonly IObserver<T> _observer;
        private readonly MySubject<T> _parent;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="observer"></param>
        public Subscription(MySubject<T> parent, IObserver<T> observer)
        {
            this._parent = parent;
            this._observer = observer;
        }
        /// <summary>
        /// 購読を解除する
        /// </summary>
        public void Dispose()
        {
            // Dispose されたら Observer リストから削除する
            _parent._observers.Remove(_observer);
        }
    }
    /// <summary>
    /// 購読を解除する
    /// </summary>
    public void Dispose()
    {
        lock (_lockObject)
        {
            if (IsDisposed) return;
            _observers.Clear();
            _observers = null;
            error = null;
        }
    }
}
