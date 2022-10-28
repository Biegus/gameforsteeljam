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
        [SerializeField] private Camera camera;
        private bool inInteractiveArea;
        private IInteractive interactiveElement;
        private Vector2 mousePos;

        private void MakeProgress(int at, float delta)
        {
            if (at == state)
            {
                this.transform.Translate(xProgress*delta * Mathf.Sign(mousePos.x- this.transform.position.x),0,0);
                progress += delta* (1f/cycleTime);
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            IInteractive interactable = other.GetComponent<IInteractive>();
            if (interactable == null || !interactable.Begin()) return;
            this.interactiveElement = interactable;
            Debug.Log("ssup");
            this.interactiveElement.EndEvent += OnInteractiveElementExit;
        }

        private void OnInteractiveElementExit()
        {
            this.interactiveElement.EndEvent -= OnInteractiveElementExit;
            this.interactiveElement = null;
        }

        private void OnGUI()
        { 
            GUIStyle style = new GUIStyle("label") {fontSize = 30};
            GUILayout.Label($"{progress}, {state}",style);
        }

        private void Update()
        {
            mousePos=camera.ScreenToWorldPoint(Input.mousePosition);
            bool left = Input.GetMouseButton(0);
            bool right = Input.GetMouseButton(1);
            if (interactiveElement == null && !(left && right))
            {
                if(left)
                    MakeProgress(0,Time.deltaTime);
                if(right)
                    MakeProgress(1,Time.deltaTime);
            }
            else
            {
                interactiveElement.InteractiveUpdate(left,right,mousePos);
            }
        }
    }
}