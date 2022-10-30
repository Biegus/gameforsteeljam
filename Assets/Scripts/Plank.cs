using System;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class Plank : MonoBehaviour
    {
      
        [SerializeField] private Collider2D toActivate;

        [SerializeField] private AudioClip[] clips; 
        
        [SerializeField] private AnimationCurve curve; 
        
        private AudioSource audio;

        public void Start()
        {
            audio = GetComponent<AudioSource>();
        }

        public void Let()
        {
            audio.clip = clips[0];
            audio.Play();
            
            toActivate.gameObject.SetActive(true);
            this.transform.DORotate(new Vector3(0, 0, 90), 1)
                .SetEase(curve)
                .OnComplete(() =>
                {
                    audio.clip = clips[1];
                    audio.Play();
                });
            
        }
    }
}