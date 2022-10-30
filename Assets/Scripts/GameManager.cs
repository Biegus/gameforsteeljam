using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    public class GameManager: MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        [SerializeField] private SpriteRenderer spriteRenderer; 
        
        [SerializeField] 
        private float defFadeDuration=0.5f;
        private void Awake()
        {
            Instance = this;
        }

        public void Die(float? fadeDuration=null)
        {
            spriteRenderer.DOFade(1, fadeDuration ?? defFadeDuration
                )
                .SetLink(spriteRenderer.gameObject)
                .OnComplete(() =>
                {
                    DOVirtual.DelayedCall(1f, () =>
                    {
                        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    }).SetLink(this.gameObject);
                });
        }
    }
}