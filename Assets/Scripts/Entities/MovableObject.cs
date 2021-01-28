using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour, IGravityChangeable, IDestroyable
{
    Rigidbody2D rb;

    Vector3 startPosition;

    private void Awake()
    {
        startPosition = transform.position;
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    public void ChangeGravity()
    {
        rb.gravityScale *= -1;
    }

    public void DestroyObject()
    {
        Activator.ActivateMe(gameObject, 1.0f, startPosition);
        gameObject.SetActive(false);
    }
}
