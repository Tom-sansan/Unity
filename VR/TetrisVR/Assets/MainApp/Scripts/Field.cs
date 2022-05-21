using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField]
    private GameObject scoreViewObj;

    // When a block is placed then true, otherwise false
    bool[,] blocks = new bool[10,20];

    void Start()
    {
        InitBlocks();
    }


    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        // var rigidbody = collision.gameObject.GetComponent<Rigidbody>();
        // Remove rigidbody -> Lose physics in the block -> stick to the field
        // Destroy(rigidbody);
        var p = collision.transform.position;
        GameObject sv = Instantiate(scoreViewObj);
        sv.transform.position = collision.gameObject.transform.position;
        sv.GetComponent<ScoreView>().textMeshPro.text = $"({p.x}, {p.y}, {p.z})";
    }
    /// <summary>
    /// Initialize blocks
    /// </summary>
    private void InitBlocks()
    {
        for (int i = 0; i < blocks.GetLength(0); i++)
            for (int j = 0; j < blocks.GetLength(1); j++)
                blocks[i,j] = false;
    }
}
