using UnityEngine;

/// <summary>
/// Class for Avatar action
/// </summary>
public class MazeAvatarScript : MonoBehaviour
{
    Animator animator;
    string speed = "Speed";
    string direction = "Direction";
    string jump = "Jump";

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var appScript = Camera.main.GetComponent<MazeAppScript>();
        // End game if game is ended
        if (appScript.IsEnd())
        {
            animator.SetFloat(speed, 0);
            animator.SetFloat(direction, 0);
            animator.SetBool(jump, false);
            return;
        }

        var mazeAvatarColliderScript = GameObject.Find("unitychan").GetComponent<MazeAvatarColliderScript>();
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        // Make unitychan move checking collision
        if (!mazeAvatarColliderScript.collisionFlg)
        {
            // Normal move
            animator.SetFloat(speed, v);
            animator.SetFloat(direction, h);
            animator.SetBool(jump, false);
            Vector3 vector = new Vector3(0, 0, v);
            vector = transform.TransformDirection(vector) * 5f;
            transform.localPosition += vector * Time.fixedDeltaTime;
            transform.Rotate(0, h, 0);
        }
        else
        {
            // Invert unitychan
            Vector3 vec = transform.forward;
            vec *= -1;
            vec.y = 0;
            transform.position += vec;
            Vector3 vec2 = new Vector3(0, 180, 0);
            transform.Rotate(vec2);
            mazeAvatarColliderScript.collisionFlg = false;
            // Stop rotation move
            GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
        }
    }
}
