using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    private Vector3 startPos;
    private float repeartWidth;
    void Start()
    {
        startPos = transform.localPosition;
        repeartWidth = GetComponent<BoxCollider>().size.x / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < startPos.x - repeartWidth) // 57
        {
            transform.position = startPos;
        }
    }
}
