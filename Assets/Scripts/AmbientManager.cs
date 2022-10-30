using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game 
{
    public class AmbientManager: MonoBehaviour
    {
        [SerializeField] private AudioSource[] sources;


        [FormerlySerializedAs("clip")] [SerializeField] private AnimationCurve[] curves;
        [SerializeField] private Transform up, down, right;
        private float startX;

        private void Start()
        {
            startX = this.transform.position.x;
        }

        private void OnDrawGizmos()
        {
            Color[] colors = new Color[] {Color.red, Color.green, Color.blue,Color.grey};
            float real = Application.isPlaying ? startX : this.transform.position.x;
            int i = 0;
            foreach (var curve in curves)
            {
                Gizmos.color = colors[(i++) % 4];
                Helper.DrawGraphOnScene(this.gameObject,real,curve,up.position.y,down.position.y,right.position.x);
            }
        }

        private void Update()
        {
            for (int i = 0; i < sources.Length; i++)
            {
                sources[i].volume = curves[i].Evaluate((this.transform.position.x - startX) / 32);
            }
        }
    }
}