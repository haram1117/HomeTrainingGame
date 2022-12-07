using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSoundPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip bgm;

    private bool _activated;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = bgm;
        audioSource.volume = 0.8f;
        audioSource.loop = true;
        this.enabled = false;
    }

    public void MusicStart()
    {
        audioSource.mute = false;
    }

    private void MusicEnd()
    {
        audioSource.mute = true;
    }
    // Update is called once per frame
    void Update()
    {
        if(this.enabled != _activated && this.enabled)
        {
            print("activated");
            audioSource.Play();
            MusicStart();
        }
        if (this.enabled != _activated && this._activated)
        {
            print("disActivated");
            MusicEnd();
        }
        _activated = this.enabled;
    }
}
