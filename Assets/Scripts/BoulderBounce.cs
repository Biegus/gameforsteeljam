using System;
using System.Collections;
using System.Collections.Generic;
using Honey;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoulderBounce : MonoBehaviour
{
    [Layer]
    [SerializeField]
    private int layer;

    [SerializeField] 
    private float BounceStrength = 2;

    [SerializeField] private float RandomRotate = 20;
        
    [SerializeField]
    private AudioClip BounceSound;
    
    private bool bounced = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Collider2D>() && !bounced) //not work
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = BounceSound;
            audio.Play();
            
            bounced = true;
            var rbody = GetComponent<Rigidbody2D>();
            rbody.velocity = Vector2.up * BounceStrength;
            gameObject.layer = layer; // magic
            rbody.angularVelocity = Random.Range(-RandomRotate, RandomRotate);
        }
    }
}
