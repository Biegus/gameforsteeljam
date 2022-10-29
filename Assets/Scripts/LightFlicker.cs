using System;
using System.Collections;
using System.Collections.Generic;
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
    
    private void Start()
    {
        light = GetComponent<Light2D>();
    }

    private void Update()
    {
        float noiseVal = 0;
        float current = 1;
        for (int i = 0; i < Octaves; i++)
        {
            noiseVal += Mathf.PerlinNoise(Time.time * Frequency * Mathf.Pow(2,i),transform.position.x) * current;
            current *= Persistence;
        }
        
        light.intensity = noiseVal * Amplitude + Offset;
    }
}
