using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            this.gameObject.SetActive(false);
            Messenger.Broadcast(GameEvent.FRUIT_COLLECTED.ToString());
        }
    }
}
