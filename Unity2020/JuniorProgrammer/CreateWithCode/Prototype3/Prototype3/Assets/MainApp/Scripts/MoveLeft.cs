using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    private PlayerController playerController;
    private float speed = 30f;
    private float leftBound = -15f;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerController.gameOver) transform.Translate(Vector3.left * Time.deltaTime * speed);
        if (transform.position.x < leftBound && gameObject.CompareTag("Obstacle")) Destroy(gameObject);
    }
}
