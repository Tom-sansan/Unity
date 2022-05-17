using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    private static AudioManager instance;
    private readonly Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();
    public static AudioManager Instance
    {
        get { return instance; }
    }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
        var audioClips = Resources.LoadAll<AudioClip>("2D_SE");
        foreach (var clip in audioClips)
        {
            clips.Add(clip.name, clip);
        }
    }

    public void Play(string clipName)
    {
        if (!clips.ContainsKey(clipName)) throw new System.Exception("Sound " + clipName + " is not defined.");

        audioSource.clip = clips[clipName];
        audioSource.Play();
    }
}
