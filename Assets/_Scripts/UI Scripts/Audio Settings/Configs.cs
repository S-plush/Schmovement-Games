using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Configs : MonoBehaviour
{
    public float volBG, volSFX;
    AudioConfig audioConfig;
    public Button SaveButton;

    public GameObject bgmObject;
    public GameObject sfxObject;
    public Slider bgmSlider;
    public Slider sfxSlider;

    //private GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioConfig = GetComponent<AudioConfig>();

        if(audioConfig == null)
        {
            audioConfig = FindObjectOfType<AudioConfig>();
        }

        LoadAll();
        //player = Alpha.PlayerRef;
        //sfxSlider = GetComponent<Slider>();
        //bgmSlider = GetComponent<Slider>();
        bgmObject = GameObject.Find("Music Volume");
        sfxObject = GameObject.Find("Sound Effects Volume");
        bgmSlider = bgmObject.GetComponent<Slider>();
        sfxSlider = sfxObject.GetComponent<Slider>();
        bgmSlider.value = volBG;
        sfxSlider.value = volSFX;

        if(audioConfig != null)
        {
            audioConfig.SetAll();
        }
        else
        {
            Debug.Log("audioConfig is still null");
        }

        Application.targetFrameRate = -1;
    }

    private void Update()
    {
        //if (bgmSlider == null && sfxSlider == null)
        //{
        //    bgmObject = GameObject.Find("Music Volume");
        //    sfxObject = GameObject.Find("Sound Effects Volume");
        //    bgmSlider = bgmObject.GetComponent<Slider>();
        //    sfxSlider = sfxObject.GetComponent<Slider>();
        //}
    }

    public void BGMSliderChange(float value)
    {
        volBG = value;
        PlayerPrefs.SetFloat("bgmVolume", volBG);
        //Debug.Log("slider changed new volBG: " + volBG);
        SetAllAudio();
        SaveAll();
    }

    public void SFXSliderChange(float value)
    {
        volSFX = value;
        PlayerPrefs.SetFloat("sfxVolume", volSFX);
        SetAllAudio();
        SaveAll();
    }

    public void SaveAll()
    {
        PlayerPrefs.SetFloat("bgmVolume", volBG);
        PlayerPrefs.SetFloat("sfxVolume", volSFX);

        if (audioConfig != null)
        {
            audioConfig.SetAll();
        }
        else
        {
            Debug.Log("audioConfig is still null");
        }
    }

    void LoadAll()
    {
        volBG = PlayerPrefs.GetFloat("bgmVolume");
        volSFX = PlayerPrefs.GetFloat("sfxVolume");
    }

    public void SetAllAudio()
    {
        AudioSource[] audios = GameObject.FindObjectsByType<AudioSource>(FindObjectsSortMode.None);

        foreach (AudioSource source in audios)
        {
            if (source.name.Contains("BG"))
            {
                //Debug.Log("setting musicmanager volume to: " + volBG);
                MusicManager music = source.GetComponent<MusicManager>();
                music.bgmVolume = volBG;
            }
            else if (source.name.Contains("SFX"))
            {
                source.GetComponent<SFXManager>().sfxVolume = volSFX;
            }
        }
    }
}
