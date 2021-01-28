using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPlatform : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var target = collision.gameObject.GetComponent<IGravityChangeable>();

        if (target != null)
        {
            target.ChangeGravity();
        }
    }
    
}
