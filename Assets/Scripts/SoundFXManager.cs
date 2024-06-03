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

    public void PlaySoundFXClipLoop(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        audioSource = Instantiate(soundFXLoopObject, spawnTransform);

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.pitch = 2f;
        
        audioSource.Play();
    }

    public void ImmediateStopSoundFXClipLoop()
    {
        Destroy(audioSource.gameObject);
    }
}
