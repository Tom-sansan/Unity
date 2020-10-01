using UnityEngine;

public class RigidScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Method2_9();
        Method2_10();
    }

    /// <summary>
    /// Move by arrows on kayboard
    /// </summary>
    private void Method2_9()
    {
        if (Input.GetKey(KeyCode.UpArrow)) GetComponent<Rigidbody>().AddForce(0, 0, 1);
        if (Input.GetKey(KeyCode.DownArrow)) GetComponent<Rigidbody>().AddForce(0, 0, -1);
        if (Input.GetKey(KeyCode.RightArrow)) GetComponent<Rigidbody>().AddForce(1, 0, 0);
        if (Input.GetKey(KeyCode.LeftArrow)) GetComponent<Rigidbody>().AddForce(-1, 0, 0);
    }

    /// <summary>
    /// Movement of camera with cube
    /// </summary>
    private void Method2_10()
    {
        Vector3 pos = transform.position;
        pos.y += 2.5f;
        pos.z -= 3f;
        var camera = GameObject.Find("Main Camera");
        camera.transform.position = pos;
    }
}
