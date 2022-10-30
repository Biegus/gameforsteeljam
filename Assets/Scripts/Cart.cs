using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class Cart : MonoBehaviour
{
    [SerializeField]
    private Vector2 MoveDirection = Vector2.right;

    
    [SerializeField] 
    private Rope Rope;

    [SerializeField] public GameObject particle;

    private bool awake = false;
    private AudioSource[] audio;
    private bool once = true;
    public Vector2 Gravity=Vector2.down;
   public Rigidbody2D Rigidbody { get;private set; }
   private float gravLenght;

   private float prevIntensity;
   private Light2D light;
   [FormerlySerializedAs("curve")] [SerializeField] private AnimationCurve speed;
   private float startX = 0;
   [SerializeField] private Transform gizmosDown, gizmosUp;
   [SerializeField] private Transform gizmosEnd;
    private void Awake()
    {
        particle.gameObject.SetActive(false);
        audio = GetComponents<AudioSource>();
        Rigidbody = this.GetComponent<Rigidbody2D>();
        Rigidbody.gravityScale = 0;
        gravLenght = Physics2D.gravity.magnitude;

        light = GetComponentInChildren<Light2D>();
        prevIntensity = light.intensity;
        light.intensity = 0;
    }

    private void Start()
    {
        startX = this.transform.position.x;
    }

    private void OnDrawGizmos()
    {
        if (!gizmosDown || !gizmosUp) return;
        
        float startX = this.transform.position.x;
        if (Application.isPlaying)
            startX = this.startX;
        float max = speed.keys.Max(item => item.value);

        Vector2 Get(float x)
        {
            return new Vector2(x + startX,
                speed.Evaluate(x) * (gizmosUp.position.y - gizmosDown.position.y) + gizmosDown.position.y);
        }
        for (int i = 0; i <= 100; i++)
        {
            float x = startX + (gizmosEnd.position.x - startX) * i / ((float) 100);
            float x2 = startX + (gizmosEnd.position.x - startX) * (i+1) / ((float) 100);
            Gizmos.DrawLine(Get(x), Get(x2));
        } 
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

                GetComponentInChildren<Light2D>().intensity = prevIntensity;
                
                once = false;
            }
            
            particle.gameObject.SetActive(true);
            transform.position += (Vector3)(MoveDirection * (speed.Evaluate((this.transform.position.x-startX)) * Time.deltaTime));
        }
    }

    private void FixedUpdate()
    {
        this.Rigidbody.velocity += Gravity * gravLenght* Time.fixedDeltaTime ;
    }
}
