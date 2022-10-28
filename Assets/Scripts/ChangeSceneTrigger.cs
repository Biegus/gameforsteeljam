using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneTrigger : MonoBehaviour
{
    [SerializeField] 
    private string SceneName;

    [SerializeField] 
    private List<string> TriggeringTags;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (TriggeringTags.Contains(col.tag))
        {
            SceneManager.LoadScene(SceneName);
        }
        
    }
}
