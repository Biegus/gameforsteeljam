using System;
using Settings;
using UnityEngine;

namespace Game
{
    public class LegTutorial : MonoBehaviour

    {
        [SerializeField] private Transform[] points;

        [SerializeField] private Movement movement;
        private int index = 0;
        private Hint last;
        private void Awake()
        {
            movement.LegChanged += OnLegChanged;
            Spawn();
        }

        public void Spawn()
        {
           
            if (last != null)
            {
                last.FadeOut();
            }
            if (index % 2 == 0)
               last= Hint.Spawn("Left", points[index% points.Length].position);
            else
               last= Hint.Spawn("Right", points[index% points.Length].position);
            index++;
        }

        private void OnDestroy()
        {
            movement.LegChanged -= OnLegChanged;
        }

        private void OnLegChanged(int obj)
        {
           Spawn(); 
        }
    }
}