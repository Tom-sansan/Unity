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
    // List2-8
    // default size of cube
    Vector3 stdSize;
    // small size of cube
    Vector3 smlSize;
    RaycastHit hit;
    // counter called by update during small size
    int counter2_8 = 0;
    // flag if cube is small size or not
    bool isSmallSize = false;

    // Start is called before the first frame update
    void Start()
    {
        Initialize2_8();
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
        //if (Input.GetKey(KeyCode.UpArrow)) transform.Translate(transform.forward * 0.1f);
        //if (Input.GetKey(KeyCode.DownArrow)) transform.Translate(transform.forward * -0.1f);
        //if (Input.GetKey(KeyCode.RightArrow)) transform.Translate(transform.right * 0.1f);
        //if (Input.GetKey(KeyCode.LeftArrow)) transform.Translate(transform.right * -0.1f);
        #endregion List2-5

        #region List2-6
        //if (Input.GetMouseButton(0)) transform.Rotate(new Vector3(0.1f, 0.2f, 0.3f));
        #endregion List2-6

        #region List2-7
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector3 pos = Input.mousePosition;
        //    pos.z = 3.0f;
        //    Vector3 newPos = Camera.main.ScreenToWorldPoint(pos);
        //    transform.position = newPos;
        //}
        #endregion List2-7

        #region List2-8
        Method2_8();
        #endregion
    }

    private void Initialize2_8()
    {
        stdSize = new Vector3(1f, 1f, 1f);
        smlSize = new Vector3(0.5f, 0.5f, 0.5f);
    }

    private void Method2_8()
    {
        if (isSmallSize)
        {
            if (counter2_8 <= 0)
            {
                // Return to standard size for cube
                hit.transform.localScale = stdSize;
                isSmallSize = false;
            }
            else counter2_8--;
        }
        if (Input.GetMouseButtonDown(0))
        {
            // Get mouse position
            Vector3 pos = Input.mousePosition;
            // Set z-depth to 3.0
            pos.z = 3.0f;
            // Prepare ray instance from clicked point
            Ray ray = Camera.main.ScreenPointToRay(pos);
            // Process when cube is standard size
            if (!isSmallSize)
            {
                // Make cube size small if cube is clicked
                if (Physics.Raycast(ray, out hit, 100))
                {
                    hit.transform.localScale = smlSize;
                    counter2_8 = 100;
                    isSmallSize = true;
                }
                // Set the position of cube to clicked position
                else
                {
                    Vector3 newPos = Camera.main.ScreenToWorldPoint(pos);
                    transform.position = newPos;
                }
            }
        }
    }
}
