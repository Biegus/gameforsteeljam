using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (TriggeringTags.Contains(col.tag))
        {
            SpriteRenderer sprite = fadeSprite.GetComponent<SpriteRenderer>();
            sprite.DOFade(1, fadeDuration)
                .SetLink(sprite.gameObject)
                .OnComplete(() =>
                {
                    DOVirtual.DelayedCall(1f, () =>
                    {
                        SceneManager.LoadScene(SceneName);
                    }).SetLink(this.gameObject);
                });
        }
        
    }
}
