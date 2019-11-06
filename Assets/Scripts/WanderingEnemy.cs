using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingEnemy : MonoBehaviour
{
    [SerializeField] private float speed = 150f;
    [SerializeField] private float jumpForce = 6.0f;
    [SerializeField] private float jumpReset = .1f;

    [SerializeField] private float dashLength = .01f;
    [SerializeField] private float dashingDelay;
    private bool dashingAvailable = true;

    private Rigidbody2D _body;
    private Animator _anim;
    private BoxCollider2D _box;

    private int layerMask;
    private bool isGrounded;
    private bool hurtable = true;

    private float jumpTimer = 5f;

    private float direction = -1;

    [SerializeField] private GameObject particles;


    private void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _box = GetComponent<BoxCollider2D>();

        layerMask = (1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("Ignore Raycast"));
        layerMask = ~layerMask;

        InvokeRepeating("ChangeDirection", 0f, 2f);

    }

    private void ChangeDirection()
    {
        direction = Random.Range(-1, 1);
        direction = Mathf.Sign(direction);
    }

    private void Update()
    {
        // Speed movement


        float deltaX = direction *speed * Time.deltaTime;
        Vector2 movement = new Vector2(deltaX, _body.velocity.y);
        _body.velocity = movement;
        _anim.SetFloat("speed", Mathf.Abs(deltaX));

        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, jumpReset);
        _anim.SetBool("grounded", isGrounded);

        // Flips the sprite dependind on the sign of deltaX
        if (!Mathf.Approximately(deltaX, 0)) transform.localScale = new Vector3(Mathf.Sign(deltaX), 1, 1);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(direction,0f), 2f, layerMask);
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

        jumpTimer -= Time.deltaTime;

        if (jumpTimer < 0f && isGrounded)
        {
            _body.velocity = Vector2.up * jumpForce;
            jumpTimer = Random.Range(2f,5f);
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
        if(hurtable)
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

