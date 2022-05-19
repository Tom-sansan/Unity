using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Game Controller
/// </summary>
public class GameController : MonoBehaviour
{
    #region Enum
    /// <summary>
    /// Game state
    /// </summary>
    public enum PlayState
    {
        None,
        Ready,
        Play,
        Finish
    }
    #endregion

    #region Variables
    /// <summary>
    /// Lap text
    /// </summary>
    [SerializeField]
    Text lapText = null;
    /// <summary>
    /// Retry UI
    /// </summary>
    [SerializeField]
    GameObject retryUI = null;
    /// <summary>
    /// Player
    /// </summary>
    [SerializeField]
    private PlayerController player = null;
    /// <summary>
    /// Countdonw start time
    /// </summary>
    [SerializeField]
    private int countStartTime = 5;
    /// <summary>
    /// Countdown text
    /// </summary>
    [SerializeField]
    private Text countdownText = null;
    /// <summary>
    /// Timer text
    /// </summary>
    [SerializeField]
    private Text timerText = null;
    /// <summary>
    /// CPU list
    /// </summary>
    [SerializeField]
    private List<CPUController> cpuList = new List<CPUController>();
    /// <summary>
    /// Collider call of lake for going off course
    /// </summary>
    [SerializeField]
    ColliderCallReceiver waterColliderCall = null;
    /// <summary>
    /// Relocation point list for going off course
    /// </summary>
    [SerializeField]
    List<Transform> relocationPoints = new List<Transform>();
    /// <summary>
    /// Back to the Lane Text
    /// </summary>
    [SerializeField]
    GameObject backToLaneText = null;
    [SerializeField]
    GameObject raceTrackBoxVR02 = null;
    [SerializeField]
    GameObject raceTrackBox02 = null;
    [SerializeField]
    GameObject lakeTerrain = null;
    [SerializeField]
    Transform OVRCameraRig = null;
    /// <summary>
    /// Current state
    /// </summary>
    public PlayState CurrentState = PlayState.None;
    /// <summary>
    /// Goal list
    /// </summary>
    private List<GameObject> goalList = new List<GameObject>();
    /// <summary>
    /// Current value of countdown
    /// </summary>
    private float currentCountDown = 0;
    /// <summary>
    /// Current value of game elapsed time
    /// </summary>
    private float timer = 0;
    /// <summary>
    /// Time '000.0'
    /// </summary>
    private const string timerZero = "000.0";
    private bool isRoadVRMode = false;
    private bool isTerrainVRMode = false;
    private bool isCameraVRMode = false;
    #endregion

    void Start()
    {
        CountDownStart();
        CreateEvent();
        GetAntiMotionSicknessComponent();
        retryUI.SetActive(false);
        backToLaneText.SetActive(false);
		waterColliderCall.TriggerEnterEvent.AddListener(OnWaterEnter);
        timerText.text = SetTimerText(timerZero);
        lapText.text = setLapText(0, player.GoalLap);
    }

    void Update()
    {
        timerText.text = SetTimerText(timerZero);
        switch (CurrentState)
        {
            case PlayState.Ready:
                // Deduct time
                currentCountDown -= Time.deltaTime;
                int intNum = 0;
                // Countdonw
                if (currentCountDown <= (float)countStartTime && currentCountDown > 0)
                {
                    intNum = (int)Mathf.Ceil(currentCountDown);
                    countdownText.text = intNum.ToString();
                    SetAntiMotionSickness();
                }
                else if (currentCountDown <= 0)
                {
                    // Start
                    StartPlay();
                    intNum = 0;
                    countdownText.text = "Start!!";
                    // SetActive after 2 seconds
                    StartCoroutine(WaitAfterStart());
                }
                break;
            case PlayState.Play:
                timer += Time.deltaTime;
                timerText.text = SetTimerText(timer.ToString(timerZero));
                // Back to the lane if player pushed A button.
                if (player.IsAButtonPushed) OnBackToLaneClicked();
                break;
            case PlayState.Finish:
                timer = 0;
                if (player.IsAnyButtonPushing) OnRetryButtonClicked();
                timerText.text = SetTimerText(timerZero);
                break;
            default:
                timer = 0;
                timerText.text = SetTimerText(timerZero);
                break;
        }
    }
    /// <summary>
    /// Call back for back to the lane
    /// </summary>
    public void OnBackToLaneClicked() =>
        Relocation(player.gameObject.transform);
    /// <summary>
    /// Call back for Retry button click
    /// </summary>
    public void OnRetryButtonClicked()
    {
        retryUI.SetActive(false);
        backToLaneText.SetActive(false);
        timerText.text = SetTimerText(timerZero);
        lapText.text = setLapText(0, player.GoalLap);
        goalList.Clear();
        player.OnRetry();
        CountDownStart();
    }
    /// <summary>
    /// Create event for the game
    /// </summary>
    private void CreateEvent()
    {
        player.LapEvent.AddListener(OnLap);
        player.GoalEvent.AddListener(OnGoal);
        foreach (var cpu in cpuList) cpu.GoalEvent.AddListener(OnGoal);
    }
    /// <summary>
    /// Processing when the number of laps changes
    /// </summary>
    private void OnLap()
    {
        var current = player.LapCount;
        var goalLap = player.GoalLap;
        lapText.text = setLapText(current, goalLap);
    }
    /// <summary>
    /// Goal Processing
    /// </summary>
    private void OnGoal(GameObject goal)
    {
        var player = goal.GetComponent<PlayerController>();
        var cpu = goal.GetComponent<CPUController>();
        if (player != null)
        {
            // Get Player's ranking
            CurrentState = PlayState.Finish;
            var playerNumber = goalList.Count + 1;
            var rank = playerNumber == 1 ? "1st" : playerNumber == 2 ? "2nd" : "3rd";
            countdownText.text = "Goal!! The " + rank + " Place!!";
            countdownText.gameObject.SetActive(true);
            retryUI.SetActive(true);
            backToLaneText.SetActive(false);
        }
        else if (cpu != null) goalList.Add(goal);

    }
    /// <summary>
    /// Countdonw Start
    /// </summary>
    private void CountDownStart()
    {
        currentCountDown = (float)countStartTime;
        SetPlayState(PlayState.Ready);
        countdownText.gameObject.SetActive(true);
        player.OnStart();
        foreach (var cpu in cpuList) cpu.OnStart(player.GoalLap);
    }
    /// <summary>
    /// Start game
    /// </summary>
    private void StartPlay()
    {
        Debug.Log("Start!!");
        SetPlayState(PlayState.Play);
    }
    /// <summary>
    /// Controll SetActive after waiting for 2 seconds
    /// </summary>
    /// <returns></returns>
    private IEnumerator WaitAfterStart()
    {
        yield return new WaitForSeconds(2f);
        countdownText.gameObject.SetActive(false);
        backToLaneText.SetActive(true);
    }
    /// <summary>
    /// Set current state
    /// </summary>
    /// <param name="state"> state </param>
    private void SetPlayState(PlayState state)
    {
        CurrentState = state;
        player.CurrentState = state;
        foreach (var cpu in cpuList) cpu.CurrentState = state;
    }
    /// <summary>
    /// Collider call when player enters the lake.
    /// </summary>
    /// <param name="collider"></param>
    private void OnWaterEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            Debug.Log("Entered the Lake!");
            Relocation(collider.gameObject.transform);
        }
    }
    /// <summary>
    /// Relocation process
    /// </summary>
    /// <param name="playerTransform"></param>
    private void Relocation(Transform playerTransform)
    {
        float near = 0;
        Transform nearPoint = null;
        foreach (var point in relocationPoints)
        {
            var sqrMag = (point.position - playerTransform.position).sqrMagnitude;
            if (near == 0 || near > sqrMag)
            {
                near = sqrMag;
                nearPoint = point;
            }
        }
        player.transform.position = nearPoint.transform.position;
        player.transform.rotation = nearPoint.transform.rotation;
        player.StopAllCoroutines();
    }

    private void GetAntiMotionSicknessComponent()
    {
        raceTrackBoxVR02 = GetComponent<GameObject>();
        raceTrackBox02 = GetComponent<GameObject>();
        lakeTerrain = GetComponent<GameObject>();
        OVRCameraRig = GetComponent<Transform>();
    }
    /// <summary>
    /// Enable or disable VR modes
    /// </summary>
    private void SetAntiMotionSickness()
    {
        if (player.IsBButtonPushed)
        {
            isRoadVRMode = !isRoadVRMode;
            raceTrackBoxVR02.SetActive(isRoadVRMode);
            raceTrackBox02.SetActive(!isRoadVRMode);
        }
        if (player.IsXButtonPushed)
        {
            isTerrainVRMode = !isTerrainVRMode;
            lakeTerrain.SetActive(isTerrainVRMode);
        };
        if (player.IsYButtonPushed)
        {
            isCameraVRMode = !isCameraVRMode;
            Vector3 cameraPosition;
            if (isCameraVRMode) cameraPosition = new Vector3(0, 3f, -7f);
            else cameraPosition = new Vector3(0, 1.4f, 0.6f);
            OVRCameraRig.transform.position = cameraPosition;
        }
    }
    /// <summary>
    /// Set time
    /// </summary>
    /// <param name="time">time</param>
    /// <returns>Timer text</returns>
    private string SetTimerText(string time) =>
        $"Time : {time} s";
    /// <summary>
    /// Set lap
    /// </summary>
    /// <param name="currentLap">Current lap</param>
    /// <param name="totalLap">Total lap</param>
    /// <returns></returns>
    private string setLapText(int currentLap, int totalLap) =>
        $"Lap : {currentLap}/{totalLap}";
}
