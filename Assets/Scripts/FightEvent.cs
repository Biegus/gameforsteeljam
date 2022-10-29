using System;
using System.Collections;
using Animancer;
using Settings;
using UnityEngine;

namespace Game
{
   
    public class FightEvent
    {
        [SerializeField] private AnimationClip animAtack;
        [SerializeField] private AnimationClip enemyAnimAttack;
        [SerializeField] private Movement movement;
        [SerializeField] private Transform hintSpawnPlace;
        [SerializeField] private AnimancerComponent enemy;
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
            yield return CQuickEventTap(false, () => success = true);
            if (!success)
            {
                enemy.Play(enemyAnimAttack);
                yield return new WaitForSeconds(enemyAnimAttack.length);
                movement.enabled = true;
                movement.PlayDeathAnim();
                yield break;
            }


            movement.enabled = true;
        }
    }
}