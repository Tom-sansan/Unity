using UnityEngine;

public class CameraViewScript : MonoBehaviour
{
    public Transform target;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        // Adjustment of Camera location
        Vector3 vec = target.position;
        Vector3 fvec = target.forward;
        vec.y = 2.5f;
        fvec *= 4f;
        fvec.y = -1f;
        Camera.main.transform.position = vec - fvec;
        Camera.main.transform.LookAt(vec);
    }
}
