using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Class
    /// <summary>
    /// Status Class
    /// </summary>
    [Serializable]
    public class Status
    {
        /// <summary>
        /// HP
        /// </summary>
        public int Hp = 10;
        /// <summary>
        /// Attack power
        /// </summary>
        public int Power = 1;
    }
    #endregion

    #region Variables
    /// <summary>
    /// HP bar slider
    /// </summary>
    [SerializeField]
    private Slider hpBar = null;
    /// <summary>
    /// Ojects for judging attacks
    /// </summary>
    [SerializeField]
    private GameObject attackHit = null;
    /// <summary>
    /// Collider call for Attack hit
    /// </summary>
    [SerializeField]
    private ColliderCallReceiver attackHitCall = null;
    /// <summary>
    /// Collider call for grounding determination
    /// </summary>
    [SerializeField]
    private ColliderCallReceiver footColliderCall = null;
    /// <summary>
    /// Touch Marker
    /// </summary>
    [SerializeField]
    private GameObject touchMarker = null;
    /// <summary>
    /// My collider
    /// </summary>
    [SerializeField]
    private Collider myCollider = null;
    /// <summary>
    /// Particle prefab of the time when player is attacked.
    /// </summary>
    [SerializeField]
    private GameObject hitParticlePrefab = null;
    /// <summary>
    /// Jump power
    /// </summary>
    [SerializeField]
    private float jumpPower = 20f;
    /// <summary>
    /// Camera controller
    /// </summary>
    [SerializeField]
    private PlayerCameraController cameraController = null;
    /// <summary>
    /// Basic status
    /// </summary>
    [SerializeField]
    private Status DefaultStatus = new Status();
    /// <summary>
    /// Current status
    /// </summary>
    public Status CurrentStatus = new Status();
    /// <summary>
    /// Game over event
    /// </summary>
    public UnityEvent GameOverEvent = new UnityEvent();
    /// <summary>
    /// Animator
    /// </summary>
    private Animator animator = null;
    /// <summary>
    /// Rigidbody
    /// </summary>
    private Rigidbody rigid = null;
    /// <summary>
    /// List to store particle object
    /// </summary>
    private List<GameObject> particleObjectList = new List<GameObject>();
    /// <summary>
    /// Start position
    /// </summary>
    private Vector3 startPosition = new Vector3();
    /// <summary>
    /// Start rotation
    /// </summary>
    private Quaternion startRotation = new Quaternion();
    /// <summary>
    /// Left half touch start position
    /// </summary>
    private Vector2 leftStartTouch = new Vector2();
    /// <summary>
    /// Left half touch input
    /// </summary>
    private Vector2 leftTouchInput = new Vector2();
    /// <summary>
    /// Flag during attack animation
    /// </summary>
    private bool isAttack = false;
    /// <summary>
    /// Grounding flag
    /// </summary>
    private bool isGround = false;
    /// <summary>
    /// Touch flag
    /// </summary>
    private bool isTouch = false;
    /// <summary>
    /// PC key horizontal input
    /// </summary>
    private float horizontalKeyInput = 0;
    /// <summary>
    /// PC Key Vertical Entry
    /// </summary>
    private float verticalKeyInput = 0;
    /// <summary>
    /// Strings of Animator parameters
    /// </summary>
    private const string strIsRun = "isRun";
    private const string strIsAttack = "isAttack";
    private const string strIsGround = "isGround";
    private const string strGround = "Ground";
    #endregion

    private void Start()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        // Hides objects for judging attacks
        attackHit.SetActive(false);
        // Event Registration for FootSphere
        footColliderCall.TriggerStayEvent.AddListener(OnFootTriggerStay);
        footColliderCall.TriggerExitEvent.AddListener(OnFootTriggerExit);
        // Collider event registration for attack judgment
        attackHitCall.TriggerEnterEvent.AddListener(OnAttackHitTriggerEnter);
        // Initialize current status
        CurrentStatus.Hp = DefaultStatus.Hp;
        CurrentStatus.Power = DefaultStatus.Power;
        // Store starting position rotation
        startPosition = this.transform.position;
        startRotation = this.transform.rotation;
        // Initialize slider
        hpBar.maxValue = DefaultStatus.Hp;
        hpBar.value = CurrentStatus.Hp;
    }

    private void Update()
    {
        // Point the camera at the player
        cameraController.UpdateCameraLook(this.transform);

        if (IsSmartphone())
        {
            // Smartphone touch operation
            // The number of fingers touching is greater than 0
            if (Input.touchCount > 0)
            {
                isTouch = true;
                // Get all of touch infomation
                Touch[] touches = Input.touches;
                // Repeat all touches to judge
                foreach (var touch in touches)
                {
                    var isLeftTouch = false;
                    var isRightTouch = false;
                    // X-axis direction of touch position is to the left of the screen
                    if (touch.position.x > 0 && touch.position.x < Screen.width / 2) isLeftTouch = true;
                    // X-axis direction of touch position is to the right of the screen
                    else if (touch.position.x > Screen.width / 2 && touch.position.x < Screen.width) isRightTouch = true;
                    
                    if (isLeftTouch)
                    {
                        // Touch start
                        if (touch.phase == TouchPhase.Began)
                        {
                            // Store the starting position
                            leftStartTouch = touch.position;
                            // Marker at start position
                            touchMarker.SetActive(true);
                            Vector3 touchPosition = touch.position;
                            touchPosition.z = 1f;
                            var markerPosition = Camera.main.ScreenToWorldPoint(touchPosition);
                            touchMarker.transform.position = markerPosition;
                            Debug.Log("Touch start");
                        }
                        // Touching
                        else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                        {
                            // Store current position at any time
                            var position = touch.position;
                            // Store directions for moving
                            leftTouchInput = position - leftStartTouch;
                            Debug.Log("Touching");
                        }
                        // Touch end
                        else if (touch.phase == TouchPhase.Ended)
                        {
                            leftTouchInput = Vector2.zero;
                            //
                            touchMarker.gameObject.SetActive(false);
                            Debug.Log("Touch end");
                        }
                    }
                    if (isRightTouch)
                    {
                        cameraController.UpdateRightTouch(touch);
                    }
                }
            }
            else isTouch = false;
        }
        else
        {
            // Get PC key input
            horizontalKeyInput = Input.GetAxis("Horizontal");
            verticalKeyInput = Input.GetAxis("Vertical");
        }
        // Adjust Player Orientation
        bool isKeyInput = horizontalKeyInput != 0 || verticalKeyInput != 0 || leftTouchInput != Vector2.zero;
        if (isKeyInput && !isAttack)
        {
            // Run
            var isCurrentRun = animator.GetBool(strIsRun);
            if (!isCurrentRun) animator.SetBool(strIsRun, true);
            var dir = rigid.velocity.normalized;
            dir.y = 0;
            // Player Orientation
            this.transform.forward = dir;
        }
        else
        {
            // Stop runing
            var isCurrentRun = animator.GetBool(strIsRun);
            if (isCurrentRun) animator.SetBool(strIsRun, false);
        }
        Debug.Log(horizontalKeyInput + ", " );
    }

    private void FixedUpdate()
    {
        // Adjust camera position to the player
        cameraController.FixedUpdateCameraPosition(this.transform);
        
        if (isAttack) return;
        var input = new Vector3();
        var move = new Vector3();
        if (IsSmartphone())
        {
            input = new Vector3(leftTouchInput.x, 0, leftTouchInput.y);
            move = input.normalized * 2f;
        }
        else
        {
            input = new Vector3(horizontalKeyInput, 0, verticalKeyInput);
            move = input.normalized * 2f;
        }

        var cameraMove = Camera.main.gameObject.transform.rotation * move;
        cameraMove.y = 0;
        var currentRigidVelocity = rigid.velocity;
        currentRigidVelocity.y = 0;
        rigid.AddForce(cameraMove - currentRigidVelocity, ForceMode.VelocityChange);
    }
    /// <summary>
    /// Attack button click callback
    /// </summary>
    public void OnAttachButtonClicked()
    {
        if (!isAttack)
        {
            // Run isAttack trigger of Animation
            animator.SetTrigger(strIsAttack);
            // Start attack
            isAttack = true;
        }
    }
    /// <summary>
    /// Jump button click callback
    /// </summary>
    public void OnJumpButtonClicked()
    { 
        if (isGround) rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
    }
    /// <summary>
    /// Process that an enemy attack hits player
    /// </summary>
    /// <param name="damage"></param>
    public void OnEnemyAttackHit(int damage, Vector3 attackPosition)
    {
        // Damage setting
        CurrentStatus.Hp -= damage;
        // Sider setting
        hpBar.value = CurrentStatus.Hp;
        var pos = myCollider.ClosestPoint(attackPosition);
        var obj = Instantiate(hitParticlePrefab, pos, Quaternion.identity);
        var par = obj.GetComponent<ParticleSystem>();

        StartCoroutine(WaitDestroy(par));
        particleObjectList.Add(obj);
        if (CurrentStatus.Hp <= 0) OnDie();
        else Debug.Log("Attacked by " + damage + " damage! Remaining HP is " + CurrentStatus.Hp);
    }
    /// <summary>
    /// Retry
    /// </summary>
    public void Retry()
    {
        // Initialize the current status
        CurrentStatus.Hp = DefaultStatus.Hp;
        CurrentStatus.Power = DefaultStatus.Power;
        hpBar.value = CurrentStatus.Hp;
        // Returns the rotational position to the initial position
        this.transform.position = startPosition;
        this.transform.rotation = startRotation;
        // For the time when being beaten during player attack
        isAttack = false;
    }
    /// <summary>
    /// Heal process
    /// </summary>
    public void OnHeal(int healPoint)
    {
        CurrentStatus.Hp += healPoint;
        if (CurrentStatus.Hp > DefaultStatus.Hp) CurrentStatus.Hp = DefaultStatus.Hp;
        hpBar.value = CurrentStatus.Hp;
        Debug.Log("HP recovered by " + healPoint + "P");
    }
    /// <summary>
    /// Destroy particle when it ends
    /// </summary>
    /// <param name="particle"></param>
    /// <returns></returns>
    private IEnumerator WaitDestroy(ParticleSystem particle)
    {
        yield return new WaitUntil(() => particle.isPlaying == false);
        if (particleObjectList.Contains(particle.gameObject)) particleObjectList.Remove(particle.gameObject);
        Destroy(particle.gameObject);
    }
    /// <summary>
    /// Attack Animation Hit Event Call
    /// </summary>
    private void Anim_AttackHit()
    {
        Debug.Log("Hit");
        // Show Ojects for judging attacks
        attackHit.SetActive(true);
    }
    /// <summary>
    /// Attack Animation End Event Call
    /// </summary>
    private void Anim_AttackEnd()
    {
        Debug.Log("End");
        // Hides Ojects for judging attacks
        attackHit.SetActive(false);
        // End attack
        isAttack = false;
    }
    /// <summary>
    /// FootSphere trigger enter call
    /// </summary>
    /// <param name="collider"></param>
    private void OnFootTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == strGround)
        {
            isGround = true;
            animator.SetBool(strIsGround, true);
        }
    }
    /// <summary>
    /// FootSphere trigger stay call
    /// </summary>
    /// <param name="collider"></param>
    private void OnFootTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == strGround)
        {
            if (!isGround) isGround = true;
            if (!animator.GetBool(strIsGround)) animator.SetBool(strIsGround, true);
        }
    }
    /// <summary>
    /// FootSphere trigger exit call
    /// </summary>
    /// <param name="collider"></param>
    private void OnFootTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == strGround)
        {
            isGround = false;
            animator.SetBool(strIsGround, false);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="collider"></param>
    private void OnAttackHitTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            var enemy = collider.gameObject.GetComponent<EnemyBase>();
            enemy?.OnAttackHit(CurrentStatus.Power, this.transform.position);
            attackHit.SetActive(false);
        }
    }
    /// <summary>
    /// Process of death
    /// </summary>
    private void OnDie()
    {
        StopAllCoroutines();
        if (particleObjectList.Count > 0)
        {
            foreach (var obj in particleObjectList) Destroy(obj);
            particleObjectList.Clear();
        }
        GameOverEvent?.Invoke();
        Debug.Log("You died...");
    }
    /// <summary>
    /// Check if platform is a smartphone device or not.
    /// </summary>
    /// <returns></returns>
    private bool IsSmartphone() =>
        Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android;
}
