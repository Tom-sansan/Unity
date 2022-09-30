using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BallController : MonoBehaviour
{
    /// <summary>
    /// メインボール
    /// </summary>
    [SerializeField]
    private GameObject mainBall = null;
    /// <summary>
    /// 方向表示用オブジェクトのトランスフォーム
    /// </summary>
    [SerializeField]
    private Transform arrow = null;
    /// <summary>
    /// ボールリスト
    /// </summary>
    [SerializeField]
    private List<ColorBall> ballList = new List<ColorBall>();
    /// <summary>
    /// 打つ力
    /// </summary>
    [SerializeField]
    private float power = 0.1f;
    /// <summary>
    /// 「MainBall」のリジッドボディ
    /// </summary>
    private Rigidbody mainRigid = null;
    /// <summary>
    /// マウス位置保管用
    /// </summary>
    private Vector3 mousePosition = new Vector3();
    /// <summary>
    /// リセット時のためにメインボールの初期位置を保管
    /// </summary>
    private Vector3 mainBallDefaultPosition = new Vector3();
    /// <summary>
    /// マウス位置保管用
    /// </summary>
    private Vector3 startTouch = new Vector3();

    private void Start()
    {
        mainRigid = mainBall.GetComponent<Rigidbody>();
        mainBallDefaultPosition = mainBall.transform.localPosition;
        arrow.gameObject.SetActive(false);
    }

    private void Update()
    {
        // メインボールがアクティブでないときは return
        if (!mainBall.activeSelf) return;
        // スマートフォンかつタッチしている指の数が0より多い
        if ((Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) && Input.touchCount > 0)
            UpdateForSmartphone();
        // PC向けの操作
        else
            UpdateForPC();
    }
    /// <summary>
    /// リセットボタンクリックコールバック
    /// </summary>
    public void OnResetButtonClicked()
    {
        mainBall.SetActive(true);
        // メインボールの速度を強制的に0にする
        mainRigid.velocity = Vector3.zero;
        // リジッドボディの回転速度を強制的に0にする
        mainRigid.angularVelocity = Vector3.zero;
        // 初期位置に戻す
        mainRigid.transform.localPosition = mainBallDefaultPosition;
        // カラーボールのリセット
        foreach (var ball in ballList) ball.Reset();
    }
    private void UpdateForPC()
    {
        // マウスクリック開始時
        if (Input.GetMouseButtonDown(0))
        {
            BeginTouchOrClick(false, Input.mousePosition);
            // 開始位置を保管
            mousePosition = Input.mousePosition;
            // 方向線を表示
            arrow.gameObject.SetActive(true);
            Debug.Log("クリック開始");
        }
        // マウスクリック中
        else if (Input.GetMouseButton(0))
        {
            KeepTouchingOrClicking(mousePosition, Input.mousePosition);
            // 現在の位置を随時保管
            // var position = Input.mousePosition;
            // 角度を算出
            // ボールの飛ぶ方向のベクトルを算出
            //var def = mousePosition - position;
            //// アークタンジェントを使用し角度（ラジアン単位）を算出
            //var rad = Mathf.Atan2(def.x, def.y);
            //// ラジアンの角度を度数（°）に変換
            //var angle = rad * Mathf.Rad2Deg;
            //// 算出した角度を Vector3 として扱うために新しい Vector3 を作成
            //var rot = new Vector3(0, angle, 0);
            //// Quaternion という Unity で角度を表すために用いられる型に変換
            //var qua = Quaternion.Euler(rot);
            //// 方向線の位置角度を設定
            //arrow.localRotation = qua;
            //arrow.transform.position = mainBall.transform.position;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // 終了時の位置を保管
            var upPosition = Input.mousePosition;
            // 開始位置と終了位置のベクトル計算から打ち出す方向を算出
            var def = mousePosition - upPosition;
            var add = new Vector3(def.x, 0, def.y);
            // メインボールに力を加える
            mainRigid.AddForce(add * power);
            // 方向線を非表示に
            arrow.gameObject.SetActive(false);
            Debug.Log("クリック終了");
        }
    }
    /// <summary>
    /// スマートフォン用のUpdate関数
    /// </summary>
    private void UpdateForSmartphone()
    {
        // タッチしている指の最初の値を取得
        Touch touch = Input.GetTouch(0);
        // タッチ処理
        if (touch.phase == TouchPhase.Began)
            BeginTouchOrClick(true, touch.position);
        else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            KeepTouchingOrClicking(startTouch, touch.position);
        else if (touch.phase == TouchPhase.Ended)
            EndTouchOrClick(startTouch, touch.position);
    }
    /// <summary>
    /// タッチ or クリックスタート
    /// </summary>
    /// <param name="isSmartPhone"></param>
    /// <param name="startPosition"></param>
    private void BeginTouchOrClick(bool isSmartPhone, Vector3 startPosition)
    {
        // 開始位置を保管
        if (isSmartPhone) startTouch = Input.GetTouch(0).position;
        else mousePosition = Input.mousePosition;

        // 方向線を表示
        arrow.gameObject.SetActive(true);
        Debug.Log("タッチ開始");
    }
    /// <summary>
    /// タッチ or クリックの最中
    /// </summary>
    /// <param name="startPosition">開始位置</param>
    /// <param name="position">現在の位置</param>
    private void KeepTouchingOrClicking(Vector3 startPosition, Vector3 currentPosition)
    {
        // ボールの飛ぶ方向のベクトルを算出
        var def = startPosition - currentPosition;
        // アークタンジェントを使用し角度（ラジアン単位）を算出
        var rad = Mathf.Atan2(def.x, def.y);
        // ラジアンの角度を度数（°）に変換
        var angle = rad * Mathf.Rad2Deg;
        // 算出した角度を Vector3 として扱うために新しい Vector3 を作成
        var rot = new Vector3(0, angle, 0);
        // Quaternion という Unity で角度を表すために用いられる型に変換
        var qua = Quaternion.Euler(rot);
        // 方向線の位置角度を設定
        arrow.localRotation = qua;
        arrow.transform.position = mainBall.transform.position;
    }
    /// <summary>
    /// タッチ or クリック終了時の処理
    /// </summary>
    /// <param name="startPosition"></param>
    /// <param name="upPosition"></param>
    private void EndTouchOrClick(Vector3 startPosition, Vector3 upPosition)
    {
        // 開始位置と終了位置のベクトル計算から打ち出す方向を算出
        var def = startPosition - upPosition;
        var add = new Vector3(def.x, 0, def.y);
        // メインボールに力を加える
        mainRigid.AddForce(add * power);
        // 方向線を非表示に
        arrow.gameObject.SetActive(false);
        Debug.Log("クリック終了");
    }
}
