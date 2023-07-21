using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    #region Variables

    #region Public Variables
    /// <summary>
    /// Bullet Impact
    /// </summary>
    public GameObject bulletImpact;
    /// <summary>
    /// Shoot Interval
    /// </summary>
    [Tooltip("Shoot Interval")]
    public float shootInterval = 0.1f;
    /// <summary>
    /// Shot Damage
    /// </summary>
    [Tooltip("Shot Damage")]
    public int shotDamage;
    /// <summary>
    /// Zoom when looking in
    /// </summary>
    [Tooltip("Zoom when looking in")]
    public float adsZoom;
    /// <summary>
    /// Speed when looking in
    /// ads: Aim down sight
    /// </summary>
    [Tooltip("Speed when looking in")]
    public float adsSpeed;
    /// <summary>
    /// Sound Source
    /// </summary>
    public AudioSource shotSound;
    #endregion Public Variables

    #region Methods

    #region Public Methods
    /// <summary>
    /// Sound gunshot
    /// </summary>
    public void SoundGunShot() =>
        shotSound.Play();
    /// <summary>
    /// Sound assault rifle shots
    /// </summary>
    public void LoopOnSubmachineGun()
    {
        // Check if sound is playing
        if (!shotSound.isPlaying)
        {
            shotSound.loop = true;
            SoundGunShot();
        }
    }
    /// <summary>
    /// Stop sound of assault rifle shots
    /// </summary>
    public void LoopOffSubmachineGun()
    {
        shotSound.loop = false;
        shotSound.Stop();
    }
    #endregion Public Methods

    #endregion Methods

    #endregion
}
