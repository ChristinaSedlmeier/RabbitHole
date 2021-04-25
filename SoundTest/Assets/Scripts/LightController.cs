using UnityEngine;
using UnityEngine.Video;
using UnityEngine.EventSystems;


public class LightController : MonoBehaviour

{

    public AudioSource audioSourceHeartBeat;
    public AudioSource audioSourceInterview;

    public float updateStep = 0.01f;
    public int sampleDataLength = 1024;

    private float currentUpdateTime = 0f;

    public float clipLoudness;
    private float[] clipSampleData;

    private bool blinken = false;

    public float sizeFactor = 1;

    public float minSize = 0;
    public float maxSize = 500;
    UnityEngine.Light mylight;

    Material lightOn;
    Material lightOff;

    string audioName;



    void Start()
    {
        Cursor.visible = false;
        lightOn = Resources.Load("LightOn", typeof(Material)) as Material;
        lightOff = Resources.Load("Light", typeof(Material)) as Material;

        
        mylight = GetComponent<UnityEngine.Light>();
        mylight.intensity = 1f;
        FindObjectOfType<SoundManager>().Play(audioName, audioSourceInterview);

    }

    private void Awake()

    {
        audioName = GetComponent<AudioSource>().clip.name;
        clipSampleData = new float[sampleDataLength];
    }


    void OnTriggerEnter(Collider collider)
    {
        Debug.Log(collider);
        if (collider.tag == "Player")
        {
            GetComponent<Renderer>().material = lightOn;
            GameObject.Find("SoundManager").GetComponent<SoundManager>().muteVideos();
            Debug.Log("play Heartbeat");
            audioSourceHeartBeat.Play();
            blinken = true;
            mylight.intensity = 1f;
            FindObjectOfType<SoundManager>().PlayKey(audioName);
            //mylight.intensity = clipLoudness;
        }

    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            GetComponent<Renderer>().material = lightOff;
            GameObject.Find("SoundManager").GetComponent<SoundManager>().unMuteVideos();
            audioSourceHeartBeat.Stop();
            blinken = false;
            mylight.intensity = 1f;
        }
    }


    void Update()
    {
        currentUpdateTime += Time.deltaTime;
        if (currentUpdateTime >= updateStep)

        {
            currentUpdateTime = 0f;
            audioSourceHeartBeat.clip.GetData(clipSampleData, audioSourceHeartBeat.timeSamples);
            clipLoudness = 0f;

            foreach (var sample in clipSampleData)
            {
                clipLoudness += Mathf.Abs(sample);
            }
            clipLoudness /= sampleDataLength;

            clipLoudness *= sizeFactor;
            clipLoudness = Mathf.Clamp(clipLoudness, minSize, maxSize);
            
            if  (blinken == true)
            {
                mylight.intensity = clipLoudness;
            }


        }

    }

}
