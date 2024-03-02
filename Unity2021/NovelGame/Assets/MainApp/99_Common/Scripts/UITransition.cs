using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class UITransition : MonoBehaviour
{
    #region Class

    /// <summary>
    /// Setting value
    /// </summary>
    [Serializable]
    public class TransitionParam
    {
        /// <summary>
        /// Active flag
        /// </summary>
        public bool IsActive = true;
        /// <summary>
        /// In value
        /// </summary>
        public Vector2 In = new Vector2(0, 1f);
        /// <summary>
        /// Out value
        /// </summary>
        public Vector2 Out = new Vector2(1f, 0);
    }

    #endregion Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    /// <summary>
    /// Setting value of fade
    /// </summary>
    [SerializeField]
    private TransitionParam fade = new TransitionParam();
    /// <summary>
    /// Transition duration
    /// </summary>
    [SerializeField]
    private float duration = 1f;

    #endregion SerializeField

    #region Public Variables

    #region Properties

    /// <summary>
    /// For RectTransform acquisition
    /// </summary>
    public RectTransform Rect => rect ??= GetComponent<RectTransform>();
    //public RectTransform Rect
    //{
    //    get
    //    {
    //        if (rect == null) rect = GetComponent<RectTransform>();
    //        return rect;
    //    }
    //}
    /// <summary>
    /// For CanvasGroup acquisition
    /// </summary>
    public CanvasGroup Canvas => canvas ??= GetComponent<CanvasGroup>();

    #endregion Properties

    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// RectTransform for storage
    /// </summary>
    private RectTransform rect = null;
    /// <summary>
    /// CanvasGroup for storage
    /// </summary>
    private CanvasGroup canvas = null;

    #endregion Private Variables

    /// <summary>
    /// In Sequence
    /// </summary>
    private Sequence inSequence = null;
    /// <summary>
    /// Out Sequence
    /// </summary>
    private Sequence outSequence = null;
    /// <summary>
    /// In CancellationTokenSource
    /// </summary>
    private CancellationTokenSource inCancellation = null;
    /// <summary>
    /// Out CancellationTokenSource
    /// </summary>
    private CancellationTokenSource outCancellation = null;

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {
        // Init();
        // await TransitionInWait();
        // Debug.Log("Transition has ended!");
    }
    void Update()
    {

    }
    /// <summary>
    /// Callback when destroyed
    /// </summary>
    private void OnDestroy()
    {
        if (inCancellation != null) inCancellation.Cancel();
        if (outCancellation != null) outCancellation.Cancel();
    }

    #endregion Unity Methods

    #region Public Methods

    /// <summary>
    /// Transition in
    /// </summary>
    public void TransitionIn(UnityAction onCompleted = null)
    {
        if (inSequence != null)
        {
            inSequence.Kill();
            inSequence = null;
        }
        inSequence = DOTween.Sequence();
        if (fade.IsActive && Canvas != null)
        {
            Canvas.alpha = fade.In.x;
            inSequence.Join
            (
                Canvas.DOFade(fade.In.y, duration)
                      .SetLink(gameObject)
            );
        }
        inSequence.SetLink(gameObject)
                  .OnComplete(() => onCompleted?.Invoke());
    }
    /// <summary>
    /// Transition out
    /// </summary>
    public void TransitionOut(UnityAction onCompleted = null)
    {
        if (outSequence != null)
        {
            outSequence.Kill();
            outSequence = null;
        }
        outSequence = DOTween.Sequence();
        if (fade.IsActive && Canvas != null)
        {
            Canvas.alpha = fade.Out.x;
            outSequence.Join
            (
                Canvas.DOFade(fade.Out.y, duration)
                      .SetLink(gameObject)
            );
        }
        outSequence.SetLink(gameObject)
                   .OnComplete(() => onCompleted?.Invoke());
    }
    /// <summary>
    /// Wait for transition-in termination
    /// </summary>
    /// <returns></returns>
    public async UniTask TransitionInWait()
    {
        bool isDone = false;
        if (inCancellation != null) inCancellation.Cancel();
        inCancellation = new CancellationTokenSource();
        TransitionIn(() => { isDone = true; });
        try
        {
            await UniTask.WaitUntil(() => isDone == true, PlayerLoopTiming.Update, inCancellation.Token);
        }
        catch(OperationCanceledException e)
        {
            Debug.Log("Cancelled: " + e);
        }
    }
    /// Wait for transition-out termination
    /// </summary>
    /// <returns></returns>
    public async UniTask TransitionOutWait()
    {
        bool isDone = false;
        if (outCancellation != null) outCancellation.Cancel();
        outCancellation = new CancellationTokenSource();
        TransitionOut(() => { isDone = true; });
        try
        {
            await UniTask.WaitUntil(() => isDone == true, PlayerLoopTiming.Update, outCancellation.Token);
        }
        catch (OperationCanceledException e)
        {
            Debug.Log("Cancelled: " + e);
        }
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialization
    /// </summary>
    //private void Init()
    //{
    //    //var rand = UnityEngine.Random.Range(0, 2);
    //    //if (rand == 0) MoveSplashViewToRight();
    //    //else TransitionOut();
    //    TransitionOut();
    //}
    /// <summary>
    /// Move SplashView to right
    /// </summary>
    private void MoveSplashViewToRight() =>
        Rect.DOLocalMove(new Vector3(1000f, 0, 0), 3f);

    #endregion Private Methods

    #endregion Methods
}
