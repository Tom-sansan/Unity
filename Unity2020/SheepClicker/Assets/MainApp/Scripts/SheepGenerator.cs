using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepGenerator : MonoBehaviour
{
    /// <summary>
    /// Sheep prefab
    /// </summary>
    [SerializeField]
    private Sheep sheepPrefab;

    /// <summary>
    /// Create sheep
    /// </summary>
    public void CreateSheep(SheepData sheepData)
    {
        var sheep = Instantiate(sheepPrefab);
        sheep.sheepData = sheepData;
    }
}
