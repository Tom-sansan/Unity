using UnityEngine;

public class MazeGoalScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        // Rotate Goal
        transform.Rotate(new Vector3(1, 1, 1));
    }

    /// <summary>
    /// Event of Trigger Enter.
    /// Process when unitychan enters.
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerEnter(Collider collider)
    {
        var appScript = Camera.main.GetComponent<MazeAppScript>();
        if (appScript.IsEnd()) return;
        if (collider.gameObject.name == "unitychan") appScript.GoodEnd();
    }
}
