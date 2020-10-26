using UnityEngine;
using UnityEngine.Audio;

public class ChangeEffectScript : MonoBehaviour
{
    [SerializeField]
    AudioMixer audioMixer;
    [SerializeField]
    AudioMixerSnapshot[] audioMixerSnapshots;
    int num = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (num == 0)
            {
                num = 1;
                audioMixer.TransitionToSnapshots(audioMixerSnapshots, new float[] { 0f, 1f }, 1);
                Debug.Log("Echo");
            }
            else if (num == 1)
            {
                num = 0;
                audioMixer.TransitionToSnapshots(audioMixerSnapshots, new float[] { 1f, 0f }, 1);
                Debug.Log("Normal");
            }
        }
    }
}
