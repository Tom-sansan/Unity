using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    Rigidbody rigidbody;
    Vector3 defaultVec;
    public AudioClip sound;
    public Text score;
    int scorePoint = 0;

    // Start is called before the first frame update
    void Start()
    {
        defaultVec = transform.position;
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) rigidbody.velocity = new Vector3(0f, 10f, 0f);
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            rigidbody.velocity = Vector3.zero;
            transform.position = defaultVec;
        }
        else if (Input.GetKey(KeyCode.Return)) rigidbody.velocity = rigidbody.velocity * 0.50f;

        MoveByKeys();
    }

    private void OnCollisionEnter(Collision collision)
    {
        AudioSource.PlayClipAtPoint(sound, transform.position);
        // GetComponent<Audio();Source>().Pla
        if (collision.gameObject.name.StartsWith("Cube"))
        {
            GetScore(collision.gameObject);
            DeleteCube(collision.gameObject);
        }
    }

    private void GetScore(GameObject gameObject)
    {
        scorePoint += 100;
        Debug.Log(scorePoint);
        score.text = "Point:" + scorePoint;
    }

    private void DeleteCube(GameObject gameObject)
    {
        Destroy(gameObject, 0f);
    }

    private void MoveByKeys()
    {
        if (Input.GetKey(KeyCode.LeftArrow)) SetRigidbodyAddForm(-1f, 0, 1f);
        if (Input.GetKey(KeyCode.RightArrow)) SetRigidbodyAddForm(1f, 0, -1f);
        if (Input.GetKey(KeyCode.UpArrow)) SetRigidbodyAddForm(1f, 0, 1f);
        if (Input.GetKey(KeyCode.DownArrow)) SetRigidbodyAddForm(-1f, 0, -1f);
    }

    //public void KeyPush(KeyCode keyCode)
    //{
    //    switch (keyCode)
    //    {
    //        case KeyCode.LeftArrow:
    //            SetRigidbodyAddForm(-1f, 0, 1f);
    //            break;
    //        case 1:
    //            // audioSource.PlayOneShot(audioClip[i]);
    //            break;
    //        default:
    //            break;
    //    }
    //}

    private void SetRigidbodyAddForm(float x, float y, float z)
    {
        rigidbody.AddForce(new Vector3(x, y, z));
    }
}
