using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using WeaverCore;
using WeaverCore.Components;
using WeaverCore.Features;
using WeaverCore.Interfaces;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;

public class BigAnt : EnemyReplacement
{
    
    private IEnemyMove[] moves;

    private Rigidbody2D rb;

    private Coroutine mainRoutine;

    private Transform playerTransform;

    private EntityHealth entityHealth;

    public bool AntennaeBroken = false;
    public GameObject Goop;

    public GameObject ExplosionPrefab;
    


    void Start()
    {
        playerTransform = Player.Player1.transform;
        moves = GetComponents<IEnemyMove>();
        entityHealth = GetComponent<EntityHealth>();

        
    }


    protected override void OnDeath()
    {
        StopCoroutine(mainRoutine);
        gameObject.SetActive(false);
    }

    
    void Update()
    {
        if (AntennaeBroken == false && entityHealth.LastAttackInfo.Direction == 270)
        {
            if (Player.Player1.transform.position.y - transform.position.y > 0.8f)
            {
                AntennaeBroken = true;
                Goop.SetActive(false);
                Instantiate(ExplosionPrefab, Goop.transform.GetChild(0).position, Quaternion.identity);
            }
        }
    }
}
