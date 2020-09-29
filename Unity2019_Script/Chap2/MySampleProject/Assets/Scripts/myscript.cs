using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myscript : MonoBehaviour
{
    // List2-1
    int counter = 0;
    float plus = 0.1f;
    // List2-2
    float zvalue = 0f;
    // List2-4
    float dx = 0.01f;
    float dy = -0.01f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        #region List2-1
        //Vector3 p = new Vector3(0, 0, plus);
        //transform.Translate(p);
        //counter++;
        //if (counter == 100)
        //{
        //    counter = 0;
        //    plus *= -1;
        //}
        #endregion List2-1

        #region List2-2
        //zvalue += 0.1f;
        //Vector3 p = new Vector3(0, 0, zvalue);
        //transform.position = p;
        //if (zvalue > 10) zvalue = 0;
        #endregion List2-2

        #region List2-3
        //var v = new Vector3(0.1f, 0.2f, 0.3f);
        //transform.Rotate(v);
        #endregion

        #region List2-4
        //Vector3 s = transform.localScale;
        //if (s.x > 3 || s.x < 0.1) dx *= -1;
        //if (s.y > 3 || s.y < 0.1) dy *= -1;
        //s.x += dx;
        //s.y += dy;
        //transform.localScale = s;
        #endregion List2-4

        #region List2-5
        if (Input.GetKey(KeyCode.UpArrow)) transform.Translate(transform.forward * 0.1f);
        if (Input.GetKey(KeyCode.DownArrow)) transform.Translate(transform.forward * -0.1f);
        if (Input.GetKey(KeyCode.RightArrow)) transform.Translate(transform.right * 0.1f);
        if (Input.GetKey(KeyCode.LeftArrow)) transform.Translate(transform.right * -0.1f);
        #endregion List2-5
    }
}
