using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _DetectCollisions : MonoBehaviour
{
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        Destroy(other.gameObject);
        Debug.Log("Destroyed : " + gameObject.name);
    }
}
