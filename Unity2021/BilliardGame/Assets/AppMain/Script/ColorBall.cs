using UnityEngine;

public class ColorBall : MonoBehaviour
{
    private Rigidbody rigid = null;
    // リセットのための初期位置
    private Vector3 defaultPosition = new Vector3();

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        defaultPosition = this.transform.localPosition;
    }

    void Update()
    {
        
    }
    /// <summary>
    /// リセット時の処理
    /// </summary>
    public void Reset()
    {
        gameObject.SetActive(true);
        // リジッドボディの速度を強制的に0にする
        rigid.velocity = Vector3.zero;
        // リジッドボディの回転速度を強制的に0にする
        rigid.angularVelocity = Vector3.zero;
        // 初期位置に戻す
        this.transform.localPosition = defaultPosition;
    }
}
