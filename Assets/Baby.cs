using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baby : MonoBehaviour
{

    private GameObject player = null;
    private CapsuleCollider2D _collider  = null;
    private WanderingNPC _npc = null;
    private string _layer;

    [SerializeField] private BabyLayers _layerType;
    [SerializeField] private bool _captured = false;
    [SerializeField] private bool _secured = false;
    [SerializeField] private Vector3 _securedPosition;
    [SerializeField] private float _calmSpeedRatio = 0.5f;
    public enum BabyLayers {
        Baby1,
        Baby2
        
    }

    public bool getIsCaptured() {
        return this._captured;
    }

    private void Start() {
        this._collider = this.gameObject.GetComponent<CapsuleCollider2D>();
        this._npc = this.gameObject.GetComponent<WanderingNPC>();

        switch (_layerType) {

            case BabyLayers.Baby1 :
                this._layer = "Baby";
                break;
            case BabyLayers.Baby2 :
                this._layer = "Baby 2";
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log(this._layerType + " " + collision.transform.tag);
        if (!this._secured && !this._captured && collision.gameObject.CompareTag("Player")) {
            _captured = true;
            this.GetComponent<WanderingNPC>().setWandering(false);
            this._collider.isTrigger = true;
            this._npc.setAnimBool("captured", true);

            player = collision.gameObject;
        }
    }

    private void Update() {

        if (_captured && !_secured) {
            this.transform.position = player.transform.position + new Vector3(0,.2f,0);
            // Sets the baby behind the player
            this.transform.localScale =  new Vector3(- player.transform.localScale.x, player.transform.localScale.y, player.transform.localScale.z);
        }
    }

    public void setSecured() {

        this._secured = true;
        this.transform.position = this._securedPosition;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer(this._layer), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Checkpoints"), LayerMask.NameToLayer(this._layer), true);

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Nest Limits"), LayerMask.NameToLayer(this._layer), false);

        this._collider.isTrigger = false;
        
        this._npc.setWandering(true);
        this._npc.setLayerMask(this._npc.getLayerMask() | (1 << LayerMask.NameToLayer("Nest Limits")));
        this._npc.setSpeed(this._npc.Speed() * this._calmSpeedRatio);
        
        this._npc.setAnimBool("captured", false);
    }

}
