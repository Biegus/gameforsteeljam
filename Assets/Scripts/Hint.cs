using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game
{
    public class Hint : MonoBehaviour
    {
        public static Lazy<Hint> Prefab = new Lazy<Hint>(()=> Resources.Load<Hint>("prefabs/hint"));
        [FormerlySerializedAs("text")] [SerializeField] private TMP_Text textEnitity;

        public static Hint Spawn(string text, Vector2 pos)
        {
            var instance = Instantiate(Prefab.Value);
            instance.transform.position = pos;
            instance.Init(text);
            return instance;
        }
        private void Init(string text)
        {
            this.textEnitity.text = text;
            this.textEnitity.color = new Color(this.textEnitity.color.r, this.textEnitity.color.g,
                this.textEnitity.color.b, 0);
            this.textEnitity.DOFade(1, 1)
                .SetLink(this.gameObject);
        }

        public void FadeOut()
        {
            this.textEnitity.DOFade(0, 1);
        }
        


    }
}