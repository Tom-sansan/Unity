using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AudioManager Class
/// </summary>
public class AudioManager : MonoBehaviour
{
    #region Nested Class

    #endregion Nested Class

    #region Enum

    #endregion Enum

    #region Variables

    #region SerializeField
    /// <summary>
    /// AudioClip List for BGM
    /// </summary>
    [Header("BGM")]
    [SerializeField]
    private List<AudioClip> BGMClipList;
    /// <summary>
    /// AudioClip List for Attack SE
    /// </summary>
    [Header("SE for Attack")]
    [SerializeField]
    private List<AudioClip> SEAttackClipList;
    /// <summary>
    /// AudioClip List for Heal SE
    /// </summary>
    [Header("SE for Heal")]
    [SerializeField]
    private List<AudioClip> SEHealClipList;
    #endregion SerializeField

    #region Protected Variables

    #endregion Protected Variables

    #region Public Variables

    #region Public Properties

    #endregion Public Properties

    #endregion Public Variables

    #region Private Variables
    /// <summary>
    /// AudioSource for BGM
    /// </summary>
    private AudioSource bgmSource;
    /// <summary>
    /// AudioSource for Attack SE
    /// </summary>
    private AudioSource seAttackSource;
    /// <summary>
    /// AudioSource for Heal SE
    /// </summary>
    private AudioSource seHealSource;
    #region Private Properties

    #endregion Private Properties

    #endregion Private Variables

    #endregion Variables

    #region Methods

    #region Unity Methods
    void Start()
    {
        Init();
    }
    void Update()
    {

    }
    #endregion Unity Methods

    #region Public Methods
    /// <summary>
    /// Play attack SE randomly
    /// </summary>
    public void PlayRandomAttackSE()
    {
        if (SEAttackClipList.Count == 0) return;
        PlaySoundRandomly(seAttackSource, SEAttackClipList);
    }
    /// <summary>
    /// Play heal SE randomly
    /// </summary>
    public void PlayHealSE()
    {
        if (SEHealClipList.Count == 0) return;
        PlaySoundRandomly(seHealSource, SEHealClipList);
    }
    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Initialize this class
    /// </summary>
    private void Init()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        seAttackSource = gameObject.AddComponent<AudioSource>();
        seHealSource = gameObject.AddComponent<AudioSource>();
        // Play BGM randomly
        PlayRandomBGM();
    }
    /// <summary>
    /// Play BGM randomly
    /// </summary>
    private void PlayRandomBGM()
    {
        PlaySoundRandomly(bgmSource, BGMClipList);
        bgmSource.loop = true;
    }
    /// <summary>
    /// Play sound randomly
    /// </summary>
    /// <param name="audioSource"></param>
    /// <param name="list"></param>
    private static void PlaySoundRandomly(AudioSource audioSource, List<AudioClip> list)
    {
        if (list.Count == 0) return;
        int randomIndex = Random.Range(0, list.Count);
        audioSource.clip = list[randomIndex];
        audioSource.Play();
    }
    #endregion Private Methods

    #endregion Methods

    #region For Debug

#if DEBUG

#endif

    #endregion For Debug
}
