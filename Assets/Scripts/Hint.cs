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
        private bool done = false;
        public static Hint Spawn(string text, Vector2 pos,float? despawnTime=null,Color? color=null,float? inTime=null)
        {
            var instance = Instantiate(Prefab.Value);
            instance.transform.position = pos;
            instance.Init(text,color?? Color.white,inTime??1);
            if (despawnTime != null)
            {
                DOVirtual.DelayedCall(despawnTime.Value+1, () => instance.FadeOut());
            }
            return instance;
        }
        private void Init(string text,Color color, float inTime)
        {
            this.textEnitity.text = text;
            this.textEnitity.color = new Color(this.textEnitity.color.r, this.textEnitity.color.g,
                this.textEnitity.color.b, 0);
            this.textEnitity.DOFade(1, inTime)
                .SetLink(this.gameObject);
            this.textEnitity.color = color;
        }

        public void FadeOut()
        {
            if (done) return;
            done = true;
            this.textEnitity.DOFade(0, 0.6f).SetLink(this.gameObject)
                .OnComplete(() => Destroy(this.gameObject));
        }
        


    }
}