using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleUnityChanTrigger : MonoBehaviour
{
    Color oldColor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        oldColor = collider.GetComponent<Renderer>().material.color;
        collider.GetComponent<Renderer>().material.color = Color.magenta;
    }

    private void OnTriggerExit(Collider collider)
    {
        collider.GetComponent<Renderer>().material.color = oldColor;
    }
}
