using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
    private AudioSource[] audio;
    private bool once = true;
    
    private void Awake()
    {
        particle.gameObject.SetActive(false);
        audio = GetComponents<AudioSource>();
    }
    
    void Update()
    {
        awake |= Rope.IsMaxed;
        if (awake)
        {
            if (once)
            {
                audio[0].Play();
                DOVirtual.DelayedCall(0.5f, () =>
                {
                    audio[1].Play();
                    audio[1].DOFade(0.05f, 2f);
                });
                once = false;
            }
            
            particle.gameObject.SetActive(true);
            transform.position += (Vector3)(MoveDirection * (Speed * Time.deltaTime));
        }
    }
}
