using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AGC = AppGameController;

public class AppMobileUIController : MonoBehaviour
{
    /// <summary>
    /// Mobile UI stick
    /// </summary>
    [SerializeField]
    private RectTransform moveStick = null;
    /// <summary>
    /// Stick position
    /// </summary>
    public Vector2 StickPosition = Vector2.zero;
    /// <summary>
    /// Position where the touch is initiated
    /// </summary>
    private Vector2 moveTouchStartPos = Vector2.zero;
    /// <summary>
    /// Finger number of the moving UI touched
    /// </summary>
    private int moveFingerId = -1;
    /// <summary>
    /// Flag during move button touch
    /// </summary>
    private bool isMoveButtonPushing = false;
    /// <summary>
    /// Flag during jump button touch
    /// </summary>
    private bool isJumpButtonPushing = false;
    void Start()
    {
        
    }


    void Update()
    {
        MoveButtonPushing();
    }
    /// <summary>
    /// Moving UI pointer down
    /// </summary>
    public void OnMoveUIPointerDown()
    {
        if (AGC.CheckPlatform(RuntimePlatform.WindowsEditor, RuntimePlatform.OSXEditor))
            moveTouchStartPos = Input.mousePosition;
        else
        {
            // For Smartphone
            // Last finger touched
            var _touch = Input.touches[Input.touchCount - 1];
            moveTouchStartPos = _touch.position;
            moveFingerId = _touch.fingerId;
        }
        // State of clicking on the movement UI.
        isMoveButtonPushing = true;
    }
    /// <summary>
    /// Moving UI pointer up
    /// </summary>
    public void OnMoveUIPointerUp()
    {
        // State that the touch of the movement UI is over
        isMoveButtonPushing = false;
        moveTouchStartPos = Vector2.zero;
        moveStick.anchoredPosition = Vector2.zero;
        StickPosition = Vector2.zero;
        moveFingerId = -1;
    }
    /// <summary>
    /// Any of the UI buttons pressed
    /// </summary>
    /// <returns></returns>
    public bool IsAnyButtonPushing() =>
        isMoveButtonPushing || isJumpButtonPushing;
    /// <summary>
    /// Jump button down call back
    /// </summary>
    public void OnJumpButtonDown() =>
        isJumpButtonPushing = true;
    /// <summary>
    /// Jump button up call back
    /// </summary>
    public void OnJumpButtonUp() =>
        isJumpButtonPushing = false;
    /// <summary>
    /// Process for move buttn pushing
    /// </summary>
    private void MoveButtonPushing()
    {
        if (isMoveButtonPushing)
        {
            var currentPos = moveTouchStartPos;
            if (AGC.CheckPlatform(RuntimePlatform.WindowsEditor, RuntimePlatform.OSXEditor))
                currentPos = Input.mousePosition;
            else if (moveFingerId != -1 && Input.touchCount > 0)
            {
                // For Smartphone
                foreach (var touch in Input.touches)
                {
                    if (touch.fingerId == moveFingerId)
                    {
                        currentPos = touch.position;
                        break;
                    }
                }
            }
            var def = currentPos - moveTouchStartPos;
            // Value adjustment to match the amount of stick movement against the value
            def *= 0.5f;

            if (def.x > 75f) def.x = 75f;
            else if (def.x < -75f) def.x = -75f;
            StickPosition.x = def.x / 75f;

            if (def.y > 75f) def.y = 75f;
            else if (def.y < -75f) def.y = -75f;
            StickPosition.y = def.y / 75f;

            var pos = moveStick.anchoredPosition;
            pos.x = def.x;
            pos.y = def.y;
            moveStick.anchoredPosition = pos;

            Debug.Log("Pushing...");
        }
    }
}
