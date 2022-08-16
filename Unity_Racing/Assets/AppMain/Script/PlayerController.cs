using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Player Controller Class
/// </summary>
public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// Goal event class
    /// </summary>
    public class GoalEventClass : UnityEvent<GameObject> { }

    #region Variables
    /// <summary>
    /// Forward mobility force
    /// </summary>
    [SerializeField]
    float movePower = 20000f;
    /// <summary>
    /// Rotation force
    /// </summary>
    [SerializeField]
    float rotationPower = 30000f;
    /// <summary>
    /// Speed limit
    /// </summary>
    [SerializeField]
    float speedSqrLimit = 200f;
    /// <summary>
    /// Rotation speed limit
    /// </summary>
    [SerializeField]
    float rotationSqrLimit = 0.5f;
    /// <summary>
    /// Car tracking speed of camera
    /// </summary>
    [SerializeField, Range(1f, 10f)]
    float cameraTrackingSpeed = 4f;
    /// <summary>
    /// Height Offset of camera
    /// </summary>
    [SerializeField, Range(0, 5f)]
    float cameraLookHeightOffset = 4f;
    /// <summary>
    /// Position of the camera from the car
    /// </summary>
    [SerializeField]
    Vector3 tpCameraOffset = new Vector3(0, 4f, -10f);
    /// <summary>
    /// Camera
    /// </summary>
    [SerializeField]
    Camera tpCamera = null;
    /// <summary>
    /// Transform for map marker
    /// </summary>
    [SerializeField]
    Transform mapMarker = null;
    /// <summary>
    /// RootTransform for map camera
    /// </summary>
    [SerializeField]
    Transform mapCamera = null;
    /// <summary>
    /// Root for Phone UI
    /// </summary>
    [SerializeField]
    GameObject phoneUi = null;
    /// <summary>
    /// Goal event
    /// </summary>
    // public UnityEvent GoalEvent = new UnityEvent();
    public GoalEventClass GoalEvent = new GoalEventClass();
    /// <summary>
    /// Play state
    /// </summary>
    public GameController.PlayState CurrentState = GameController.PlayState.None;
    /// <summary>
    /// Goal event
    /// </summary>
    // public UnityEvent GoalEvent = new UnityEvent();
    /// <summary>
    /// Lap count
    /// </summary>
    public int LapCount = 0;
    /// <summary>
    /// Goal Laps
    /// </summary>
    public int GoalLap = 2;
    /// <summary>
    /// Lap event
    /// </summary>
    public UnityEvent LapEvent = new UnityEvent();
    /// <summary>
    /// Rigidbody
    /// </summary>
    private Rigidbody rigid;
    /// <summary>
    /// Start position
    /// </summary>
    private Vector3 startPosition = Vector3.zero;
    /// <summary>
    /// Start rotation
    /// </summary>
    private Quaternion startRotation = Quaternion.identity;
    /// <summary>
    /// Lap switch to detect reverse run
    /// </summary>
    private bool lapSwitch = false;
    /// <summary>
    /// Flag for the starting lap or not
    /// </summary>
    private bool isStartLap = true;
    #endregion

    void Start()
    {
        if (rigid == null) rigid = GetComponent<Rigidbody>();
        // TODO:
        // rigid.centerOfMass = new Vector3(0, -0.2f, 0);
        EnablePhoneUI();
    }
    private void FixedUpdate()
    {
        MoveUpdate();
        RotationUpdate();
        TrackingCameraUpdate();
        MoveMapMarker();
        MoveMapCamera();
    }
    /// <summary>
    /// Call at the start of the countdown
    /// </summary>
    public void OnStart()
    {
        startPosition = this.transform.position;
        startRotation = this.transform.rotation;
    }
    /// <summary>
    /// Front gate call
    /// </summary>
    public void OnFrontGateCall()
    {
        // Normal gate pass
        if (lapSwitch)
        {
            lapSwitch = false;
            if (isStartLap)
            {
                isStartLap = false;
                return;
            }
            LapCount++;
            Debug.Log("Lap " + LapCount);
            if (LapCount >= GoalLap) OnGoal();
            else LapEvent?.Invoke();
        }
        // Reverse run gate pass
        else
        {
            LapCount--;
            if (LapCount < 0)
            {
                LapCount = 0;
                isStartLap = true;
            }
            Debug.Log("Reverse run Lap " + LapCount);
            LapEvent?.Invoke();
        }
    }
    /// <summary>
    /// Backward gate call
    /// </summary>
    public void OnBackGateCall()
    {
        if (!lapSwitch) lapSwitch = true;
    }
    /// <summary>
    /// Goal process
    /// </summary>
    public void OnGoal()
    {
        LapCount = 0;
        Debug.Log("Goal!!");
        CurrentState = GameController.PlayState.Finish;
        GoalEvent?.Invoke(gameObject);
    }
    /// <summary>
    /// Call on retry
    /// </summary>
    public void OnRetry()
    {
        this.transform.position = startPosition;
        this.transform.rotation = startRotation;
        isStartLap = true;
        var rotOffset = this.transform.rotation * tpCameraOffset;
        var anchor = this.transform.position + rotOffset;
        tpCamera.gameObject.transform.position = anchor;
    }
    /// <summary>
    /// Pushing call back for Phone UI front button
    /// </summary>
    public void OnPhoneFrontButtonPushing()
    {
        if (GameController.IsCurrentStateNotPlay()) return;
        var sqrVel = rigid.velocity.sqrMagnitude;
        // Forward speed limit
        if (sqrVel > speedSqrLimit) return;
        rigid.AddForce(transform.forward * movePower, ForceMode.Force);
    }
    /// <summary>
    /// Pushing call back for Phone UI back button
    /// </summary>
    public void OnPhoneBackButtonPushing()
    {
        if (GameController.IsCurrentStateNotPlay()) return;
        var sqrVel = rigid.velocity.sqrMagnitude;
        // Backforward speed limit
        if (sqrVel > (speedSqrLimit * 0.2f)) return;
        rigid.AddForce(-transform.forward * movePower, ForceMode.Force);
    }
    /// <summary>
    /// Pushing call back for Phone UI left button
    /// </summary>
    public void OnPhoneLeftButtonPushing()
    {
        if (GameController.IsCurrentStateNotPlay()) return;
        var sqrAng = rigid.angularVelocity.sqrMagnitude;
        // Rotation speed limit
        if (sqrAng > rotationSqrLimit) return;
        rigid.AddTorque(-transform.up * rotationPower, ForceMode.Force);
    }
    /// <summary>
    /// Pushing call back for Phone UI right button
    /// </summary>
    public void OnPhoneRightButtonPushing()
    {
        if (GameController.IsCurrentStateNotPlay()) return;
        var sqrAng = rigid.angularVelocity.sqrMagnitude;
        // Rotation speed limit
        if (sqrAng > rotationSqrLimit) return;
        rigid.AddTorque(transform.up * rotationPower, ForceMode.Force);
    }
    /// <summary>
    /// Set to 0 for velocity
    /// </summary>
    public void StopAllVelocity()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }
    /// <summary>
    /// Move process
    /// </summary>
    private void MoveUpdate()
    {
        if (GameController.IsCurrentStateNotPlay()) return;
        float sqrVel = rigid.velocity.sqrMagnitude;
        // Forward speed limit
        if (sqrVel > speedSqrLimit) return;
        // Move forward
        if (Input.GetKey(KeyCode.UpArrow)) rigid.AddForce(transform.forward * movePower, ForceMode.Force);
        // Backforward speed limit
        if (sqrVel > (speedSqrLimit * 0.2f)) return;
        // Move backward
        if (Input.GetKey(KeyCode.DownArrow)) rigid.AddForce(-transform.forward * movePower, ForceMode.Force);
    }
    /// <summary>
    /// Rotation process
    /// </summary>
    private void RotationUpdate()
    {
        if (GameController.IsCurrentStateNotPlay()) return;
        float sqrAng = rigid.angularVelocity.sqrMagnitude;
        // Rotation speed limit
        if (sqrAng > rotationSqrLimit) return;
        // Move forward
        if (Input.GetKey(KeyCode.LeftArrow)) rigid.AddTorque(-transform.up * rotationPower, ForceMode.Force);
        // Move backward
        if (Input.GetKey(KeyCode.RightArrow)) rigid.AddTorque(transform.up * rotationPower, ForceMode.Force);
    }
    /// <summary>
    /// Car tracking of Camera
    /// </summary>
    private void TrackingCameraUpdate()
    {
        // Rotate offset value by current car angle
        var rotOffset = this.transform.rotation * tpCameraOffset;
        // The camera position is calculated by adding the calculated offset value to the current position value
        var anchor = this.transform.position + rotOffset;
        // Gradual change of camera position from current position
        tpCamera.gameObject.transform.position = Vector3.Lerp(tpCamera.gameObject.transform.position, anchor, Time.fixedDeltaTime * cameraTrackingSpeed);

        // Point the camera in the direction of the car
        var look = this.transform.position;
        look.y += cameraLookHeightOffset;
        tpCamera.gameObject.transform.LookAt(look);
    }
    /// <summary>
    /// Place markers for maps on top of the car
    /// </summary>
    private void MoveMapMarker()
    {
        if (mapMarker == null) return;
        var current = this.transform.position;
        current.y = mapMarker.transform.position.y;
        mapMarker.transform.position = current;
    }
    /// <summary>
    /// Position angle adjustment of camera for map
    /// </summary>
    private void MoveMapCamera()
    {
        if (mapCamera == null) return;
        var current = this.transform.position;
        current.y = mapCamera.transform.position.y;
        mapCamera.position = current;
        // Make map camera look at player's forward
        mapCamera.forward = this.transform.forward;
    }
    /// <summary>
    /// Enable Phone UI
    /// </summary>
    private void EnablePhoneUI() =>
        phoneUi.SetActive(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer);
}
