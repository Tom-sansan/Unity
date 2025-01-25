using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomMove : MonoBehaviour
{
    public InputActionProperty moveAction; // InputActionPropertyに変更
    public float moveSpeed = 2.5f;
    private CharacterController characterController;
    private XROrigin rig;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        rig = GetComponentInParent<XROrigin>();
        if (rig == null)
        {
            Debug.LogError("DebugInVR XRRigが見つかりません。親オブジェクトにXRRigが存在するか確認してください。");
            enabled = false; // スクリプトを無効化
            return;
        }
        if (characterController == null)
        {
            Debug.LogError("DebugInVR CharacterControllerが見つかりません。このGameObjectにCharacterControllerが存在するか確認してください。");
            enabled = false; // スクリプトを無効化
            return;
        }
    }

    void Update()
    {
        if (rig == null || characterController == null) return;
        // コントローラーのスティック入力を取得
        Vector2 inputAxis = moveAction.action.ReadValue<Vector2>();
        // if (!device.isValid) return;
        if (inputAxis.magnitude > 0.1f) // magnitudeでスティックが倒れているか確認
        {
            // XR RigのTransformを取得
            Transform rigTransform = rig.transform;

            // 移動方向を計算（XR RigのXZ平面を基準）
            Vector3 moveDirection = rigTransform.TransformDirection(new Vector3(inputAxis.x, 0, inputAxis.y));
            moveDirection.y = 0; // Y軸方向の移動は無視
            moveDirection.Normalize();

            // 移動を実行
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
            Debug.Log($"DebugInVR {moveDirection * moveSpeed * Time.deltaTime}");
        }
    }
}