using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioConfig : MonoBehaviour
{
    public void SetAll()
    {
        AudioSource[] audios = GameObject.FindObjectsByType<AudioSource>(FindObjectsSortMode.None);

        foreach (AudioSource source in audios)
        {
            if (source.name.Contains("BG"))
            {
                source.GetComponent<MusicManager>().bgmVolume = PlayerPrefs.GetFloat("bgmVolume");
            }
            else if (source.name.Contains("SFX"))
            {
                source.GetComponent<SFXManager>().sfxVolume = PlayerPrefs.GetFloat("sfxVolume");
            }
        }
    }
}
