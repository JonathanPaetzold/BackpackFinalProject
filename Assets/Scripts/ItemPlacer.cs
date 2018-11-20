using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlacer : MonoBehaviour
{
    private BasicGrid grid;

    public GameObject flashlight;

    private void Awake()
    {
        grid = FindObjectOfType<BasicGrid>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitInfo))
            {
                PlaceCubeNear(hitInfo.point);
            }
        }
    }

    private void PlaceCubeNear(Vector3 clickPoint)
    {
        var finalPosition = grid.GetNearestPointOnGrid(clickPoint);
        GameObject item = Instantiate(flashlight);
        item.transform.position = finalPosition;
        //GameObject.CreatePrimitive(PrimitiveType.Cube).transform.position = finalPosition;

    }
}