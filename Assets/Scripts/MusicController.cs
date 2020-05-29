using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] AudioClip[] backgroundMusic;
    
    void Start()
    {
        int index = Random.Range(0, backgroundMusic.Length);        
        GetComponent<AudioSource>().PlayOneShot(backgroundMusic[index]);
    }
}
