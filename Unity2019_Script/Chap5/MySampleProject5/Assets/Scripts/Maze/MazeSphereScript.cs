using UnityEngine;

/// <summary>
/// Class for enemy 
/// </summary>
public class MazeSphereScript : MonoBehaviour
{
    MazeAppScript mazeAppScript;
    private Color oldColor;     // Initial color on Plane
    private int counter = 0;    // term of halt

    // Start is called before the first frame update
    void Start()
    {
        oldColor = GetComponent<Renderer>().material.color;
        mazeAppScript = Camera.main.GetComponent<MazeAppScript>();
    }

    /// <summary>
    /// Return color by counter value
    /// Go forward to unitychan
    /// </summary>
    void FixedUpdate()
    {
        if (mazeAppScript.IsEnd()) return;
        var renderer = GetComponent<Renderer>();
        var rigidbody = GetComponent<Rigidbody>();
        if (counter > 0)
        {
            // Halt sphere
            counter--;
            return;
        }
        else renderer.material.color = oldColor;

        // Make sphere approach unitychan
        Vector3 v1 = GameObject.Find("unitychan").transform.position;
        Vector3 v2 = transform.position;
        if (rigidbody.velocity.magnitude < mazeAppScript.mazeLevel)
        {
            Vector3 vd = v1 - v2;
            vd /= vd.magnitude;
            rigidbody.AddForce(vd);
        }
    }

    /// <summary>
    /// Event of Collision Enter
    /// Make unitychan power down and then halt for a while
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (mazeAppScript.IsEnd()) return;
        if (counter > 0) return;

        if (collision.gameObject.name == "unitychan")
        {
            Renderer renderer = GetComponent<Renderer>();
            oldColor = renderer.material.color;
            renderer.material.color = new Color(0, 0, 1, 0.5f);
            counter = (int)(1000 / mazeAppScript.mazeLevel);
            mazeAppScript.LossPower(10);
            GameObject.Find("Plane").GetComponent<Renderer>().material.color = Color.red;
        }
    }

    /// <summary>
    /// Event of Collision Exit
    /// Return color on Plane
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "unitychan")
        {
            GameObject.Find("Plane").GetComponent<Renderer>().material.color = Color.white;
            collision.gameObject.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
        }
    }
}
