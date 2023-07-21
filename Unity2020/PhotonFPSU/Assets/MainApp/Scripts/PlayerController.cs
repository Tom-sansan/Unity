using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player Controller Class
/// </summary>
public class PlayerController : MonoBehaviourPunCallbacks
{
    #region Variables

    #region Public Variables
    /// <summary>
    /// Animator
    /// </summary>
    public Animator animator;
    /// <summary>
    /// Blood Effect
    /// </summary>
    public GameObject hitEffect;
    /// <summary>
    /// Player Model
    /// </summary>
    public GameObject[] playerModel;
    /// <summary>
    /// Gunholder for player
    /// </summary>
    public Gun[] gunsHolder;
    /// <summary>
    /// Gunholder for other players
    /// </summary>
    public Gun[] otherGunsHolder;
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
    /// The number of bullets in machine gun
    /// </summary>
    [Tooltip("The number of bullets in machine gun")]
    public int[] ammoClip;
    /// <summary>
    /// The maximum number of ammunition in a machine gun
    /// </summary>
    [Tooltip("The maximum number of ammunition in a machine gun")]
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
    /// <summary>
    /// Maximum HP
    /// </summary>
    public int maxHP = 100;
    #endregion Public Variables

    #region Private Variables
    /// <summary>
    /// Game Manager
    /// </summary>
    private GameManager gameManager;
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
    /// SpawnManager格納
    /// </summary>
    private SpawnManager spawnManager;
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
    /// Interval time of firing
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
    /// <summary>
    /// The current HP
    /// </summary>
    private int currentHP;
    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    private void Awake()
    {
        // Set uIManager
        uIManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        // Set SpawnManager
        spawnManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>();
        // Set GameManager
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void Start()
    {
        // Set MAX HP to the current HP
        currentHP = maxHP;
        // Camera storage
        cam = Camera.main;
        // Get Rigidboty
        rb = GetComponent<Rigidbody>();
        // ランダムな位置でスポーンさせる関数呼び出し
        // transform.position = spawnManager.GetSpawnPoint().position;
        // Initialize the gun list
        guns.Clear();
        SwitchDisplayModelsGuns();
        // Cursor display decision function
        UpdateCursorLock();
    }

    private void Update()
    {
        // Return if this object is other players(Otherwise, this PlayerController is by itself)
        if (!photonView.IsMine) return;

        // Call the viewpoint shift function
        RotatePlayer();
        // Call move function
        MovePlayer();

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
        // Animation transition
        SetAnimator();
        // Stop gun sound
        StopGunSound();
        // Back to menu
        BackToMenu();
        // Cursor display decision function
        UpdateCursorLock();
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        // Update magasin text
        uIManager.SetBulletsText(ammoClip[selectedGun], ammunition[selectedGun]);
    }

    private void LateUpdate()
    {
        if (!photonView.IsMine) return;
        // Adjust camera view
        AdjustCameraView();
    }
    /// <summary>
    /// Called when the player component is turned off
    /// </summary>
    public override void OnDisable()
    {
        DisplayMouse();
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Remote callable gun switching method
    /// </summary>
    [PunRPC]
    public void SetGun(int gunNo)
    {
        if (gunNo < guns.Count)
        {
            selectedGun = gunNo;
            SwitchGun();
        }
    }
    /// <summary>
    /// Being shot (Common for all players)
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="name"></param>
    /// <param name="actor"></param>
    [PunRPC]
    public void Hit(int damage, string name, int actor)
    {
        Debug.Log("Hit is called.");
        // Decrease HP
        ReceiveDamage(damage, name, actor);
    }
    /// <summary>
    /// Generate sound
    /// </summary>
    [PunRPC]
    public void GenerateSound()
    {
        switch (selectedGun)
        {
            case 2:
                guns[selectedGun].LoopOnSubmachineGun();
                break;
            default:
                guns[selectedGun].SoundGunShot();
                break;
        }
    }
    /// <summary>
    /// Stop sound
    /// </summary>
    [PunRPC]
    public void StopSound() =>
        guns[2].LoopOffSubmachineGun();
    #endregion

    #region Private Methods
    /// <summary>
    /// Adjust camera view
    /// </summary>
    private void AdjustCameraView()
    {
        // Adjusting the camera position
        cam.transform.position = viewPoint.position;
        // The camera rotation
        cam.transform.rotation = viewPoint.rotation;
    }
    /// <summary>
    /// Move player
    /// </summary>
    private void MovePlayer()
    {
        // Detects input of keys for movement and stores values
        moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        // Give the direction of travel and store it in a variable
        movement = ((transform.forward * moveDir.z) + (transform.right * moveDir.x)).normalized;
        // Reflecting on the current position
        transform.position += movement * activeMoveSpeed * Time.deltaTime;
    }
    /// <summary>
    /// Rotate player
    /// </summary>
    private void RotatePlayer()
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
    /// Run
    /// </summary>
    private void Run()
    {
        // Switching speeds when the shift button is pressed
        if (Input.GetKey(KeyCode.LeftShift)) activeMoveSpeed = runSpeed;
        else activeMoveSpeed = walkSpeed;
    }
    /// <summary>
    /// Jump
    /// </summary>
    private void Jump()
    {
        if (IsGround() && Input.GetKeyDown(KeyCode.Space)) rb.AddForce(jumpForce, ForceMode.Impulse);
    }
    /// <summary>
    /// True if the player is on ground
    /// </summary>
    /// <returns></returns>
    private bool IsGround()
    {
        // Return bool (レーザーを飛ばすポジション、方向、距離、判定するレイヤー)
        return Physics.Raycast(groundCheckPoint.position, Vector3.down, 0.25f, groundLayers);
    }
    /// <summary>
    /// Switching guns by scrolling mouse wheel or number keys
    /// </summary>
    private void SwitchingGuns()
    {
        // Switching guns by scrolling mouse wheel
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
        {
            selectedGun++;
            if (selectedGun >= guns.Count) selectedGun = 0; // Set the gun to the first one
            // Switch guns
            // SwitchGun();
            // Switching guns that can be shared by all players
            photonView.RPC("SetGun", RpcTarget.All, selectedGun);
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        {
            selectedGun--;
            if (selectedGun < 0) selectedGun = guns.Count - 1;  // Set the gun to the last one
            // Switch guns
            // SwitchGun();
            // Switching guns that can be shared by all players
            photonView.RPC("SetGun", RpcTarget.All, selectedGun);
        }

        // Switching guns by number keys
        for (int i = 0; i < guns.Count; i++)
        {
            // Whether a numeric key has been pressed
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                // Switch guns
                selectedGun = i;
                // SwitchGun();
                // Switching guns that can be shared by all players
                photonView.RPC("SetGun", RpcTarget.All, selectedGun);
            }
            Debug.Log("Current number: " + i);
        }
    }
    /// <summary>
    /// Switch gun
    /// </summary>
    private void SwitchGun()
    {
        // First, hide all gun objects
        foreach (var gun in guns) gun.gameObject.SetActive(false);
        // Gun of certain elements in the list is displayed
        guns[selectedGun].gameObject.SetActive(true);
    }
    /// <summary>
    /// Aim
    /// </summary>
    private void Aim()
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
    private void Fire()
    {
        // Check if a gun can be shot
        if (Input.GetMouseButtonDown(0) && ammoClip[selectedGun] > 0 && Time.time > shotTimer)
            // Call to fire a bullet
            FiringBullet();
    }
    /// <summary>
    /// Fire a bullet
    /// </summary>
    private void FiringBullet()
    {
        // Reduces one bullet from the magazine
        ammoClip[selectedGun]--;

        // Create a ray
        Ray ray = cam.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Debug.Log("Objects hit is " + hit.collider.gameObject.name);
            if (hit.collider.gameObject.tag.Equals("Player"))
            {
                // If the player is hit
                // Blood effect
                PhotonNetwork.Instantiate(hitEffect.name, hit.point, Quaternion.identity);
                hit.collider.gameObject.GetPhotonView().RPC
                (
                    "Hit",                                  // Target method. The followings are parameters of Hit method.
                    RpcTarget.All,                          // Target is all(Destination target)
                    guns[selectedGun].shotDamage,           // Damage value of the gun currently equipped
                    photonView.Owner.NickName,              // Player name who shot
                    PhotonNetwork.LocalPlayer.ActorNumber   // The number to manage player
                );
            }
            else
            {
                // If object other than player is hit
                // Generates a bullet hole where it hit
                GameObject buletImpactObject = Instantiate(guns[selectedGun].bulletImpact, hit.point + (hit.normal * 0.02f), Quaternion.LookRotation(hit.normal, Vector3.up));
                Destroy(buletImpactObject, 10f);
            }
        }
        // Firing time interval setting
        shotTimer = Time.time + guns[selectedGun].shootInterval;
        // Sound
        photonView.RPC(nameof(GenerateSound), RpcTarget.All);
    }
    /// <summary>
    /// Reload a gun
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
    /// <summary>
    /// Animator transition
    /// </summary>
    private void SetAnimator()
    {
        // Check walk
        // moveDir != Vector3.zero: true; walk, false; not walk
        animator.SetBool("walk", moveDir != Vector3.zero);
        // Check Run
        // Input.GetKey(KeyCode.LeftShift): true; run, false; not run
        animator.SetBool("run", Input.GetKey(KeyCode.LeftShift));
    }
    /// <summary>
    /// Switch display of models and guns
    /// </summary>
    private void SwitchDisplayModelsGuns()
    {
        if (photonView.IsMine)
        {
            // For player
            // Hide 3D Model of mine
            foreach (var model in playerModel) model.SetActive(false);
            // Set a gun for display
            foreach (var gun in gunsHolder) guns.Add(gun);
            // Reflect HP in the slider
            UpdateHP();
        }
        // For other players
        else foreach (var gun in otherGunsHolder) guns.Add(gun);
        // Display gun
        // SwitchGun();
        // Switching guns that can be shared by all players
        photonView.RPC("SetGun", RpcTarget.All, selectedGun);
    }
    /// <summary>
    /// Decrease HP
    /// </summary>
    private void ReceiveDamage(int damage, string name, int actor)
    {
        Debug.Log("ReceiveDamage is called.");
        if (!photonView.IsMine) return;
        // Decrease HP
        currentHP -= damage;
        Debug.Log("currentHP is " + currentHP);
        // Check if the current HP is less than or equal 0
        // Death
        if (currentHP <= 0) Death(name, actor);
        // Reflect HP in the slider
        UpdateHP();
    }
    /// <summary>
    /// Death
    /// </summary>
    private void Death(string playerName, int actor)
    {
        currentHP = 0;
        // Death process
        uIManager.UpdateDeathUI(playerName);
        spawnManager.Die();
        // Call KillDeathEvent
        // Calling of events upon one's own death
        gameManager.GetScore(PhotonNetwork.LocalPlayer.ActorNumber, 1, 1);
        // Kill event. actor means a user's number that the user beat you
        gameManager.GetScore(actor, 0, 1);
    }
    /// <summary>
    /// Update HP
    /// </summary>
    private void UpdateHP() =>
        uIManager.UpdateHP(maxHP, currentHP);
    /// <summary>
    /// Display mouse
    /// </summary>
    private void DisplayMouse()
    {
        cursorLock = false;
        Cursor.lockState = CursorLockMode.None;
    }
    /// <summary>
    /// Stop gun sound
    /// </summary>
    private void StopGunSound()
    {
        if (Input.GetMouseButtonUp(0) || ammoClip[2] <= 0)
            photonView.RPC(nameof(StopSound), RpcTarget.All);
    }
    /// <summary>
    /// Return to the menu when a specific button is pressed
    /// </summary>
    private void BackToMenu()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            // Delete player from player list
            gameManager.RemovePlayerInfo(PhotonNetwork.LocalPlayer.ActorNumber);
            // Set scene synchronization
            PhotonNetwork.AutomaticallySyncScene = false;
            // Leave room
            PhotonNetwork.LeaveRoom();
        }
    }
    /// <summary>
    /// Display mouse cursor or not
    /// </summary>
    private void UpdateCursorLock()
    {
        // Change bool
        if (Input.GetKeyDown(KeyCode.Escape)) cursorLock = false;   // Display cursor
        else if (Input.GetMouseButton(0)) cursorLock = true;        // Hide cursor
        if (cursorLock) Cursor.lockState = CursorLockMode.Locked;   // Fixes the cursor in the center and hides it
        else Cursor.lockState = CursorLockMode.None;                // Display cursor
    }
    #endregion

    #endregion
}
