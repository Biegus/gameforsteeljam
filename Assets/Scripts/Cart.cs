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
    
    [SerializeField] 
    private Rope Rope;

    [SerializeField] public GameObject particle;

    private bool awake = false;
    
    private void Awake()
    {
        particle.gameObject.SetActive(false);
    }
    
    void Update()
    {
        awake |= Rope.IsMaxed;
        if (awake)
        {
            particle.gameObject.SetActive(true);
            transform.position += (Vector3)(MoveDirection * (Speed * Time.deltaTime));
        }
    }
}
