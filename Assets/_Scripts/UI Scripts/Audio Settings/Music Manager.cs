using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public float bgmVolume = 1.0f;
    public float pitch = 1f;
    public AudioClip bgm;

    private void Start()
    {
        GetComponent<AudioSource>().volume = bgmVolume;
        GetComponent<AudioSource>().pitch = pitch;
        PlayRepeat(bgm);
    }

    private void Update()
    {
        GetComponent<AudioSource>().volume = bgmVolume;
        GetComponent<AudioSource>().pitch = pitch;
        //Debug.Log("AudioSource.volume = " + GetComponent<AudioSource>().volume);
    }

    void PlayRepeat(AudioClip song)
    {
        GetComponent<AudioSource>().clip = song;
        GetComponent<AudioSource>().loop = true;
        GetComponent<AudioSource>().Play();
    }
}
