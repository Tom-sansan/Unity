using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MovementController Class
/// </summary>
public class MovementController : MonoBehaviour
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField
    /// <summary>
    /// Joystick class
    /// </summary>
    [SerializeField]
    private Joystick joystick;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    private float speed;
    /// <summary>
    /// Max velocity change
    /// </summary>
    [SerializeField]
    private float maxVelocityChange;
    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

    #region Public Properties

    #endregion Public Properties

    #endregion Public Variables

    #region Private Variables
    /// <summary>
    /// Rigidbody
    /// </summary>
    private Rigidbody rb;
    /// <summary>
    /// Current Velocity Vector
    /// </summary>
    private Vector3 currentVelocity;

    private Vector3 velocityChange;
    /// <summary>
    /// Move Horizontal Vector
    /// </summary>
    private Vector3 moveHorizontal;
    /// <summary>
    /// 
    /// </summary>
    private Vector3 moveVertical;
    /// <summary>
    /// Move Velocity Vector
    /// </summary>
    private Vector3 moveVelocityVector;
    /// <summary>
    /// Velocity Vector
    /// Initial velocity
    /// </summary>
    private Vector3 velocityVector = Vector3.zero;
    /// <summary>
    /// Move Direction Vector
    /// </summary>
    private Vector3 moveDirection = Vector3.zero;
    /// <summary>
    /// X Move Input by Joystick
    /// </summary>
    private float xMoveInput;
    /// <summary>
    /// Z Move Input by Joystick
    /// </summary>
    private float zMoveInput;

    #region Private Const Variables

    #endregion Private Const Variables

    #region Private Properties

    #endregion Private Properties

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {
        Init();
    }
    void Update()
    {
        InputJoystick();
    }
    void FixedUpdate()
    {
        MovePlayer();
    }
    #endregion Unity Methods

    #region Public Methods

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialize this class
    /// </summary>
    private void Init()
    {
        rb = GetComponent<Rigidbody>();
    }
    /// <summary>
    /// Reads joystick input, calculates the movement vector, and applies it.
    /// ジョイスティックの入力を読み取り、移動ベクトルを計算して適用します。
    /// </summary>
    private void InputJoystick()
    {
        /*
        // Gets the horizontal input value from the joystick.
        // ジョイスティックから水平方向の入力値を取得します。
        xMoveInput = joystick.Horizontal;
        // Gets the vertical input value from the joystick.
        // ジョイスティックから垂直方向の入力値を取得します。
        zMoveInput = joystick.Vertical;
        // Calculates the horizontal movement vector based on the joystick input and the object's right direction.
        // ジョイスティックの入力とオブジェクトの右方向に基づいて、水平方向の移動ベクトルを計算します。
        moveHorizontal = transform.right * xMoveInput;
        // Calculates the vertical movement vector based on the joystick input and the object's forward direction.
        // ジョイスティックの入力とオブジェクトの前方向に基づいて、垂直方向の移動ベクトルを計算します。
        moveVertical = transform.forward * zMoveInput;
        // Combines the horizontal and vertical movement vectors, normalizes the result, and multiplies by the speed.
        // 水平方向と垂直方向の移動ベクトルを組み合わせ、結果を正規化し、速度を掛けます。
        moveVelocityVector = (moveHorizontal + moveVertical).normalized * speed;
        */

        // Calculate velocity vectors
        // 速度ベクトルを計算します。
        moveDirection = new Vector3(joystick.Horizontal, 0f, joystick.Vertical);
        velocityVector = transform.TransformDirection(moveDirection.normalized) * speed;
        // Calls the Move method to set the target velocity.
        // Move メソッドを呼び出して目標速度を設定します。
        // Move(moveVelocityVector);
    }

    // private void Move(Vector3 moveVector)
    // {
    //     // Assigns the input moveVector to the velocityVector.
    //     // 入力された moveVector を velocityVector に代入します。
    //     velocityVector = moveVector;
    // }

    /// <summary>
    /// Move player
    /// </summary>
    private void MovePlayer()
    {
        if (velocityVector != Vector3.zero)
        {
            // Gets the current velocity of the Rigidbody component.
            // Rigidbody コンポーネントの現在の速度を取得します。
            currentVelocity = rb.velocity;
            // Calculates the difference between the target velocity and the current velocity.
            // 目標速度と現在の速度の差を計算します。
            velocityChange = velocityVector - currentVelocity;
            // Clamps the x component of the velocity change within the maximum allowed change.
            // 速度変化の x 成分を、許可された最大変化量内に制限します。
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            // Clamps the z component of the velocity change within the maximum allowed change.
            // 速度変化の z 成分を、許可された最大変化量内に制限します。
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            // Sets the y component of the velocity change to zero, preventing vertical movement.
            // 速度変化の y 成分をゼロに設定し、垂直方向の動きを防ぎます。
            velocityChange.y = 0f;
            // Applies a force to the Rigidbody to reach the target velocity, using acceleration mode.
            // 加速度モードを使用して、目標速度に達するために Rigidbody に力を加えます。
            rb.AddForce(velocityChange, ForceMode.Acceleration);
        }
    }
    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
