using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SheepButton : MonoBehaviour
{
    /// <summary>
    /// Button object
    /// </summary>
    [SerializeField]
    private Button button;
    /// <summary>
    /// Sheep image
    /// </summary>
    [SerializeField]
    private Image sheepImage;
    /// <summary>
    /// Price text
    /// </summary>
    [SerializeField]
    private Text priceText;
    /// <summary>
    /// Number of sheep
    /// </summary>
    [SerializeField]
    private Text countText;
    /// <summary>
    /// Sale Availability Text
    /// </summary>
    [SerializeField]
    private Text infoText;
    /// <summary>
    /// Money possessed object
    /// </summary>
    public Wallet wallet;
    /// <summary>
    /// Sheep data
    /// </summary>
    public SheepData sheepData;
    /// <summary>
    /// Sheep generator
    /// </summary>
    public SheepGenerator sheepGenerator;
    /// <summary>
    /// The current number of sheep
    /// </summary>
    public int currentCount;

    void Start()
    {
        Debug.Log(button.gameObject.name);
        button.onClick.AddListener(CreateSheep);
    }

    void Update()
    {
        SetText();
    }

    /// <summary>
    /// Create sheep
    /// </summary>
    public void CreateSheep()
    {
        Debug.Log(nameof(CreateSheep));
        var price = GetPrice();
        // Purchase is deducted from the amount of money held
        wallet.money -= price;
        currentCount++;
        sheepGenerator.CreateSheep(sheepData);
    }

    /// <summary>
    /// Set UI Texts
    /// </summary>
    private void SetText()
    {
        var price = GetPrice();
        Debug.Log(price);
        sheepImage.color = sheepData.color;
        priceText.text = price.ToString("C0");
        // The current / maximum number of sheep
        countText.text = $"{currentCount}頭/{sheepData.maxCount}頭";
        // Whether the purchase limit
        if (currentCount >= sheepData.maxCount)
        {
            infoText.text = "完売";
            button.interactable = false;
        }
        // Money in possession is higher
        else if (wallet.money >= price)
        {
            infoText.text = "購入";
            button.interactable = true;
        }
        // Not enough money in possession
        else
        {
            infoText.text = "お金が足りません";
            button.interactable = false;
        }
    }
    /// <summary>
    /// Return the current amount of sheep
    /// </summary>
    /// <returns></returns>
    private int GetPrice() =>
        //  Calculate next purchase amount based on current sheep count
        sheepData.basePrice + sheepData.extendPrice * currentCount;
}
