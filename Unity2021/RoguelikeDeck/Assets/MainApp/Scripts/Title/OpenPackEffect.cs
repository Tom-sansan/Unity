using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pack opening performance processing class
/// </summary>
public class OpenPackEffect : MonoBehaviour
{
    #region Class

    #endregion Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField

    #endregion SerializeField

    /// <summary>
    /// Opening particle
    /// </summary>
    [SerializeField]
    private ParticleSystem unpackParticle = null;
    /// <summary>
    /// Canvas group
    /// </summary>
    [SerializeField]
    private CanvasGroup canvasGroup = null;
    /// <summary>
    /// Card prefab
    /// </summary>
    [SerializeField]
    private GameObject cardPrefab = null;
    /// <summary>
    /// Parent Transform of card object
    /// </summary>
    [SerializeField]
    private Transform cardsParent = null;

    #region Public Variables

    #endregion Public Variables

    #region Private Variables

    /// <summary>
    /// Shopping window class
    /// </summary>
    private ShoppingWindow shoppingWindow;
    /// <summary>
    /// Card display direction Sequence
    /// </summary>
    private Sequence effectSequence;

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {

    }
    void Update()
    {

    }
    #endregion Unity Methods

    #region Public Methods

    /// <summary>
    /// Initialization (called from ShoppingWindow.cs)
    /// </summary>
    /// <param name="shoppingWindow"></param>
    public void Init(ShoppingWindow shoppingWindow)
    {
        this.shoppingWindow = shoppingWindow;
        // Initialize UI
        gameObject.SetActive(true);
        SetUI(0.0f, false);
    }
    /// <summary>
    /// Starts production
    /// </summary>
    /// <param name="cardData"></param>
    public void StartEffect(List<CardDataSO> cardData)
    {
        effectSequence = DOTween.Sequence();
        // Background image display direction
        effectSequence.Append(canvasGroup.DOFade(1.0f, 1.0f));
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        // Production of displaying cards in sequence
        foreach (var card in cardData)
        {
            // Create object
            var obj = Instantiate(cardPrefab, cardsParent);
            Card objCard = obj.GetComponent<Card>();
            // Initialize card
            objCard.Init(null, Vector2.zero);
            objCard.SetInitialCardData(card, Card.CharaIDPlayer);
        }
        // Play particle
        unpackParticle.Play();
    }
    /// <summary>
    /// Hide screen
    /// </summary>
    public void ClosePanel()
    {
        // Hide UI
        SetUI(0.0f, false);
        // End of production
        if (effectSequence != null) effectSequence.Kill();
        // Delete generated cards
        for (int i = 0; i < cardsParent.childCount; i++)
        {
            var cardObject = cardsParent.GetChild(i);
            Destroy(cardObject.gameObject);
        }
        // Stop particle
        unpackParticle.Stop();
    }

    #endregion Public Methods

    #region Private Methods

    #endregion Private Methods

    /// <summary>
    /// Hide UI
    /// </summary>
    /// <param name="alphaValue"></param>
    /// <param name="isEnabled"></param>
    private void SetUI(float alphaValue, bool isEnabled)
    {
        canvasGroup.alpha = alphaValue;
        canvasGroup.interactable = isEnabled;
        canvasGroup.blocksRaycasts = isEnabled;
    }
    #endregion Methods
}
