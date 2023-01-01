using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class AppPlayerController : MonoBehaviour
{
    #region Variables
    #region SerializeField
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
    /// Jumping flag
    /// </summary>
    private bool isJumping = false;
    #endregion

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        GetMouseButton();
        ProcessJump();
        // Measuring the shooting position
        UpdateShootRay();
    }

    private void FixedUpdate()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

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
    /// <summary>
    /// Trigger Enter Callback for Grounding Collider
    /// </summary>
    /// <param name="collider"></param>
    public void OnGroundTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag.Equals("Ground"))
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
        if (collider.gameObject.tag.Equals("Ground"))
        {
            if (isJumping) StartCoroutine(WaitSwitch(0.5f));
            Debug.Log("Ground enter");
        }
    }
    /// <summary>
    /// Mouse button action
    /// </summary>
    private void GetMouseButton()
    {
        // Start to click
        if (Input.GetMouseButtonDown(0))
        {
            // Store mouse position and camera angle
            startMousePosition = Input.mousePosition;
            startCameraRotation = Camera.main.gameObject.transform.localRotation.eulerAngles;
            // currentArrow = Instantiate(arrowPrefab, arrowPoint.position, arrowPoint.rotation, arrowPoint);
            var currentArrowGo = Instantiate(arrowPrefab, arrowPoint.position, arrowPoint.rotation, arrowPoint);
            currentArrow = currentArrowGo.GetComponent<Arrow>();
            currentArrow.OnCreated();
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
        if (shootPoint != null)
        {
            var dir = ((Vector3)shootPoint - currentArrow.transform.position).normalized;
            currentArrow.Shoot(dir * arrowPower * 0.1f, ForceMode.Impulse);
        }
        else
        {
            currentArrow.Shoot(Camera.main.gameObject.transform.forward * arrowPower * 0.1f, ForceMode.Impulse);
        }
        currentArrow.transform.parent = null;
        currentArrow = null;
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
                if (h.collider.gameObject.tag.Equals("Arrow")) continue;
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
}
