﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baby : MonoBehaviour
{

    private GameObject player = null;
    private CapsuleCollider2D _collider = null;
    private WanderingNPC _npc = null;
    private string _layer;

    [SerializeField] private BabyLayers _layerType = BabyLayers.Baby1;
    [SerializeField] private bool _captured = false;
    [SerializeField] private bool _secured = false;
    [SerializeField] private Vector3 _securedPosition = Vector3.zero;
    [SerializeField] private float _calmSpeedRatio = 0.5f;
    private float direction = -1;
    private int layerMask;
    private Vector3 _initPosition;

    public enum BabyLayers
    {
        Baby1,
        Baby2,
        Baby3,
        Baby4
    }

    public bool getIsCaptured()
    {
        return this._captured;
    }

    private void Start()
    {
        this._collider = this.gameObject.GetComponent<CapsuleCollider2D>();
        this._npc = this.gameObject.GetComponent<WanderingNPC>();
        this._initPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        this._layer = LayerMask.LayerToName(this.gameObject.layer);

        layerMask = (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Baby Limits"));
        Messenger.AddListener(GameEvent.DEATH.ToString(), OnDeath);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!this._secured && !this._captured && collision.gameObject.CompareTag("Player"))
        {
            this.setCaptured(true);
            player = collision.gameObject;
        }
    }

    private void Update()
    {
        this.direction = _npc.Direction();

        if (_captured && !_secured)
        {
            this.transform.position = player.transform.position + new Vector3(0, .2f, 0);
            // Sets the baby behind the player
            this.transform.localScale = new Vector3(-player.transform.localScale.x, player.transform.localScale.y, player.transform.localScale.z);
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(direction, 0f), 0.5f, layerMask);

        if (hit && hit.collider.CompareTag("Baby Limits"))
        {
            _npc.ChangeDirection();
        }
    }

    public void setSecured()
    {

        this._secured = true;
        this.transform.position = this._securedPosition;
        // Ignore
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer(this._layer), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Checkpoints"), LayerMask.NameToLayer(this._layer), true);
        // Notice
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Nest Limits"), LayerMask.NameToLayer(this._layer), false);

        this._collider.isTrigger = false;

        this._npc.setWandering(true);
        this._npc.setLayerMask(this._npc.getLayerMask() | (1 << LayerMask.NameToLayer("Nest Limits")));
        this._npc.setSpeed(this._npc.Speed() * this._calmSpeedRatio);

        this._npc.setAnimBool("captured", false);
    }

    private void OnDeath()
    {
        this.setCaptured(false);
        this.transform.position = this._initPosition;
    }

    private void setCaptured(bool captured)
    {
        _captured = captured;
        this.GetComponent<WanderingNPC>().setWandering(!captured);
        this._collider.isTrigger = captured;
        this._npc.setAnimBool("captured", captured);
    }


}
