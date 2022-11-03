using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.Schema;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    /// <summary>
    /// Transforms for rotary operation
    /// </summary>
    [SerializeField]
    private Transform rotationRoot = null;
    /// <summary>
    /// Transforms for height manipulation
    /// </summary>
    [SerializeField]
    private Transform heightRoot = null;
    /// <summary>
    /// Player Camera
    /// </summary>
    [SerializeField]
    private Camera mainCamera = null;
    /// <summary>
    /// Camera movement limit MinMax
    /// </summary>
    [SerializeField]
    private Vector2 heightLimit_MinMax = new Vector2(-1f, 3f);
    /// <summary>
    /// Height from the center player that the camera captures
    /// </summary>
    [SerializeField]
    private float lookHeight = 1.0f;
    /// <summary>
    /// Camera rotation speed
    /// </summary>
    [SerializeField]
    private float rotationSpeed = 0.01f;
    /// <summary>
    /// Camera height change speed
    /// </summary>
    [SerializeField]
    private float heightSpeed = 0.001f;
    /// <summary>
    /// Touch start position
    /// </summary>
    private Vector2 cameraStartTouch = Vector2.zero;
    /// <summary>
    /// Current touch position
    /// </summary>
    private Vector2 cameraTouchInput = Vector2.zero;

    private void Start()
    {
        mainCamera.transform.position = new Vector3(0, 1.5f, 2);
    }


    private void Update()
    {
        
    }
    /// <summary>
    /// Camera that always captures the player
    /// </summary>
    /// <param name="player"></param>
    public void UpdateCameraLook(Transform player)
    {
        // Fix the camera slightly above the character
        var cameraMarker = player.position;
        cameraMarker.y += lookHeight;
        var cameraLook = (cameraMarker - mainCamera.transform.position).normalized;
        mainCamera.transform.forward = cameraLook;
    }
    /// <summary>
    /// Tracking the player's pocation
    /// </summary>
    /// <param name="player"></param>
    public void FixedUpdateCameraPosition(Transform player) =>
        this.transform.position = player.position;

    public void UpdateRightTouch(Touch touch)
    {
        // Touch start
        if (touch.phase == TouchPhase.Began)
        {
            cameraStartTouch = touch.position;
            Debug.Log("Right touch start!");
        }
        // Touching
        else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
        {
            // Store current location at any time
            var position = touch.position;
            // Calculate the movement vector from the start position
            cameraTouchInput = position - cameraStartTouch;
            // Camera Rotation
            var yRot = new Vector3(0, cameraTouchInput.x * rotationSpeed, 0);
            var rResult = rotationRoot.rotation.eulerAngles + yRot;
            var qua = Quaternion.Euler(rResult);
            rotationRoot.rotation = qua;
            // Camera High/Low
            var yHeight = new Vector3(0, -cameraTouchInput.y * heightSpeed, 0);
            var hResult = heightRoot.transform.localPosition + yHeight;
            if (hResult.y > heightLimit_MinMax.y) hResult.y = heightLimit_MinMax.y;
            else if (hResult.y <= heightLimit_MinMax.x) hResult.y = heightLimit_MinMax.x;
            heightRoot.localPosition = hResult;
            Debug.Log("Right touching...");
        }
        // Touch end
        else if (touch.phase == TouchPhase.Ended)
        {
            cameraTouchInput = Vector2.zero;
            Debug.Log("Right touch ended");
        }
    }
}
