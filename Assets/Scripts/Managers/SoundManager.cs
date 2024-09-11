using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    
    [SerializeField] private AudioSource _correctSound;
    [SerializeField] private AudioSource _wrongSound;
    [SerializeField] private AudioSource _gameMusic;
    [SerializeField] private AudioSource _menuMusic;
    [SerializeField] private AudioSource _timerSound;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void Play(AudioSource audioSource)
    {
        audioSource.Play();
    }

    public void Stop(AudioSource audioSource)
    {
        audioSource.Stop();
    }
}
