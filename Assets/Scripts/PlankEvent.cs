using System;
using System.Collections;
using Animancer;
using DG.Tweening;
using Settings;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Game
{
   
    public class PlankEvent : MonoBehaviour, IInteractive
    {
        [SerializeField] private Movement movement;
        [SerializeField] private Transform hintSpawnPlace;
        [FormerlySerializedAs("enemy")] [SerializeField] private GameObject plankO;
        [FormerlySerializedAs("pickupAnim")] [SerializeField] private AnimationClip idlePickupAnim;
        public event Action EndEvent;
        private bool activated;
        [SerializeField] private AnimationClip whilePickupAnim;
        [SerializeField] private AnimationClip finishPickupAnim;
        [SerializeField] private Camera cam;
        private Hint hint;
        private IEnumerator CQuickEventTap(bool a)
        {
            Hint hint= Hint.Spawn($"Tap {(a ? "left" : "right")}",hintSpawnPlace.transform.position,inTime:0.1f);
            int val = a ? 0 : 1;
            int count = 0;
            float start = Time.time;
            while (count < 4) 
            {
                if (Input.GetMouseButtonDown(val))
                {
                    count++;
                }
                yield return null;
            }
            hint.FadeOut();

        }
        private IEnumerator CRun()
        {
            
            //movement.enabled = false;
            
            bool success = false;
             hint=Hint.Spawn("Left&Rigth", hintSpawnPlace.position, inTime: 0.1f);
            while (!Input.GetMouseButton(0) || !Input.GetMouseButton(1))
            {
                yield return null;
            }
            movement.Animancer.enabled = true;
            movement.Animancer.Play(idlePickupAnim);
            plankO.GetComponent<SpriteRenderer>().enabled = false;
            hint.FadeOut();
            int counter = 0;
            Hint swipeHint=null;
            while(counter<3)
            {
                if (swipeHint == null)
                    swipeHint = Hint.Spawn("Swipe up", hintSpawnPlace.position);
                while (!Input.GetMouseButtonDown(0))
                {
                    yield return null;
                }

                Vector2 pos = cam.ScreenToWorldPoint( Input.mousePosition);
                float y = 0; 
                while (!Input.GetMouseButtonUp(0))
                {
                    Vector2 curPos = cam.ScreenToWorldPoint(Input.mousePosition);
                    y = (curPos.y - pos.y);
                    print(y);
                    y = Mathf.Clamp(y, 0, 0.25f) * 4f - 0.1f;
                    yield return null;
                }
                if (y >=0.8f)
                {
                    counter++;
                    swipeHint.FadeOut();
                    swipeHint = null;
                    movement.Animancer.Play(whilePickupAnim);
                    yield return new WaitForSeconds(whilePickupAnim.length+0.2f);
                }

                movement.Animancer.Play(idlePickupAnim);


            }

            if (swipeHint != null)
                swipeHint.FadeOut();

            movement.Animancer.Play(finishPickupAnim);
            yield return new WaitForSeconds(finishPickupAnim.length);
            movement.Animancer.enabled = false;
            this.gameObject.SetActive(false);
           // movement.enabled = true;
            EndEvent();
          
        }
       
        public bool Begin(bool already)
        {
            if (activated) return false;
            activated = true;
            StartCoroutine(CRun());
            return true;
        }

        public void InteractiveUpdate(bool left, bool right, Vector2 pos)
        {
        }

        public void Abort()
        {
            if(hint!=null)
                hint.FadeOut(0.1f);
        }
    }
}