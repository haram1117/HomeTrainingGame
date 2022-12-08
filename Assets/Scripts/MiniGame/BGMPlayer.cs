using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip bgm;

    private bool _activated;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = bgm;
        audioSource.volume = 0.5f;
        audioSource.loop = true;
        audioSource.Play();
    }
}
