using System;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class Plank : MonoBehaviour
    {
      
        [SerializeField] private Collider2D toActivate;
       

        public void Start()
        {
        }

        public void Let()
        {
            toActivate.gameObject.SetActive(true);
            this.transform.DORotate(new Vector3(0, 0, 90), 1);
            
        }
    }
}