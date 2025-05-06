using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// BattleScript Class
/// </summary>
public class BattleScript : MonoBehaviour
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField
    /// <summary>
    /// Spinner
    /// </summary>
    [SerializeField]
    private Spinner spinnerScript;
    /// <summary>
    /// Spin speed bar image
    /// </summary>
    [SerializeField]
    private Image spinSpeedBarImage;
    /// <summary>
    /// Spin speed ratio text
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI spinSpeedRatioText;
    /// <summary>
    /// Common damage coefficient
    /// </summary>
    [SerializeField]
    private float commonDamageCoefficient = 0.04f;
    /// <summary>
    /// Attacker flag
    /// </summary>
    [SerializeField]
    private bool isAttacker;
    /// <summary>
    /// Defender flag
    /// </summary>
    [SerializeField]
    private bool isDefender;
    [Header("Player Type Damage Coefficients")]
    [Header("Attacker")]
    /// <summary>
    /// Do damage coefficient for Attacker
    /// * Do more damage than defender  -- Advantage
    /// </summary>
    [SerializeField]
    private float doDamageCoefficientAttacker = 10f;
    /// <summary>
    /// Get damage coefficient for Attacker
    /// * Get more damage than defender -- Disadvantage
    /// </summary>
    [SerializeField]
    private float getDamageCoefficientAttacker = 1.2f;
    /// <summary>
    /// Do damage coefficient for Defender
    /// * Do less damage than attacker  -- Disadvantage
    /// </summary>
    [Header("Defender")]
    [SerializeField]
    private float doDamageCoefficientDefender = 0.75f;
    /// <summary>
    /// Get damage coefficient for Defender
    /// * Get less damage than attacker -- Advantage
    /// </summary>
    [SerializeField]
    private float getDamageCoefficientDefender = 0.2f;

    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

    #region Public Const Variables

    #endregion Public Const Variables

    #region Public Properties

    #endregion Public Properties

    #endregion Public Variables

    #region Private Variables
    /// <summary>
    /// Start spin speed
    /// </summary>
    private float startSpinSpeed;
    /// <summary>
    /// Current spin speed
    /// </summary>
    private float currentSpinSpeed;

    #region Private Const Variables

    #endregion Private Const Variables

    #region Private Properties

    #endregion Private Properties

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    private void Awake()
    {
        startSpinSpeed = spinnerScript.spinnerSpeed;
        currentSpinSpeed = spinnerScript.spinnerSpeed;
        spinSpeedBarImage.fillAmount = currentSpinSpeed / startSpinSpeed;
    }
    void Start()
    {
        Init();
    }
    void Update()
    {

    }
    /// <summary>
    /// OnCollisionEnter callback method
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Comparing the speeds of the spinnerTops
            float myPlayerSpeed = gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            // Remote player speed in local
            float otherPlayerSpeed = collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            Debug.Log($"My player speed: {myPlayerSpeed}\nOther player speed: {otherPlayerSpeed}");
            if (myPlayerSpeed > otherPlayerSpeed)
            {
                if (collision.collider.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    // Apply damage to the other player
                    collision.collider.gameObject.GetComponent<PhotonView>().RPC(nameof(DoDamage), RpcTarget.AllBuffered, CalculateDamage());
                }
                Debug.Log("You DAMAGED the other player");
            }
            else
            {
                Debug.Log("You GET DAMAGED!");
            }
        }
    }
    #endregion Unity Methods

    #region Public Methods

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialize this class
    /// </summary>
    private void Init()
    {
        CheckPlayerType();
        AdjustSpinSpeedForDefender();
    }
    /// <summary>
    /// Do damage to the other player
    /// </summary>
    [PunRPC]
    private void DoDamage(float _damageAmount)
    {
        if (isAttacker) _damageAmount *= getDamageCoefficientAttacker;
        else if (isDefender) _damageAmount *= getDamageCoefficientDefender;
        spinnerScript.spinnerSpeed -= _damageAmount;
        currentSpinSpeed = spinnerScript.spinnerSpeed;
        spinSpeedBarImage.fillAmount = currentSpinSpeed / startSpinSpeed;
        spinSpeedRatioText.text = GetSpinSpeedRatioText(currentSpinSpeed, startSpinSpeed);
    }
    /// <summary>
    /// Check player type, Attacker or Defender
    /// </summary>
    private void CheckPlayerType()
    {
        isAttacker = gameObject.name.Contains("Attacker");
        isDefender = gameObject.name.Contains("Defender");
    }
    /// <summary>
    /// Calculate damage
    /// </summary>
    /// <returns></returns>
    private float CalculateDamage()
    {
        float defaultDamageAmount = gameObject.GetComponent<Rigidbody>().velocity.magnitude * 3600 * commonDamageCoefficient;
        if (isAttacker) defaultDamageAmount *= doDamageCoefficientAttacker;
        else if (isDefender) defaultDamageAmount *= doDamageCoefficientDefender;
        return defaultDamageAmount;
    }
    /// <summary>
    /// Adjust spin speed for Defender
    /// </summary>
    private void AdjustSpinSpeedForDefender()
    {
        if (isAttacker) return;
        spinnerScript.spinnerSpeed = 4400;
        startSpinSpeed = spinnerScript.spinnerSpeed;
        currentSpinSpeed = spinnerScript.spinnerSpeed;
        spinSpeedRatioText.text = GetSpinSpeedRatioText(currentSpinSpeed, startSpinSpeed);
    }
    /// <summary>
    /// Get spin speed ratio text
    /// </summary>
    /// <param name="currentSpinSpeed"></param>
    /// <param name="startSpinSpeed"></param>
    /// <returns></returns>
    private string GetSpinSpeedRatioText(float currentSpinSpeed, float startSpinSpeed) =>
        $"{currentSpinSpeed.ToString("F0")}/{startSpinSpeed}";

    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
