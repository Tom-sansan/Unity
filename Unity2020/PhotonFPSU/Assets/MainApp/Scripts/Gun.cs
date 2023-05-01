using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    #region Variables

    #region Public
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
    /// </summary>
    [Tooltip("Speed when looking in")]
    public float adsSpeed;
    #endregion

    #endregion
}
