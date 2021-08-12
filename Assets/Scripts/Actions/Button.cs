using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] Sprite On;
    [SerializeField] Sprite Off;

    SpriteRenderer sr;


    private void Awake()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var target = collision.gameObject.GetComponent<Rigidbody2D>();

        if(target == null)
        {
            return;
        }

        ButtonOn();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var target = collision.gameObject.GetComponent<Rigidbody2D>();

        if (target == null)
        {
            return;
        }

        ButtonOff();
    }

    private void ButtonOn()
    {
        sr.sprite = On;  
    }

    private void ButtonOff()
    {
        sr.sprite = Off;
    }
}
