using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField]
    private string name = string.Empty;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == name) Object.Destroy(this.gameObject);
    }
}
