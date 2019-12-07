using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private UnityEngine.SpriteRenderer _treeLeaves;
    private void OnTriggerEnter2D(Collider2D other)
    {
        _treeLeaves.color = Color.white;
    }
}
