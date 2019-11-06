using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platformer : MonoBehaviour
{
    [SerializeField] private float speed = .0005f;
    [SerializeField] private float jumpForce = 12.0f;
    [SerializeField] private float jumpReset = 10f;

    [SerializeField] private float dashLength = .01f;
    [SerializeField] private float dashingDelay;
    [SerializeField] private float megaJumpRatio = 1.5f;
    private bool isDashing = false;
    private bool dashingAvailable = true;
    private bool handleKeyInput = true;
    private float deltaX = 0.0f;
    private bool isWallJumping = false;

    private Rigidbody2D _body;
    private Animator _anim;
    private BoxCollider2D _box;

    private int layerMaskGrounded;
    private int layerMaskGripping;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isGripping;

    [SerializeField] private GameObject particles;


    private void Start() {
        _body = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _box = GetComponent<BoxCollider2D>();

        //layerMaskGrounded = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Ignore Raycast")) | (1 << LayerMask.NameToLayer("Baby"));
        //layerMaskGripping = layerMaskGrounded | (1 << LayerMask.NameToLayer("Platform")) | (1 << LayerMask.NameToLayer("Nest"));
        //layerMaskGrounded = ~layerMaskGrounded;
        //layerMaskGripping = ~layerMaskGripping;

        layerMaskGrounded = (1 << LayerMask.NameToLayer("Terrain")) | (1 << LayerMask.NameToLayer("Platform"));
        layerMaskGripping = (1 << LayerMask.NameToLayer("Terrain"));
        
        Manager.Player.SetPlayer(this.gameObject);
    }

    private void Update() {
        deltaX = 0.0f;

        if (handleKeyInput) {
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
        if (!Mathf.Approximately(deltaX, 0)) {
            transform.localScale = new Vector3(Mathf.Sign(deltaX), 1, 1);
        }
    
        isGripping = Physics2D.Raycast(transform.position, new Vector2(transform.localScale.x , transform.localScale.y), jumpReset / 2, layerMaskGripping) && ! Physics2D.Raycast(transform.position, Vector2.down, jumpReset * 2, layerMaskGrounded);
        
        //bool rayForward = Physics2D.Raycast(transform.position, new Vector2(transform.localScale.x , transform.localScale.y), jumpReset / 2, layerMaskGripping);
        //bool rayDownard = Physics2D.Raycast(transform.position, Vector2.down, jumpReset * 2, layerMaskGrounded);
        //isGripping = rayDownard && !rayDownard;

        if (isGripping) {
            _body.gravityScale = 0;
            _body.velocity = Vector2.zero;
        } else if (!isGripping && !isDashing) {
            _body.gravityScale = 1;
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && isGripping) {
            StartCoroutine(WallJump());
        } 


        //Input of jumping handling 
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
            _body.velocity = Vector2.up * jumpForce;
        } 

        // Input of dashing handling
        if (Input.GetKeyDown(KeyCode.W) && !isDashing && dashingAvailable) {
            StartCoroutine(Dash());
        }
    }

    public IEnumerator WallJump() {
        if (!isWallJumping) {
            isWallJumping = true;
            handleKeyInput = false;
            _body.velocity =  new Vector2(-transform.localScale.x * 1.5f, .8f) * jumpForce;
            // Switch the sprite
            transform.localScale = new Vector3(- transform.localScale.x, 1, 1);
            float current = dashLength ;
            while (current > 0) {
                current -= Time.deltaTime * 100;
                yield return null;
            }
            
            handleKeyInput = true;
            _body.velocity = new Vector2(0, 0);
            isWallJumping = false;
        }
    }

    public IEnumerator Dash() {
    
        _body.gravityScale = 0;
        _body.velocity = Vector2.zero;

        GameObject effect = Instantiate(particles, transform.position, Quaternion.identity);
        float current = dashLength;

        isDashing = true;
        dashingAvailable = false;

        while (current > 0) {
            current -= Time.deltaTime * 100;
            _body.velocity = new Vector2(transform.localScale.x * 10, _body.velocity.y);
            yield return null;
        }

        //_body.gravityScale = 1;
        isDashing = false;
        StartCoroutine(DashTimer());

    }

    private IEnumerator DashTimer() {

        float time = 0;
        Messenger<float, float>.Broadcast(GameEvent.DASH_DELAY_UPDATED, time, dashingDelay);

        while (time < dashingDelay) {
            yield return new WaitForSeconds(1);
            time += 1;
            Messenger<float, float>.Broadcast(GameEvent.DASH_DELAY_UPDATED, time, dashingDelay);
        }

        Messenger<float, float>.Broadcast(GameEvent.DASH_DELAY_UPDATED, time, dashingDelay);
        dashingAvailable = true;
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.CompareTag("Jumps")) {
            _body.gravityScale = 1;
            _body.velocity = Vector2.up * jumpForce;
        } else if (collision.CompareTag("Check Point")) {
            Manager.Progress.SetCheckPoint(collision.gameObject.transform);
        } else if (collision.CompareTag("Mega Jumps")) {
            _body.gravityScale = 1;
            _body.velocity = Vector2.up * jumpForce * this.megaJumpRatio;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {

        if (collision.gameObject.CompareTag("Spike")) {
            Manager.Player.Hurt(1);
            //Manager.Player.RespawnAtCheckPoint();
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawRay(transform.position, Vector2.down * jumpReset);
    }
}
