﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlacer : MonoBehaviour
{
    private BasicGrid grid;

    public GameObject item;

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
                if (hitInfo.collider.tag == "ClickableItem")
                {
                    item = hitInfo.collider.gameObject;
                }
                else if (item != null)
                {
                    PlaceItemNear(hitInfo.point);
                }
            }
        }
    }

    private void PlaceItemNear(Vector3 clickPoint)
    {
        var finalPosition = grid.GetNearestPointOnGrid(clickPoint);
        item.transform.position = finalPosition;
        item = null;


    }
}