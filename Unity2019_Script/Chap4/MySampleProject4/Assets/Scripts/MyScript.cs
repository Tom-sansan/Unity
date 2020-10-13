using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Rendering;

/// <summary>
/// Attached to Sphere
/// </summary>
public class MyScript : MonoBehaviour
{
    string cubeStr = "Cube";
    string playerStr = "Player";
    string gameObjectStr = "GameObject";
    int counter = 0;
    GameObject obj = null;
    GameObject[] cubes = new GameObject[4];
    GameObject[] gos = new GameObject[4];

    // Start is called before the first frame update
    void Start()
    {
        // Start4_5();
        // Start4_6();
        Start4_8();
    }

    // Update is called once per frame
    void Update()
    {
        // Update4_1();
        // Update4_3();
        // Update4_4_5_6_7();
        Update4_8();
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
        // OnCollisionEnter4_5(collision);
    }

    /// <summary>
    /// Event of collision exit
    /// </summary>
    /// <param name="collision">The parameter of collision is the one of cube object.</param>
    private void OnCollisionExit(Collision collision)
    {
        // OnCollisionExit4_4(collision);
        // OnCollisionExit4_5(collision);
    }

    /// <summary>
    /// Event of trigger enter
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerEnter(Collider collider)
    {
        // OnTriggerEnter4_6(collider);
        // OnTriggerEnter4_7(collider);
        OnTriggerEnter4_8(collider);
    }

    /// <summary>
    /// Event of trigger exit
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerExit(Collider collider)
    {
        // OnTriggerExit4_6(collider);
        // OnTriggerExit4_7(collider);
    }

    /// <summary>
    /// Collistion Event
    /// </summary>
    private void Update4_1()
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
    private void Update4_3()
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
        if (collision.gameObject.name == cubeStr) ChangeMaterialColor(collision.gameObject, 1f, 0, 0, 0.25f);
    }

    /// <summary>
    /// Make cube normal color
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit4_4(Collision collision)
    {
        if (collision.gameObject.name == cubeStr) ChangeMaterialColor(collision.gameObject, 1f, 0, 0, 1f);
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
    private void Update4_4_5_6_7()
    {
        var obj = GetGameObjectAndRigidbody(cubeStr);
        // var gameObj = (cube: GameObject.Find(cubeStr), rigidbody: GetComponent<Rigidbody>());
        //GameObject cube = GameObject.Find(cubeStr);
        //Rigidbody rigidbody = GetComponent<Rigidbody>();
        try
        {
            SetTransformRotate(obj.gameObject, 1f, -1f, -1f);
        }
        catch (NullReferenceException e) { }

        MoveByKeys(obj.rigidbody);
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
    /// Start method for Trigger Event
    /// </summary>
    private void Start4_6()
    {
        GameObject cube = GameObject.Find(cubeStr);
        cube.GetComponent<Renderer>().material.color = new Color(1f, 0, 0, 0.5f);
    }

    /// <summary>
    /// Make cube green color
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerEnter4_6(Collider collider)
    {
        if (collider.gameObject.CompareTag(playerStr)) ChangeMaterialColor(collider.gameObject, 0, 1, 0, 0.5f);
    }

    /// <summary>
    /// Make cube normal color
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerExit4_6(Collider collider)
    {
        if (collider.gameObject.CompareTag(playerStr)) ChangeMaterialColor(collider.gameObject, 1, 0, 0, 0.5f);
    }

    /// <summary>
    /// Event of trigger enter for Velocity / AngularVelocity
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerEnter4_7(Collider collider)
    {
        if (collider.gameObject.CompareTag(playerStr))
        {
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            rigidbody.angularVelocity = Vector3.zero;
            rigidbody.velocity = Vector3.zero;
        }
    }

    /// <summary>
    /// Event of trigger exit for Velocity / AngularVelocity
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerExit4_7(Collider collider)
    {
        if (collider.gameObject.CompareTag(playerStr))
        {
            Rigidbody rigidbody = GetComponent<Rigidbody>();
            Vector3 v = rigidbody.velocity;
            v.x *= 25;
            v.y *= 25;
            v.z *= 25;
            rigidbody.velocity = v;

            Vector3 v2 = rigidbody.angularVelocity;
            v2.x *= 25;
            v2.y *= 25;
            v2.z *= 25;
            rigidbody.angularVelocity = v2;
        }
    }

    /// <summary>
    /// Start method for ParticleSystem
    /// </summary>
    private void Start4_8()
    {
        for (int i = 0; i < 4; i++)
        {
            cubes[i] = GameObject.Find(cubeStr + i);
            gos[i] = GameObject.Find(gameObjectStr + i);
        }
    }

    /// <summary>
    /// Update method for ParticleSystem
    /// </summary>
    private void Update4_8()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        foreach (var obj in cubes)
        {
            SetTransformRotate(obj, 1f, 1f, 1f);
        }
        
        Vector3 v = transform.position;
        v.y += 2;
        v.z -= 7;
        Camera.main.transform.position = v;
        MoveByKeysForParticlelSystem(rigidbody);
    }

    /// <summary>
    /// Event of trigger enter for ParticleSystem
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerEnter4_8(Collider collider)
    {
        if (collider.gameObject.name.StartsWith(cubeStr))
        {
            for (int i = 0; i < 4; i++)
            {
                if (cubes[i] == collider.gameObject)
                {
                    ParticleSystem ps = gos[i].GetComponent<ParticleSystem>();
                    ps.Play();
                    cubes[i].SetActive(false);
                }
            }
        }
    }

    /// <summary>
    /// Tuple of Initialization for GameObject and Rigidbody
    /// </summary>
    /// <param name="gameObjStr"></param>
    /// <returns></returns>
    private (GameObject gameObject, Rigidbody rigidbody) GetGameObjectAndRigidbody(String gameObjStr)
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
    /// Move by keys on keyboard for ParticleSystem
    /// </summary>
    /// <param name="rigidbody"></param>
    private void MoveByKeysForParticlelSystem(Rigidbody rigidbody)
    {
        if (Input.GetKey(KeyCode.LeftArrow)) SetRigidbodyAddForm(rigidbody, -1f, 0, 0f);
        if (Input.GetKey(KeyCode.RightArrow)) SetRigidbodyAddForm(rigidbody, 1f, 0, 0f);
        if (Input.GetKey(KeyCode.UpArrow)) SetRigidbodyAddForm(rigidbody, 0, 0, 1f);
        if (Input.GetKey(KeyCode.DownArrow)) SetRigidbodyAddForm(rigidbody, 0f, 0, -1f);
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
    private void ChangeMaterialColor(GameObject gameObject, float r, float g, float b, float a)
    {
        gameObject.GetComponent<Renderer>().material.color = new Color(r, g, b, a);
    }
}
