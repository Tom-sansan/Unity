using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField]
    private GameObject scoreViewObj;

    List<GameObject> blockList;
    // When a block is placed then true, otherwise false
    private bool[,] blocks = new bool[10,20];

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

        if (collision.gameObject.tag != "blockUnits") return;
        var b = collision.gameObject.transform.position;
        var u = 0.1f;
        var g = new Vector3
                (
                    // (float)((int)(b.x / u)) * u,  // x-coordinate
                    // (float)((int)(b.y / u)) * u,  // y-coordinate
                    ((int)(b.x / u)) * u,  // x-coordinate
                    ((int)(b.y / u)) * u,  // y-coordinate
                    transform.position.z   // z-coordinate
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
        // blocks[bx, by] = true;
        // Set name for reference
        // collision.gameObject.name = $"name:{bx}, {by}";
        // Reflects coordinates
        // collision.gameObject.transform.position = new Vector3(bx * u + px, by * u + py, transform.position.z);

        // var rb = collision.gameObject.GetComponent<Rigidbody>();
        // Destroy(rb);

        // Show position
        // var sv = Instantiate(scoreViewObj);
        // sv.transform.position = collision.gameObject.transform.position;
        // sv.GetComponent<ScoreView>().textMeshPro.text = $"({bx},{by})";

        // Return Error if out of range
        if (bx < 0 || by < 0 || bx >= 10 || by >= 20 || CheckExistBlock(collision.gameObject, bx, by))
        {
            for (int i = 0; i < collision.gameObject.transform.childCount; i++)
                Destroy(collision.gameObject.transform.GetChild(i).transform.GetComponent<BoxCollider>());
        }
        else
        {
            // Reflect coordinates
            collision.gameObject.transform.position = new Vector3(bx * u + px, by * u + py, transform.position.z);
            // Make array of placed position true
            ApplyBlockUnits(collision.gameObject, bx, by);
            // Show position
            var sv = Instantiate(scoreViewObj);
            sv.transform.position = collision.gameObject.transform.position;
            sv.GetComponent<ScoreView>().textMeshPro.text = $"({bx}, {by})";
            // Delete parent object
            var count = collision.transform.childCount;
            for (int i = 0; i < count; i++)
                collision.transform.GetChild(0).parent = null;
            collision.transform.tag = "Untagged";
            collision.transform.position = Vector3.forward * 1000f;
        }
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
    /// <summary>
    /// Check if block exists at the target
    /// </summary>
    /// <returns></returns>
    private bool CheckExistBlock(GameObject target, int x, int y)
    {
        for (int i = 0; i < target.transform.childCount; i++)
        {
            var g = target.transform.GetChild(i).localPosition;
            var bx = (int)(g.x * 10);
            var by = (int)(g.y * 10);
            // Make it a rule that block exits outside the frame
            if (x + bx < 0 || y + by < 0 || x + bx >= 10 || y + by >= 20) return true;
            // Check if block exits in the location on (x + bx, y + by)
            if (!(x + bx < 0 || x + bx >= 10 || y + by < 0 || y + by >= 20) && blocks[x + bx, y + by]) return true;
        }
        return false;
    }
    /// <summary>
    /// Save block
    /// </summary>
    /// <param name="target"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void ApplyBlockUnits(GameObject target, int x, int y)
    {
        blockList = new List<GameObject>();
        for (int i = 0; i < target.transform.childCount; i++)
        {
            var g = target.transform.GetChild(i).localPosition;
            var bx = (int)(g.x * 10);
            var by = (int)(g.y * 10);
            blocks[x + bx, y + by] = true;
            // Set coordinates for judgment
            target.transform.GetChild(i).GetComponent<Block>().x = x + bx;
            target.transform.GetChild(i).GetComponent<Block>().y = y + by;
            // Set name for reference
            target.transform.GetChild(i).name = $"name:{x + bx}, {y + by}";
            // Set to GameObject for falling
            blockList.Add(target.transform.GetChild(i).gameObject);
        }
    }

    private void SortBlockList()
    {
        for (int i = 0; i < blockList.Count; i++)
            for (int j = i; j < blockList.Count; j++)
                if (blockList[i].GetComponent<Block>().y > blockList[j].GetComponent<Block>().y)
                {
                    var tmp = blockList[i];
                    blockList[i] = blockList[j];
                    blockList[j] = tmp;
                }
    }
}
