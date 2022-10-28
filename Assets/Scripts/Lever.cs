using System;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class Lever : MonoBehaviour, IInteractive
    {
        public event Action EndEvent;
        private Vector2 start;
        [SerializeField] private Vector2 desiredDirection=Vector2.down;
        [SerializeField] private float minLen=1;
        private bool active = false;
        [SerializeField] private UnityEvent onFinished;
        public bool Begin()
        {
            return true;
        }

        public void InteractiveUpdate(bool left, bool right, Vector2 pos)
        {
                if (left && !active)
            {
                active = true;
                start = pos;
            }
            else if (!left && active)
            {
                Vector2 dir = (pos - start);
                float angle = Vector2.Angle(dir, desiredDirection);
                active = false;
                if (angle < 30 && dir.sqrMagnitude >= minLen)
                {
                    EndEvent?.Invoke();
                    onFinished?.Invoke();
                }
            }
        }
    }
}