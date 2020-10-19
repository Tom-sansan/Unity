using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

/// <summary>
/// Script for Smart Ball
/// </summary>
public class SmartBallScript : MonoBehaviour
{
    GameObject[] ob_cubes;
    GameObject[] goals;
    Rigidbody rigidbody;
    Renderer renderer;
    string ob_cube = "ob_cube";
    string goal = "goal";
    string clip1 = "clip1";
    string halo = "Halo";
    string ob_wall = "ob_wall";
    float power = 0f;
    bool flg = true;
    public Text score;

    // Start is called before the first frame update
    void Start()
    {
        ob_cubes = GameObject.FindGameObjectsWithTag(ob_cube);
        goals = GameObject.FindGameObjectsWithTag(goal);
        rigidbody = GetComponent<Rigidbody>();
        renderer = GetComponent<Renderer>();
        int n = 0;
        foreach (var obj in goals)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            renderer.material.SetFloat("_Mode", 3f);
            renderer.material.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
            renderer.material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
            renderer.material.SetInt("_ZWrite", 0);
            renderer.material.DisableKeyword("_ALPHATEST_ON");
            renderer.material.EnableKeyword("_ALPHABLEND_ON");
            renderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            renderer.material.renderQueue = 3000;
            renderer.material.color = new Color(0f, 0.15f * n, 1f - 0.15f * n++, 0.5f);
        }
        foreach (var obj in ob_cubes)
        {
            Vector3 move = obj.transform.position;
            AnimationClip clip = new AnimationClip { legacy = true };

            // Key Frame X
            Keyframe[] keysX = new Keyframe[2];
            keysX[0] = new Keyframe(0f, move.x - 6);    // Animation Start
            keysX[1] = new Keyframe(1f, move.x + 4);    // Animation End
            // Animation Curve 1
            AnimationCurve curveX = new AnimationCurve(keysX);
            clip.SetCurve(string.Empty, typeof(Transform), "localPosition.x", curveX);
            clip.wrapMode = WrapMode.PingPong;

            // Key Frame Y
            Keyframe[] keysY = new Keyframe[2];
            keysY[0] = new Keyframe(0f, move.y);
            keysY[1] = new Keyframe(1f, move.y);
            // Animation Curve 2
            AnimationCurve curveY = new AnimationCurve(keysY);
            clip.SetCurve(string.Empty, typeof(Transform), "localPosition.y", curveY);

            // Key Frame Z
            Keyframe[] keysZ = new Keyframe[2];
            keysZ[0] = new Keyframe(0f, move.z);
            keysZ[1] = new Keyframe(1f, move.z);
            // Animation Curve 3
            AnimationCurve curveZ = new AnimationCurve(keysZ);
            clip.SetCurve(string.Empty, typeof(Transform), "localPosition.z", curveZ);

            Animation animation = obj.GetComponent<Animation>();
            // Set animation clip
            animation.AddClip(clip, clip1);
            animation.Play(clip1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Move obstacles
        MoveCube();

        rigidbody.AddForce(0f, 0f, -1f);
        if (flg)
        {
            // Space Key
            if (Input.GetKey(KeyCode.Space))
            {
                power += 0.01f;
                if (power > 1f) power = 0.25f;
                renderer.material.color = new Color(1f, power, 0f);
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            rigidbody.AddForce(new Vector3(0f, 0f, power * 3000f));
            power = 0f;
            renderer.material.color = Color.red;
            flg = false;    // Flag that space key is clicked only once.
        }
    }

    /// <summary>
    /// Move ob_cubes
    /// </summary>
    void MoveCube()
    {
        foreach (var obj in ob_cubes)
        {
            obj.transform.Rotate(new Vector3(0f, 3f, 0f));
        }
    }

    /// <summary>
    /// Event of collision enter for ob_cubes
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(ob_cube))
        {
            Behaviour b = (Behaviour)collision.gameObject.GetComponent(halo);
            b.enabled = true;
        }
    }

    /// <summary>
    /// Event of collision exit for ob_cubes and ob_walls
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit(Collision collision)
    {
        // Rigidbody rigidbody = GetComponent<Rigidbody>();
        // Exit process for ob_cube
        if (collision.gameObject.CompareTag(ob_cube))
        {
            Behaviour b = (Behaviour)collision.gameObject.GetComponent(halo);
            b.enabled = false;
            Vector3 v = rigidbody.velocity;
            if (v.magnitude < 10)
            {
                v *= 1.5f;
                if (v.magnitude < 1) v *= 1.5f;
                rigidbody.velocity = v;
            }
        }
        // Exit process for ob_wall
        if (collision.gameObject.CompareTag(ob_wall))
        {
            Vector3 v = rigidbody.velocity;
            if (v.magnitude < 10)
            {
                v *= 2.0f;
                if (v.magnitude < 1) v *= 2.0f;
                rigidbody.velocity = v;
            }
        }
    }

    /// <summary>
    /// Event of trigger enter for goals
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerEnter(Collider collider)
    {
        // Rigidbody rigidbody = GetComponent<Rigidbody>();
        if (collider.gameObject.CompareTag(goal))
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            int n = 1;
            foreach (var obj in goals)
            {
                if (obj == collider.gameObject)
                {
                    score.text = "point:" + (n * 100);
                    ParticleSystem ps = collider.gameObject.GetComponent<ParticleSystem>();
                    ps.Play();
                }
                n++;
            }
        }
    }
}
