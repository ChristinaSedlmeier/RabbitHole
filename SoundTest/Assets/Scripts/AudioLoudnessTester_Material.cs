using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AudioLoudnessTester_Material : MonoBehaviour
{

    public AudioSource audioSource;
    public float updateStep = 0.01f;
    public int sampleDataLength = 1024;

    private float currentUpdateTime = 0f;

    public float clipLoudness;
    private float[] clipSampleData;

    public float sizeFactor = 1;

    public float minSize = -2.5f;
    public float maxSize = 0.0f;

    private void Awake()

    {
        clipSampleData = new float[sampleDataLength];
    }

    private void Update()

    {
        currentUpdateTime += Time.deltaTime;
        if (currentUpdateTime >= updateStep)

        {
            currentUpdateTime = 0f;
            audioSource.clip.GetData(clipSampleData, audioSource.timeSamples);
            clipLoudness = 0f;

            foreach (var sample in clipSampleData)
             {
             clipLoudness += Mathf.Abs(sample);
             }
            clipLoudness /= sampleDataLength;

            clipLoudness *= sizeFactor;
            clipLoudness = Mathf.Clamp(clipLoudness, minSize, maxSize);

            UnityEngine.Material mat = Resources.Load("Light", typeof(Material)) as Material;
            //(Material)AssetDatabase.LoadAssetAtPath("Assets/Resources/Light.mat", typeof(Material));
            Color color = mat.GetColor("_Color");

            //float adjustedIntensity = intensiy * clipLoudness;

            //color *= Mathf.Pow(2.0F, adjustedIntensity);

            color *= clipLoudness;
            mat.SetColor("_EmissionColor", color);
            //Debug.Log("<b>Set custom emission intensity of " + intensity + "(" + adjustedIntensity + "internally) on Material </b>" + mat);
        

        }
    }

}