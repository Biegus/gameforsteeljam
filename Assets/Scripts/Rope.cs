using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Settings;
using UnityEngine;
using UnityEngine.Serialization;

public class Rope : MonoBehaviour
{

    [SerializeField] private Transform visualB;
    [SerializeField] 
    private Transform EndA;
    
    [SerializeField] 
    private Transform EndB;
    
    [SerializeField] 
    private float Length;

    [SerializeField] private Sprite lineSprite;
    
    [FormerlySerializedAs("Points")] [SerializeField] 
    private int Link;

    [SerializeField] private Transform floor;
    public bool IsMaxed { get; private set; }
    
    private float ApplySag(float x, float distance)
    {
        float l = Length;
        float u = Mathf.Min(Length-0.001f,distance) ;
        x =  Mathf.Max(0,Mathf.Min(x, distance - 0.01f));
        float q = -Mathf.Sqrt(Mathf.Pow(l / 2, 2) - Mathf.Pow(u / 2, 2));
        float a = -q / Mathf.Pow(u / 2, 2);
        return a * x * (x - u);
    }

    private LineRenderer _lineRenderer;
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = Link+1;
    }
    
    void Update()
    {
        float dist = Vector2.Distance(EndA.position, EndB.position);

        if (dist > Length)
        {
            IsMaxed = true;
            Vector2 dir = (EndB.position - EndA.position).normalized;

            EndB.position = dir * Length + (Vector2)EndA.position;
        }
        else
        {
              IsMaxed = false;
        }
        if (dist > Length || Mathf.Abs(EndA.position.y - EndB.position.y) > 0.5f)
        { 
            Vector3[] points = { EndA.position, visualB.position };
            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPositions(points);
        }
        else
        {
          
            dist = Vector2.Distance(EndA.position, EndB.position);
            
            Vector3[] points = new Vector3[Link+1];
            for (int i = 0; i < Link; i++)
            {
                float x = dist* ((float)i / Link);
                float y = Mathf.Max(floor.transform.position.y- EndA.position.y, ApplySag(x,dist));
                points[i] = new Vector2(-x*Mathf.Sign(this.EndA.position.x-this.visualB.position.x), y) + (Vector2)EndA.position;
            }

            points[Link] = visualB.position;

            _lineRenderer.positionCount = Link+1;
            _lineRenderer.SetPositions(points); 
        }
    }
}
