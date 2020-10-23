using UnityEngine;

/// <summary>
/// Class for Avatar Collider
/// </summary>
public class MazeAvatarColliderScript : MonoBehaviour
{
    /// <summary>
    /// Collision flag that shows if enemy touches unitychan or not.
    /// </summary>
    public bool collisionFlg;

    // Start is called before the first frame update
    void Start()
    {
        collisionFlg = false;
    }

    // Update is called once per frame
    void Update() { }

    /// <summary>
    /// Event of Collision Enter
    /// Process that is collided on wall
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        var mazeAppScript = Camera.main.GetComponent<MazeAppScript>();
        if (mazeAppScript.IsEnd()) return;
        collisionFlg = true;
        mazeAppScript.LossPower(1);
    }

    /// <summary>
    /// Event of Collision Exit
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit(Collision collision) => collisionFlg = false;
}
