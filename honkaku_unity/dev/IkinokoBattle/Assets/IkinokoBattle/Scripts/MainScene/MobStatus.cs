using UnityEngine;

public abstract class MobStatus : MonoBehaviour
{
    /// <summary>
    /// State definition
    /// </summary>
    protected enum StateEnum
    {
        Normal,
        Attack,
        Die
    }

    public bool IsMovable => StateEnum.Normal == _state;
    public bool IsAttackable => StateEnum.Normal == _state;
    public float LifeMax => lifeMax;
    public float Life => _life;

    [SerializeField] private float lifeMax = 2;
    protected Animator _animator;
    // Mob state
    protected StateEnum _state = StateEnum.Normal;
    // Current life value
    private float _life;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        // Life is Max in initial state
        _life = lifeMax;
        _animator = GetComponentInChildren<Animator>();
        // Start to display life gauge
        LifeGaugeContainer.Instance.Add(this);
    }

    // Process of die
    protected virtual void OnDie()
    {
        // End of displaying life gauge
        LifeGaugeContainer.Instance.Remove(this);
    }

    /// <summary>
    /// Damage
    /// </summary>
    /// <param name="damage"></param>
    public void Damage(string target, int damage)
    {
        if (_state == StateEnum.Die) return;

        _life -= damage;
        Debug.Log("Target: " + target + " Life : " + _life);
        if (_life > 0) return;

        _state = StateEnum.Die;
        _animator.SetTrigger("Die");
        OnDie();
    }

    /// <summary>
    /// Go to Attack state if possible
    /// </summary>
    public void GoToAttackStateIfPossible()
    {
        if (!IsAttackable) return;

        _state = StateEnum.Attack;
        _animator.SetTrigger("Attack");
    }

    /// <summary>
    /// Go to Normal state if possible
    /// </summary>
    public void GoToNormalStateIfPossible()
    {
        if (_state == StateEnum.Die) return;
        _state = StateEnum.Normal;
    }
}
