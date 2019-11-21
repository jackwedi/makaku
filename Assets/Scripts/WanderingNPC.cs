using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingNPC : MonoBehaviour
{
    [SerializeField] private float speed = 150f;
    [SerializeField] private float jumpForce = 6.0f;
    [SerializeField] private float jumpReset = .6f;

    private bool _wandering = true;

    private Rigidbody2D _body;
    private Animator _anim;
    private BoxCollider2D _box;

    private bool isGrounded;

    private float jumpTimer = 5f;

    private float direction = 1;

    private int layerRaycastMask = 0;

    public float Speed() {
        return this.speed;
    }

    public float JumpForce() {
        return this.jumpForce;
    }

    public float JumpReset() {
        return this.jumpForce;
    }

    public float Direction() {
        return this.direction;
    }

    public void setWandering(bool param) {
        this._wandering = param;
    }

    public int getLayerMask() {
        return this.layerRaycastMask;
    }

    public void setLayerMask(int mask) {
        this.layerRaycastMask = mask;
    }

    public void setSpeed(float p_speed) {
        this.speed = p_speed;
    }


    private void Start() {
        _body = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _box = GetComponent<BoxCollider2D>();

        //InvokeRepeating("ChangeDirection", 0f, 2f);

        this.layerRaycastMask = (1 << LayerMask.NameToLayer("Terrain"));
        //this.layerRaycastMask = ~this.layerRaycastMask;
    }

    public void ChangeDirection() {
        direction = Random.Range(-1, 1);
        direction = Mathf.Sign(direction);
    }

    private void Update() {

        if (!this._wandering) return;

        // Speed movement
        float deltaX = direction * speed * Time.deltaTime;
        Vector2 movement = new Vector2(deltaX, _body.velocity.y);
        _body.velocity = movement;
        _anim.SetFloat("speed", Mathf.Abs(deltaX));

        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, jumpReset);
        _anim.SetBool("grounded", isGrounded);

        RaycastHit2D wall = Physics2D.Raycast(transform.position, new Vector2(direction, 0), jumpReset, this.layerRaycastMask);
        if (wall) {
            direction = -direction;
        }

        // Flips the sprite dependind on the sign of deltaX
        if (!Mathf.Approximately(deltaX, 0)) {
            transform.localScale = new Vector3(Mathf.Sign(deltaX), 1, 1);
        }
        
        jumpTimer -= Time.deltaTime;

        if (jumpTimer < 0f && isGrounded) {

            _body.velocity = Vector2.up * jumpForce;
            jumpTimer = Random.Range(2f, 5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.CompareTag("Jumps")) {
             _body.velocity = Vector2.up * jumpForce * 1.5f;
        }
    }


    public void setAnimBool(string animationLabel, bool param) {   
        _anim.SetBool(animationLabel, param);
    }

    private void OnDrawGizmos() {
        Gizmos.DrawRay(transform.position, new Vector2(direction, 0)* jumpReset);
    }
 
}
