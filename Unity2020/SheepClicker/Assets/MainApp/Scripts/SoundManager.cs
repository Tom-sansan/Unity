using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sound Manager Class
/// </summary>
public class SoundManager : MonoBehaviour
{
    /// <summary>
    /// Sound Type
    /// </summary>
    public enum SoundType
    {
        Coin,
        Button,
        Shear,
        Cry
    }
    /// <summary>
    /// Sound Class
    /// </summary>
    [Serializable]
    public class SoundData
    {
        public string name;
        public AudioClip audioClip;
        /// <summary>
        /// Last Playback Time
        /// </summary>
        public float playedTime;
    }
    /// <summary>
    /// Sound data
    /// </summary>
    [SerializeField]
    private SoundData[] soundData;
    /// <summary>
    /// Interval (in seconds) between one playback and the next
    /// </summary>
    [SerializeField]
    private float playableDistance = 0.2f;
    /// <summary>
    /// SoundManager instance
    /// </summary>
    public static SoundManager Instance { get; private set; }
    /// <summary>
    /// Dictionary for management with alias (name) as key
    /// </summary>
    private Dictionary<string, SoundData> soundDictionary = new Dictionary<string, SoundData>();
    /// <summary>
    /// Prepare as many AudioSources (speakers) as to play simultaneously
    /// </summary>
    private AudioSource[] audioSourceList = new AudioSource[20];

    private void Awake()
    {
        InitAwake();
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    /// <summary>
    /// Play specified AudioClip with unused AudioSource
    /// </summary>
    /// <param name="clip"></param>
    public void Play(AudioClip clip)
    {
        var audioSource = GetUnusedAudioSounce();
        if (audioSource == null) return;
        audioSource.clip = clip;
        audioSource.Play();
    }
    /// <summary>
    /// Play registered AudioClip with specified alias
    /// </summary>
    /// <param name="name"></param>
    public void Play(string name)
    {
        // Search by alias from Administrative Dictionary
        if (soundDictionary.TryGetValue(name, out var soundData))
        {
            // It's too early to play
            if (Time.realtimeSinceStartup - soundData.playedTime < playableDistance) return;
            // Retain this playback time for next time
            soundData.playedTime = Time.realtimeSinceStartup;
            Play(soundData.audioClip);
        }
        else Debug.LogWarning($"The alias is not registered.{name}");
    }
    /// <summary>
    /// Init Awake
    /// </summary>
    private void InitAwake()
    {
        CreateInstance();
        CreateAudioSource();
    }
    /// <summary>
    /// Create instance
    /// </summary>
    private void CreateInstance()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }
    /// <summary>
    /// Create AudioSounce
    /// </summary>
    private void CreateAudioSource()
    {
        // Create as many AudioSources as there are in the audioSourceList array and store them in the array
        for (var i = 0; i < audioSourceList.Length; i++) audioSourceList[i] = gameObject.AddComponent<AudioSource>();
        // Set to soundDictionary
        foreach (var sound in soundData) soundDictionary.Add(sound.name, sound);
    }
    /// <summary>
    /// Get unused AudioSouce. Or return null if all are in use.
    /// </summary>
    /// <returns></returns>
    private AudioSource GetUnusedAudioSounce()
    {
        for (int i = 0; i < audioSourceList.Length; i++)
            if (!audioSourceList[i].isPlaying) return audioSourceList[i];
        // No unused AudioSource found
        return null;
    }

}
