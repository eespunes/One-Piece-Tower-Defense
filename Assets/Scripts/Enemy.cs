using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    public GameObject[] directions;
    public GameObject particle;

    public int score, coins;
    private int direction;

    public float v;
    public float life;

    private MyGrid grid;
    private GameController gc;
    private Node next;
    private Node[,] theNodes;
    private List<Node> path;

    public Slider slider;

    void Start()
    {
        direction = 0;

        grid = GameObject.Find("MyGrid").GetComponent<MyGrid>();
        gc = GameObject.Find("GameController").GetComponent<GameController>();
        next = grid.TheNodes[(int)grid.start.x, (int)grid.start.y];
        transform.position = next.Coord;

        path = GetPath(grid.TheNodes[(int)grid.finish.x, (int)grid.finish.y], next);

        slider.maxValue = life;
        slider.value = life;

        AllDirectionsFalse();
        directions[direction].SetActive(true);
    }

    private void Update()
    {
        if (transform.position == next.Coord)
        {
            if (path.Count > 1)
            {
                Node actual = next;
                path.RemoveAt(0);
                next = path[0];

                if (actual.Pos_x != next.Pos_x)
                {
                    if (next.Pos_x > actual.Pos_x)
                        direction = 3;
                    else
                        direction = 1;
                }
                else
                {
                    if (next.Pos_y > actual.Pos_y)
                        direction = 2;
                    else
                        direction = 0;
                }
                AllDirectionsFalse();
                directions[direction].SetActive(true);
            }
            else
                Die();

        }
        else
            transform.position = Vector3.MoveTowards(transform.position, next.Coord, v);
    }
    private void AllDirectionsFalse()
    {
        foreach (GameObject go in directions)
            go.SetActive(false);
    }

    public void Hurt(float damage, Tower t)
    {
        life -= damage;
        if (life <= 0)
        {
            Die(t);
        }
        slider.value = life;
    }

    private void Die(Tower t)
    {
        gc.AddCoins(coins);
        gc.AddScore(score);
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Tower"))
        {
            go.GetComponent<Tower>().HasBeenKilled = true;
            go.GetComponent<Tower>().Enemies.Remove(gameObject);
        }
        Instantiate(particle).transform.position = transform.position;
        if (transform.parent.name == "Enemies")
            Destroy(gameObject);
        else
            Destroy(transform.parent.gameObject);
    }
    private void Die()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Tower"))
        {
            if (go.GetComponent<Tower>().Enemies.Contains(gameObject))
            {
                go.GetComponent<Tower>().HasBeenKilled = true;
                go.GetComponent<Tower>().Enemies.Remove(gameObject);
            }
        }
        gc.LoseLife();
        if (transform.parent.name == "Enemies")
            Destroy(gameObject);
        else
            Destroy(transform.parent.gameObject);
    }

    private List<Node> GetPath(Node start, Node end)
    {
        Node actual, neighbour;
        List<Node> open, close;
        open = new List<Node>();
        close = new List<Node>();
        open.Add(start);

        while (open.Count > 0)
        {
            actual = open[0];
            close.Add(actual);
            open.Remove(actual);

            if (actual.Coord == end.Coord)
                return CalculatePath(actual, close.Count);

            for (int x = actual.Pos_x - 1; x <= actual.Pos_x + 1; x++)
                for (int y = actual.Pos_y - 1; y <= actual.Pos_y + 1; y++)
                    if (actual.Pos_x == x || actual.Pos_y == y)
                    {
                        if (actual.Pos_x == x && actual.Pos_y == y)
                            continue;

                        if (x < 0 || y < 0 || x >= grid.sizeX || y >= grid.sizeY)
                            continue;

                        neighbour = grid.TheNodes[x, y];

                        if (neighbour.Walkable || close.Contains(neighbour))
                            continue;

                        neighbour.Mother = actual;
                        open.Add(neighbour);
                    }
        }
        return null;

    }
    private List<Node> CalculatePath(Node actual, int length)
    {
        List<Node> path = new List<Node>();
        path.Add(actual);
        Node n = actual;
        int i = 0;
        while (n.Mother != null && i <= length)
        {
            n = n.Mother;
            if (!path.Contains(n))
                path.Add(n);
            i++;
        }
        return path;
    }

}
