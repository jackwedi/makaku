﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float speed = 150f;
    private float jumpForce = 6.0f;
    private float jumpReset = .6f;

    private float dashLength = 20f;
    private float dashingDelay = 2;
    private bool dashingAvailable = true;

    private Rigidbody2D _body;
    private Animator _anim;
    private BoxCollider2D _box;
    private WanderingNPC _npc;

    private int layerMask;
    private bool isGrounded;
    private bool hurtable = true;

    private float direction = -1;

    [SerializeField] private GameObject particles;

    private void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _box = GetComponent<BoxCollider2D>();
        _npc = GetComponent<WanderingNPC>();

        this.speed = _npc.Speed();
        this.jumpForce = _npc.JumpForce();
        this.jumpReset = _npc.JumpReset();
        this.direction = _npc.Direction();
        layerMask = (1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("Ignore Raycast"));
        layerMask = ~layerMask;

    }

    private void Update()
    {
        this.direction = _npc.Direction();

        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(direction, 0f), 2f, layerMask);
        if (hit && hit.collider.CompareTag("Player") && dashingAvailable)
        {
            StartCoroutine(Dash());

        }

        hit = Physics2D.Raycast(transform.position, new Vector2(direction, 0f), .5f, layerMask);
        if (hit && hit.collider.CompareTag("Player"))
        {
            direction = -direction;
        }

        hit = Physics2D.Raycast(transform.position, Vector2.up, .5f, layerMask);
        if (hit && hit.collider.CompareTag("Player"))
        {
            Debug.Log(hit.collider.name);
            Hurt();
        }
        
    }

    public IEnumerator Dash()
    {
        hurtable = false;
        _body.gravityScale = 0;
        GameObject effect = Instantiate(particles, transform.position, Quaternion.identity);
        float current = dashLength;

        dashingAvailable = false;

        while (current > 0)
        {
            current -= Time.deltaTime * 100;
            _body.velocity = new Vector2(transform.localScale.x, 0f) * 10;
            yield return null;
        }
        _body.gravityScale = 1;

        StartCoroutine(DashTimer());
    }

    private IEnumerator DashTimer()
    {

        float time = 0;
        hurtable = true;
        while (time < dashingDelay)
        {
            yield return new WaitForSeconds(1);
            time += 1;
        }
        dashingAvailable = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Jump")) _body.velocity = Vector2.up * jumpForce * 1.5f;
    }

    public void Hurt()
    {
        if (hurtable)
        {
            // ADD Animation
            Messenger.Broadcast(GameEvent.ENEMY_KILLED);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) direction = -direction;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            direction = -direction;
            _body.velocity = Vector2.up * jumpForce;
        }
    }
}
