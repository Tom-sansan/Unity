using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject centerOfMass;
    [SerializeField] private TextMeshProUGUI speedometerText;
    [SerializeField] private TextMeshProUGUI rpmText;
    [SerializeField] private List<WheelCollider> allWheels;
    [SerializeField] private int wheelsOnGround;
    [SerializeField] private float speed = 20f;
    [SerializeField] private float rpm = 20f;
    [SerializeField] private float horsePower = 0;
    [SerializeField] private float turnSpeed = 45f;
    private Rigidbody playerRb;
    private float horizontalInput;
    private float verticalInput;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerRb.centerOfMass = centerOfMass.transform.position;
    }

    private void Update()
    {

    }

    void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        // Move the car forward based on vertical input
        // transform.Translate(Vector3.forward * speed * forwardInput * Time.deltaTime);
        if (!IsOnGround()) return;
        playerRb.AddRelativeForce(Vector3.forward * horsePower * verticalInput);
        Debug.Log(Vector3.forward * horsePower * verticalInput);
        // Rotates the car based on horizontal input
        transform.Rotate(Vector3.up, turnSpeed * horizontalInput * Time.deltaTime);

        SetSpeed();
        SetRpm();
    }

    private void SetSpeed()
    {
        speed = Mathf.Round(playerRb.velocity.magnitude * 2.237f);  // For kph, change 2.237 to 3.6
        speedometerText.SetText("Speed: " + speed + "mph");
    }

    private void SetRpm()
    {
        rpm = Mathf.Round((speed % 30) * 40);
        rpmText.SetText("RPM: " + rpm);
    }

    private bool IsOnGround()
    {
        wheelsOnGround = 0;
        foreach (var wheel in allWheels) if (wheel.isGrounded) wheelsOnGround++;
        Debug.Log("Wheels: " + wheelsOnGround);
        return wheelsOnGround >= 2;
    }
}
