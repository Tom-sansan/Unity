using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Audio Manager class
/// Note: Implemented singleton not to be disposed when scene is changed
/// </summary>
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource _audioSource;
    private readonly Dictionary<string, AudioClip> _clips = new Dictionary<string, AudioClip>();
    private const string BGMVolume = "BGMVolume";

    private static AudioManager instance;
    public static AudioManager Instance => instance;

    private void Start()
    {
        // Set value to BGMVolume
        // Note: Volume is dB so the range is from -80 to 0
        audioMixer.SetFloat(BGMVolume, -20);
        // Get volume
        float volumeValue;
        _ = audioMixer.GetFloat(BGMVolume, out volumeValue);
    }

    private void Awake()
    {
        if (null != instance)
        {
            // Destroy if instance exists
            Destroy(gameObject);
            return;
        }

        // Don't destroy when scene is changed
        DontDestroyOnLoad(gameObject);
        instance = this;

        // Get all of Audio Clip under directory of Resources/2D_SE
        var audioClips = Resources.LoadAll<AudioClip>("2D_SE");
        foreach (var clip in audioClips)
        {
            _clips.Add(clip.name, clip);
        }
    }

    /// <summary>
    /// Play audio file of designated name
    /// </summary>
    /// <param name="clipName"></param>
    public void Play(string clipName)
    {
        // Error if not existing name is selected
        if (!_clips.ContainsKey(clipName)) throw new Exception("Sound " + clipName + " is not defined!");

        // Play based on selected name
        _audioSource.clip = _clips[clipName];
        _audioSource.Play();
    }
}
