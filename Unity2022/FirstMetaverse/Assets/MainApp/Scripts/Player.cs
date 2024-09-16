using Fusion;
using UnityEngine;

/// <summary>
/// Player Class
/// </summary>
public class Player : NetworkBehaviour
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    /// <summary>
    /// Camera on Scene
    /// </summary>
    [SerializeField]
    private GameObject mainCamera;
    /// <summary>
    /// CameraScript camera target object variable
    /// </summary>
    [SerializeField]
    private GameObject cameraTargetObject;
    /// <summary>
    /// Animator
    /// </summary>
    [SerializeField]
    private Animator animator;
    /// <summary>
    /// Control of physical collisions and movement of characters
    /// </summary>
    [SerializeField]
    private CharacterController characterController;
    /// <summary>
    /// Player movement speed
    /// </summary>
    [SerializeField]
    private float playerSpeed = 5.0f;
    /// <summary>
    /// Player directional speed
    /// </summary>
    [SerializeField]
    private float rotationSpeed = 10f;
    /// <summary>
    /// Player's jumping ability
    /// </summary>
    [SerializeField]
    private float jumpForce = 7f;
    /// <summary>
    /// Strength of gravity(Gravity at the Earth's surface is about -9.81 m/s^2)
    /// </summary>
    [SerializeField]
    private float gravityValue = -9.81f;

    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// IsWalking string
    /// </summary>
    private const string strIsWalking = "IsWalking";
    /// <summary>
    /// IsJumping string
    /// </summary>
    private const string strIsJumping = "IsJumping";
    /// <summary>
    /// Jump string
    /// </summary>
    private const string strJump = "Jump";
    /// <summary>
    /// Horizontal string
    /// </summary>
    private const string strHorizontal = "Horizontal";
    /// <summary>
    /// Vertical string
    /// </summary>
    private const string strVertical = "Vertical";
    /// <summary>
    /// Velocity vector of player
    /// </summary>
    private Vector3 velocity;
    /// <summary>
    /// Flag whether the player has pressed the jump button or not
    /// </summary>
    private bool isJumpPressed;
    /// <summary>
    /// Flag to record the landing state in the immediately preceding frame
    /// </summary>
    private bool isGrounded;

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {
        // Get CharacterController component
        // characterController = GetComponent<CharacterController>();
    }
    void Update()
    {
        var move = MovePlayer();
        JumpPlayer(move);
        AnimatePlayer();
    }
    #endregion Unity Methods

    #region Public Methods

    /// <summary>
    /// Override methods from NetworkBehaviour, called when the player is created
    /// </summary>
    public override void Spawned()
    {
        // If it is its own local object with operating rights
        if (HasStateAuthority)
        {
            // Get main Camera
            mainCamera = Camera.main.gameObject;
            mainCamera.GetComponent<MainCamera>().targetObject = cameraTargetObject;
        }
        base.Spawned();
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Move player
    /// </summary>
    private Vector3 MovePlayer()
    {
        // Get the rotation of the camera around the y-axis
        // (Y軸回りのカメラの回転を定義)
        var cameraRotationY = Quaternion.Euler(0, mainCamera.transform.rotation.eulerAngles.y, 0);
        // Keyboard inputs (WASD or arrow keys) are acquired and movement vectors are calculated
        // (キーボードの入力(WASDもしくは矢印キー)を取得して移動ベクトルを計算する)
        // Multiply cameraRotationY so that the avatar moves in the direction of the arrow keys as seen from the camera, no matter which direction the avatar is facing
        // (cameraRotationY を掛けることでアバターがどの向きを向いたとしても、カメラから見た矢印キーの方向にアバターが移動するように処理)
        var move = cameraRotationY * new Vector3(Input.GetAxis(strHorizontal), 0, Input.GetAxis(strVertical)) * Time.deltaTime * playerSpeed;
        // Use the Move method of the CharacterController to move the player according to the calculated movement vector
        // (CharacterController の Move メソッドを使用してプレイヤーを計算された移動ベクトルに従って移動させる)
        // characterController.Move(move);

        if (move != Vector3.zero)
        {
            // Create a rotation towards the direction of the vector of movement created using the MOVE method of the character controller.
            // CharacterController の Move メソッドを使用して作った移動のベクトル方向に向けて回転を作る
            var targetRotation = Quaternion.LookRotation(move);
            // Point the avatar towards the direction of the above vectors
            // (カメラの向いている方向に滑らかにアバターが向く処理)
            // (Avatar faces in that direction according to the direction of movement)
            // (移動方向に合わせてアバターがそちらの方向に向く処理)
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        return move;
    }

    private void JumpPlayer(Vector3 move)
    {
        // Apply downward gravitational acceleration to the player's velocity vector
        // プレイヤーの速度ベクトルに下方向の重力加速度を適用する
        velocity.y += gravityValue * Time.deltaTime;
        // Initialise downward gravitational acceleration and set a downward velocity vector to continue landing when the player is on the ground
        // 下方向の重力加速度を初期化して、プレイヤーが地面にいるときは下方向の速度ベクトルを設定して着地を続けるようにする
        if (characterController.isGrounded)
        {
            velocity = new Vector3(0, -1, 0);
            // If the player has landed in the previous frame and the jump button has not been pressed
            // 直前のフレームで着地状態かつジャンプボタンが押されていない場合
            if (isGrounded && !isJumpPressed)
                // Stop jump animation
                animator.SetBool(strIsJumping, false);
        }
        // Play animation while jumping if in the air
        // 空中にいる場合はジャンプ中のアニメーションを再生
        else animator.SetBool(strIsJumping, true);
        // If the player presses the jump button
        if (Input.GetButtonDown(strJump)) isJumpPressed = true;
        // If the player presses the jump button and is in contact with the ground, an upward jump force is applied
        // プレイヤーがジャンプボタンを押下かつ地面に接触している場合、上方向にジャンプ力が加えられる
        if (isJumpPressed && characterController.isGrounded) velocity.y += jumpForce;
        // Multiply the movement vector by a speed that takes into account the effects of gravity and jumping to move the player
        // 移動ベクトルに重力やジャンプの影響が加味された速度を乗算してプレイヤーを移動させる
        characterController.Move(move + velocity * Time.deltaTime);
        // Memorise landing conditions
        // 着地状態を記憶
        isGrounded = characterController.isGrounded;
        // Reset jump button status flags
        isJumpPressed = false;
    }
    /// <summary>
    /// Animate player
    /// </summary>
    private void AnimatePlayer()
    {
        // Get keybord input
        var horizontalInput = Input.GetAxis(strHorizontal);
        var verticalInput = Input.GetAxis(strVertical);
        // Play walking animation if keyboard input is present.
        // If there is no keyboard input, the walking animation is stopped.
        animator.SetBool(strIsWalking, horizontalInput != 0 || verticalInput != 0);
    }

    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug


}
