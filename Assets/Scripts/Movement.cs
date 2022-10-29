using System;
using Animancer;
using Game;
using Honey;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Settings
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private int states;
        
        private float progress;
         public int State { get; private set; } = 0;
        [SerializeField] private Transform[] legs;
        [SerializeField] private Vector2 angleRange;
        [SerializeField] private float xProgress;
        [SerializeField] private float cycleTime=1;
        [SerializeField] private Camera camera;
        [SerializeField] private AnimationClip idle;
        [SerializeField] private float forgiveAfterTime = 0.2f;
        [FormerlySerializedAs("left")] [SerializeField] private AnimationClip[] walking;
        private bool inInteractiveArea;
        private IInteractive interactiveElement;
        private Vector2 mousePos;
        private bool moved;
         [SerializeField] private AnimationCurve speedPerStep;
         private float movableSpeed = 1f;
         private Timer moveResetTimer;
         [SerializeField] private float forgiveValue = 0.3f;
         public event Action<int> LegChanged;
         [SerializeField] private AnimationClip ropeOutClip;

         private AnimancerComponent animancer;
         private bool noHope = false;
         private Timer forgiveAfterStoper;
         [SerializeField] private AnimationClip mistakeAnim;
         private Timer wrongCooldown;

         private void Awake()
        {
            animancer = this.GetComponent<AnimancerComponent>();
        }

        private void Start()
        {
            moveResetTimer = new Timer(2f);
            animancer.Play(idle);
            wrongCooldown = new Timer(0.5f);
            forgiveAfterStoper = new Timer(forgiveAfterTime);
        }

        public void RopeOut()
        {
      
         PlayDeathAnim(ropeOutClip);
        }

        public void OnMistake()
        {
            PlayDeathAnim(mistakeAnim);
        }

        public void PlayDeathAnim(AnimationClip clip)
        {
            animancer.enabled = true;
            animancer.Play(clip)
                .Events.OnEnd = () =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            };
            noHope = true; 
        }
        private bool MakeProgress(int at, float delta)
        {
            if ((State + 1) % states == at && progress >= forgiveValue)
            {
                State = at;
                LegChanged?.Invoke(State);
                progress = 0;
            }
            if (at == State)
            {
                animancer.enabled = false;
                moved = true;
                if (progress < 1)
                {

                    float sign = Mathf.Sign(mousePos.x - this.transform.position.x);
                    this.transform.Translate(xProgress * sign * delta * speedPerStep.Evaluate(progress), 0,
                        0);
                    this.transform.localScale =
                        new Vector3(this.transform.localScale.x * Mathf.Sign(this.transform.localScale.x * sign),
                            this.transform.localScale.y,1 );
                }
                progress += delta* (1f/cycleTime);
             
                progress = Mathf.Min(progress, 1); ;

                walking[State].SampleAnimation(this.gameObject, progress * walking[State].length); 
      
                if ( progress>= 1)
                {
                    State = (State + 1) % states;
                    progress = 0;
                    LegChanged?.Invoke(State);
                }

                return true;
            }
            return false;
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
            GUILayout.Label($"{progress}, {State}",style);
        }

        private void ResetProgress()
        {
            forgiveAfterStoper.Reset();
            progress = 0;
        }
        private void Update()
        {
            if (noHope) return;
            if (moveResetTimer.Push())
            {
                ResetProgress();
                animancer.Play(idle);
                animancer.enabled = true;

            }
            mousePos=camera.ScreenToWorldPoint(Input.mousePosition);
            bool left = Input.GetMouseButton(0);
            bool right = Input.GetMouseButton(1);
            if (interactiveElement == null)
            {

                bool correct = true;
                if(left)
                     correct=MakeProgress(0,Time.deltaTime);
                if(right)
                    correct=correct&MakeProgress(1,Time.deltaTime);
                if (!correct && (progress>0 || forgiveAfterStoper.Done) && wrongCooldown.Push())
                {
                    Hint.Spawn("WRONG", Vector2.zero, 1f,Color.red);
                }
                
            }
            else
            {
                interactiveElement?.InteractiveUpdate(left,right,mousePos);
            }
        }
    }
}