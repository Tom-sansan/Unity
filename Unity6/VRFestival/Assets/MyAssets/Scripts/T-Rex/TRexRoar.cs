using UnityEngine;

/// <summary>
/// TRexRoar Class
/// </summary>
public class TRexRoar : MonoBehaviour
{
    #region Variables

    #region SerializeField

    /// <summary>
    /// AudioSource for T-Rex
    /// </summary>
    [SerializeField]
    private AudioSource audioSource;

    #endregion SerializeField

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {
        // var audioSourceObj = GameObject.Find(nameof(TRexRoar));
        audioSource = GameObject.Find(nameof(TRexRoar)).GetComponent<AudioSource>();

    }
    #endregion Unity Methods

    #region Public Methods

    /// <summary>
    /// Roar by T-Rex
    /// </summary>
    public void Roar()
    {
        audioSource.Play();
    }

    #endregion Public Methods

    #endregion Methods
}
