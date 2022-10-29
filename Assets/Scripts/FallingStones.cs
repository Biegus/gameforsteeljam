﻿using System.Collections;
using UnityEngine;

namespace Game
{
    public class FallingStones : MonoBehaviour
    {
        [SerializeField] private GameObject particlePrefab;
        [SerializeField] private float waitTime;
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private GameObject stoneCollectionPrefab;
        private bool started = false;
        private IEnumerator CRun()
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
                var particle=Instantiate(particlePrefab);
                particle.transform.position = spawnPoint.position;
            }

            yield return new WaitForSeconds(waitTime);
            foreach (Transform spawnPoint in spawnPoints)
            {
                var el = Instantiate(stoneCollectionPrefab);
                el.transform.position = spawnPoint.position;
            }
        }

        public void Run()
        {
            if (started) return;
            started = true;
            StartCoroutine(CRun());
            
        }
    }
}