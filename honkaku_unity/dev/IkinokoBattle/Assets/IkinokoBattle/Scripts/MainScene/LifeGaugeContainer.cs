using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Management of multiple life gauge
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class LifeGaugeContainer : MonoBehaviour
{
    // Camera to show Mob of life gauge display target
    [SerializeField] private Camera mainCamera;
    // Prefab of life gauge
    [SerializeField] private LifeGauge lifeGaugePrefab;
    private RectTransform rectTransform;
    // Container to keep active life gauge
    private readonly Dictionary<MobStatus, LifeGauge> _statusLifeBarMap = new Dictionary<MobStatus, LifeGauge>();

    private static LifeGaugeContainer _instance;
    public static LifeGaugeContainer Instance => _instance;

    private void Awake()
    {
        // シーン上に1つしか存在させないスクリプトのため、このような疑似シングルトンが成り立つ
        if (_instance != null) throw new Exception("LifeBarContainer instance already exists!");

        _instance = this;
        rectTransform = GetComponent<RectTransform>();
    }

    /// <summary>
    /// Add lige gauge
    /// </summary>
    /// <param name="status"></param>
    public void Add(MobStatus status)
    {
        var lifeGauge = Instantiate(lifeGaugePrefab, transform);
        lifeGauge.Initialize(rectTransform, mainCamera, status);
        _statusLifeBarMap.Add(status, lifeGauge);
    }

    /// <summary>
    /// Remove life gauge
    /// </summary>
    /// <param name="status"></param>
    public void Remove(MobStatus status)
    {
        Destroy(_statusLifeBarMap[status].gameObject);
        _statusLifeBarMap.Remove(status);
    }
}
