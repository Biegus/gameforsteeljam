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

        [SerializeField] private bool leftright = false;
        private void Start()
        {
            if (destroyAfter > 0)
                DOVirtual.DelayedCall(destroyAfter, () => Destroy(this.gameObject)).SetLink(this.gameObject);
        }

        private void Update()
        {
            foreach (KeyCode code in codes)
            {
                if (Input.GetKeyDown(code))
                {
                    SceneManager.LoadScene(sceneName);
                }
            }

            if (leftright && (Input.GetMouseButton(0) || Input.GetMouseButton(1)))
            {
                
                    SceneManager.LoadScene(sceneName);
            }
        }
    }
}