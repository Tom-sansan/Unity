using UnityEngine;

/// <summary>
/// Fire Class
/// </summary>
public class Fire : MonoBehaviour
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    /// <summary>
    /// Firework particle system
    /// </summary>
    [SerializeField]
    private ParticleSystem fireWork;

    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

    #region Public Properties

    #endregion Public Properties

    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// Firework sound
    /// </summary>
    private AudioSource explosionSound;

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

    /// <summary>
    /// Fireworks are set off when the arrow hits the target
    /// 矢が当たった時にも花火を打ち上げる
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        Play();
    }

    #endregion Unity Methods

    #region Public Methods

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialize this class
    /// </summary>
    private void Init()
    {
        explosionSound = GetComponent<AudioSource>();
        // Play the firework sound at the start of game
        Play();
    }

    #endregion Private Methods

    /// <summary>
    /// Start ParticleSystem and sound simultaneously
    /// ParticleSystem を開始させ、同時に音を鳴らす
    /// </summary>
    private void Play()
    {
        fireWork.Play();
        // Delay the firework sound by 1 second to match the sound of the fireworks
        // 花火の音を合わせるために1秒遅らせた
        explosionSound.PlayDelayed(1.0f);
    }

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
