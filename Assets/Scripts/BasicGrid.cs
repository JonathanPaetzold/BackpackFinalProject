using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class BasicGrid : MonoBehaviour
{

    //grid specifics
    [SerializeField]
    private int rows;
    [SerializeField]
    private int cols;
    [SerializeField]
    private Vector2 gridSize;
    [SerializeField]
    private Vector2 gridOffset;

    //about cells
    [SerializeField]
    private Sprite cellSprite;
    private Vector2 cellSize;
    private Vector2 cellScale;

    public Items defualt;
    public Items[,] gridArray;
    private Boolean canPlace;

    private void Start()
    {
        InitCells(); //Initialize all cells
        gridArray = new Items[rows,cols];
        //Vector3 position = new Vector3(5,-9, -0.2f);
        //Quaternion rotation = Quaternion.Euler(0, 0, 0);
        //gridArray[5, 9] = Instantiate(defualt, position, rotation);
        //gridArray[5, 7] = Instantiate(defualt);
        //gridArray[5, 5] = Instantiate(defualt);
    }


    void InitCells()
    {
        GameObject cellObject = new GameObject();

        //creates an empty object and adds a sprite renderer component -> set the sprite to cellSprite
        cellObject.AddComponent<SpriteRenderer>().sprite = cellSprite;

        //catch the size of the sprite
        cellSize = cellSprite.bounds.size;

        //get the new cell size -> adjust the size of the cells to fit the size of the grid
        Vector2 newCellSize = new Vector2(gridSize.x / (float)cols, gridSize.y / (float)rows);

        //Get the scales so you can scale the cells and change their size to fit the grid
        cellScale.x = newCellSize.x / cellSize.x;
        cellScale.y = newCellSize.y / cellSize.y;

        cellSize = newCellSize; //the size will be replaced by the new computed size, we just used cellSize for computing the scale

        cellObject.transform.localScale = new Vector2(cellScale.x, cellScale.y);

        //fix the cells to the grid by getting the half of the grid and cells add and minus experiment
        gridOffset.x = -(gridSize.x / 2) + cellSize.x / 2;
        gridOffset.y = -(gridSize.y / 2) + cellSize.y / 2;

        //fill the grid with cells by using Instantiate
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                //add the cell size so that no two cells will have the same x and y position
                Vector2 pos = new Vector2(col * cellSize.x + gridOffset.x + transform.position.x, row * cellSize.y + gridOffset.y + transform.position.y);

                //instantiate the game object, at position pos, with rotation set to identity
                GameObject cO = Instantiate(cellObject, pos, Quaternion.identity) as GameObject;

                //set the parent of the cell to GRID so you can move the cells together with the grid;
                cO.transform.parent = transform;
            }
        }

        //destroy the object used to instantiate the cells
        Destroy(cellObject);
    }

    //so you can see the width and height of the grid on editor
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, gridSize);
    }


    public Vector3 GetNearestPointOnGrid(Vector3 position, Items item)
    {
        //position -= transform.parent.parent.position;

        int xCount = Mathf.RoundToInt(position.x * 10);
        int yCount = Mathf.RoundToInt(position.y * 8);
        int zCount = Mathf.RoundToInt(position.z * 10);

        Vector3 result = new Vector3(
            (float)(xCount / 10),
            (float)yCount / 8,
            (float)(zCount / 10));

        //result += transform.parent.parent.position;
        PlaceInGrid(result, item);
        return result;
    }

    private void PlaceInGrid(Vector3 position, Items item)
    {
        canPlace = true;

        for (int row = 0; row < item.rows; row++)
        {
            for (int col = 0; col < item.cols; col++)
            {
                int tempX = (int)(position.x - row);
                int tempY = (int)(-position.y + col);
                //Debug.Log(tempX.ToString() + " " + tempY.ToString());

                if (gridArray[(int)position.x - row, (int)(-position.y) + col] != null)
                {
                    Debug.Log("overlap");
                    canPlace = false;
                }
            }
        }



        if (canPlace)
        {
            for (int row = 0; row < item.rows; row++)
            {
                for (int col = 0; col < item.cols; col++)
                {
                    gridArray[(int)position.x - row, (int)(-position.y) + col] = item;
                }
            }

            item.SetPosition(position);
        }
        else
        {
            item.transform.position = item.initPos;
        }


    }

    public void Interaction()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Items temp = gridArray[row, col];
                if (temp != null)
                {
                    SpriteRenderer render = temp.GetComponent<SpriteRenderer>();
                    float combinedWeight = 0;
                    combinedWeight = CheckWeights(temp, row, col);
                    Boolean punctured = false;
                    if (temp.punctureable)
                    {
                        punctured = CheckPuncture(temp, row, col, temp.cols, temp.rows);
                    }
                    if (punctured)
                    {
                        render.sprite = temp.extra;
                    }
                    //Debug.Log(row.ToString() + col.ToString() + "toughness" + temp.toughness.ToString());
                    if (combinedWeight > temp.toughness)
                    {
                        Debug.Log(combinedWeight.ToString() + "Toughness: " + temp.toughness.ToString() +"Position: " + row.ToString() + col.ToString());
                        temp.damaged = true;
                        temp.interacted = true;
                        render.sprite = temp.extra;

                    }

                }   
            }
        }
    }

    public void Debugging()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Items temp = gridArray[row, col];
                if (temp != null)
                {
                    Debug.Log(gridArray[row, col] + row.ToString() + col.ToString() + " " + temp.damaged);
                }



            }
        }
    }

    private float CheckWeights(Items item, int x, int y)
    {
        float totalWeight = 0;

        for (int col = y - 1; col >= 0; col--)
        {
            Items temp = gridArray[x, col];
            if (temp != null)
            {
                totalWeight += temp.weight;
            }
        }
        //Debug.Log(totalWeight.ToString() + "Position: " + x.ToString() + y.ToString());
        return totalWeight;

    }

    public Boolean CheckPuncture(Items item, int x, int y, int cols, int rows)
    {
        Boolean tempbol = false;

        for (int col = y - 1; col >= y - cols; col--)
        {
            Items temp = gridArray[x, col];
            if (temp != null && temp.canPuncture && item.punctureable)
            {
                float randValue = Random.value;
                if (randValue < .30f)
                {
                    tempbol = true;
                }

            }
        }

        for (int row = x - 1; row >= x - rows; row--)
        {
            if (x != 0)
            {
                Items temp = gridArray[row, y];
                if (temp != null && temp.canPuncture && item.punctureable)
                {
                    float randValue = Random.value;
                    if (randValue < .30f)
                    {
                        tempbol = true;
                    }

                }
            }
        }

        return tempbol;
    }

    public void RemoveInstances(Items item) 
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                if (gridArray[row, col] == item)
                {
                    gridArray[row, col] = null;
                }

            }
        }

    }




}