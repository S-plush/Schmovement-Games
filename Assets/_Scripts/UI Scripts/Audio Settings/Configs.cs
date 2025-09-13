using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Configs : MonoBehaviour
{
    public float volBG, volSFX;
    public bool optionsGUI;
    public Rect optionsRect = new Rect(20, 20, 800, 800);
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
        LoadAll();
        //player = Alpha.PlayerRef;
        sfxSlider = GetComponent<Slider>();
        bgmSlider = GetComponent<Slider>();
        bgmSlider.value = volBG;
        sfxSlider.value = volSFX;
        //bgmObject = GameObject.Find("Music Volume");
        //sfxObject = GameObject.Find("Sound Effects Volume");
        //bgmSlider = bgmObject.GetComponent<Slider>();
        //sfxSlider = sfxObject.GetComponent<Slider>();

        audioConfig = GetComponent<AudioConfig>();
        audioConfig.SetAll();
        Application.targetFrameRate = -1;
    }

    //private void Update()
    //{
    //    if (bgmSlider == null && sfxSlider == null)
    //    {
    //        bgmObject = GameObject.Find("Music Volume");
    //        sfxObject = GameObject.Find("Sound Effects Volume");
    //        bgmSlider = bgmObject.GetComponent<Slider>();
    //        sfxSlider = sfxObject.GetComponent<Slider>();
    //    }
    //}

    public void OpenOptions()
    {
        optionsGUI = !optionsGUI;
    }

    public void BGMSliderChange(float value)
    {
        volBG = value;
        PlayerPrefs.SetFloat("bgmVolume", volBG);
        //Debug.Log("slider changed new volBG: " + volBG);
        SetAllAudio();
        //SaveAll();
    }

    public void SFXSliderChange(float value)
    {
        volSFX = value;
        PlayerPrefs.SetFloat("sfxVolume", volSFX);
        SetAllAudio();
        //SaveAll();
    }

    public void SaveAll()
    {
        PlayerPrefs.SetFloat("bgmVolume", volBG);
        PlayerPrefs.SetFloat("sfxVolume", volSFX);
        audioConfig.SetAll();
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
