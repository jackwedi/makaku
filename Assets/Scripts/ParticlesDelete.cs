using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesDelete : MonoBehaviour
{
    // Start is called before the first frame update
    public float deathTimer = 2f;

    void Start()
    {
        Destroy(gameObject, deathTimer);
    }

}
