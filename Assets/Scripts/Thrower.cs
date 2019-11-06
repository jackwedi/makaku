 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrower : MonoBehaviour
{
    [SerializeField] private GameObject rockPrefab;
    [SerializeField] private Transform throwingPoint;

    [SerializeField] private float xThrowForce = 10;
    [SerializeField] private float yThrowForce = 4;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            GameObject rock = Instantiate(rockPrefab, throwingPoint.position, Quaternion.identity);

            Rigidbody2D rock_rb = rock.GetComponent<Rigidbody2D>();

            rock_rb.velocity = new Vector2( Mathf.Sign(transform.localScale.x)* xThrowForce, yThrowForce);
        }
    }
}
