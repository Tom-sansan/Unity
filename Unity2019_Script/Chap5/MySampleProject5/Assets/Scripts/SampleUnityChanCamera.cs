using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleUnityChanCamera : MonoBehaviour
{
    public GameObject target = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = target.transform.position;
        position.y += 1f;
        position.z -= 2f;
        transform.position = position;
    }
}
