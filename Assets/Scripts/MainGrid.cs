using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshRenderer))]
public class MainGrid : MonoBehaviour
{

    public Vector2 gridSize = new Vector2(1, 1);

    Vector2 gridOffset;
    Vector3 lastGridSize;
    Vector3 lastPosition;

    public List<Vector4> occupiedPositions;

    void Awake()
    {
        UpdateScale();
    }

    void Update()
    {

        // If the transform has changed
        if (transform.hasChanged)
        {

            // Update the grid texture size and fix positions of the draggable gameobjects
            transform.hasChanged = false;
            occupiedPositions.Clear();
            GetComponent<Renderer>().material.mainTextureScale = gridSize;
            FixPositions();

        }

        // If grid size has changed
        if ((gridSize.x != lastGridSize.x) || (gridSize.y != lastGridSize.y))
        {
  
            lastGridSize = gridSize;
            UpdateScale();

        }
    }

    // Update transform and texture size
    void UpdateScale()
    {
        transform.localScale = new Vector3(gridSize.x, gridSize.y, 1);
        GetComponent<Renderer>().material.mainTextureScale = gridSize;
    }

    // Fix draggable gameobjects positions
    void FixPositions()
    {
        var objs = FindObjectsOfType<Drag>();
        var diff = transform.localPosition - lastPosition;
        foreach (Drag i in objs)
        {
            i.FixPosition(diff);
            i.UpdateAll();
        }
        lastPosition = transform.localPosition;
    }

    public Vector2 GetGridOffset()
    {
        gridOffset.x = transform.localPosition.x;
        gridOffset.y = transform.localPosition.y;
        return gridOffset;
    }
}