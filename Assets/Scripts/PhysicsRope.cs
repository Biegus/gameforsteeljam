using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsRope : MonoBehaviour
{
    [SerializeField] 
    private Transform EndA;
    
    [SerializeField] 
    private Transform EndB;
    
    [SerializeField] 
    private float Length;
    
    [SerializeField] 
    private int Segments;

    [SerializeField] 
    private GameObject Segment;

    private List<GameObject> Generated = new List<GameObject>();

    public bool IsMaxed { get; private set; }
        
        
    void Start()
    {
        float xPos = EndA.position.x;
        float yPos = EndA.position.y;
        Rigidbody2D previousBody = EndA.GetComponent<Rigidbody2D>();
        for (int i = 0; i < Segments; i++)
        {
            var seg = Instantiate(Segment, this.transform);
            seg.layer = 9; // rope
            
            seg.transform.position = new Vector2(xPos, yPos);
            seg.transform.localScale = new Vector2(Length / Segments + 0.02f, seg.transform.localScale.y);
            seg.GetComponent<HingeJoint2D>().connectedBody = previousBody;
            
            previousBody = seg.GetComponent<Rigidbody2D>();
            Generated.Add(seg);

            xPos -= Length / Segments;
        }
        EndB.GetComponent<HingeJoint2D>().connectedBody = previousBody.GetComponent<Rigidbody2D>();
        EndA.GetComponent<HingeJoint2D>().connectedBody = Generated[0].GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        if (Vector2.Distance(EndA.position, EndB.position) > Length)
        {
            Vector2 dir = (EndB.position - EndA.position).normalized;

            EndB.position = dir * Length + (Vector2) EndA.position;
            IsMaxed = true;

        }
        else IsMaxed = false;
    }
}
