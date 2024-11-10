using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// MoveTo Class
/// Indicate destination to NavMeshAgent
/// </summary>
public class MoveTo : MonoBehaviour
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    /// <summary>
    /// Destination
    /// </summary>
    [SerializeField]
    private GameObject destination = null;
    /// <summary>
    /// Destination(Position property indicates world coordinates)
    /// </summary>
    [SerializeField]
    private Transform goal;
    /// <summary>
    /// Maximum number of stalls to stop along the way
    /// </summary>
    [SerializeField]
    private int maxStallVisits = 3;
    /// <summary>
    /// Maximum number of destination to stop along the way
    /// </summary>
    [SerializeField]
    private int maxDestinations = 2;

    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// Distance to destination considered to have arrived
    /// </summary>
    private const float minRadius = 0.5f;
    /// <summary>
    /// NavMeshAgent that makes object move to designated destination
    /// </summary>
    private NavMeshAgent navMeshAgent;
    /// <summary>
    /// Animator
    /// </summary>
    private Animator animator;
    /// <summary>
    /// Exit
    /// </summary>
    private GameObject exitPoint;
    /// <summary>
    /// Destination Candidates
    /// </summary>
    private Stack<GameObject> destinations;

    #region Strings in MoveTo class

    #region Strings of Tag
    /// <summary>
    /// EntryPoint
    /// </summary>
    private string strEntryPoint = "EntryPoint";
    /// <summary>
    /// ExitPoint
    /// </summary>
    private string strExitPoint = "ExitPoint";
    /// <summary>
    /// StallPoint
    /// </summary>
    private string strStallPoint = "StallPoint";
    /// <summary>
    /// DestinationPoint
    /// </summary>
    private string strDestination = "DestinationPoint";
    /// <summary>
    /// MainShrinePoint
    /// </summary>
    private string strMainShrinePoint = "MainShrinePoint";

    #endregion Strings of Tag

    #region Strings of Animator
    /// <summary>
    /// Idle string
    /// </summary>
    private string strIdle = "Idle";
    /// <summary>
    /// Walk string
    /// </summary>
    private string strWalk = "Walk";
    /// <summary>
    /// Pray
    /// </summary>
    private string strPray = "Pray";

    #endregion Strings of Animator

    #endregion Strings in MoveTo class

    #endregion Private Variables

    #region Properties

    #endregion Properties

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {
        Init();
    }
    void Update()
    {
        // It's on pause, so nothing to do
        if (navMeshAgent.isStopped) return;
        // Processing on arrival
        if (IsArrived()) ProcessOnArrival();
    }
    #endregion Unity Methods

    #region Public Methods

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialization
    /// </summary>
    private void Init()
    {
        SetEntryPoint();
        SetDestinations();
        SetExitPoint();
        SetAnimator();
    }
    /// <summary>
    /// Set entry point
    /// </summary>
    private void SetEntryPoint()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        var entryPoint = GameObject.FindGameObjectWithTag(strEntryPoint);
        // 最初の目標点(destination)に入口(entryPoint)を指定し、これを参詣者の目標とする
        destination = entryPoint;
        navMeshAgent.destination = destination.transform.position;
    }
    /// <summary>
    /// Set destinations
    /// </summary>
    private void SetDestinations()
    {
        var mainShrinePoint = GameObject.FindGameObjectWithTag(strMainShrinePoint);
        var stallPoints = GameObject.FindGameObjectsWithTag(strStallPoint);
        var destPoints = GameObject.FindGameObjectsWithTag(strDestination);
        // Movement order
        // Intermediate target group
        var list = new List<GameObject>();
        // 1回だけ必ず立ち寄る本殿を追加
        list.Add(mainShrinePoint);
        // 屋台を最小で0から、maxStallVisits を超えない数だけ無作為に取り出して追加する
        var c = UnityEngine.Random.Range(1, Math.Min(stallPoints.Length + 1, maxStallVisits));
        Debug.Log($"The number of stall points:{c}");
        list.AddRange(stallPoints.OrderBy(i => Guid.NewGuid()).Take(c));
        // 散策目標を最小で0から、maxDestinations を超えない数だけ無作為に取り出して追加する
        c = UnityEngine.Random.Range(1, Math.Min(destPoints.Length + 1, maxDestinations));
        Debug.Log($"The number of destination points:{c}");
        list.AddRange(destPoints.OrderBy(i => Guid.NewGuid()).Take(c));
        // 出来上がった中間目標群を無作為に並べ直したものを destination に持たせる
        destinations = new Stack<GameObject>(list.OrderBy(i => Guid.NewGuid()).ToArray());
    }
    /// <summary>
    /// Set exit point
    /// </summary>
    private void SetExitPoint() =>
        exitPoint = GameObject.FindGameObjectWithTag(strExitPoint);
    /// <summary>
    /// Set animator
    /// </summary>
    private void SetAnimator()
    {
        animator = GetComponent<Animator>();
        // Set walk trigger animator
        animator.SetTrigger(strWalk);
    }
    /// <summary>
    /// Determination of arrival
    /// </summary>
    /// <returns></returns>
    private bool IsArrived()
    {
        // Return false if destination is null
        if (destination == null) return false;
        // calculate the distance between self(this class) and destination
        var d = destination.transform.position - transform.position;
        var radius = (new Vector2(d.x, d.z)).magnitude;
        // If the distance is less than the distance for arrival judgment, the target point is considered to have been reached
        return radius < minRadius;
    }
    /// <summary>
    /// Processing on arrival
    /// </summary>
    private void ProcessOnArrival()
    {
        // Stop NavMeshAgent
        navMeshAgent.isStopped = true;
        if (destination.CompareTag(strMainShrinePoint))
        {
            // Arrived at Main Shrine
            ManageAfterArrived(strPray, 10);
        }
        else
        {
            // Other than above
            ManageAfterArrived(strIdle, 2);
        }
        // Manage actions after arrived at a place
        void ManageAfterArrived(string trrigerName, int seconds)
        {
            animator.SetTrigger(trrigerName);
            Invoke(nameof(ResumeWalk), seconds);
        }
    }
    /// <summary>
    /// Resume walk after ProcessOnArrival
    /// </summary>
    private void ResumeWalk()
    {
        // Get next destination
        destination = GetNextDestination();
        // if desinations is not null
        if (destination != null)
        {
            // Set walk animation
            animator.SetTrigger(strWalk);
            // designate a new destination to navMeshAgent
            navMeshAgent.destination = destination.transform.position;
            // 
            navMeshAgent.isStopped = false;
        }
    }
    /// <summary>
    /// Get next destination
    /// </summary>
    /// <returns></returns>
    private GameObject GetNextDestination()
    {
        // If the current destination is null, the move is considered to be finished and null is returned
        if (destination == null) return null;
        // If exitPoint, returns null since there is no next arrival point
        if (destination == exitPoint) return null;
        // If destination is empty, return exitPoint
        if (destinations.Count == 0) return exitPoint;
        // If other than the above, retrieve and return the arrival point registered in destinations
        return destinations.Pop();
    }
    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug


}
