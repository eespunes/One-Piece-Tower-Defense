using Boo.Lang;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGrid : MonoBehaviour
{
    public GameObject player, extras;

    public int sizeX, sizeY;

    public Vector2 start, finish;

    private Node[,] theNodes;
    private System.Collections.Generic.List<Node> occupiedList;

    public LayerMask notWalkable;

    void Awake()
    {
        GameObject go;

        int middleX = sizeX / 2, middleY = sizeY / 2;

        bool walk, occupied;

        Vector3 position;

        theNodes = new Node[sizeX, sizeY];
        occupiedList = new System.Collections.Generic.List<Node>();
        theNodes = new Node[sizeX, sizeY];

        for (int x = 0; x < sizeX; x++)
            for (int y = 0; y < sizeY; y++)
            {
                position = new Vector3((transform.position.x + (x - middleX)), 0, (transform.position.z + (y - middleY)));
                if (Physics.CheckSphere(position, 0.05f, notWalkable))
                {
                    walk = false;
                    occupied = true;
                }
                else
                {

                    occupied = false;
                    walk = true;
                }
                    go = Instantiate(player, position, Quaternion.Euler(90, 0, 0));
                    go.transform.parent = extras.transform;
                    go.name = x + "," + y;
                theNodes[x, y] = new Node(position, x, y, walk, occupied);
            }
    }

    private void OnDrawGizmos()
    {
        int middleX = sizeX / 2, middleY = sizeY / 2;
        for (int x = 0; x < sizeX; x++)
            for (int y = 0; y < sizeY; y++)
            {
                if (Physics.CheckSphere(new Vector3((transform.position.x + (x - middleX)), 0, (transform.position.z + (y - middleY))), 0.05f, notWalkable))
                    Gizmos.color = Color.red;
                else
                    Gizmos.color = Color.white;
                Gizmos.DrawWireCube(new Vector3((transform.position.x + (x - middleX)), 0, (transform.position.z + (y - middleY))), Vector3.one);
            }
    }

    public void AddOccupied(int x, int y)
    {
        theNodes[x, y].Occupied = false;
        occupiedList.Add(theNodes[x, y]);
    }

    //Gets and setters
    public Node[,] TheNodes
    {
        get
        {
            return theNodes;
        }

        set
        {
            theNodes = value;
        }
    }
    public System.Collections.Generic.List<Node> OccupiedList
    {
        get
        {
            return occupiedList;
        }
        set
        {
            occupiedList = value;
        }
    }
}