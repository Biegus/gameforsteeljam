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



    private LineRenderer _lineRenderer;
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    
    void Update()
    {
        /*
        float sag = Length - Vector2.Distance(EndA.position, EndB.position);

        Vector2[] points = new Vector2[Points];
        
        for (int i = 0; i < Points; i++)
        {
            
        } //-sag(x - a) (x - b)
        */

        Vector3[] points = { EndA.position, EndB.position };

        if (Vector2.Distance(EndA.position, EndB.position) > Length)
        {
            Vector2 dir = (EndB.position - EndA.position).normalized;

            EndB.position = dir * Length + (Vector2)EndA.position;
        }
        
        _lineRenderer.SetPositions(points); 
    }
}
