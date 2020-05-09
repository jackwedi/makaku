using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    [SerializeField] private GameObject _particles = null;

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            this.gameObject.SetActive(false);
            Instantiate(_particles, transform.position, Quaternion.identity);
            Messenger.Broadcast(GameEvent.FRUIT_COLLECTED.ToString());
        }
    }
}
