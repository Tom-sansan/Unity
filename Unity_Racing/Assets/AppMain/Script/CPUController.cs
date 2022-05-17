using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// CPU controller class
/// </summary>
public class CPUController : MonoBehaviour
{
    /// <summary>
    /// Goal event class
    /// </summary>
    public class GoalEventClass : UnityEvent<GameObject> { }
    /// <summary>
    /// Cinemachine pass administration
    /// </summary>
    [SerializeField]
    CinemachineSmoothPath smoothPath = null;
    /// <summary>
    /// Cinemachine cart
    /// </summary>
    [SerializeField]
    CinemachineDollyCart dollyCart = null;
    /// <summary>
    /// Lap time(Time required per lap)
    /// </summary>
    [SerializeField, Range(20f, 200f)]
    float lapTime = 40f;
    /// <summary>
    /// Car's transform
    /// </summary>
    [SerializeField]
    Transform carTransform = null;
    /// <summary>
    /// Current state
    /// </summary>
    public GameController.PlayState CurrentState = GameController.PlayState.None;
    /// <summary>
    /// Lap event
    /// </summary>
    public UnityEvent LapEvent = new UnityEvent();
    /// <summary>
    /// Goal event
    /// </summary>
    // public UnityEvent GoalEvent = new UnityEvent();
    public GoalEventClass GoalEvent = new GoalEventClass();
    /// <summary>
    /// Lap count
    /// </summary>
    public int LapCount = 0;
    /// <summary>
    /// Number of laps required to reach goal
    /// </summary>
    private int goalLap = 0;
    /// <summary>
    /// Flag for lap measurement
    /// </summary>
    private bool lapSwitch = false;
    /// <summary>
    /// Velocity
    /// </summary>
    private float velocity = 0;

    void Start()
    {
        // Get velocity
        velocity = smoothPath.PathLength / lapTime;
    }

    void Update()
    {
        if (CurrentState == GameController.PlayState.Play || CurrentState == GameController.PlayState.Finish)
        {
            // Distance in one frame at current velocity
            dollyCart.m_Position += velocity * Time.deltaTime;
            if (carTransform.localPosition != Vector3.zero || carTransform.localRotation != Quaternion.identity)
                FixedCarPosition();
        }
        else if (CurrentState == GameController.PlayState.Ready)
        {
            dollyCart.m_Position = 0;
        }
    }
    /// <summary>
    /// Call for start
    /// </summary>
    /// <param name="goal">Number of laps required to reach goal</param>
    public void OnStart(int goal)
    {
        goalLap = goal;
    }
    /// <summary>
    /// Goal processing
    /// </summary>
    public void OnGoal()
    {
        if (CurrentState != GameController.PlayState.Play) return;
        LapCount = 0;
        Debug.Log("CPU Goal!");
        CurrentState = GameController.PlayState.Finish;
        // GoalEvent?.Invoke();
        GoalEvent?.Invoke(gameObject);
    }
    /// <summary>
    /// Call for front gate
    /// </summary>
    public void OnFrontGateCall()
    {
        // Standard gate passage
        if (lapSwitch)
        {
            LapCount++;
            Debug.Log("CPU Lap " + LapCount);
            lapSwitch = false;

            if (LapCount > goalLap) OnGoal();
            else LapEvent?.Invoke();
        }
        // Reverse gate passage
        else
        {
            LapCount--;
            if (LapCount < 0) LapCount = 0;
            Debug.Log("CPU Reverse Lap " + LapCount);
            LapEvent?.Invoke();
        }
    }
    /// <summary>
    /// Call for Backward gate
    /// </summary>
    public void OnBackGateCall()
    {
        if (CurrentState != GameController.PlayState.Play) return;
        if (!lapSwitch) lapSwitch = true;
    }
    /// <summary>
    /// Gradually adjust the car's position to the cart
    /// </summary>
    private void FixedCarPosition()
    {
        carTransform.localPosition = Vector3.Lerp(carTransform.localPosition, Vector3.zero, Time.deltaTime);
        carTransform.localRotation = Quaternion.Lerp(carTransform.localRotation, Quaternion.identity, Time.deltaTime * 1f);
    }
}
