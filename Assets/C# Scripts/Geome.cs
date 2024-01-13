using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WeaverCore;
using WeaverCore.Assets.Components;
using WeaverCore.Components;
using WeaverCore.Enums;
using WeaverCore.Features;

public class Geome : Enemy
{
    Rigidbody2D rb;
    public float Speed;
    public bool Dead;
    public bool Flung;
    public float XMovement;
    public bool Moving = true;

    public Animator animator;
    public GameObject DeathParticles;
    public GameObject BounceParticles;

    public UnityEvent DeathEvent;
    [Tooltip("Invoked when bumping into walls")] public UnityEvent WallEvent;
    [Tooltip("Invoked when approaching an edge")] public UnityEvent EdgeEvent;

    [Header("Raycasts")]
    public LayerMask Layers;
    public float WallDetectDistance;
    public float EdgeDetectDistance;

    public bool Turning;
    public GameObject Legs;
    public AudioSource CrawlingSound;
    public AudioSource TurnSound;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        XMovement = -transform.right.x * Speed;
    }

    private void Fling(CardinalDirection direction)
    {
        switch (direction)
        {
            case CardinalDirection.Right:
                rb.AddForce(Vector2.right * Random.Range(60, 100) + Vector2.up * Random.Range(15, 20), ForceMode2D.Impulse);
                rb.AddTorque(Random.Range(-30, -20), ForceMode2D.Impulse);
                break;
            case CardinalDirection.Left:
                rb.AddForce(Vector2.left * Random.Range(60, 100) + Vector2.up * Random.Range(15, 20), ForceMode2D.Impulse);
                rb.AddTorque(Random.Range(30, 20), ForceMode2D.Impulse);
                break;
            case CardinalDirection.Up:
                rb.AddForce(Vector2.right * Random.Range(-20, 27) + Vector2.up * Random.Range(20, 25), ForceMode2D.Impulse);
                rb.AddTorque(Random.Range(-30, -20), ForceMode2D.Impulse);
                break;
            case CardinalDirection.Down:
                rb.AddForce(Vector2.right * Random.Range(-20, 27) + Vector2.up * Random.Range(-10, -15), ForceMode2D.Impulse);
                rb.AddTorque(Random.Range(-5, 5), ForceMode2D.Impulse);
                break;
        }
    }

    private EnemyDamager GetClosestDamager()
    {
        EnemyDamager[] enemyDamagers = GameObject.FindObjectsOfType<EnemyDamager>();
        EnemyDamager closestDamager = null;
        foreach (EnemyDamager damager in enemyDamagers)
        {
            if (closestDamager == null)
            {
                closestDamager = damager;
            }
            else
            {
                if ((damager.transform.position - transform.position).magnitude < (closestDamager.transform.position - transform.position).magnitude)
                {
                    closestDamager = damager;
                }
            }

        }
        return closestDamager;
    }

    protected override void OnDeath()
    {

        Dead = true;
        DeathEvent.Invoke();
        Instantiate(DeathParticles, transform.position + Vector3.forward * 1.01f, Quaternion.identity);
        animator.Play("GeomeDie");
        Fling(GetClosestDamager().hitDirection);
        transform.position += Vector3.forward * Random.Range(-0.9f, 1.1f);
        Flung = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Flung)
        {
            Instantiate(BounceParticles, transform.position + Vector3.forward * 1.01f, Quaternion.identity);
            Flung = false;
        }
    }


    void FixedUpdate()
    {
        if (!Dead && Moving)
        {
            RaycastHit2D wallRay = Physics2D.Raycast(transform.position, new Vector3(Mathf.Clamp01(XMovement), 0), WallDetectDistance, Layers);
            if (wallRay.collider)
            {
                WallEvent.Invoke();
            }

            RaycastHit2D edgeRay = Physics2D.Raycast(transform.position + (new Vector3(Mathf.Clamp01(XMovement), 0) * 1.5f), Vector2.down, EdgeDetectDistance, Layers);
            if (!edgeRay.collider)
            {
                EdgeEvent.Invoke();
            }

            rb.velocity = new Vector2(XMovement, rb.velocity.y);
        }
    }

    private IEnumerator TurnRoutine()
    {
        Moving = false;
        yield return new WaitForSeconds(0.1f);
        Moving = true;
        XMovement = -XMovement;
        yield return new WaitForSeconds(0.2f);
        Turning = false;
    }

    public void Turn()
    {
        if (animator)
        {
            if (Turning == false)
            {
                Turning = true;
                TurnSound.Play();
                StartCoroutine(TurnRoutine());
            }
        }
    }
}
