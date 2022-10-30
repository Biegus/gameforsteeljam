using System;
using Game;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Game
{
    public abstract class HintableInteractive : MonoBehaviour, IInteractive
    {
        public event Action EndEvent;
        [SerializeField] private UnityEvent onFinished;
        private Hint hint;
        [SerializeField] private string hintText;
 public bool Used { get; private set; }= false;
        [SerializeField] private Transform hintPoint;

        public Transform HintPoint => hintPoint;
        public virtual bool Begin(bool already)
        {
            if (Used) return false;
            hint=Hint.Spawn(hintText,hintPoint.position);
            Used = true;
            return true;
        }

        protected void Finish()
        {
            EndEvent?.Invoke();
                
            onFinished?.Invoke();
            hint.FadeOut();
        }

        public abstract void InteractiveUpdate(bool left, bool right, Vector2 pos);
        public void Abort()
        {
            
        }
    }
}