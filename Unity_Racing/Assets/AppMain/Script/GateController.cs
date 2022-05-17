using UnityEngine;

/// <summary>
/// Gate Controller
/// </summary>
public class GateController : MonoBehaviour
{
    // Front collider call
    [SerializeField]
    ColliderCallReceiver frontColliderCall = null;
    // Back collider call
    [SerializeField]
    ColliderCallReceiver backColliderCall = null;

    void Start()
    {
        frontColliderCall.TriggerEnterEvent.AddListener(OnFrontTriggerEnter);
        backColliderCall.TriggerEnterEvent.AddListener(OnBackTriggerEnter);
    }
    /// <summary>
    /// Front trigger enter call
    /// </summary>
    /// <param name="collider">Intruding Collider</param>
    private void OnFrontTriggerEnter(Collider collider)
    {
        // The tag of the game object of the intruding collider is Player
        if (collider.gameObject.tag == "Player")
        {
            var player = collider.gameObject.GetComponent<PlayerController>();
            player?.OnFrontGateCall();
        }
        // The tag of the game object of the intruding collider is CPU
        else if (collider.gameObject.tag == "CPU")
        {
            var cpuObj = collider.gameObject.transform.parent.parent.gameObject;
            var cpu = cpuObj.GetComponent<CPUController>();
            cpu?.OnFrontGateCall();
        }
    }
    /// <summary>
    /// Back trigger enter call
    /// </summary>
    /// <param name="collider">Intruding collider</param>
    private void OnBackTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            var player = collider.gameObject.GetComponent<PlayerController>();
            player?.OnBackGateCall();
        }
        // The tag of the game object of the intruding collider is the CPU
        else if (collider.gameObject.tag == "CPU")
        {
            var cpuObj = collider.gameObject.transform.parent.parent.gameObject;
            var cpu = cpuObj.GetComponent<CPUController>();
            cpu?.OnBackGateCall();
        }
    }
}
