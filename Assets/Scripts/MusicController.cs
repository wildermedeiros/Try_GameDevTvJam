using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] AudioClip[] backgroundMusic;

    AudioSource audioSource;

    float timePlaying;
    
    private void Awake() 
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        PlayBackgroundMusic();
    }

    private void PlayBackgroundMusic()
    {
        int index = Random.Range(0, backgroundMusic.Length);
        audioSource.clip = backgroundMusic[index];
        audioSource.Play();
    }

    private void Update()
    {
        timePlaying += Time.deltaTime;

        if (timePlaying > audioSource.clip.length)
        {
            PlayBackgroundMusic();
            timePlaying = 0f;
        }
    }
}
