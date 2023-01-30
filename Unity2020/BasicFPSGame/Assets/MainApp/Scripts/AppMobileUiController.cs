using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppMobileUiController : MonoBehaviour
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
    public void OnMoveUiPointerDown()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
            moveTouchStartPos = Input.mousePosition;
        else
        {
            var _touch = Input.touches[Input.touchCount - 1];
            moveTouchStartPos = _touch.position;
            moveFingerId = _touch.fingerId;
        }
        isMoveButtonPushing = true;
    }
    /// <summary>
    /// Moving UI pointer up
    /// </summary>
    public void OnMoveUiPointerUp()
    {
        isMoveButtonPushing = false;
        moveTouchStartPos = Vector2.zero;
        moveStick.anchoredPosition = Vector2.zero;
        StickPosition = Vector2.zero;
        moveFingerId = -1;
    }
    /// <summary>
    /// Process for move buttn pushing
    /// </summary>
    private void MoveButtonPushing()
    {
        if (isMoveButtonPushing)
        {
            var currentPos = moveTouchStartPos;
            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
                moveTouchStartPos = Input.mousePosition;
            else if (moveFingerId != -1 && Input.touchCount > 0)
            {
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
