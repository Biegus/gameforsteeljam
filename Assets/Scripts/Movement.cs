using System;
using Game;
using UnityEngine;

namespace Settings
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private int states;
        
        private float progress;
        private int state = 0;
        [SerializeField] private Transform[] legs;
        [SerializeField] private Vector2 angleRange;
        [SerializeField] private float xProgress;
        [SerializeField] private float cycleTime=1;
        

        private void MakeProgress(int at, float delta)
        {
            if (at == state)
            {
                this.transform.Translate(xProgress*delta,0,0);
                progress += delta* (1f/cycleTime);
                Debug.Log(progress);
                if (progress >= 1)
                {
                    state = (state + 1) % states;
                    progress = 0;
                }
                else
                {
                    progress = Mathf.Min(progress, 1); ;
                }

            }
        }

        private void OnGUI()
        {
            GUIStyle style = new GUIStyle("label") {fontSize = 30};
           GUILayout.Label($"{progress}, {state}",style);
        }

        private void Update()
        {
            if(Input.GetMouseButton(0))
                MakeProgress(0,Time.deltaTime);
            if(Input.GetMouseButton(1))
                MakeProgress(1,Time.deltaTime);
        }
    }
}