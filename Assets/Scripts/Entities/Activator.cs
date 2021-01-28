using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{
    public static Activator instance;

    private void Awake()
    {
        instance = this;
    }

    IEnumerator ActivateObject(GameObject objectToActivate, float time)
    {
        yield return new WaitForSeconds(time);
        objectToActivate.SetActive(true);
    }
    IEnumerator ActivateObjectInPosition(GameObject objectToActivate, float time, Vector3 position)
    {
        yield return new WaitForSeconds(time);
        objectToActivate.transform.position = position;
        objectToActivate.SetActive(true);
    }


    public static void ActivateMe(GameObject objectToActivate, float time)
    {
        instance.StartCoroutine(instance.ActivateObject(objectToActivate, time));
    }

    public static void ActivateMe(GameObject objectToActivate, float time, Vector3 position)
    {
        instance.StartCoroutine(instance.ActivateObjectInPosition(objectToActivate, time, position));
    }
}