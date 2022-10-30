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

    [SerializeField] private bool soft;

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
            if (!soft)
            {
                GameManager.Instance.Die(fadeDuration);

            }
            else FindObjectOfType<Movement>().GoToSleep();

            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);
            var tut=FindObjectOfType<LegTutorial>();
            tut.Despawn(); 
            
            DOVirtual.DelayedCall(3f, () =>
            {
                sprite.DOFade(0, 1).SetLink(this.gameObject);


            }).SetLink(this.gameObject);
        }
        
    }
}
