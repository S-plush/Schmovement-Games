using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioConfig : MonoBehaviour
{
    public void SetDefaults()
    {
        SetAudioType("Stereo");
    }

    public void SetAll()
    {
        SetAudioType(PlayerPrefs.GetString("AudioType"));

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

    public void SetAudioType(string speakerMode)
    {
        switch (speakerMode)
        {
            case "Mono":
                AudioSettings.speakerMode = AudioSpeakerMode.Mono;
                break;
            case "Stereo":
                AudioSettings.speakerMode = AudioSpeakerMode.Stereo;
                break;
            case "Surround":
                AudioSettings.speakerMode = AudioSpeakerMode.Surround;
                break;
            case "Surround 5.1":
                AudioSettings.speakerMode = AudioSpeakerMode.Mode5point1;
                break;
            case "Surround 7.1":
                AudioSettings.speakerMode = AudioSpeakerMode.Mode7point1;
                break;
        }
    }
}
