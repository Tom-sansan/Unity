using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField]
    public TextMeshPro textMeshPro;

    [SerializeField]
    private int count = 0;

    private const int MAXCOUNT = 60;

    void Start()
    {
        
    }

    void Update()
    {
        count++;
        count %= MAXCOUNT;
        if (count == 0) Destroy(gameObject);
        transform.position += Vector3.up * 0.05f;
    }
}
