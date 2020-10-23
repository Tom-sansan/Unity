using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTest : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField]    // よそからアクセスできない
    AudioClip audioClip;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) audioSource.PlayOneShot(audioClip);
    }

    public void KeyPush(int key)
    {
        switch (key)
        {
            case 0:
                audioSource.PlayOneShot(audioClip);
                break;
            case 1:
                // audioSource.PlayOneShot(audioClip[i]);
                break;
            default:
                break;
        }
    }
}
