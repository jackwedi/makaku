using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveCollider : MonoBehaviour
{
    [SerializeField] private GameEvent _event;
    
    private void OnCollision2DEnter(Collision other) {
        if (other.gameObject.CompareTag("Player")) {
            Messenger.Broadcast(_event.ToString());
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            Messenger.Broadcast(_event.ToString());
        }
    }
}
