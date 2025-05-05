using UniRx;
using UniRx.Triggers;
using UnityEngine;

/// <summary>
/// ThrottleButton Class
/// </summary>
public class ThrottleButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Update のタイミングで Attack ボタンが押されているか判定
        // Attack ボタンが押されたら Subscribe() の処理を実行
        // そのあと 30 フレームの間ボタン入力を無視する
        this.UpdateAsObservable()
            .Where(_ => Input.GetButtonDown("Attack"))
            .ThrottleFirstFrame(30)
            .Subscribe(_ =>
            {
                Debug.Log("Attack!!");
            });
    }
}
