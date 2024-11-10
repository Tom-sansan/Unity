using UnityEngine;

/// <summary>
/// TRexRoar Class
/// </summary>
public class TRexRoar : MonoBehaviour
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// AudioSource for T-Rex
    /// </summary>
    public AudioSource audioSource;

    #endregion Private Variables

    #region Properties

    #endregion Properties

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {

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

    #region Private Methods

    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug


}
