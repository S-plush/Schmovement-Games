using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public float sfxVolume = 1.0f;

    public AudioClip explosionSpellSound;
    public AudioClip iceicleSpearSpellSound;
    public AudioClip soundWaveSpellSound;
    public AudioClip lightningSpellSound;

    public GameObject runSource;

    private AudioSource audio;

    public void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.volume = sfxVolume;
    }

    private void Update()
    {
        GetComponent<AudioSource>().volume = sfxVolume;
    }

    public void ExplosionSpellSFX()
    {
        audio.clip = explosionSpellSound;
        audio.Play();
    }

    public void IceicleSpearSpellSFX()
    {
        audio.clip = iceicleSpearSpellSound;
        audio.Play();
    }

    public void SoundWaveSpellSFX()
    {
        audio.clip = soundWaveSpellSound;
        audio.Play();
    }

    public void LightningSpellSFX()
    {
        audio.clip = lightningSpellSound;
        audio.Play();
    }
}
