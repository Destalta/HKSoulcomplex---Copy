using HutongGames.PlayMaker.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeaverCore.Components;
using WeaverCore.Features;

public class LykeScript : Enemy
{
    private Rigidbody2D rb;
    private GroundDetector groundDetector;
    private GameObject SpriteObject;

    public float RayDistance;
    public LayerMask LayerMask;

    public bool Drop;
    public float DropAnimTime;
    [Tooltip("Min time it'll take for the Lyke to appear after dropping")] public float MinStageTime;
    [Tooltip("Max time it'll take for the Lyke to appear after dropping")] public float MaxStageTime;

    public Transform Target;

    public float XVelocity;
    public bool Grounded;

    public int State = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.simulated = false;
        groundDetector = GetComponent<GroundDetector>();
        SpriteObject = transform.GetChild(0).gameObject;
    }

    private IEnumerator DropRoutine()
    {
        yield return new WaitForSeconds(DropAnimTime);
        yield return new WaitForSeconds(Random.Range(MinStageTime, MaxStageTime));
    }

    private void Update()
    {
        if (Drop == false)
        {
            RaycastHit2D ray = Physics2D.Raycast(transform.position, Vector2.down, RayDistance, LayerMask);
            if (ray.collider)
            {
                Drop = true;
                Target = ray.collider.gameObject.transform;
                StartCoroutine(DropRoutine());
            }
        }

        if (groundDetector.TouchingGround)
        {
            Grounded = true;
        }
    }

    private void FixedUpdate()
    {

    }
}
