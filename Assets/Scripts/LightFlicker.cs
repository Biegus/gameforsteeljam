using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFlicker : MonoBehaviour
{
    [SerializeField] private float Frequency;
    [SerializeField] private float Amplitude;
    [SerializeField] private float Offset;
    [SerializeField] private float Octaves;
    [SerializeField] private float Persistence;
    
    private Light2D light;
    private Perlin noise;
    
    private void Start()
    {
        light = GetComponent<Light2D>();
        noise = new Perlin();
        noise.SetSeed((int)transform.position.x);
    }

    private void Update()
    {
        float noiseVal = 0;
        float current = 1;
        for (int i = 0; i < Octaves; i++)
        {
            noiseVal += noise.Noise(Time.time * Frequency * Mathf.Pow(2,i)) * current;
            current *= Persistence;
        }
        
        light.intensity = noiseVal * Amplitude + Offset;
    }
}
