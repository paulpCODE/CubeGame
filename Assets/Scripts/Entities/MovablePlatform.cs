using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePlatform : MonoBehaviour
{
    [SerializeField] Vector3 moveToPosition;
    [SerializeField] float timeInSeconds;

    private bool move = false;
    private Vector3 deltaVec;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        move = true;
    }

    public void MoveToPosition(Vector3 position, float timeInSec)
    {
        moveToPosition = position;
        timeInSeconds = timeInSec;
        move = true;
    }

    private void Moving()
    {
        if (moveToPosition.x < transform.position.x || moveToPosition.y < transform.position.y)
        {
            move = false;
            return;
        }
        transform.position += deltaVec;
    }

    void Start()
    {
        deltaVec.x = (moveToPosition.x - transform.position.x) / (timeInSeconds / Time.deltaTime);
        deltaVec.y = (moveToPosition.y - transform.position.y) / (timeInSeconds / Time.deltaTime);
    }
    // Update is called once per frame
    void Update()
    {
        if (move)
            Moving();
    }
}