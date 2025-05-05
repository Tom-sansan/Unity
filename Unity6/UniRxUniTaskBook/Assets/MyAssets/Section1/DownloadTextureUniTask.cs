using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DownloadTextureUniTask : MonoBehaviour
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
        // この GetObject に紐づいた CancellationToken を取得
        var token = this.GetCancellationTokenOnDestroy();
        // テクスチャのセットアップを実行
        SetupTextureAsync(token).Forget();
    }
    /// <summary>
    /// SetupTextureAsync
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    private async UniTaskVoid SetupTextureAsync(CancellationToken token)
    {
        try
        {
            var uri = "https://f.media-amazon.com/images/I/81t02zqQ2-L._SY522_.jpg";
            // UniRx の Retry を使いたいので、UniTask から Observable へ変換する
            var observable = Observable
                .Defer(() =>
                {
                    // UniTask -> IObservale の変換
                    return GetTextureAsync(uri, token)
                        .ToObservable();

                })
                .Retry(3);
            // Observale も await で待ち受けが可能
            var texture = await observable;
            rawImage.texture = texture;
        }
        catch (Exception e) when (!(e is OperationCanceledException))
        {
            Debug.LogError(e);
        }
    }
    /// <summary>
    /// コルーチンの代わりに async/await を利用する
    /// </summary>
    /// <param name="uri"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    private async UniTask<Texture> GetTextureAsync(string uri, CancellationToken token)
    {
        using var uwr = UnityWebRequestTexture.GetTexture(uri);
        // リクエストを送信し完了を待つ
        await uwr.SendWebRequest().WithCancellation(token);
        return ((DownloadHandlerTexture)uwr.downloadHandler).texture;
    }
}
