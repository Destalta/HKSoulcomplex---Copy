using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRemover : MonoBehaviour
{

    private void Start()
    {
        
    }

    private void Update()
    {
        EnemyDeathEffects[] enemyDeathEffects = GameObject.FindObjectsOfType<EnemyDeathEffects>();
        if (enemyDeathEffects != null && enemyDeathEffects.Length > 0)
        {
            for (int i = 0; i < enemyDeathEffects.Length; i++)
            {
                enemyDeathEffects[i].gameObject.SetActive(false);
            }
        }

    }
}
