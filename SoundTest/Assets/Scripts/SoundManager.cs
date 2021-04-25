using UnityEngine.Video;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using System;

public  class SoundManager : MonoBehaviour
{


    public Sound[] sounds;

    public AudioSource[] videos;
    private float frameNumber;

    public VideoPlayer[] player;

    void Awake ()
    {

        foreach (Sound s in sounds)
        {
            
            //s.source.clip = s.clip;
            //s.source.spatialBlend = 1f;
            //s.source.rolloffMode = AudioRolloffMode.Logarithmic;
            //s.source.minDistance = 1;
            //s.source.maxDistance = 40;
            //s.keyFrames = s.keyFrames;
           // s.source.volume = s.volume;
           
        }
    }

    public void Play(string name, AudioSource source) 
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source = source;
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + "not found");
            return;
        }

        s.source.Play();
    }


    public void PlayKey(string name)
    {

        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound " + name + "not found");
            //yield return null;
        }

        float currentTime = s.source.time;

        Debug.Log(currentTime + "is current time ");

        if(currentTime >= s.keyFrames[0].delay && currentTime < s.keyFrames[s.keyFrames.Length-1].delay)
        {

            KeyFrame f = Array.Find(s.keyFrames, fr => (fr.sec <= currentTime) && (fr.delay > currentTime));

            StartCoroutine(PlayDelayed(currentTime, f.delay, s));
        }




       
    }

    private IEnumerator PlayDelayed(float sec, float delay, Sound s)
    {
        float delaySound = delay - sec + 1 ;
        Debug.Log(delaySound + " is delay ");

        yield return new WaitForSeconds(delaySound);
        s.source.Stop();
        s.source.time = 0;
        s.source.Play();
    }


    void Start ()
    {

    }

    public void muteVideos()
    {


        StartCoroutine(UpdateFadeOut());


    }

    private IEnumerator UpdateFadeOut()
    {

        float currentTime = 0;
        float start = videos[0].volume;
        float duration = 1f;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            foreach(AudioSource v in videos)
            {
                v.volume = Mathf.Lerp(start, 0.01f, currentTime / duration);
            }
            foreach (VideoPlayer p in player)
            {
                p.playbackSpeed = Mathf.Lerp(1, 0.4f, currentTime / duration);

            }
            yield return null;
        }
    }

    private IEnumerator UpdateFadeIn()
    {
        float currentTime = 0;
        float start = videos[0].volume;
        float duration = 0.5f;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            foreach (AudioSource v in videos)
            {
                v.volume = Mathf.Lerp(start, 1f, currentTime / duration);
                v.pitch = 1f;
            }
            foreach (VideoPlayer p in player)
            {
                p.playbackSpeed = Mathf.Lerp(0.4f, 1f, currentTime / duration);
                
            }

            yield return null;
        }
    }




    public void unMuteVideos()
    {

        StartCoroutine(UpdateFadeIn());

    }
}
