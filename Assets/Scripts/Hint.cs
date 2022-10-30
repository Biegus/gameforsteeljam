using System;
using System.Collections;
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
        private Tween effectTween;
        public static Hint Spawn(string text, Vector2 pos,float? despawnTime=null,Color? color=null,float? inTime=null)
        {
            var instance = Instantiate(Prefab.Value);
            instance.transform.position = pos;
            instance.Init(text,color?? Color.white,inTime??1);
            if (despawnTime != null)
            {
                DOVirtual.DelayedCall(despawnTime.Value + 1, () => instance.FadeOut()).SetLink(instance.gameObject);
            }
            return instance;
        }
        private void Init(string text,Color color, float inTime)
        {
            this.textEnitity.text = text;
            this.textEnitity.color = color;
            this.textEnitity.color = new Color(this.textEnitity.color.r, this.textEnitity.color.g,
                this.textEnitity.color.b, 0);
            
            this.textEnitity.DOFade(1, inTime)
                .SetLink(this.gameObject)
                .OnComplete(() =>
                {
                  
                    Vector2 basePos = this.transform.position;
                    StartCoroutine(Cor());     
                    IEnumerator Cor()
                    {
                        Vector2[] dirs = new Vector2[] {Vector2.left, Vector2.right, Vector2.up, Vector2.down};
                        for (int i = 0;; i++)
                        {
                            Vector2 rand = dirs[UnityEngine.Random.Range(0, dirs.Length)];
                            this.transform.position = basePos + rand * 0.05f;
                            yield return new WaitForSeconds(0.1f - Mathf.Clamp(i/30f,0,1)*0.08f);
                            this.transform.position = basePos;


                        }
                    }
                })
                ;//.OnComplete(() => effectTween= this.textEnitity.transform.DOScale(Vector3.one * 3f,15 ).SetLink(this.gameObject));
        }

        public void FadeOut(float? time=null)
        {
            if (done) return;
            done = true;
            effectTween?.Kill();
            this.textEnitity.DOFade(0, time??0.6f).SetLink(this.gameObject)
                .OnComplete(() => Destroy(this.gameObject));
        }
        


    }
}