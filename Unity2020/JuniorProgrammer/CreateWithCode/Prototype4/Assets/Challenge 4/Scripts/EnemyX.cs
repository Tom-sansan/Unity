using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyX : MonoBehaviour
{
    private SpawnManagerX spawnManamgerX;
    private Rigidbody enemyRb;
    private GameObject playerGoal;


    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        playerGoal = GameObject.Find("Player");
        spawnManamgerX = new SpawnManagerX();
    }

    // Update is called once per frame
    void Update()
    {
        // spawnManamgerX = new SpawnManagerX();
        // Set enemy direction towards player goal and move there
        Vector3 lookDirection = (playerGoal.transform.position - transform.position).normalized;
        enemyRb.AddForce(lookDirection * spawnManamgerX.enemySpeed * Time.deltaTime);
        Debug.Log(spawnManamgerX.enemySpeed);
    }

    private void OnCollisionEnter(Collision other)
    {
        // If enemy collides with either goal, destroy it
        if (other.gameObject.name == "Enemy Goal")
        {
            Destroy(gameObject);
        } 
        else if (other.gameObject.name == "Player Goal")
        {
            Destroy(gameObject);
        }

    }

}
