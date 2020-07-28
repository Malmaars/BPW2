using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public LayerMask TileLayer;
    public GameObject nodePrefab;
    private List<node> open, closed;

    class node
    {
        public Vector2 pos { get; set; }
        public  float fCost { get; set; }
        public Transform myTransform { get; set; }
    }

    public void findPath(Vector2 startLoc, Vector2 target)
    {
        open = new List<node>();
        closed = new List<node>();
        node currentNode;
        node startNode = new node { pos = startLoc, fCost = 0f, myTransform = Instantiate(nodePrefab, startLoc, new Quaternion(0, 0, 0, 0)).transform };
        open.Add(startNode);
        currentNode = startNode;

        //The lower the fCost, the more efficient the route. Lowest fCost always wins
        float globalfCost = 0;

        //Find a path until we reach the target
        while (currentNode.pos != target)
        {
            foreach (node openNode in open)
            {
                float fCostTarget = Vector2.Distance(openNode.pos, target);
                float fCostStart = Vector2.Distance(openNode.pos, startLoc);

                openNode.fCost = fCostStart + fCostTarget;

                if (openNode.fCost <= globalfCost || globalfCost == 0)
                {
                    globalfCost = openNode.fCost;
                    currentNode = openNode;
                    closed.Add(openNode);
                    open.Remove(openNode);
                }


                List<node> neighbours;
                neighbours = new List<node>();
                neighbours.Add(new node { pos = new Vector2(currentNode.pos.x, currentNode.pos.y - 1), fCost = 0f });
                neighbours.Add(new node { pos = new Vector2(currentNode.pos.x, currentNode.pos.y + 1), fCost = 0f });
                neighbours.Add(new node { pos = new Vector2(currentNode.pos.x - 1, currentNode.pos.y - 1), fCost = 0f });
                neighbours.Add(new node { pos = new Vector2(currentNode.pos.x - 1, currentNode.pos.y), fCost = 0f });
                neighbours.Add(new node { pos = new Vector2(currentNode.pos.x - 1, currentNode.pos.y + 1), fCost = 0f });
                neighbours.Add(new node { pos = new Vector2(currentNode.pos.x + 1, currentNode.pos.y - 1), fCost = 0f });
                neighbours.Add(new node { pos = new Vector2(currentNode.pos.x + 1, currentNode.pos.y), fCost = 0f });
                neighbours.Add(new node { pos = new Vector2(currentNode.pos.x + 1, currentNode.pos.y + 1), fCost = 0f });

                foreach (node neighbour in neighbours)
                {
                    if (neighbour.myTransform == null)
                    {
                        neighbour.myTransform = Instantiate(nodePrefab, neighbour.pos, new Quaternion(0, 0, 0, 0)).transform;
                    }
                    neighbour.fCost = Vector2.Distance(neighbour.pos, target) + Vector2.Distance(neighbour.pos, startLoc);
                    if (closed.Contains(neighbour) || Physics2D.OverlapPoint(neighbour.pos, TileLayer).gameObject.tag != "Walkable" || Physics2D.OverlapPoint(neighbour.pos, TileLayer).gameObject == null)
                    {
                        continue;
                    }

                    if (!open.Contains(neighbour))
                    {
                        neighbour.myTransform.SetParent(currentNode.myTransform);
                        open.Add(neighbour);
                    }
                }
            }
        }
        if(currentNode.pos == target)
        {
            Debug.Log(currentNode.pos);
        }

    }
}
