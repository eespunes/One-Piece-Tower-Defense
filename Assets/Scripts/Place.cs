using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Place : MonoBehaviour
{
    private bool occupied;

    string[] s;

    private MyGrid grid;

    public Material transparent, canBePlaced, canNotBePlaced;

    private MeshRenderer mr;

    private void Start()
    {
        occupied = false;

        s = gameObject.name.Split(',');

        mr = GetComponent<MeshRenderer>();
        grid = GameObject.Find("MyGrid").GetComponent<MyGrid>();
    }

    //Show places where you can place the character
    public void LightUp()
    {

        if (!grid.TheNodes[int.Parse(s[0]),int.Parse(s[1])].Occupied)
            mr.material = canBePlaced;
        else
            mr.material = canNotBePlaced;
    }
    public void LightDown()
    {
        mr.material = transparent;
    }

    public void SetOccupied(Tower t)
    {
        if(!grid.TheNodes[int.Parse(s[0]), int.Parse(s[1])].Occupied)
        grid.TheNodes[int.Parse(s[0]), int.Parse(s[1])].Occupied = true;
        else
        {
            Node actual, neighbour;
            List<Node> open, close;
            open = new List<Node>();
            close = new List<Node>();
            open.Add(grid.TheNodes[int.Parse(s[0]), int.Parse(s[1])]);

            while (open.Count > 0)
            {
                actual = open[0];
                close.Add(actual);
                open.Remove(actual);

                if (!actual.Occupied)
                {
                    t.Place(actual.Coord);
                    actual.Occupied = true;
                    return;
                }

                for (int x = actual.Pos_x - 1; x <= actual.Pos_x + 1; x++)
                    for (int y = actual.Pos_y - 1; y <= actual.Pos_y + 1; y++) { 
                            if (actual.Pos_x == x && actual.Pos_y == y)
                                continue;

                            if (x < 0 || y < 0 || x >= grid.sizeX || y >= grid.sizeY)
                                continue;

                            neighbour = grid.TheNodes[x, y];

                            if (neighbour.Occupied || close.Contains(neighbour))
                                continue;

                            open.Add(neighbour);
                        }
            }
        }
    }

    //Get and setter
    public bool Occupied
    {
        get
        {
            return occupied;
        }

        set
        {
            occupied = value;
        }
    }
}
