using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{

    [SerializeField] 
    private Transform EndA;
    
    [SerializeField] 
    private Transform EndB;
    
    [SerializeField] 
    private float Length;
    
    [SerializeField] 
    private int Points;

    private float ApplySag(float x, float distance)
    {
        float l = Length;
        float u = distance;

        float q = -Mathf.Sqrt(Mathf.Pow(l / 2, 2) - Mathf.Pow(u / 2, 2));
        float a = -q / Mathf.Pow(u / 2, 2);
        return a * x * (x - u);
    }

    private LineRenderer _lineRenderer;
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = Points;
    }
    
    void Update()
    {
        float dist = Vector2.Distance(EndA.position, EndB.position);
        
        //Vector3[] points = { EndA.position, EndB.position };

        if (Vector2.Distance(EndA.position, EndB.position) > Length)
        {
            Vector2 dir = (EndB.position - EndA.position).normalized;

            EndB.position = dir * Length + (Vector2)EndA.position;
        }
        
        dist = Vector2.Distance(EndA.position, EndB.position);
        
        Vector3[] points = new Vector3[Points];
        
        for (int i = 0; i < Points; i++)
        {
            float x = Mathf.Lerp(0, dist, (float)i / Points);
            float y = ApplySag(Mathf.Min(x,dist), dist);
            points[i] = new Vector2(x, y) + (Vector2)EndB.position;
        }

        _lineRenderer.SetPositions(points); 
    }
}
