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

        private void Start()
        {
            if (destroyAfter > 0)
                DOVirtual.DelayedCall(destroyAfter, () => Destroy(this.gameObject)).SetLink(this.gameObject);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) | Input.GetKeyDown(KeyCode.Space) | Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene("level0");
            }
        }
    }
}