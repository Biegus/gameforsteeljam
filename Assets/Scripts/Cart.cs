using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cart : MonoBehaviour
{
    [SerializeField]
    private Vector2 MoveDirection = Vector2.right;

    [SerializeField] 
    private float Speed = 1;

    [SerializeField] private float startAfter = 2f;
    private float startTime;
    [SerializeField] public GameObject particle;
    private void Awake()
    {
        startTime = Time.time;
        
        particle.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Time.time - startTime > startAfter)
        {
            particle.gameObject.SetActive(true);
            transform.position += (Vector3)(MoveDirection * (Speed * Time.deltaTime));
        }
    }
}
