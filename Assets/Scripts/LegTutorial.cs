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
            movement.onInteractionEnd += () =>
            {
                Debug.Log("end");
                Spawn();
            };
            movement.OnInteraction += () =>
            {Debug.Log("start");
               if(last!=null) 
                last.FadeOut(0.2f);
            };
            Spawn();
        }

        private string GetDescr(int index,string val)
        {
            if (index < 2)
            {
                return $"Hold {val}";
            }
            else return $"{val} leg";
        }
        public void Spawn()
        {
           
            if(last!=null)
                last.FadeOut(0.2f);
            if (movement.State%2==0 )
               last= Hint.Spawn(GetDescr(index,"Left"), points[index% points.Length].position);
            else
               last= Hint.Spawn(GetDescr(index,"Right"), points[index% points.Length].position);
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

        public void SpawnIfZero()
        {
            if(last==null) Spawn();
        }
        public void  Despawn()
        {
            if (last != null)
                last.FadeOut(0.2f);
        }
    }
}