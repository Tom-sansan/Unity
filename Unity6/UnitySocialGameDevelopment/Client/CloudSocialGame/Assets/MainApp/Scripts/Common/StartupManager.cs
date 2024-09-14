using System;
using UnityEngine;
using S = SingletonData;

/// <summary>
/// StartupManager Class
/// </summary>
public class StartupManager : MonoBehaviour
{
    private static StartupConfig startupConfig;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void OnBeforeSceneLoad()
    {
        try
        {
            startupConfig = ResourcesData.GetResourcesData<StartupConfig>(nameof(StartupConfig));
            if (startupConfig == null)
            {
                Debug.LogError($"Failed to load {nameof(StartupConfig)}.");
                return;
            }

            var instance = new GameObject(nameof(StartupManager)).AddComponent<StartupManager>();
            instance.Init();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Initialization
    /// </summary>
    private void Init()
    {
        if (startupConfig != null) S.Instance.LoadData(ResourcesData.GetResourcesData<TextAsset>(startupConfig.jsonFileName)?.text);
        else Debug.LogError($"{nameof(StartupConfig)} is not assigned.");
    }
}
