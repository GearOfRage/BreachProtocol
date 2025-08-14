using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager _instance;

    [SerializeField] private AudioSource soundFXLoopObject;
    private AudioSource audioSource;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public AudioSource PlaySoundFXClipLoop(AudioClip audioClip, Transform spawnTransform, float pitch, float volume, bool loop)
    {
        audioSource = Instantiate(soundFXLoopObject, spawnTransform);

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.loop = loop;
        
        audioSource.Play();

        return audioSource;
    }
    public void PlaySoundFXClipOneShot(AudioClip audioClip, Transform spawnTransform, float pitch, float volume)
    {
        audioSource = Instantiate(soundFXLoopObject, spawnTransform);

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.loop = false;
        
        audioSource.Play();
        
        Destroy(audioSource.gameObject, audioClip.length);
    }
}
