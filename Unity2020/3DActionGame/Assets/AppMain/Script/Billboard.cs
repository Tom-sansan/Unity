using UnityEngine;

public class Billboard : MonoBehaviour
{
    /// <summary>
    /// Target camera
    /// </summary>
    [SerializeField]
    private Camera lookCamera = null;
    /// <summary>
    /// Flag for Y-axis rotation only
    /// </summary>
    [SerializeField]
    private bool isY = false;

    void Start()
    {
        if (lookCamera == null) lookCamera = Camera.main;
    }

    void FixedUpdate()
    {
        if (lookCamera == null) return;

        // Y-axis rotation only
        if (isY)
        {
            var cameraPos = lookCamera.transform.position;
            cameraPos.y = this.transform.position.y;
            var look = this.transform.position - cameraPos;
            this.transform.forward = look;
        }
        else
        {
            // Completely face front to camera
            var cameraPos = lookCamera.transform.position;
            var look = this.transform.position - cameraPos;
            this.transform.forward = look;

        }
    }
}
