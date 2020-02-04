using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node
{
    private int pos_x, pos_y;
    private int h, g, f;

    private bool corridor;
    private bool walkable;
    private bool occupied;

    private Vector3 coord;

    private Node mother;

    public Node(Vector3 c, int px, int py, bool walk, bool occupied)
    {
        coord = c;
        pos_x = px;
        pos_y = py;
        walkable = walk;
        h = 0;
        g = 0;
        f = 0;
        this.occupied = occupied;
        mother = null;
        corridor = false;
    }

    //Gets and setters
    public bool Walkable
    {
        get
        {
            return walkable;
        }

        set
        {
            walkable = value;
        }
    }

    public int Pos_x
    {
        get
        {
            return pos_x;
        }

        set
        {
            pos_x = value;
        }
    }

    public int Pos_y
    {
        get
        {
            return pos_y;
        }

        set
        {
            pos_y = value;
        }
    }

    public Vector3 Coord
    {
        get
        {
            return coord;
        }

        set
        {
            coord = value;
        }
    }



    public Node Mother
    {
        get
        {
            return mother;
        }

        set
        {
            mother = value;
        }
    }

    public int H
    {
        get
        {
            return h;
        }

        set
        {
            h = value;
        }
    }

    public int G
    {
        get
        {
            return g;
        }

        set
        {
            g = value;
        }
    }

    public int F
    {
        get
        {
            return f;
        }

        set
        {
            f = value;
        }
    }

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

    public bool Corridor
    {
        get
        {
            return corridor;
        }

        set
        {
            corridor = value;
        }
    }

}