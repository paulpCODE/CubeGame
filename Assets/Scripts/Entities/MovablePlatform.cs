using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePlatform : MonoBehaviour
{
    [SerializeField] Vector3 toPosition;
    //from 0 to 1
    [SerializeField] float step = 0.01f;

    //if platform activates by player it cant be activated by mechanism or be cyclic and it returns back if player falled from it
    [SerializeField] bool activateByPlayer = false;
    //if platform activates by mechanism it cant be activated by player and cyclic parametr sends by mechanism
    [SerializeField] bool activateByMechanism = false;


    private Vector3 fromPosition;
    private bool move = false;
    //if platform dont activates by player or mechanism it automatically becomes cyclic and moves always
    private bool cyclic = false;
    //0 - start position, 1 - end position 
    private bool state = false;
    //t parametr of Vector3.Lerp.
    private float t;

    private EdgeCollider2D edgecollider;

    public float T
    {
        get { return t; }
        private set
        {
            if (value > 1)
                t = 1;
            else if (value < 0)
                t = 0;
            else
                t = value;
        }
    }

    private void Start()
    {
        fromPosition = transform.position;
        T = 0;

        edgecollider = gameObject.GetComponent<EdgeCollider2D>();

        if (!activateByPlayer && !activateByMechanism)
        {
            move = true;
            cyclic = true;
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (move)
        {
            T += step;
            transform.position = Vector3.Lerp(fromPosition, toPosition, T);

            if (T == 1)
            {
                move = false;
                state = !state;

                if (cyclic || activateByPlayer)
                    StartCoroutine(Reloud());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger");
        if (activateByPlayer)
            move = true;
    }

    private IEnumerator Reloud()
    {
        yield return new WaitForSeconds(1.0f);

        toPosition = fromPosition;
        fromPosition = transform.position;
        T = 0;

        if (activateByPlayer && !state && !edgecollider.IsTouchingLayers(LayerMask.GetMask("Player")))
            yield break;

        move = true;
    }
}