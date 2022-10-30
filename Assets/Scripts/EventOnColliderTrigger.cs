using System;
using Honey;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class EventOnColliderTrigger: MonoBehaviour
    {
        [SerializeField] private UnityEvent ev;
        [Tag][SerializeField]
        private string triggerTag;
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag(triggerTag))
            {
                Debug.Log("hejooo");
                ev?.Invoke();
            }
        }
    }
}