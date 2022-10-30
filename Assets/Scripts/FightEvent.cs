using System;
using System.Collections;
using Animancer;
using DG.Tweening;
using Settings;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Game
{
    public class FightEvent : HintableInteractive
    {
        [SerializeField] private Movement movement;
        [SerializeField] private float reactionLimit = 4f;
        [SerializeField] private AnimationClip attackAnim;
        [SerializeField] private AnimationClip enemyAttackAnim;
        [SerializeField] private AnimationClip enemyGetDmg;
        [SerializeField] 
        private Vector2 movementDir;
        [SerializeField] private float speed;
        [SerializeField] private AnimationClip enemyDie;
        [SerializeField] private AnimationClip walking;
        [SerializeField] private Camera camera;
        public Rigidbody2D Rigi { get; private set; }
        public AnimancerComponent AnimancerComponent { get; private set; }
        private bool isRunning;
        private void Awake()
        {
            AnimancerComponent = this.GetComponent<AnimancerComponent>();
            Rigi = this.GetComponent<Rigidbody2D>();
            
        }

        public void Run()
        {
            if (isRunning) return;
            isRunning = true;
            AnimancerComponent.Play(walking);
        }

        public override bool Begin(bool already)
        {
            
          
           bool val= base.Begin(already);
         
           if (!val) return false;
           Debug.Log(already);
           if (already)
           {
               
               StartCoroutine(CAttackByEnemy(0,null));
               return true;
           }
           cor=StartCoroutine(CRun());
           this.Rigi.velocity = Vector2.zero;//TODO maybe don't just u know from 100 mph to 0 in one frame
           return true;
        }

        private IEnumerator CAttackByEnemy(int hp,Hint hint)
        {
            camera.GetComponent<Volume>().profile.TryGet<Vignette>(out Vignette vin);
            DOVirtual.Float(0, 0.5f, 0.3f, value =>
            {
                vin.intensity.value = value;
            }).SetLoops(2, LoopType.Yoyo);
            
            this.AnimancerComponent.Stop();
            yield return this.AnimancerComponent.Play(enemyAttackAnim);
            this.AnimancerComponent.Stop();
            if(hint!=null)
                hint.FadeOut(0.05f);
            if (hp == 0)
            {
                GameManager.Instance.Die(0);
            }
            

            yield return null;

        }
        private void FixedUpdate()
        {
            if (!Used && isRunning)
            {
                this.Rigi.velocity = movementDir * speed;
            }
               
        }

        private Coroutine cor;

        Hint Spawn(string text)
        {
            return Hint.Spawn(text, HintPoint.position);
        }
        private IEnumerator CRun()
        {
            AnimancerComponent.Stop();
         //   movement.enabled = false;
            movement.Animancer.enabled=true;
            if (!movement.HasPlank)
            {
                Debug.Log("No planka haha");
                yield return CAttackByEnemy(0,null);
                yield break;
            }

            
                int hp = 2;
                bool first = true;
            for (int enemyHp=2;enemyHp>0;)
            {
                Hint spawn = null;
                if (!first)
                {
                     spawn = Spawn("Wait ");
                    yield return new WaitForSeconds(1.8f);
                    if (Input.GetMouseButton(0))
                    {
                        Debug.Log("wait:Failure");
                        spawn.FadeOut(0.3f);
                        yield return CAttackByEnemy(--hp,spawn);
                        continue;
                    } 
                Debug.Log("wait:ok");
                spawn.FadeOut(0.3f);
                yield return new WaitForSeconds(0.3f);

                }

                first = false;
                

                 spawn = Spawn("Hold left");
                float time = Time.time;
                yield return Helper.Untill(() => Time.time - time > reactionLimit || Input.GetMouseButtonDown(0));
                if (Time.time - time > reactionLimit)
                {
                    Debug.Log("Holding:failure");
                        yield return CAttackByEnemy(--hp,spawn);
                        continue;
                }
                Debug.Log("Holding: ok");
                var state=  movement.Animancer.Play(attackAnim);
                while (state.Time < attackAnim.length/2f)
                {
                   
                    if (!Input.GetMouseButton(0))
                    {
                        movement.Animancer.Stop();//todo: maybe idle instead
                        
                      
                        yield return CAttackByEnemy(--hp,spawn);
                        Destroy(spawn.gameObject);
                        Debug.Log("Final:failure");
                        continue;
                    }

                    yield return null;
                }
                spawn.FadeOut(attackAnim.length/2);
                this.AnimancerComponent.Stop();
                this.AnimancerComponent.Play(enemyGetDmg);
                Debug.Log("final: ok");
                
                yield return new WaitForSeconds(attackAnim.length / 2);
                yield return new WaitForSeconds(0.2f);
                movement.Animancer.Stop();
              
                
                enemyHp--;
            }

            yield return this.AnimancerComponent.Play(enemyDie);
         //   movement.enabled = true;
            Finish();



        }

        public override void InteractiveUpdate(bool left, bool right, Vector2 pos)
        {
            print(movement.Rope.IsMaxed);
            if (movement.Rope.IsMaxed && movement.transform.position.x < movement.Cart.transform.position.x)
            {
                GameManager.Instance.Die();
                if(cor!=null) StopCoroutine(cor);
                
            }
        }
    }
}