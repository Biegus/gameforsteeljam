using System;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class Lever : HintableInteractive
    {
        private Vector2 start;
        [SerializeField] private Vector2 desiredDirection=Vector2.down;
        [SerializeField] private float minLen=1;
        private bool active = false;
        [SerializeField] private Collider2D toEnable;
        [SerializeField] private AnimationClip clip;
        
        public override void InteractiveUpdate(bool left, bool right, Vector2 pos)
        {
             
            if (left)
            {
                if (!active)
                {
                    active = true;
                    start = pos;
                }

                float x = start.y - pos.y;
                
                float progress = Mathf.Clamp(x / minLen, 0, 1);
                clip.SampleAnimation(this.gameObject,progress * clip.length);
                
            }
            else if (active)
            {
                if (start.y - pos.y >= minLen-0.1)
                {
                    Finish();
                    
                }
                else
                {
                    active = false;
                     clip.SampleAnimation(this.gameObject,0);       
                }
             
                    
            }
            
                
        }

     
    }
}