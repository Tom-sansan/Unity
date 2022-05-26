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

        //var p = collision.transform.position;
        //GameObject sv = Instantiate(scoreViewObj);
        //sv.transform.position = collision.gameObject.transform.position;
        //sv.GetComponent<ScoreView>().textMeshPro.text = $"({p.x}, {p.y}, {p.z})";

        var b = collision.gameObject.transform.position;
        var u = 0.1f;
        var g = new Vector3
                (
                    (float)((int)(b.x / u)) * u,  // x-coordinate
                    (float)((int)(b.y / u)) * u,  // y-coordinate
                    transform.position.z        // z-coordinate
                );
        // Reflects coordinates
        // collision.gameObject.transform.position = g;
        // Constant Orientation
        collision.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        // Define bottom left coordinate
        var px = -0.4f;
        var py = 0.1f;
        // Get blocks element number
        var bx = GetPosition((g.x - px) / u, 0.01f, 0, 10);
        var by = GetPosition((g.y - py) / u, 0.01f, 0, 20);
        // Set the array of placed positions to true
        blocks[bx, by] = true;
        // Set name for reference
        collision.gameObject.name = $"name:{bx}, {by}";
        // Reflects coordinates
        collision.gameObject.transform.position = new Vector3(bx * u + px, by * u + py, transform.position.z);

        var rb = collision.gameObject.GetComponent<Rigidbody>();
        Destroy(rb);

        // Show position
        var sv = Instantiate(scoreViewObj);
        sv.transform.position = collision.gameObject.transform.position;
        sv.GetComponent<ScoreView>().textMeshPro.text = $"({bx},{by})";
    }
    /// <summary>
    /// transform the position of coordinates.Returns array numbers based on coordinates
    /// </summary>
    /// <param name="value"></param>
    /// <param name="distance"></param>
    /// <param name="minRange"></param>
    /// <param name="max"></param>
    private int GetPosition(float value, float distance, int minRange, int maxRange)
    {
        for (int i = minRange; i < maxRange; i++)
            if (Mathf.Abs(value - (float)(i)) < distance) return i;
        return minRange - 1;
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
