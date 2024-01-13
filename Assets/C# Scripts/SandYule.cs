using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using WeaverCore;
using WeaverCore.Components;
using WeaverCore.Features;
using WeaverCore.Interfaces;

public class SandYule : Enemy
{
    
    [Header("Section1")]
    private Rigidbody2D rb;
    public GroundDetector GroundDetector;
    private Animator animator;
    private SpriteRenderer sp;
    public GameObject DeadBody;

    [Header("Section 2")]
    public float WallDetectDistance = 2;
    public LayerMask TerrainLayer;
    public bool Moving = true;
    public bool PrepJump = false;
    public bool Jumping = false;
    public bool JumpDead = false;
    public float JumpDeadTime = 0.1f;
    public float Speed = 2;
    public float JumpXVel;
    public float JumpYVel;
    private bool flipped = false;
    public float BounceRayDistance = 0.25f;

    [Header("Sounds")]
    public AudioSource CrawlingSound;
    public AudioSource BendSound;
    public AudioSource JumpSound;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
    }

    public void Flip()
    {
        print("Flip");
        if (flipped == false)
        {
            flipped = true;
            rb.velocity = new Vector3(-rb.velocity.x, rb.velocity.y);
            if (transform.rotation.y == 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        
    }

    private IEnumerator Jump()
    {
        flipped = false;
        Moving = false;
        PrepJump = true;
        CrawlingSound.Pause();
        BendSound.Play();
        animator.Play("Jump");
        yield return new WaitForSeconds(0.04f);

        Jumping = true;
        PrepJump = false;
        JumpSound.Play();

        rb.AddForce(new Vector2(-transform.right.x * JumpXVel, JumpYVel), ForceMode2D.Impulse);

        JumpDead = true;

        yield return new WaitForSeconds(JumpDeadTime);
        JumpDead = false;
    }

    private void Update()
    {
        RaycastHit2D rayWall = Physics2D.Raycast(transform.position, -transform.right, BounceRayDistance, TerrainLayer);
        RaycastHit2D ray = Physics2D.Raycast(transform.position, -transform.right, WallDetectDistance, TerrainLayer);
        if (ray.collider)
        {
            if (PrepJump == false && Jumping == false && GroundDetector.TouchingGround)
            {
                StartCoroutine(Jump());
            }
        }
        if (rayWall.collider)
        {
            Flip();
        }

        if (Moving)
        {
            rb.velocity = new Vector2(-transform.right.x * Speed, rb.velocity.y);
        }

        if (Jumping)
        {
            if (JumpDead == false && GroundDetector.TouchingGround)
            {
                Jumping = false;
                Moving = true;
                animator.Play("Run");
                CrawlingSound.UnPause();
            }
        }
    }

    protected override void OnDeath()
    {
        DeadBody.SetActive(true);
        DeadBody.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0.5f);
        DeadBody.GetComponent<Rigidbody2D>().velocity = -rb.velocity * 1.5f;
        gameObject.SetActive(false);
    }

}
