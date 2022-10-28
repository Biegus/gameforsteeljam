using System;
using UnityEngine;

namespace Game
{
    public class Plank : MonoBehaviour
    {
        private Rigidbody2D rigi;

        private void Awake()
        {
            rigi = this.GetComponent<Rigidbody2D>();
        }

        public void Start()
        {
        }

        public void Let()
        {
            rigi.gravityScale = 1;
            rigi.AddTorque(100);
        }
    }
}