using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundSoundPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip bgm;

    private Collider collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<MeshCollider>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = bgm;
        audioSource.volume = 0.8f;
        audioSource.loop = false;
        this.enabled = false;
    }


    public void MusicStart()
    {
        audioSource.Play();
    }
    
    

}
