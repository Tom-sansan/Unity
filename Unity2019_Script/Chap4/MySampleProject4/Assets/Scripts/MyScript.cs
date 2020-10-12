using System;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Attached to Sphere
/// </summary>
public class MyScript : MonoBehaviour
{
    string cubeStr = "Cube";
    string playerStr = "Player";
    int counter = 0;
    GameObject obj = null;

    // Start is called before the first frame update
    void Start()
    {
        Start4_5();
    }

    // Update is called once per frame
    void Update()
    {
        // Method4_1();
        // Method4_3();
        Method4_4_5();
    }

    /// <summary>
    /// Event of collision enter
    /// </summary>
    /// <param name="collision">The parameter of collision is the one of cube object.</param>
    private void OnCollisionEnter(Collision collision)
    {
        // OnCollisionEnter4_1(collision);
        // OnCollisionEnter4_2(collision);
        // OnCollisionEnter4_3(collision);
        // OnCollisionEnter4_4(collision);
        OnCollisionEnter4_5(collision);
    }

    /// <summary>
    /// Event of collision exit
    /// </summary>
    /// <param name="collision">The parameter of collision is the one of cube object.</param>
    private void OnCollisionExit(Collision collision)
    {
        // OnCollisionExit4_4(collision);
        OnCollisionExit4_5(collision);
    }

    /// <summary>
    /// Collistion Event
    /// </summary>
    private void Method4_1()
    {
        GameObject cube = GameObject.Find(cubeStr);
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        SetTransformRotate(cube, 1f, -1f, -1f);
        MoveByKeys(rigidbody);
    }

    private void OnCollisionEnter4_1(Collision collision)
    {
        if (collision.gameObject.name != "Plane") collision.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
    }

    private void OnCollisionEnter4_2(Collision collision)
    {
        if (collision.gameObject.tag == "Player") collision.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
    }

    /// <summary>
    /// Delete Object
    /// </summary>
    private void Method4_3()
    {
        GameObject cube = GameObject.Find(cubeStr);
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        if (obj != null)
        {
            if (counter > 300)
            {
                obj.SetActive(true);
                obj = null;
            }
            counter++;
        }

        try
        {
            SetTransformRotate(cube, 1f, -1f, -1f);
        }
        catch (NullReferenceException e) { }

        MoveByKeys(rigidbody);
    }

    private void OnCollisionEnter4_3(Collision collision)
    {
        if (collision.gameObject.tag == playerStr)
        {
            collision.gameObject.SetActive(false);
            obj = collision.gameObject;
            counter = 0;
        }
    }

    /// <summary>
    /// Make cube transparent
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter4_4(Collision collision)
    {
        if (collision.gameObject.name == cubeStr) ChangeMaterialColor(collision, 1f, 0, 0, 0.25f);
    }

    /// <summary>
    /// Make cube normal color
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit4_4(Collision collision)
    {
        if (collision.gameObject.name == cubeStr) ChangeMaterialColor(collision, 1f, 0, 0, 1f);
    }

    /// <summary>
    /// Start method to set up rendering mode to opaque
    /// </summary>
    private void Start4_5()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(playerStr);
        foreach (var obj in objs)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            renderer.material.SetFloat("_Mode", 3f);
            renderer.material.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
            renderer.material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
            renderer.material.SetInt("_ZWrite", 0);
            renderer.material.DisableKeyword("_ALPHATEST_ON");
            renderer.material.EnableKeyword("_ALPHABLEND_ON");
            renderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            renderer.material.renderQueue = 3000;
        }
    }

    /// <summary>
    /// Make cube transparent
    /// </summary>
    private void Method4_4_5()
    {
        var gameObj = GetGameObjectAndRigidbody(cubeStr);
        // var gameObj = (cube: GameObject.Find(cubeStr), rigidbody: GetComponent<Rigidbody>());
        //GameObject cube = GameObject.Find(cubeStr);
        //Rigidbody rigidbody = GetComponent<Rigidbody>();
        try
        {
            SetTransformRotate(gameObj.cube, 1f, -1f, -1f);
        }
        catch (NullReferenceException e) { }

        MoveByKeys(gameObj.rigidbody);
    }

    /// <summary>
    /// Make material color opaque
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter4_5(Collision collision)
    {
        if (collision.gameObject.CompareTag(playerStr))
        {
            Renderer renderer = collision.gameObject.GetComponent<Renderer>();
            Color color = renderer.material.color;
            color.a = 0.25f;
            renderer.material.color = color;
        }
    }

    /// <summary>
    /// Make material color nomal
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit4_5(Collision collision)
    {
        if (collision.gameObject.CompareTag(playerStr))
        {
            Renderer renderer = collision.gameObject.GetComponent<Renderer>();
            Color color = renderer.material.color;
            color.a = 1.0f;
            renderer.material.color = color;
        }
    }

    /// <summary>
    /// Tuple of Initialization for GameObject and Rigidbody
    /// </summary>
    /// <param name="gameObjStr"></param>
    /// <returns></returns>
    private (GameObject cube, Rigidbody rigidbody) GetGameObjectAndRigidbody(String gameObjStr)
    {
        return (GameObject.Find(gameObjStr), GetComponent<Rigidbody>());
    }

    /// <summary>
    /// Move by keys on keyboard
    /// </summary>
    /// <param name="rigidbody"></param>
    private void MoveByKeys(Rigidbody rigidbody)
    {
        if (Input.GetKey(KeyCode.LeftArrow)) SetRigidbodyAddForm(rigidbody, -1f, 0, 1f);
        if (Input.GetKey(KeyCode.RightArrow)) SetRigidbodyAddForm(rigidbody, 1f, 0, -1f);
        if (Input.GetKey(KeyCode.UpArrow)) SetRigidbodyAddForm(rigidbody, 1f, 0, 1f);
        if (Input.GetKey(KeyCode.DownArrow)) SetRigidbodyAddForm(rigidbody, -1f, 0, -1f);
    }

    /// <summary>
    /// Set Transform.Rotate
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    private void SetTransformRotate(GameObject obj, float x, float y, float z)
    {
        obj.transform.Rotate(x, y, z);
    }

    /// <summary>
    /// Set Rigidbody.AddForce
    /// </summary>
    /// <param name="rigidbody"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    private void SetRigidbodyAddForm(Rigidbody rigidbody, float x, float y, float z)
    {
        rigidbody.AddForce(new Vector3(x, y, z));
    }

    /// <summary>
    /// Change Material Color
    /// </summary>
    /// <param name="collision"></param>
    /// <param name="r"></param>
    /// <param name="g"></param>
    /// <param name="b"></param>
    /// <param name="a">Alpha channel; Transparent value. 0 is complete transparent. 1 is complete opaque.</param>
    private void ChangeMaterialColor(Collision collision, float r, float g, float b, float a)
    {
        collision.gameObject.GetComponent<Renderer>().material.color = new Color(r, g, b, a);
    }
}
