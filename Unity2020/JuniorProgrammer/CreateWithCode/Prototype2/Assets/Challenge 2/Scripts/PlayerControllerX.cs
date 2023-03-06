using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public GameObject dogPrefab;
    private GameObject dog;
    // Update is called once per frame
    void Update()
    {
        // On spacebar press, send dog
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (dog != null) return;
            dog = Instantiate(dogPrefab, transform.position, dogPrefab.transform.rotation);
        }
    }
}
