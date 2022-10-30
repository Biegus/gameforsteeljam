using System;
using Animancer;
using DG.Tweening;
using Game;
using Honey;
using TMPro;
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

         public AnimancerComponent Animancer { get; private set; }
         private bool noHope = false;
         private Timer forgiveAfterStoper;
         [SerializeField] private AnimationClip mistakeAnim;
         private Timer wrongCooldown;
         [SerializeField] private Transform mistakePos;
         [SerializeField] private Rope rope;
         private AnimationClip currentClip = null;
         [FormerlySerializedAs("crate")] [SerializeField] private Transform cargo;
         [SerializeField] private AnimationClip fail;
         private bool isFalling = false;
         private AnimancerState ropeState;
         private bool sleep = true;
         [FormerlySerializedAs("clip")] [SerializeField] private AnimationClip sleepClip;
         private Hint menu;
         [SerializeField] private GameObject enableOnStart;
         private bool wakingUp;
         [SerializeField] private AnimationClip wakingUpClip;
         [SerializeField] private Transform menuTextPoint;
         private Timer ropeOutTimer;
         private Timer autoSkipTimer;
         private bool autoSkip = false;
         private bool canWakeUp = false;
         [SerializeField] private TMP_Text toFadeAtStart;
         public bool HasPlank { get; set; } = true;
         public event Action OnInteraction;
         public event Action onInteractionEnd;
         public Rope Rope => rope;
         public Transform Cart => cargo;

         private AudioSource[] audio;
         private void Awake()
        {
            Animancer = this.GetComponent<AnimancerComponent>();
            audio = GetComponents<AudioSource>();
            
        }

        private void Start()
        {
            Animancer.Play(sleepClip);
            DOVirtual.DelayedCall(4f, () =>
            {
                canWakeUp = true;
                menu = Hint.Spawn("Press Left to wake up", menuTextPoint.transform.position,disableEffect:true);
            });
            sleep = false; 
            moveResetTimer = new Timer(2f);
            wrongCooldown = new Timer(0.2f);
            forgiveAfterStoper = new Timer(forgiveAfterTime);
            ropeOutTimer = new Timer(ropeOutClip.length);
            ropeOutTimer.StartTime = -ropeOutClip.length;
            autoSkipTimer = new Timer(0.12f);
            
        }

        private AnimancerState Play(AnimationClip clip)
        {
            
            var es=Animancer.Play(clip);
            currentClip = clip;
            return es;
        }

        private AnimancerState SoftPlay(AnimationClip clip)
        {
            
            if (currentClip == clip)
                return null;
            if (currentClip==ropeOutClip &&ropeState != null && ropeState.Time > ropeOutClip.length - 0.1f)
            {
                return null;
            } 
            return Play(clip);
        }


        public void OnMistake()
        {
            PlayDeathAnim(mistakeAnim);
        }

        public void PlayDeathAnim()
        {
            PlayDeathAnim(ropeOutClip);
        }
        public void PlayDeathAnim(AnimationClip clip)
        {
            Animancer.enabled = true;
            Animancer.Play(clip)
                .Events.OnEnd += () =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                Animancer.Stop();
            };
            noHope = true; 
        }
        private bool MakeProgress(int at, float delta){
            
            
            float sign = Mathf.Sign(mousePos.x - this.transform.position.x);
            if ( sign!= Mathf.Sign(cargo.position.x-this.transform.position.x) && rope.IsMaxed)
            {
                return false;
            }
            if ((State + 1) % states == at && progress >= forgiveValue)
            {
                State = at;
                 ResetProgress(); 
                LegChanged?.Invoke(State);
                return true;
            }
            if (at == State)
            {
                Animancer.enabled = false;
                moved = true;
                if (progress > 0.5 )
                {
                        autoSkip = true;
                        autoSkipTimer.Reset();
                }
                
                
                if (progress < 1)
                {
                    this.transform.Translate(xProgress * sign * delta * speedPerStep.Evaluate(progress), 0,
                        0);
                    this.transform.localScale =
                        new Vector3(this.transform.localScale.x * Mathf.Sign(this.transform.localScale.x * sign),
                            this.transform.localScale.y,1 );
                }
                progress += delta* (1f/cycleTime);
                progress = Mathf.Min(progress, 1); ;

                walking[State].SampleAnimation(this.gameObject, progress * walking[State].length); 
                
                audio[State].Play();
                
                if ( progress>= 1)
                {
                    State = (State + 1) % states;
                    ResetProgress();
                    LegChanged?.Invoke(State);
                }

                return true;
            }
            return false;
        }
        

     

        private void OnTriggerEnter2D(Collider2D other)
        {
            IInteractive interactable = other.GetComponent<IInteractive>();
            if (interactable == null || !interactable.Begin(this.interactiveElement!=null)) return;
            if (this.interactiveElement != null)
            {
                interactiveElement.EndEvent -= OnInteractiveElementExit;
                interactiveElement.Abort();
                onInteractionEnd?.Invoke();
            }

            OnInteraction?.Invoke();
            this.interactiveElement = interactable;
            this.interactiveElement.EndEvent += OnInteractiveElementExit;
            this.transform.localScale =
                new Vector3(Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, 1);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (interactiveElement!=null&&other.GetComponent<IInteractive>() == interactiveElement)
            {
                interactiveElement.Abort();
                interactiveElement = null;
            }
        }

        private void OnInteractiveElementExit()
        {
            if (this.interactiveElement == null) return;
            onInteractionEnd?.Invoke();
            this.interactiveElement.EndEvent -= OnInteractiveElementExit;
            this.interactiveElement = null;
        }

        private void OnGUI()
        { 
          //  GUIStyle style = new GUIStyle("label") {fontSize = 30};
          //  GUILayout.Label($"{progress}, {State}",style);
        }

        private void ResetProgress()
        {
            forgiveAfterStoper.Reset();
            autoSkip = false;
            progress = 0;
        }
        private void Update()
        {

            mousePos=camera.ScreenToWorldPoint(Input.mousePosition);
            if (interactiveElement != null)
            {
                interactiveElement.InteractiveUpdate(Input.GetMouseButton(0),Input.GetMouseButton(1),mousePos);
                return;
            };
            if (sleep && !wakingUp)
            {
                if (Input.GetMouseButtonUp(0) &&canWakeUp)
                {
                    wakingUp = true;
                    menu.FadeOut();
                    
                    audio[2].Play();
                  
                    Animancer.Play(wakingUpClip).Events.OnEnd+= () =>
                    {
                        if(enableOnStart!=null) 
                    enableOnStart.gameObject.SetActive(true);
                        sleep = false;
                        toFadeAtStart.GetComponent<Animator>().enabled = false;
                        toFadeAtStart.DOFade(0, 0.5f);
                        Animancer.Stop();
                        return;
                    };
                    return;

                }
            }
            if (noHope) return;
           
            if (isFalling && !rope.IsMaxed) return;
            if (sleep)return;
            if (!ropeOutTimer.Done) return;
            if (autoSkip)
            {
                if(autoSkipTimer.Done)
                {
                    State = (State + 1) % states;
                    LegChanged?.Invoke(State);
                   ResetProgress(); 
                    autoSkip = false;
                }
                
               
            }

            if (Animancer.enabled || rope.IsMaxed)
            {
            
                Animancer.enabled = true;
                if (rope.IsMaxed )
                {
                    if (cargo.position.x < this.transform.position.x)
                    {
                        ropeOutTimer.Reset();
                    }
                    SoftPlay(ropeOutClip);
                }
                else SoftPlay(idle);
            }
            if (moveResetTimer.Push())
            {
                ResetProgress();
                Play(idle);
                Animancer.enabled = true;

            }
            bool left = Input.GetMouseButton(0);
            bool right = Input.GetMouseButton(1);
           

                bool correct = true;
                if (left && right) correct = false;
                else
                {
                    if(left)
                        correct=MakeProgress(0,Time.deltaTime);
                    if(right)
                        correct=correct&MakeProgress(1,Time.deltaTime); 
                }
               
                if (!correct && (progress>0 || forgiveAfterStoper.Done) &&  !rope.IsMaxed && wrongCooldown.Push()  )
                {
                    
                    //Hint.Spawn("WRONG",mistakePos.transform.position,  1f,Color.red);
                    Animancer.enabled = true;
                    progress = 0;
                    Animancer.Play(fail)
                        .Events.OnEnd+= () => { isFalling = false;
                        Animancer.Stop();
                    };
                    isFalling = true;                    
                    audio[3].Play();
                }
                
        }
    }
}