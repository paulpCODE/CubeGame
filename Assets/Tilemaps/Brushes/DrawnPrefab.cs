using UnityEngine;

//if you add new type - dont forget to add new object in list in palette window of Prefab Brush.
public enum Prefabs
{
    None,
    Portal,
    Spike,
    DoubleJumpSphere
}

[System.Serializable]
public class DrawnPrefab
{
    public Prefabs Type;

    public GameObject gameObject;

    public float xOffset;
    public float yOffset;
}
