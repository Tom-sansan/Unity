using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float horizontalInput;
    public float speed = 10.0f;
    public float xRange = 10f;
    private int horizontalDirection = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        KeyInput();
        LimitPlayerPosition();
    }
    /// <summary>
    /// Key Input functions
    /// </summary>
    private void KeyInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontalInput * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Launch a projectile from the player
            Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation);
        }
    }
    private void LimitPlayerPosition()
    {
        // Keep the player in bounds
        if (transform.position.x < -xRange) LimitPlayerHorizontalPosition(-1);
        else if (transform.position.x > xRange) LimitPlayerHorizontalPosition(1);
    }

    private void LimitPlayerHorizontalPosition(int direction) =>
        transform.position = new Vector3(xRange * direction, transform.position.y, transform.position.z);
}
