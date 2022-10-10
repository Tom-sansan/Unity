using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField]
    private string objName = string.Empty;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == objName) Object.Destroy(this.gameObject);
    }
}
