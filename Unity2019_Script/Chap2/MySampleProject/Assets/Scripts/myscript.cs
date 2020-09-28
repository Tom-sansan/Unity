using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myscript : MonoBehaviour
{
    int counter = 0;
    float plus = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 p = new Vector3(0, 0, plus);
        transform.Translate(p);
        counter++;
        if (counter == 100)
        {
            counter = 0;
            plus *= -1;
        }

    }
}
