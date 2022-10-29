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
            
            movement.enabled = false;
            
            bool success = false;
            var hint=Hint.Spawn("Left&Rigth", hintSpawnPlace.position, inTime: 0.1f);
            while (!Input.GetMouseButton(0) || !Input.GetMouseButton(1))
            {
                yield return null;
            }
            movement.Animancer.enabled = true;
            movement.Animancer.Play(idlePickupAnim);
            hint.FadeOut();
            yield return CQuickEventTap(true);
            movement.Animancer.Play(whilePickupAnim);
            yield return new WaitForSeconds(whilePickupAnim.length);
            movement.Animancer.Play(finishPickupAnim);
            
            plankO.gameObject.SetActive(false);
            
            DOVirtual.DelayedCall(idlePickupAnim.length, () =>
            {

                movement.Animancer.enabled = false;
                movement.enabled = true;
                EndEvent();
            }).SetLink(movement.gameObject);

        }
       
        public bool Begin()
        {
            if (activated) return false;
            activated = true;
            StartCoroutine(CRun());
            return true;
        }

        public void InteractiveUpdate(bool left, bool right, Vector2 pos)
        {
        }
    }
}