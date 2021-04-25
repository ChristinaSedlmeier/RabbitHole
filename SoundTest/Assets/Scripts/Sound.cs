using UnityEngine.Audio;
using UnityEngine;

[System. Serializable]
public class Sound
{

    public string name; 

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;





    public KeyFrame[] keyFrames;



    public AudioSource source;



}

[System.Serializable]
public class KeyFrame
{
    public int frame;
    public float sec;
    public float delay;

}