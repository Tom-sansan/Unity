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
    /// Controller for left/right
    /// </summary>
    private OVRInput.Controller controller;

    void Start()
    {
        // Get left or right controller
        controller = GetComponent<OVRControllerHelper>().m_controller;
        CreateBlockType();
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One, controller))
            Fire(transform.position, transform.forward, blockObj);
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
        GameObject go = Instantiate(target);
        // Set launch position of block
        go.transform.position = startPos;
        // Fire block from controller
        go.GetComponent<Rigidbody>().AddForce(direction * 10f, ForceMode.Impulse);
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
}
