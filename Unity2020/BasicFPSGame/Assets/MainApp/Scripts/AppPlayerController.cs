using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using C = Constant;

public class AppPlayerController : MonoBehaviour
{
    #region Variables
    #region SerializeField
    /// <summary>
    /// Display text at start
    /// </summary>
    [SerializeField]
    private Text startText = null;
    /// <summary>
    /// Display UI at clear
    /// </summary>
    [SerializeField]
    private RectTransform clearUi = null;
    /// <summary>
    /// Display text at clear
    /// </summary>
    [SerializeField]
    private Text clearText = null;
    /// <summary>
    /// Display text of time, number of destroyed enemy
    /// </summary>
    [SerializeField]
    private Text countText = null;
    /// <summary>
    /// Arrow prefab
    /// </summary>
    [SerializeField]
    private GameObject arrowPrefab = null;
    /// <summary>
    /// Parent transform that generates arrows
    /// </summary>
    [SerializeField]
    private Transform arrowPoint = null;
    /// <summary>
    /// Shooting position marker
    /// </summary>
    [SerializeField]
    private GameObject shootPointMarker = null;
    /// <summary>
    /// Hit effect image
    /// </summary>
    [SerializeField]
    private Image hitEffectImage = null;
    /// <summary>
    /// HP Slider
    /// </summary>
    [SerializeField]
    private Slider hpSlider = null;
    /// <summary>
    /// Initial HP
    /// </summary>
    [SerializeField]
    private float maxHp = 50f;
    /// <summary>
    /// Move speed
    /// </summary>
    [SerializeField]
    private float moveSpeed = 300f;
    /// <summary>
    /// Speed limit
    /// </summary>
    [SerializeField]
    private float speedLimit = 100f;
    /// <summary>
    /// Camera rotation speed around X axis
    /// </summary>
    [SerializeField]
    private float xRotationSpeed = 5f;
    /// <summary>
    /// Camera rotation speed around Y axis
    /// </summary>
    [SerializeField]
    private float yRotationSpeed = 5f;
    /// <summary>
    /// Jump power
    /// </summary>
    [SerializeField]
    private float jumpPower = 20f;
    /// <summary>
    /// Arrow Firing Power
    /// </summary>
    [SerializeField]
    private float arrowPower = 5f;
    #endregion

    #region Private
    /// <summary>
    /// App Game Controller
    /// </summary>
    private AppGameController gameController = null;
    /// <summary>
    /// Starting position
    /// </summary>
    private Vector3 defaultPosition = Vector3.zero;
    /// <summary>
    /// Starting angle
    /// </summary>
    private Quaternion defaultRotation = Quaternion.identity;
    /// <summary>
    /// Arrows player currently have
    /// </summary>
    // private GameObject currentArrow = null;
    private Arrow currentArrow = null;
    /// <summary>
    /// Rigidbody
    /// </summary>
    private Rigidbody rigid = null;
    /// <summary>
    /// Hit effect coroutine
    /// </summary>
    private Coroutine hitEffectCor = null;
    /// <summary>
    /// Position to start to click mouse
    /// </summary>
    private Vector3 startMousePosition = Vector3.zero;
    /// <summary>
    /// Camera angle at the start of the click
    /// </summary>
    private Vector3 startCameraRotation = Vector3.zero;
    /// <summary>
    /// Shooting position
    /// </summary>
    private Vector3? shootPoint = null;
    /// <summary>
    /// Current HP
    /// </summary>
    private float currentHp = 0;
    /// <summary>
    /// Charging time
    /// </summary>
    private float attackChargeTime = 0;
    /// <summary>
    /// Jumping flag
    /// </summary>
    private bool isJumping = false;
    /// <summary>
    /// Attack interval flag
    /// </summary>
    private bool isAttackWait = false;
    #endregion
    #endregion

    #region Methods
    #region Unity Methods
    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        currentHp = maxHp;
        hpSlider.maxValue = maxHp;
        hpSlider.value = currentHp;
        hitEffectImage.gameObject.SetActive(false);
        // Keeping initial position rotation
        defaultPosition = gameObject.transform.position;
        defaultRotation = Camera.main.gameObject.transform.rotation;
    }

    // Update is called once per frame
    private void Update()
    {
        InputMouseButton();
        ProcessJump();
        // Measuring the shooting position
        UpdateShootRay();
        // Change UI
        UpdateCount();
    }

    private void FixedUpdate()
    {
        var horizontal = Input.GetAxis(C.Horizontal);
        var vertical = Input.GetAxis(C.Vertical);

        if (horizontal != 0 || vertical != 0)
        {
            // Get camera angle(Vector3)
            var cameraRotationEul = Camera.main.transform.rotation.eulerAngles;
            // Change 0 except for Y rotation
            cameraRotationEul.x = 0;
            cameraRotationEul.z = 0;
            var cameraRotation = Quaternion.Euler(cameraRotationEul);
            // 
            var forceX = horizontal * moveSpeed;
            var forceZ = vertical * moveSpeed;
            var force = cameraRotation * new Vector3(forceX, 0, forceZ);
            // Add Rigidboty to force
            rigid.AddForce(force, ForceMode.Force);
            // Limit move speed
            MoveResistance();
        }
        else StopForce();
    }
    #endregion

    #region Public
    /// <summary>
    /// Initialization (from AppGameManager)
    /// </summary>
    /// <param name="appGameManager"></param>
    public void Init(AppGameController appGameManager)
    {
        gameController = appGameManager;
        gameController.ClearEvent.AddListener(OnClear);
        gameController.EnemyDeadEvent.AddListener(OnEnemyDead);
        clearUi.gameObject.SetActive(false);
        SetCountText();
    }
    /// <summary>
    /// Trigger Enter Callback for Grounding Collider
    /// </summary>
    /// <param name="collider"></param>
    public void OnGroundTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag.Equals(C.Ground))
        {
            if (isJumping) StartCoroutine(WaitSwitch(0.5f));
            Debug.Log("Ground enter");
        }
    }
    /// <summary>
    /// Trigger Exit Callback for Grounding Collider
    /// </summary>
    /// <param name="collider"></param>
    public void OnGroundTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag.Equals(C.Ground))
        {
            if (isJumping) StartCoroutine(WaitSwitch(0.5f));
            Debug.Log("Ground enter");
        }
    }
    /// <summary>
    /// Call at the attack hit of enemy
    /// </summary>
    /// <param name="attack"></param>
    public void OnEnemyAttackHit(float attack)
    {
        currentHp -= attack;
        hpSlider.value = currentHp;
        if (hitEffectCor != null)
        {
            StopCoroutine(hitEffectCor);
            hitEffectCor = null;
        }
        hitEffectCor = StartCoroutine(HitEffect());
        if (currentHp <= 0)
        {
            gameController.GameOver();
            OnPlayerDead();
        }
        Debug.Log("Damaged!! The current HP : " + currentHp);
    }
    /// <summary>
    /// Retry button click call back
    /// </summary>
    public void OnRetryButtonClicked()
    {
        currentHp = maxHp;
        hpSlider.maxValue = maxHp;
        hpSlider.value = currentHp;
        hitEffectImage.gameObject.SetActive(false);
        clearUi.gameObject.SetActive(false);

        transform.position = defaultPosition;
        Camera.main.gameObject.transform.rotation = defaultRotation;

        gameController.Retry();
        SetCountText();
    }
    #endregion

    #region Private
    /// <summary>
    /// Display updates according to time
    /// </summary>
    private void UpdateCount()
    {
        var param = gameController.CurrentGameParam;
        if (param.State == AppGameController.GameState.Ready && param.ReadyTime > 0)
        {
            startText.gameObject.SetActive(true);
            var ceil = Mathf.CeilToInt(param.ReadyTime);
            startText.text = $"READY {ceil}";
        }
        else if (param.State == AppGameController.GameState.Play && param.ReadyTime > -1) startText.text = "STARAT";
        else if (param.State == AppGameController.GameState.Play)
        {
            startText.gameObject.SetActive(false);
            SetCountText();
        }
    }
    /// <summary>
    /// Clear-time process
    /// </summary>
    private void OnClear()
    {
        var param = gameController.CurrentGameParam;
        if (param.State != AppGameController.GameState.End) return;
        clearUi.gameObject.SetActive(true);
        var time = $"Time : {param.GameTime.ToString("000.0")}";
        clearText.text = $"CLEAR \n{time}";
    }
    /// <summary>
    /// Dealing with Enemy Destroyed
    /// </summary>
    private void OnEnemyDead()
    {
        SetCountText();
    }
    /// <summary>
    /// Game Over Processing
    /// </summary>
    private void OnPlayerDead()
    {
        var param = gameController.CurrentGameParam;
        if (param.State != AppGameController.GameState.End) return;
        clearUi.gameObject.SetActive(true);
        var time = $"Time : {param.GameTime.ToString("000.0")}";
        clearText.color = Color.red;
        clearText.text = $"GAME OVER \n{time}";
    }
    /// <summary>
    /// Show count
    /// </summary>
    private void SetCountText()
    {
        var param = gameController.CurrentGameParam;
        var time = $"Time : {param.GameTime.ToString("000.0")}";
        var dest = $"Beat : {param.EnemyDestroyCount}";
        countText.text = $"{time} {dest}";
    }
    /// <summary>
    /// Mouse button action
    /// </summary>
    private void InputMouseButton()
    {
        // Start to click
        if (Input.GetMouseButtonDown(0))
        {
            // Store mouse position and camera angle
            startMousePosition = Input.mousePosition;
            startCameraRotation = Camera.main.gameObject.transform.localRotation.eulerAngles;
            if (gameController != null && gameController.CurrentGameParam.State == AppGameController.GameState.Play && !isAttackWait)
            {
                // currentArrow = Instantiate(arrowPrefab, arrowPoint.position, arrowPoint.rotation, arrowPoint);
                var currentArrowGo = Instantiate(arrowPrefab, arrowPoint.position, arrowPoint.rotation, arrowPoint);
                currentArrow = currentArrowGo.GetComponent<Arrow>();
                currentArrow.OnCreated();
                isAttackWait = true;
                attackChargeTime = 0;
            }
        }
        // Clicking
        else if (Input.GetMouseButton(0))
        {
            // Get mouse position at this time
            var currentMousePosition = Input.mousePosition;
            // Caliculate the offset from the start position of click
            var def = (currentMousePosition - startMousePosition);
            // The current camera angle
            var currentCameraRotation = Camera.main.transform.localRotation.eulerAngles;
            // Calculate rotation angle
            currentCameraRotation.x = startCameraRotation.x + (def.y * xRotationSpeed * 0.01f);
            currentCameraRotation.y = startCameraRotation.y + (-def.x * yRotationSpeed * 0.01f);
            // Apply to Camera
            Camera.main.transform.localRotation = Quaternion.Euler(currentCameraRotation);
            if (currentArrow != null) attackChargeTime += Time.deltaTime;
        }
        // End clicking
        else if (Input.GetMouseButtonUp(0))
        {
            // Reset the stored values...
            startMousePosition = Vector3.zero;
            startCameraRotation = Vector3.zero;
            // Shoot arrow
            if (currentArrow != null) ShootArrow();
        }
    }
    /// <summary>
    /// Process jump
    /// </summary>
    private void ProcessJump()
    {
        // Jump process
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            isJumping = true;
            rigid.AddForce(rigid.gameObject.transform.up * jumpPower, ForceMode.Impulse);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void MoveResistance()
    {
        // Get the current speed
        var currentVelocity = rigid.velocity;
        currentVelocity.y = 0;
        if (currentVelocity.sqrMagnitude > speedLimit)
        {
            // Decrease the speed by multiplying by 0.8
            currentVelocity.x *= 0.8f;
            currentVelocity.y = rigid.velocity.y;
            currentVelocity.z *= 0.8f;
            rigid.velocity = currentVelocity;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void StopForce()
    {
        // Get the current speed
        var currentVelocity = rigid.velocity;
        currentVelocity.y = 0;
        if (currentVelocity.sqrMagnitude > 0.01f)
        {
            // Decrease the speed by multiplying by 0.8
            currentVelocity.x *= 0.8f;
            currentVelocity.y = rigid.velocity.y;
            currentVelocity.z *= 0.8f;
            rigid.velocity = currentVelocity;
        }
    }
    /// <summary>
    /// Coroutine that waits for a specified number of seconds before setting to ensure flag switching upon arrival
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    private IEnumerator WaitSwitch(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        isJumping = false;
    }
    /// <summary>
    /// Process of firing arrow
    /// </summary>
    private void ShootArrow()
    {
        //var arrowRigid = currentArrow.gameObject.GetComponent<Rigidbody>();
        //arrowRigid.useGravity = false;
        //arrowRigid.isKinematic = false;
        //arrowRigid.AddForce(Camera.main.gameObject.transform.forward * arrowPower * 0.1f, ForceMode.Impulse);
        var _charge = GetChargeValue(attackChargeTime);
        if (shootPoint != null)
        {
            var dir = ((Vector3)shootPoint - currentArrow.transform.position).normalized;
            currentArrow.Shoot(dir * arrowPower * 0.1f, ForceMode.Impulse, _charge);
        }
        else
        {
            currentArrow.Shoot(Camera.main.gameObject.transform.forward * arrowPower * 0.1f, ForceMode.Impulse, _charge);
        }
        currentArrow.transform.parent = null;
        currentArrow = null;
        StartCoroutine(AttackWait());
    }
    /// <summary>
    /// Ray processing of firing position determination
    /// </summary>
    private void UpdateShootRay()
    {
        // Create ray at the center of screen
        var ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0));
        // Keep hit position
        var hits = Physics.RaycastAll(ray);
        // Variables to get the nearest point
        var currentNear = 0f;
        RaycastHit? nearHit = null;
        // If hit on something
        if (hits != null && hits.Length > 0)
        {
            // Search for all hits
            foreach (var h in hits)
            {
                // If the arrow is hit, ignore it
                if (h.collider.gameObject.tag.Equals(C.Arrow)) continue;
                if (h.collider.gameObject.layer.Equals(9)) continue;
                // Calculate the distance (squared) from the camera to the hit location to find the closest point
                var dis = (Camera.main.transform.position - h.point).sqrMagnitude;
                if (nearHit == null || (currentNear != 0 && currentNear > dis))
                {
                    nearHit = h;
                    currentNear = dis;
                }
            }
        }
        else nearHit = null;

        // If there is a point closest
        if (nearHit != null)
        {
            var hit = (RaycastHit)nearHit;
            // Show marker
            shootPointMarker.gameObject.SetActive(true);
            shootPointMarker.transform.position = hit.point;
            shootPoint = shootPointMarker.transform.position;
        }
        else
        {
            // Hide marker
            shootPointMarker.gameObject.SetActive(false);
            shootPoint = null;
        }
    }
    /// <summary>
    /// Effect of attach hit
    /// </summary>
    /// <returns></returns>
    private IEnumerator HitEffect()
    {
        hitEffectImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        hitEffectImage.gameObject.SetActive(false);
    }
    /// <summary>
    /// Coroutine for adjusting the attack interval
    /// </summary>
    /// <returns></returns>
    private IEnumerator AttackWait()
    {
        yield return new WaitForSeconds(1f);
        isAttackWait = false;
    }
    /// <summary>
    /// Get the increase value from the charge time
    /// </summary>
    /// <param name="chargeTime">Charge time</param>
    /// <returns>Rate of increase</returns>
    private float GetChargeValue(float chargeTime)
    {
        if (chargeTime <= 1.5f) return 1f;
        else if (chargeTime < 3) return 1.25f;
        else return 1.5f;
    }
    #endregion
    #endregion
}
