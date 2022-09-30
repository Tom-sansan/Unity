using UnityEngine;

public class Hole : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("落ちたボールの名前：" + collider.gameObject.name);
        collider.gameObject.SetActive(false);
    }
}
