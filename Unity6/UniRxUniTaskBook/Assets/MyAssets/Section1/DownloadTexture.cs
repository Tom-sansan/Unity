using System;
using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

/// <summary>
/// DownloadTexture Class
/// </summary>
public class DownloadTexture : MonoBehaviour
{
    /// <summary>
    /// RawImage to store the texture
    /// </summary>
    [SerializeField]
    private RawImage rawImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Init();
    }

    private void Init()
    {
        var uri = "https://f.media-amazon.com/images/I/81t02zqQ2-L._SY522_.jpg";
        // Get Texture
        // Try up to 3 times when an exception
        GetTextureAsync(uri)
            .OnErrorRetry
            (
                onError: (Exception ex) => { Debug.LogWarning($"Texture download failed. Retrying... Error: {ex.Message}"); },
                retryCount: 3
            )
            .Subscribe
            (
                result => { rawImage.texture = result; },
                error => { Debug.LogError(error); }
            )
            .AddTo(this);
    }
    /// <summary>
    /// コルーチンを起動して、その結果を Observable で返す
    /// </summary>
    /// <param name="uri"></param>
    /// <returns></returns>
    private IObservable<Texture> GetTextureAsync(string uri)
    {
        return Observable
            .FromCoroutine<Texture>(obserer =>
            {
                return GetTextureCoroutine(obserer, uri);
            });
    }
    /// <summary>
    /// GetTextureCoroutine
    /// </summary>
    /// <param name="observer"></param>
    /// <param name="uri"></param>
    /// <returns></returns>
    private IEnumerator GetTextureCoroutine(IObserver<Texture> observer, string uri)
    {
        using var uwr = UnityWebRequestTexture.GetTexture(uri);
        // リクエストを送信し完了を待つ
        yield return uwr.SendWebRequest();
        // リクエスト完了後にエラーチェック
        if (uwr.result != UnityWebRequest.Result.Success)
        {
            // エラーが起きたらOnErrorメッセージを発行する
            observer.OnError(new Exception(uwr.error));
            yield break;
        }
        // ダウンロードハンドラーからテクスチャを取得
        var downloadHandler = uwr.downloadHandler as DownloadHandlerTexture;
        var result = downloadHandler?.texture;
        if (result == null)
        {
            observer.OnError(new Exception($"Failed to download the texture. UnityWebRequest error: {uwr.error}"));
            yield break;
        }
        // 成功したら OnNext / OnComplete メッセージを発行する
        observer.OnNext(result);
        observer.OnCompleted();
    }
}
