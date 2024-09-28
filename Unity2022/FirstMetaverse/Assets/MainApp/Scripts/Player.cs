using Fusion;
using Photon.Chat.Demo;
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
    /// NamePlate component
    /// </summary>
    [SerializeField]
    private NamePlate namePlate;
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

    #region Properties

    /// <summary>
    /// Network property of player name
    /// Networked:Synchronised over the network
    /// </summary>
    [Networked]
    private NetworkString<_16> NickName { get; set; }

    #endregion Properties

    #endregion Variables

    #region Methods

    #region Unity Methods

    private void Awake()
    {
        // Disable CharacterController before moving because it cannot be moved if CharacterController is enabled when moving to the generation position
        // 生成位置への移動時にCharacterControllerが有効だと移動させられないため移動前は無効にする
        characterController.enabled = false;
    }

    void Start()
    {
        characterController.enabled = true;
        // Get CharacterController component
        characterController = GetComponent<CharacterController>();
    }
    private void Update()
    {
        // The reason for not placing it within FixedUpdateNetwork() is that it may not be correctly retrieved by FixedUpdateNetwork() when the user presses the button
        // FixedUpdateNetwork() 内に配置していない理由は、ユーザーがボタンを押下した際に正しくFixedUpdateNetwork()で取得されない可能性があるから
        // 1フレーム内でボタンの押下の取得はUpdate()メソッドの方が適している
        PressJumpButton();
    }
    /// <summary>
    /// Override methods from NetworkBehaviour that run at fixed intervals related to the network
    /// NetworkBehaviour からのメソッドを上書きした、ネットワークに関連する固定間隔で実行されるメソッド
    /// </summary>
    public override void FixedUpdateNetwork()
    {
        // If it is not its own local object with administrative rights
        // 管理者権限がある自身のローカルオブジェクトでない場合
        if (!HasStateAuthority) return;
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
            // Set Main Camera
            SetMainCamera();
            // Store this script in the player variable in the NamePickGui script in the Scripts object on the Scene
            // Scene上のScriptsオブジェクトのNamePickGuiスクリプト内にあるplayer変数にこのスクリプトを格納
            GameObject.Find("Scripts").GetComponent<NamePickGui>().player = this;
            // Set Player name
            SetPlayerName();
        }
        SetNickName();
    }
    /// <summary>
    /// Local player updates own avatar name.
    /// </summary>
    /// <param name="chatPlayerName"></param>
    public void SetChatPlayerName(string chatPlayerName)
    {
        // If it is its own local object that is the operating authority
        if (HasStateAuthority)
        {
            // Set the name of the logged-in player in the chat
            PlayerData.NickName = chatPlayerName;
            SetNetworkPropertyOfPlayerName();
            RPC_SetChatPlayerName();
        }
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
        var move = cameraRotationY * new Vector3(Input.GetAxis(strHorizontal), 0, Input.GetAxis(strVertical)) * Runner.DeltaTime * playerSpeed;
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
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Runner.DeltaTime);
        }
        return move;
    }
    /// <summary>
    /// Have all online players execute the SetChatPlayerName method individually
    /// </summary>
    [Rpc]
    public void RPC_SetChatPlayerName()
    {
        SetNickName();
    }
    /// <summary>
    /// Set main camera
    /// </summary>
    private void SetMainCamera()
    {
        // Get main Camera
        mainCamera = Camera.main.gameObject;
        mainCamera.GetComponent<MainCamera>().targetObject = cameraTargetObject;
    }
    /// <summary>
    /// Jump player
    /// </summary>
    /// <param name="move"></param>
    private void JumpPlayer(Vector3 move)
    {
        // Apply downward gravitational acceleration to the player's velocity vector
        // プレイヤーの速度ベクトルに下方向の重力加速度を適用する
        velocity.y += gravityValue * Runner.DeltaTime;
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
        // If the player presses the jump button and is in contact with the ground, an upward jump force is applied
        // プレイヤーがジャンプボタンを押下かつ地面に接触している場合、上方向にジャンプ力が加えられる
        if (isJumpPressed && characterController.isGrounded) velocity.y += jumpForce;
        // Multiply the movement vector by a speed that takes into account the effects of gravity and jumping to move the player
        // 移動ベクトルに重力やジャンプの影響が加味された速度を乗算してプレイヤーを移動させる
        characterController.Move(move + velocity * Runner.DeltaTime);
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
    /// <summary>
    /// Press jump button
    /// </summary>
    private void PressJumpButton()
    {
        // If the player presses the jump button
        if (Input.GetButtonDown(strJump)) isJumpPressed = true;
    }
    /// <summary>
    /// Set player name
    /// </summary>
    private void SetPlayerName()
    {
        PlayerData.NickName = $"Player{Random.Range(0, 10000)}";
        // Set initial value of network property of player name
        SetNetworkPropertyOfPlayerName();
    }
    /// <summary>
    /// Set network property of player name
    /// </summary>
    private void SetNetworkPropertyOfPlayerName() =>
        NickName = PlayerData.NickName;
    /// <summary>
    /// Call the SetNickName method of the NamePlate script to reflect the value of NickName in the player name text
    /// NamePlateスクリプトのSetNickNameメソッドを呼び出してNickNameの値をプレイヤー名のテキストに反映する
    /// </summary>
    private void SetNickName() =>
        namePlate.SetNickName(NickName.Value);

    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug

}
