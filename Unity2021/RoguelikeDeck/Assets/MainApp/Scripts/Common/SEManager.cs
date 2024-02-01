using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SE (sound effect) playback class
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class SEManager : MonoBehaviour
{
    #region Enum

    /// <summary>
    /// List of registered sound effect definitions
    /// </summary>
    public enum SEName
    {
        DecideA,            // Button sound A
        DecideB,            // Button sound B
        DamageToEnemy,      // Damage to enemy
        DamageToPlayer,     // Damage to player
    }

    #endregion Enum

    #region Variables

    #region SerializeField

    /// <summary>
    /// Registered sound effect reference list
    /// (stored from Inspector in the same order as the definition list above)
    /// </summary>
    [SerializeField]
    private List<AudioClip> seClips = null;

    #endregion SerializeField

    #region Public Variables

    #endregion Public Variables

    /// <summary>
    /// SE Manager instance
    /// </summary>
    public static SEManager instance { get; private set; }

    #region Private Variables

    /// <summary>
    /// AudioSource for SE playback
    /// </summary>
    private AudioSource audioSource;

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {

    }
    #endregion Unity Methods

    #region Public Methods

    /// <summary>
    /// Play the specified SE
    /// </summary>
    /// <param name="seName"></param>
    public void PlaySE(SEName seName) =>
        // Play SE
        audioSource.PlayOneShot(seClips[(int)seName]);

    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    #endregion Methods
}
