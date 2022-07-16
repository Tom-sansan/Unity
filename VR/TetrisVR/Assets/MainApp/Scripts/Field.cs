using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Field : MonoBehaviour
{
    [SerializeField]
    private GameObject scoreViewObj;
    [SerializeField]
    private List<GameObject> blockList;
    /// <summary>
    /// TextMesh for score
    /// </summary>
    [SerializeField]
    private TextMeshPro scoreTextMeshPro;
    /// <summary>
    /// Delete effect
    /// </summary>
    [SerializeField]
    private GameObject deleteEffect;
    // Points to add when erasing a row of blocks
    private const int DELETE_POINT = 120;
    // When a block is placed then true, otherwise false
    private bool[,] blocks = new bool[10,20];
    // The number of frame of wait status
    private int waitCnt = 0;
    // The number of frame of the maximum wait
    private const int MAXWAIT = 60;
    // Score value
    private int score;

    void Start()
    {
        InitBlocks();
    }


    void Update()
    {
        switch (GameStatus.status)
        {
            case "Shot":
                waitCnt = MAXWAIT;
                break;
            case "Wait":
                waitCnt--;
                // If the block is not hit for 1 second, return to the Shot state.
                if (waitCnt <= 0) GameStatus.status = "Shot";
                break;
            case "Fall":
                Debug.Log("Fall Status");
                if (!FallBlocks()) GameStatus.status = "Delete";
                break;
            case "Delete":
                CheckLines();
                break;
        }
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

        GameStatus.status = "Fall";
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
            GameStatus.status = "Shot";
            for (int i = 0; i < collision.gameObject.transform.childCount; i++)
                Destroy(collision.gameObject.transform.GetChild(i).transform.GetComponent<BoxCollider>());
            collision.gameObject.transform.GetComponent<Rigidbody>().AddForce(Vector3.back * 10f, ForceMode.Impulse);
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
            //target.transform.GetChild(i).GetComponent<Block>().x = x + bx;
            //target.transform.GetChild(i).GetComponent<Block>().y = y + by;
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

    private bool FallBlocks()
    {
        var retblocks = blocks.Clone() as bool[,];
        SortBlockList();

        // Check if a block is able to be fallen.
        var isFall = true;
        // Check if there is a block below
        foreach (var go in blockList)
        {
            int x = go.GetComponent<Block>().x;
            int y = go.GetComponent<Block>().y;
            if (y - 1 < 0 || retblocks[x, y - 1])
            {
                isFall = false;
                break;
            }
            retblocks[x, y] = false;
        }
        if (isFall)
        {
            // Place a block
            foreach (var go in blockList)
            {
                int x = go.GetComponent<Block>().x;
                int y = go.GetComponent<Block>().y;
                go.transform.position += Vector3.down * 0.1f;

                blocks[x, y] = false;
                blocks[x, y - 1] = true;

                go.GetComponent<Block>().y = -1;
                go.name = $"name:{x}, {y - 1}";
            }
        }
        return isFall;
    }
    /// <summary>
    /// Check for disappearing lines.
    /// </summary>
    private void CheckLines()
    {
        for (int i = 0; i < 20; i++)
        {
            var isDelete = true;
            for (int j = 0; j < 10; j++)
            {
                // Use "20-i-1" instead of "i" to examine from the top
                if (!blocks[j, 20 - i - 1])
                {
                    isDelete = false;
                    break;
                }
            }
            // Delete if the column can be deleted
            if (isDelete)
            {
                // Delete a single column block
                DeleteBlocks(20 - i - 1);
                // Lower the block by the amount that disappears
                DropBlocks(20 - i - 1);

                // Add score
                score += DELETE_POINT;
                scoreTextMeshPro.text = "" + score;
            }
        }
    }
    /// <summary>
    /// Process to delete a single column block
    /// </summary>
    /// <param name="h"></param>
    private void DeleteBlocks(int h)
    {
        // Delete GameObjects in a single column block
        var list = GameObject.FindGameObjectsWithTag("block");
        foreach (var go in list)
        {
            if (go.GetComponent<Block>().y == h)
            {
                // Generate DeleteEffect
                var deleteEffectClone = Instantiate(deleteEffect);
                deleteEffectClone.transform.position = go.transform.position + Vector3.back * 0.1f;
                deleteEffectClone.transform.localScale = Vector3.one * 0.1f;
                blocks[go.GetComponent<Block>().x, h] = false;
                Destroy(go);
            }
        }
    }
    /// <summary>
    /// Process to drop blocks higher than the erased row
    /// </summary>
    /// <param name="h"></param>
    private void DropBlocks(int h)
    {
        // Register blocks to be dropped
        var list = new List<GameObject>(GameObject.FindGameObjectsWithTag("block"));
        list.OrderBy((x) => x.GetComponent<Block>().y);

        foreach (var go in list)
        {
            if (go.GetComponent<Block>().y > h)
            {
                // Update blocks
                int x = go.GetComponent<Block>().x;
                int y = go.GetComponent<Block>().y;
                blocks[x, y] = false;
                blocks[x, y - 1] = true;
                // Shift y-coordinate
                go.GetComponent<Block>().y -= 1;
                // Shift the coordinates of GameObject
                go.transform.position += Vector3.down * 0.1f;

            }
        }
    }
}
