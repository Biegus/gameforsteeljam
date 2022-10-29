using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class StartFadeOut : MonoBehaviour
{
    [SerializeField] private float Duration;
    IEnumerator Start()
    {
        var sprite = this.GetComponent<SpriteRenderer>();
        yield return new WaitForSeconds(0.5f);
        sprite.DOFade(0, Duration)
            .SetLink(sprite.gameObject);

    }
}
