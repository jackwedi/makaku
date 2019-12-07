using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private UnityEngine.Sprite _jumpON;
    [SerializeField] private UnityEngine.Sprite _jumpON2;
    [SerializeField] private UnityEngine.Sprite _jumpOFF;

    private SpriteRenderer _renderer;
    private Collider2D _collider;

    private Animator _animator;

    private int state = 0;

    private void Start()
    {
        _renderer = this.gameObject.GetComponent<SpriteRenderer>();
        _collider = this.gameObject.GetComponent<Collider2D>();
        _animator = this.gameObject.GetComponent<Animator>();
        /* state = 0; */
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            switch (state)
            {
                case 0:
                    _renderer.sprite = _jumpON2;
                    state++;
                    Invoke("enable", 2.0f);
                    break;
                case 1:
                    disable();
                    break;
            }
        }
    }

    private void disable()
    {
        _animator.enabled = false;
        _collider.enabled = false;
        _renderer.sprite = _jumpOFF;
        CancelInvoke("enable");

        Invoke("enable", 2.0f);
    }

    private void enable()
    {
        _collider.enabled = true;
        _renderer.sprite = _jumpON;
        state = 0;
    }
}
