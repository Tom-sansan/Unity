using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundSettingsScript : MonoBehaviour
{
    [SerializeField]
    private GameObject soundSettingCanvas;
    [SerializeField]
    private AudioMixer audioMixer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            soundSettingCanvas.SetActive(!soundSettingCanvas.activeSelf);
            Debug.Log("OK");
        }
    }

    public void SetBGM(float val) => audioMixer.SetFloat("BGMVol", val);

    public void SetSE(float val) => audioMixer.SetFloat("SEVol", val);

}
