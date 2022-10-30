using System;
using Settings;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class EventOnTrigger: MonoBehaviour
    {
        
        [SerializeField] private UnityEvent ev;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if(col.GetComponent<Movement>())
                ev?.Invoke();
        }
    }
}