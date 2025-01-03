using UnityEngine;

/// <summary>
/// Worker Class
/// </summary>
public class Worker : MonoBehaviour
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    /// <summary>
    /// Action state
    /// </summary>
    private enum ActionState
    {
        Idle,
        SitIdle,
        Clap,
        SitClapStand,
    }

    #endregion Enum

    #region Variables

    #region SerializeField

    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

    #region Public Properties

    #endregion Public Properties

    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// Stall-keeper
    /// </summary>
    private Animator animator;
    /// <summary>
    /// Next action state
    /// </summary>
    private ActionState nextActionState = ActionState.SitIdle;

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

    }
    #endregion Unity Methods

    #region Public Methods

    /// <summary>
    /// Called by an Animation Event set to Sit-Idle motion
    /// Sit-Idle モーションに設定した、Animation Event で呼ばれる
    /// </summary>
    public void EnterSitIdle()
    {
        if (nextActionState == ActionState.SitIdle)
        {
            // The next state to take has been reached, so change to the new state
            // 次にとるべき State に達したので、新しい State に変更する
            nextActionState = ActionState.Clap;
            // Call Clap() after 5-60 seconds of Sit-Idle
            // 1～10秒 Sit-Idle を続けてから Clap() を呼び出す
            Invoke(nameof(Clap), Random.Range(1.0f, 10.0f));
        }
    }
    /// <summary>
    /// Called by an Animation Event set to Idle motion
    /// Idle モーションに設定した、Animation Event で呼ばれる
    /// </summary>
    public void EnterIdle()
    {
        if (nextActionState == ActionState.Idle)
        {
            // The next State to take has been reached, so change to the new State
            // 次にとるべき State に達したので、新しい State に変更する
            nextActionState = ActionState.SitIdle;
            // Call Sit() after 5-60 seconds of idle
            // 5～60秒 idle を続けてから Sit() を呼び出す
            Invoke(nameof(Sit), Random.Range(5.0f, 60.0f));
        }
    }
    /// <summary>
    /// Called by an Animation Event set to Sit-Clap-Stand motion
    /// Sit-Clap-Stand モーションに設定した、Animation Event で呼ばれる
    /// </summary>
    public void EnterClap()
    {
        if (nextActionState == ActionState.Clap)
        {
            // The next state to take has been reached, so change to the new state
            // 次にとるべき State に達したので、新しい State に変更する
            // Set Sit or Idle as a new State
            // 新しい State として、Sit か Idle をランダムに決定
            // Call Stand() after 5-60 seconds of Clap
            // Chage State instantly without waiting
            // 待たずにすぐ移行する。
            bool isSit = false;
            if (Random.Range(0.0f, 1.0f) > 0.5f) isSit = true;
            if (isSit)
            {
                animator.SetTrigger("Sit");
                nextActionState = ActionState.SitIdle;
            }
            else
            {
                animator.SetTrigger("Idle");
                nextActionState = ActionState.Idle;
            }
        }
    }
    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialize this class
    /// </summary>
    private void Init()
    {
        animator = GetComponent<Animator>();
    }

    #endregion Private Methods

    /// <summary>
    /// Set Sit motion to animator
    /// animator に座るモーションを依頼する
    /// </summary>
    private void Sit() =>
        animator.SetTrigger("Sit");
    /// <summary>
    /// Set Clap motion to animator
    /// animator に呼び込みモーションを依頼する
    /// </summary>
    private void Clap() =>
        animator.SetTrigger("Sit-Clap-Stand");

    #endregion Methods
}
