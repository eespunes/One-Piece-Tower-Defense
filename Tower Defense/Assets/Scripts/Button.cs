using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public GameObject character, parent;
    private GameObject selected;

    public int price;

    private bool bought;

    private GameController gc;
    private MyGrid grid;
    private List<Place> nodes;

    private void Start()
    {
        bought = false;

        gc = GameObject.Find("GameController").GetComponent<GameController>();
        grid = GameObject.Find("MyGrid").GetComponent<MyGrid>();
        nodes = new List<Place>();
        GameObject go = GameObject.Find("Nodes");
        foreach (Node n in grid.TheNodes)
        {
            if (n.Walkable)
                nodes.Add(go.transform.Find(n.Pos_x + "," + n.Pos_y).GetComponent<Place>());
        }

    }

    private void OnMouseDown()
    {
        if (gc.coins >= price)
        {
            bought = true;
            foreach (Place p in nodes)
            {
                p.LightUp();
            }
            gc.LoseCoins(price);
            selected = Instantiate(character);
            selected = selected.transform.Find(gameObject.name).gameObject;
            selected.transform.position = transform.position;
        }
    }
    private void OnMouseDrag()
    {
        if (bought)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            selected.transform.position = new Vector3(pos.x, 0, pos.z);
        }
    }
    private void OnMouseUp()
    {
        if (bought)
        {
            foreach (Place p in nodes)
            {
                p.LightDown();
            }
            selected.transform.parent.parent = parent.transform;
            selected.GetComponent<Tower>().InPlace = false;
            selected.GetComponent<Tower>().Place(this);
            bought = false;
        }
    }

    public void ReturnMoney()
    {
        gc.AddCoins(price);
    }
}
