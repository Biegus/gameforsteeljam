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
    private AudioSource audio;
    private bool once = true;
    
    private void Awake()
    {
        particle.gameObject.SetActive(false);
        audio = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        awake |= Rope.IsMaxed;
        if (awake)
        {
            if (once)
            {
                audio.Play();
                once = false;
            }
            
            particle.gameObject.SetActive(true);
            transform.position += (Vector3)(MoveDirection * (Speed * Time.deltaTime));
        }
    }
}
