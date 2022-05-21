using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBazooka : MonoBehaviour
{
    /// <summary>
    /// Block GameObject
    /// </summary>
    [SerializeField]
    private GameObject blockObj;
    /// <summary>
    /// Controller for left/right
    /// </summary>
    private OVRInput.Controller controller;

    void Start()
    {
        controller = GetComponent<OVRControllerHelper>().m_controller;
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One, controller))
            Fire(transform.position, transform.forward, blockObj);
    }
    /// <summary>
    /// Fire block
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="direction"></param>
    /// <param name="target"></param>
    private void Fire(Vector3 startPos, Vector3 direction, GameObject target)
    {
        // Create block copy
        GameObject go = Instantiate(target);
        // Set launch position of block
        go.transform.position = startPos;
        // Fire block from controller
        go.GetComponent<Rigidbody>().AddForce(direction * 10f, ForceMode.Impulse);
    }
}
