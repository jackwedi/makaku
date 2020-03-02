using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platformer : MonoBehaviour
{
    [SerializeField] private float speed = .0005f;
    [SerializeField] private float jumpForce = 12.0f;
    [SerializeField] private float jumpReset = 10f;

    [SerializeField] private float dashLength = .01f;
    [SerializeField] private float dashingDelay = 0.0f;
    [SerializeField] private float megaJumpRatio = 1.5f;
    private bool isDashing = false;
    private bool dashingAvailable = true;
    private bool handleKeyInput = true;
    private float deltaX = 0.0f;
    private bool isWallJumping = false;

    private Rigidbody2D _body;
    private Animator _anim;
    private CapsuleCollider2D _box;

    private int layerMaskGrounded;
    private int layerMaskGripping;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isGripping;
    [SerializeField] private GameObject _dashParticles = null;
    [SerializeField] private GameObject _deathParticles = null;

    private void Start()
    {
        Manager.Player.SetPlayer(this.gameObject);
        Manager.Progress.SetCheckPoint(this.transform);

        _body = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _box = GetComponent<CapsuleCollider2D>();

        layerMaskGrounded = (1 << LayerMask.NameToLayer("Terrain")) | (1 << LayerMask.NameToLayer("Platform"));
        layerMaskGripping = (1 << LayerMask.NameToLayer("Terrain"));

    }

    private void Update()
    {
        deltaX = 0.0f;

        if (handleKeyInput)
        {
            // Speed movement
            deltaX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            Vector2 movement = new Vector2(deltaX, _body.velocity.y);
            _body.velocity = movement;
            _anim.SetFloat("speed", Mathf.Abs(deltaX));
        }

        // isGrounded handling
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, jumpReset, layerMaskGrounded);
        _anim.SetBool("grounded", isGrounded);

        // Flips the sprite depending on the sign of deltaX
        if (!Mathf.Approximately(deltaX, 0))
        {
            transform.localScale = new Vector3(Mathf.Sign(deltaX), 1, 1);
        }

        isGripping = Physics2D.Raycast(transform.position, new Vector2(transform.localScale.x, transform.localScale.y), jumpReset / 2, layerMaskGripping) && !Physics2D.Raycast(transform.position, Vector2.down, jumpReset * 2, layerMaskGrounded);

        if (isGripping)
        {
            _body.gravityScale = 0;
            _body.velocity = Vector2.zero;
        }
        else if (!isGripping && !isDashing)
        {
            _body.gravityScale = 1;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGripping && handleKeyInput)
        {
            StartCoroutine(WallJump());
        }


        //Input of jumping handling 
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && handleKeyInput)
        {
            _body.velocity = Vector2.up * jumpForce;
        }

        // Input of dashing handling
        if (Input.GetKeyDown(KeyCode.W) && !isDashing && dashingAvailable && handleKeyInput)
        {
            StartCoroutine(Dash());
        }
    }

    public IEnumerator WallJump()
    {
        if (!isWallJumping)
        {
            isWallJumping = true;
            handleKeyInput = false;
            _body.velocity = new Vector2(-transform.localScale.x * 1.5f, .8f) * jumpForce;
            // Switch the sprite
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            float current = dashLength;
            while (current > 0)
            {
                current -= Time.deltaTime * 100;
                yield return null;
            }

            handleKeyInput = true;
            _body.velocity = new Vector2(0, 0);
            isWallJumping = false;
        }
    }

    public IEnumerator Dash()
    {

        _body.gravityScale = 0;
        _body.velocity = Vector2.zero;

        GameObject effect = Instantiate(_dashParticles, transform.position, Quaternion.identity);
        float current = dashLength;

        isDashing = true;
        dashingAvailable = false;

        while (current > 0)
        {
            current -= Time.deltaTime * 100;
            _body.velocity = new Vector2(transform.localScale.x * 10, _body.velocity.y);
            yield return null;
        }

        //_body.gravityScale = 1;
        isDashing = false;
        StartCoroutine(DashTimer());
    }

    private IEnumerator DashTimer()
    {

        float time = 0;
        Messenger<float, float>.Broadcast(GameEvent.DASH_DELAY_UPDATED, time, dashingDelay);

        while (time < dashingDelay)
        {
            yield return new WaitForSeconds(1);
            time += 1;
            Messenger<float, float>.Broadcast(GameEvent.DASH_DELAY_UPDATED, time, dashingDelay);
        }

        Messenger<float, float>.Broadcast(GameEvent.DASH_DELAY_UPDATED, time, dashingDelay);
        dashingAvailable = true;
    }

    public IEnumerator Hurt(Transform enemyTransform)
    {

        _body.gravityScale = 0;
        _body.velocity = Vector2.zero;

        float current = dashLength;

        bool forward = Physics2D.Raycast(transform.position, new Vector2(transform.localScale.x, transform.localScale.y), jumpReset / 2, (1 << LayerMask.NameToLayer("Enemy")));

        float direction = forward ? -1f : 1f;

        while (current > 0)
        {
            current -= Time.deltaTime * 100;
            _body.velocity = new Vector2(direction * 15 * transform.localScale.x, _body.velocity.y);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Jumps"))
        {
            _body.gravityScale = 1;
            _body.velocity = Vector2.up * jumpForce;
        }
        else if (collision.CompareTag("Check Point"))
        {
            Manager.Progress.SetCheckPoint(collision.gameObject.transform);
        }
        else if (collision.CompareTag("Mega Jumps"))
        {
            _body.gravityScale = 1;
            _body.velocity = Vector2.up * jumpForce * this.megaJumpRatio;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            Death();
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(Hurt(collision.gameObject.transform));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, Vector2.down * jumpReset);
    }

    private void Death()
    {
        if (!Manager.Player.getAlive()) return;
        Messenger.Broadcast(GameEvent.DEATH);

        Manager.Player.setAlive(false);
        SetStatic(true);

        _anim.SetFloat("speed", 0.0f);
        _anim.SetTrigger("dead");

        GameObject effect = Instantiate(_deathParticles, transform.position, Quaternion.identity);
        Manager.Player.Invoke("RespawnAtCheckPoint", 1);
    }

    public void SetStatic(bool isStattic)
    {
        handleKeyInput = !isStattic;
        _body.bodyType = isStattic ? RigidbodyType2D.Static : RigidbodyType2D.Dynamic;
        _box.enabled = !isStattic;

    }

    public void resetAnim()
    {
        _anim.SetTrigger("revive");
    }

}
