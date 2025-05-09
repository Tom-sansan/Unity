using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Struct for block type
/// </summary>
public struct BlockType
{
    /// <summary>
    /// Array of shape
    /// </summary>
    public int[,] shape;
}

public class BlockBazooka : MonoBehaviour
{
    /// <summary>
    /// Block GameObject
    /// </summary>
    [SerializeField]
    private GameObject blockObj;
    /// <summary>
    /// Block type creation
    /// </summary>
    [SerializeField]
    private BlockType[] blockTypes;
    /// <summary>
    /// Pointer object
    /// </summary>
    [SerializeField]
    private GameObject pointObj;
    /// <summary>
    /// Controller for left/right
    /// </summary>
    private OVRInput.Controller controller;
    /// <summary>
    /// BlockUnit for launch
    /// </summary>
    private GameObject shotObj;

    void Start()
    {
        // Get left or right controller
        controller = GetComponent<OVRControllerHelper>().m_controller;
        CreateBlockType();
        InitiallizeBlockUnit();
    }

    void Update()
    {
        switch (GameStatus.status)
        {
            case "Shot":
                // Make pointer visible
                ViewPointer();
                // Fire block when A or X button is pushed
                if (OVRInput.GetDown(OVRInput.Button.One, controller))
                    // Fire(transform.position, transform.forward, blockObj);
                    Fire(transform.position, transform.forward, CreateBlock(Random.Range(0, 7)));
                break;
            case "Wait":
                break;
            case "Fall":
                break;
            case "Delete":
                break;
        }
    }
    /// <summary>
    /// Define block type
    /// </summary>
    private void CreateBlockType()
    {
        blockTypes = new BlockType[7];
        blockTypes[0].shape = new int[,] {
            {0,0,0,0},
            {0,0,0,0},
            {1,1,1,1},
            {0,0,0,0},
        };
        blockTypes[1].shape = new int[,] {
            {1,1},
            {1,1},
        };
        blockTypes[2].shape = new int[,] {
            {0,1,0},
            {1,1,1},
            {0,0,0},
        };
        blockTypes[3].shape = new int[,] {
            {0,0,1},
            {1,1,1},
            {0,0,0},
        };
        blockTypes[4].shape = new int[,] {
            {1,0,0},
            {1,1,1},
            {0,0,0},
        };
        blockTypes[5].shape = new int[,] {
            {1,1,0},
            {0,1,1},
            {0,0,0},
        };
        blockTypes[6].shape = new int[,] {
            {0,1,1},
            {1,1,0},
            {0,0,0},
        };
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
        // GameObject go = Instantiate(target);

        var rb = target.GetComponent<Rigidbody>();
        target.transform.parent = null;
        // Allow block collision detection
        for (int i = 0; i < target.transform.childCount; i++)
            target.transform.GetChild(i).GetComponent<BoxCollider>().enabled = true;

        // Set launch position of block
        // go.transform.position = startPos;
        target.transform.position = startPos;
        // Fire block from controller
        // go.GetComponent<Rigidbody>().AddForce(direction * 10f, ForceMode.Impulse);
        target.GetComponent<Rigidbody>().AddForce(direction * 10f, ForceMode.Impulse);
        // Change the status with Wait
        GameStatus.status = "Wait";
    }
    /// <summary>
    /// Create Block
    /// </summary>
    /// <param name="typeNum"></param>
    /// <returns></returns>
    private GameObject CreateBlock(int typeNum)
    {
        var size = blockTypes[typeNum].shape.GetLength(0);
        var blockUnits = new GameObject("BlockUnits");
        blockUnits.tag = "blockUnits";
        // Add Rigidbody for physical operation
        blockUnits.AddComponent<Rigidbody>();
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                // Generate a block if it is in a position to place a block
                if (blockTypes[typeNum].shape[j, i] == 1)
                {
                    var go = Instantiate(blockObj);
                    go.transform.parent = blockUnits.transform;
                    // The position at which the block is created is determined by the relative position of the parent object
                    go.transform.localPosition = new Vector3(i - size / 2, size / 2 - j, 0) * 0.1f;
                }
            }
        }
        return blockUnits;
    }
    /// <summary>
    /// Create BlockUnits in advance
    /// </summary>
    private void InitiallizeBlockUnit()
    {
        shotObj = CreateBlock(Random.Range(0, 7));
        shotObj.transform.parent = transform;
        shotObj.transform.localPosition = Vector3.zero;
    }
    /// <summary>
    /// Make pointer visible
    /// </summary>
    private void ViewPointer()
    {
        var hits = Physics.RaycastAll(transform.position, transform.forward, 10f);
        if (hits.Length == 0) pointObj.SetActive(false);
        else
        {
            pointObj.SetActive(true);
            foreach (var hit in hits)
                if (hit.collider.name == "Field") pointObj.transform.position = hit.point;
        }
    }
}
