using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppPlayerController : MonoBehaviour
{
    #region Variables
    #region SerializeField
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
    #endregion

    /// <summary>
    /// Rigidbody
    /// </summary>
    private Rigidbody rigid = null;

    #endregion

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        
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

    private void MoveResistance()
    {

    }

    private void StopForce()
    {

    }
}
