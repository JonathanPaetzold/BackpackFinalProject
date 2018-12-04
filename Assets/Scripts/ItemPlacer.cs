using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlacer : MonoBehaviour
{
    private BasicGrid grid;

    public Items item;
    public Boolean clickable;

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
                    GameObject temp;
                    temp = hitInfo.collider.gameObject;
                    if (temp.GetComponent<Items>().clickable)
                    {
                        item = temp.GetComponent<Items>();
                    }

                    
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
        var finalPosition = grid.GetNearestPointOnGrid(clickPoint, item);
        item.transform.position = finalPosition;
        item = null;


    }
}