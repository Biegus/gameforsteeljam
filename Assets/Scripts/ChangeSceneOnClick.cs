using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game 
{
    public class ChangeSceneOnClick : MonoBehaviour
    {
        [SerializeField] private string sceneName;
        [SerializeField] private float destroyAfter = 0;
        [SerializeField] private KeyCode[] codes;

        private void Start()
        {
            if (destroyAfter > 0)
                DOVirtual.DelayedCall(destroyAfter, () => Destroy(this.gameObject)).SetLink(this.gameObject);
        }

        private void Update()
        {
            
        }
    }
}