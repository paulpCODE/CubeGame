using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : MonoBehaviour, PowerUp 
{
    public static Action OnDoubleJumpEntered;

    public void UsePowerUp()
    {
        if (OnDoubleJumpEntered == null)
            return;

        OnDoubleJumpEntered();
        Activator.ActivateMe(gameObject, 2.0f);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        UsePowerUp();
    }
}
