using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerController : MonoBehaviour
{
    #region Variables

    #region Public
    /// <summary>
    /// Parent object of camera
    /// </summary>
    public Transform viewPoint;
    /// <summary>
    /// Jump force
    /// </summary>
    public Vector3 jumpForce = new Vector3(0, 6f, 0);
    /// <summary>
    /// Position of the object from which the ray is to be flown
    /// </summary>
    public Transform groundCheckPoint;
    /// <summary>
    /// Ground layer
    /// </summary>
    public LayerMask groundLayers;
    /// <summary>
    /// Weapons storage list
    /// </summary>
    public List<Gun> guns = new List<Gun>();
    /// <summary>
    /// Possession ammunition
    /// </summary>
    [Tooltip("Possession ammunition")]
    public int[] ammunition;
    /// <summary>
    /// The maximum number of possession ammunition
    /// </summary>
    [Tooltip("The maximum number of possession ammunition")]
    public int[] maxAmmunition;
    /// <summary>
    /// Number of bullets in machine gun
    /// </summary>
    [Tooltip("The maximum number of possession ammunition")]
    public int[] ammoClip;
    /// <summary>
    /// Maximum number of ammunition in a machine gun
    /// </summary>
    [Tooltip("The maximum number of possession ammunition")]
    public int[] maxAmmoClip;
    /// <summary>
    /// Speed of viewpoint movement
    /// </summary>
    public float mouseSensitivity = 1f;
    /// <summary>
    /// Speed of walk
    /// </summary>
    public float walkSpeed = 4f;
    /// <summary>
    /// Speed of run
    /// </summary>
    public float runSpeed = 8f;
    #endregion

    #region Private
    /// <summary>
    /// Camera
    /// </summary>
    private Camera cam;
    /// <summary>
    /// Rigidboty
    /// </summary>
    private Rigidbody rb;
    /// <summary>
    /// UI Manager
    /// </summary>
    private UIManager uIManager;
    /// <summary>
    /// Stores input values
    /// </summary>
    private Vector3 moveDir;
    /// <summary>
    /// storing the direction of movement
    /// </summary>
    private Vector3 movement;
    /// <summary>
    /// Stores user mouse input
    /// </summary>
    private Vector2 mouseInput;
    /// <summary>
    /// Y-axis rotational storage
    /// </summary>
    private float verticalMouseInput;
    /// <summary>
    /// Actual movement speed
    /// </summary>
    private float activeMoveSpeed = 4f;
    /// <summary>
    /// 
    /// </summary>
    private float shotTimer;
    /// <summary>
    /// Determination of cursor display
    /// </summary>
    private bool cursorLock = true;
    /// <summary>
    /// Numerical values for arms control in the selection
    /// </summary>
    private int selectedGun = 0;
    #endregion

    #endregion

    #region Methods

    #region Unity Methods

    private void Awake()
    {
        uIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
    }

    private void Start()
    {
        // Camera storage
        cam = Camera.main;
        // Get Rigidboty
        rb = GetComponent<Rigidbody>();
        // Cursor display decision function
        UpdateCursorLock();
    }

    private void Update()
    {
        // Call the viewpoint shift function
        PlayerRotate();
        // Call move function
        PlayerMove();

        if (IsGround())
        {
            // Call run function
            Run();
            // Call jump function
            Jump();
        }
        // Call the peek function
        Aim();
        // Call the fire button detection function
        Fire();
        // Reload
        Reload();
        // Weapon change key detection function
        SwitchingGuns();
    }

    private void LateUpdate()
    {
        // Adjusting the camera position
        cam.transform.position = viewPoint.position;
        // The camera rotation
        cam.transform.rotation = viewPoint.rotation;
    }

    #endregion

    #region Public Methods
    /// <summary>
    /// Run
    /// </summary>
    public void Run()
    {
        // Switching speeds when the shift button is pressed
        if (Input.GetKey(KeyCode.LeftShift)) activeMoveSpeed = runSpeed;
        else activeMoveSpeed = walkSpeed;
    }
    /// <summary>
    /// Jump
    /// </summary>
    public void Jump()
    {
        if (IsGround() && Input.GetKeyDown(KeyCode.Space)) rb.AddForce(jumpForce, ForceMode.Impulse);
    }
    /// <summary>
    /// True if the player is on ground
    /// </summary>
    /// <returns></returns>
    public bool IsGround()
    {
        // Return bool (レーザーを飛ばすポジション、方向、距離、判定するレイヤー)
        return Physics.Raycast(groundCheckPoint.position, Vector3.down, 0.25f, groundLayers);
    }
    /// <summary>
    /// Display mouse cursor or not
    /// </summary>
    public void UpdateCursorLock()
    {
        // Change bool
        if (Input.GetKeyDown(KeyCode.Escape)) cursorLock = false;   // Display cursor
        else if (Input.GetMouseButton(0)) cursorLock = true;        // Hide cursor
        if (cursorLock) Cursor.lockState = CursorLockMode.Locked;   // Fixes the cursor in the center and hides it
        else Cursor.lockState = CursorLockMode.None;                // Display cursor
    }
    /// <summary>
    /// Switching guns by scrolling mouse wheel or number keys
    /// </summary>
    public void SwitchingGuns()
    {
        // Switching guns by scrolling mouse wheel
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
        {
            selectedGun++;
            if (selectedGun >= guns.Count) selectedGun = 0;
            // Switch guns
            SwitchGun();
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        {
            selectedGun--;
            if (selectedGun < 0) selectedGun = guns.Count - 1;
            // Switch guns
            SwitchGun();
        }

        // Switching guns by number keys
        for (int i = 0; i < guns.Count; i++)
        {
            // Whether a numeric key has been pressed
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                // Switch guns
                selectedGun = i;
                SwitchGun();
            }
        }
    }
    /// <summary>
    /// Switch gun
    /// </summary>
    public void SwitchGun()
    {
        // First, hide all gun objects
        foreach (var gun in guns) gun.gameObject.SetActive(false);
        // Gun of certain elements in the list is displayed
        guns[selectedGun].gameObject.SetActive(true);
    }
    /// <summary>
    /// Aim
    /// </summary>
    public void Aim()
    {
        // Check right click
        if (Input.GetMouseButton(1))
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, guns[selectedGun].adsZoom, guns[selectedGun].adsSpeed * Time.deltaTime);
        else
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, 60f, guns[selectedGun].adsSpeed * Time.deltaTime);
    }
    /// <summary>
    /// Fire
    /// </summary>
    public void Fire()
    {
        // Check if a gun can be shot
        if (Input.GetMouseButtonDown(0) && ammoClip[selectedGun] > 0 && Time.time > shotTimer)
        {
            //  Call to fire a bullet
            FiringBullet();
        }
    }
    /// <summary>
    /// Fire a bullet
    /// </summary>
    public void FiringBullet()
    {
        // Reduces one bullet from the magazine
        ammoClip[selectedGun]--;
        
        // Create a ray
        Ray ray = cam.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Debug.Log("Objects hit is " + hit.collider.gameObject.name);
            // Generates a bullet hole where it hit
            GameObject buletImpactObject = Instantiate(guns[selectedGun].bulletImpact, hit.point + (hit.normal * 0.02f), Quaternion.LookRotation(hit.normal, Vector3.up));
            Destroy(buletImpactObject, 10f);
        }
        // Firing time interval setting
        shotTimer = Time.time + guns[selectedGun].shootInterval;

    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Player move
    /// </summary>
    private void PlayerMove()
    {
        // Detects input of keys for movement and stores values
        moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        // Give the direction of travel and store it in a variable
        movement = ((transform.forward * moveDir.z) + (transform.right * moveDir.x)).normalized;
        // Reflecting on the current position
        transform.position += movement * activeMoveSpeed * Time.deltaTime;
    }
    /// <summary>
    /// Player rotate
    /// </summary>
    private void PlayerRotate()
    {
        // Stores user mouse movements in variables
        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X") * mouseSensitivity, Input.GetAxisRaw("Mouse Y") * mouseSensitivity);
        // Reflects the x-axis movement of the mouse
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + mouseInput.x, transform.eulerAngles.z);
        // Add the current value to the value on the Y-axis
        verticalMouseInput += mouseInput.y;
        // Round off a number
        verticalMouseInput = Mathf.Clamp(verticalMouseInput, -60f, 60f);
        // Reflect rounded values in viewPoint
        viewPoint.rotation = Quaternion.Euler(-verticalMouseInput, viewPoint.transform.rotation.eulerAngles.y, viewPoint.transform.rotation.eulerAngles.z);
    }
    /// <summary>
    /// Reload
    /// </summary>
    private void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Get the number of bullets to be refilled by reloading
            int amountNeed = maxAmmoClip[selectedGun] - ammoClip[selectedGun];
            // Compare the number of ammunition to replenish with the number of ammunition the player have
            // and store how much player can replenish in a variable
            int ammoAvailable = amountNeed < ammunition[selectedGun] ? amountNeed : ammunition[selectedGun];
            // Whether reloading is required
            // Cannot reload when ammo is full & in possession of ammo
            if (amountNeed != 0 && ammunition[selectedGun] != 0)
            {
                // Subtract ammunition to be reloaded from the ammunition in player's possession
                ammunition[selectedGun] -= ammoAvailable;
                // Set ammunition in the gun
                ammoClip[selectedGun] += ammoAvailable;
            }
        }
    }
    #endregion

    #endregion
}
