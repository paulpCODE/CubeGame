using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
[CustomGridBrush(true, false, false, "Prefab Brush")]
public class PrefabBrush : GridBrushBase
{
    public GameObject upArrowPrefab;

    public float xOffset = .5f;
    public float yOffset = .5f;

    //private bool isArrowUp = true;

    public override void Paint(GridLayout gridLayout, GameObject brushTarget, Vector3Int cellPosition)
    {
        Vector3 correctPositionOfGameObject = new Vector3(cellPosition.x + xOffset, cellPosition.y + yOffset, 0);

        GameObject go;

        go = Instantiate(upArrowPrefab);


        go.transform.SetParent(brushTarget.transform);
        go.transform.position = gridLayout.LocalToWorld(gridLayout.CellToLocalInterpolated(correctPositionOfGameObject));
    }
}