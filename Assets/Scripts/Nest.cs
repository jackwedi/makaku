using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nest : MonoBehaviour
{
    [SerializeField] private Baby[] _babies = null;
    private List<GameObject> _rescuedBaby = null;

    private void Start()
    {
        this._rescuedBaby = new List<GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Baby") && !this._rescuedBaby.Contains(other.gameObject) && other.GetComponent<Baby>().getIsCaptured())
        {
            this._rescuedBaby.Add(other.gameObject);
            other.gameObject.GetComponent<Baby>().setSecured();
            Messenger.Broadcast(GameEvent.BABY_SAVED.ToString());
        }
    }
}
