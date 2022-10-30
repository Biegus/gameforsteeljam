using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game;
using Settings;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillTrigger : MonoBehaviour
{
    [SerializeField] 
    private string SceneName;

    [SerializeField] 
    private List<string> TriggeringTags;

    [SerializeField] 
    private GameObject fadeSprite;
        
    [SerializeField] 
    private float fadeDuration;

    [SerializeField] 
    private AudioClip sound;


    private bool Lock;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (TriggeringTags.Contains(col.tag) && !Lock)
        {
            Lock = true;
            var audio = GetComponent<AudioSource>();
            if (audio)
            {
                audio.clip = sound;
                audio.Play();
            }
            SpriteRenderer sprite = GameObject.FindWithTag("GameOver").GetComponent<SpriteRenderer>();
          
                GameManager.Instance.Die(fadeDuration);

            

        
        }
        
    }
}
