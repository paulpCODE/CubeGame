using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IMovable
{
    public void MoveToPosition(Vector3 position, float timeInSec);
}
