using UnityEngine;
using System.Collections.Generic;



[CreateAssetMenu]
[CustomGridBrush(true, false, false, "Prefab Brush")]
public class PrefabBrush : GridBrushBase
{
    public Prefabs Type = Prefabs.None;
    public bool Reflect = false;

    public List<DrawnPrefab> DrawnPrefabs;

    public override void Paint(GridLayout gridLayout, GameObject brushTarget, Vector3Int cellPosition)
    {
        if(Type == Prefabs.None)
        {
            Debug.Log("Change Type in palette window!");
            return;
        }

        if (GetObjectInCell(gridLayout, brushTarget, cellPosition) != null)
        {
            return;
        }

        DrawnPrefab dp = null;
        for(int i = 0; i < DrawnPrefabs.Count; i++)
        {
            if(DrawnPrefabs[i].Type == Type)
            {
                dp = DrawnPrefabs[i];
                break;
            }
        }
        if(dp == null)
        {
            Debug.Log("Hey! In list at palette window no object named " + Type.ToString());
            return;
        }

        GameObject go;
        go = Instantiate(dp.gameObject);
        if (Reflect)
        {
            go.transform.eulerAngles = new Vector3(0, 0, 180);
        }

        Vector3 correctPositionOfGameObject = new Vector3(cellPosition.x + dp.xOffset, cellPosition.y + dp.yOffset, 180);


        go.transform.SetParent(brushTarget.transform);
        go.transform.position = gridLayout.LocalToWorld(gridLayout.CellToLocalInterpolated(correctPositionOfGameObject));
    }

    public override void Erase(GridLayout gridLayout, GameObject brushTarget, Vector3Int cellPosition)
    {
        Transform objTransform = GetObjectInCell(gridLayout, brushTarget, cellPosition);

        if(objTransform == null)
        {
            return;
        }

        DestroyImmediate(objTransform.gameObject);
    }

    private static Transform GetObjectInCell(GridLayout gridLayout, GameObject brushTarget, Vector3Int cellPosition)
    {
        Vector3 min = gridLayout.LocalToWorld(gridLayout.CellToLocalInterpolated(cellPosition));
        Vector3 max = gridLayout.LocalToWorld(gridLayout.CellToLocalInterpolated(cellPosition + Vector3.one));

        Bounds bounds = new Bounds((max + min) * .5f, max - min);

        for(int i = 0; i < brushTarget.transform.childCount; i++)
        {
            Transform child = brushTarget.transform.GetChild(i);

            if(bounds.Contains(child.position))
            {
                return child;
            }
        }

        return null;
    }
}