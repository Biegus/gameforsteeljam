using System;
using System.Collections;
using Animancer;
using Settings;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
   
    public class PlankEvent : MonoBehaviour
    {
        [SerializeField] private Movement movement;
        [SerializeField] private Transform hintSpawnPlace;
        [SerializeField] private GameObject enemy;
        [SerializeField] private AnimationClip pickupAnim;
        
        private IEnumerator CQuickEventTap(bool a,Action onSuccess)
        {
            Hint hint= Hint.Spawn($"Tap {(a ? "left" : "right")}",hintSpawnPlace.transform.position,inTime:0.1f);
            int val = a ? 0 : 1;
            int count = 0;
            float start = Time.time;
            while (Time.time - start < 0.1f)
            {
                if (Input.GetMouseButtonDown(val))
                {
                    count++;
                }

                yield return null;
            }

            if (count > 4)
            {
                onSuccess();
            }
            
        }
        private IEnumerator CRun()
        {
            movement.enabled = false;
            
            bool success = false;
            Hint.Spawn("Left&Rigth", hintSpawnPlace.position, inTime: 0.1f);
            while (!Input.GetMouseButton(0) || !Input.GetMouseButton(1))
            {
                yield return null;
            }

            movement.Animancer.Play(pickupAnim);
           
            
            movement.enabled = true;
        }

        public event Action EndEvent;
        private bool activated;
        public bool Begin()
        {
            if (activated) return false;
            activated = true;
            return true;
        }

        public void InteractiveUpdate(bool left, bool right, Vector2 pos)
        {
        }
    }
}