using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shop Class
/// </summary>
public class Shop : MonoBehaviour
{
    /// <summary>
    /// Purchase button
    /// </summary>
    [SerializeField]
    private SheepButton sheepButtonPrefab;
    /// <summary>
    /// Sheep generation object to be set in SheepButton
    /// </summary>
    [SerializeField]
    private SheepGenerator sheepGenerator;
    /// <summary>
    /// Possession object to be set in SheepButton
    /// </summary>
    [SerializeField]
    private Wallet wallet;
    /// <summary>
    /// Array of sheep data to be generated
    /// </summary>
    public SheepData[] sheepData;
    /// <summary>
    /// Keep the created SheepButton in a List.
    /// </summary>
    public List<SheepButton> sheepButtonList;

    void Awake()
    {
        Init();
    }

    /// <summary>
    /// Initialization
    /// </summary>
    private void Init()
    {
        // Create as many SheepButtons as the number of SheepData arrays received.
        foreach (var data in sheepData)
        {
            // Generate in child elements by specifying transform
            var sheepButton = Instantiate(sheepButtonPrefab, transform);
            sheepButton.sheepData = data;
            sheepButtonList.Add(sheepButton);
            sheepButton.sheepGenerator = sheepGenerator;
            sheepButton.wallet = wallet;
        }
    }
}
