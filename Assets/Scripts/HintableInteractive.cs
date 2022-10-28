using System;
using Game;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public abstract class HintableInteractive : MonoBehaviour, IInteractive
    {
        public event Action EndEvent;
        [SerializeField] private UnityEvent onFinished;
        private Hint hint;
        [SerializeField] private string hintText;
        private bool isDone = false;
        public bool Begin()
        {
            hint=Hint.Spawn(hintText,new Vector2(0,3));
            return !isDone;
        }

        protected void Finish()
        {
            EndEvent?.Invoke();
                
            onFinished?.Invoke();
            hint.FadeOut();
            isDone = true;
        }

        public abstract void InteractiveUpdate(bool left, bool right, Vector2 pos);


    }
}