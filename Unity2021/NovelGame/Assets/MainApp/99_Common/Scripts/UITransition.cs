using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class UITransition : MonoBehaviour
{
    #region Nested Class

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

    #endregion Nested Class

    #region Variables

    #region SerializeField

    /// <summary>
    /// Setting value of fade
    /// </summary>
    [SerializeField]
    private TransitionParam fade = new TransitionParam();
    /// <summary>
    /// Setting value of scale
    /// </summary>
    [SerializeField]
    private TransitionParam scale = new TransitionParam()
    {
        IsActive = false,
        In = Vector2.zero,
        Out = Vector2.zero
    };
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
        // TestTransitionInWait();
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
    /// *Processing of UI coming into the screen
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
        if (scale.IsActive)
        {
            var current = Rect.transform.localScale;
            Rect.transform.localScale = new Vector3(scale.In.x, scale.In.y, current.z);
            inSequence.Join
            (
                Rect.DOScale(current, duration)
                    .SetLink(gameObject)
            );
        }
        inSequence.SetLink(gameObject)
                  .OnComplete(() => onCompleted?.Invoke());
    }
    /// <summary>
    /// Transition out
    /// * Processing of UI going off-screen
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
        if (scale.IsActive)
        {
            if (Rect != null)
            {
                var current = Rect.transform.localScale;
                outSequence.Join
                (
                    Rect.DOScale(new Vector3(scale.Out.x, scale.Out.y, current.z), duration)
                        .SetLink(gameObject)
                        .OnComplete(() => Rect.transform.localScale = current)
                );
            }
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
        try
        {
            bool isDone = false;
            if (inCancellation != null) inCancellation.Cancel();
            inCancellation = new CancellationTokenSource();
            TransitionIn(() => { isDone = true; });

            await UniTask.WaitUntil(() => isDone == true, PlayerLoopTiming.Update, inCancellation.Token);
        }
        catch(OperationCanceledException e)
        {
            Debug.Log("Cancelled: " + e);
            throw e;
        }
    }
    /// <summary>
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
        catch (Exception e)
        {
            Debug.Log("TransitionOutWait Exception: " + e);
            // throw e;
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
    /// <summary>
    /// Test for TransitionInWait
    /// </summary>
    /// <returns></returns>
    private async void TestTransitionInWait()
    {
        await TransitionInWait();
        Debug.Log("Transition has ended!");
    }

    #endregion Private Methods

    #endregion Methods
}
