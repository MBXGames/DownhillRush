using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioModuleController : MonoBehaviour
{
    private bool paused;
    private float initPitch;
    private float trueinitPitch;
    private AudioSource aud;
    public AudioClip[] audioClips;
    public float pitchVariation;
    public bool stopWithPause;

    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponent<AudioSource>();
        if (aud == null)
        {
            return;
        }
        initPitch = aud.pitch;
        if (audioClips.Length == 1)
        {
            aud.clip = audioClips[0];
        }
        trueinitPitch=initPitch;
    }

    private void Update()
    {
        if (aud == null)
        {
            return;
        }
        if (stopWithPause && Time.timeScale < 1)
        {
            if (Time.timeScale == 0)
            {
                Pause();
            }
            else
            {
                UnPause();
                initPitch =trueinitPitch*Time.timeScale;
            }
        }
        else if(initPitch != trueinitPitch)
        {
            initPitch = trueinitPitch;
        }
    }

    // Update is called once per frame
    public void Play()
    {
        if (aud == null)
        {
            return;
        }
        UnPause();
        aud.pitch=initPitch+Random.Range(-pitchVariation, pitchVariation);
        if(audioClips.Length>1)
        {
            aud.clip = audioClips[Random.Range(0, audioClips.Length)];
        }
        aud.Play();
    }

    public void Stop()
    {
        if(aud==null)
        { 
            return;
        }
        if (aud.isPlaying)
        {
            aud.Stop();
        }
    }

    public void Pause()
    {
        if (aud == null)
        {
            return;
        }
        paused =true;
        if (aud.isPlaying)
        {
            aud.Pause();
        }
    }
    public void UnPause()
    {
        if (aud == null)
        {
            return;
        }
        if (paused)
        {
            paused=false;
            aud.UnPause();
        }
    }

    public bool IsPlaying()
    {
        if (aud == null)
        {
            return false;
        }
        return aud.isPlaying;
    }
}
