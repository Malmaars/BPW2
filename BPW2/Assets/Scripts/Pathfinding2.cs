using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding2 : MonoBehaviour
{
    public LayerMask TileLayer;
    public GameObject nodePrefab;
    private List<node> open, closed;

    class node
    { 
        //a node consists of: A position, an fCost and a transform in the editor.
        public Vector2 pos { get; set; }

        //Gcost is the distance from the node to the starting position
        public float gCost { get; set; }

        //Hcost is the distance from the node to the target position
        public float hCost { get; set; }

        //The fCost is GCost + hCost
        public float fCost { get; set; }
        public Transform myTransform { get; set; }
    }

    public Transform findPath(Vector2 startLoc, Vector2 target)
    {
        //list of all the nodes that still need to be checked
        open = new List<node>();

        //nodes that have already been checked
        closed = new List<node>();

        //The node we're currently on
        node currentNode;

        //The first node, where we start
        node startNode = new node { pos = startLoc, gCost = 0f , hCost = 0f, fCost = 0f, myTransform = Instantiate(nodePrefab, startLoc, new Quaternion(0, 0, 0, 0)).transform};
        currentNode = startNode;

        //We run the code until we reach the destination
        while(currentNode.pos != target)
        {
            //This is the global fCost to check what the lowest fCost is
            float fcostTemp = 0;
            foreach(node opennode in open)
            {
                //We set the fCost for each node
                opennode.fCost = Vector2.Distance(opennode.pos, startLoc) + Vector2.Distance(opennode.pos, target);

                //In here we check if the fCost is lower than the last fCost that was saved. This way we work down the list and eventually get the node with the lowest fCost
                if (fcostTemp == 0 || opennode.fCost < fcostTemp)
                {
                    currentNode = opennode;
                    fcostTemp = opennode.fCost;
                }
            }
            //We add the current Node to the closed list so we don't check it again
            open.Remove(currentNode);
            closed.Add(currentNode);

            //A list of the nodes neighbouring the current node. There might be a more efficient way to do this.
            List<node> neighbours;
            neighbours = new List<node>();
            neighbours.Add(new node { pos = new Vector2(currentNode.pos.x, currentNode.pos.y - 1), gCost = 0f, hCost = 0f, fCost = 0f });
            neighbours.Add(new node { pos = new Vector2(currentNode.pos.x, currentNode.pos.y + 1), gCost = 0f, hCost = 0f, fCost = 0f });
            neighbours.Add(new node { pos = new Vector2(currentNode.pos.x - 1, currentNode.pos.y - 1), gCost = 0f, hCost = 0f, fCost = 0f });
            neighbours.Add(new node { pos = new Vector2(currentNode.pos.x - 1, currentNode.pos.y), gCost = 0f, hCost = 0f, fCost = 0f });
            neighbours.Add(new node { pos = new Vector2(currentNode.pos.x - 1, currentNode.pos.y + 1), gCost = 0f, hCost = 0f, fCost = 0f });
            neighbours.Add(new node { pos = new Vector2(currentNode.pos.x + 1, currentNode.pos.y - 1), gCost = 0f, hCost = 0f, fCost = 0f });
            neighbours.Add(new node { pos = new Vector2(currentNode.pos.x + 1, currentNode.pos.y), gCost = 0f, hCost = 0f, fCost = 0f });
            neighbours.Add(new node { pos = new Vector2(currentNode.pos.x + 1, currentNode.pos.y + 1), gCost = 0f, hCost = 0f, fCost = 0f });

            foreach(node neighbour in neighbours)
            {
                //calculate the g- and hCost of each neighbour
                neighbour.gCost = Vector2.Distance(neighbour.pos, startLoc);
                neighbour.hCost = Vector2.Distance(neighbour.pos, target);

                //I'm checking if the nodes already exist in the closed list. The neighbours are new nodes, so I check if the positions already exist in the closed list
                List<Vector2> closedPoss = new List<Vector2>();
                foreach (node closednode in closed)
                {
                    closedPoss.Add(closednode.pos);
                }

                //If the nodes are already in the closed list or the node is on a non walkable tile, we skip to the next neighbour
                if (Physics2D.OverlapPoint(neighbour.pos, TileLayer) == null || Physics2D.OverlapPoint(neighbour.pos, TileLayer).gameObject == null || closedPoss.Contains(neighbour.pos) || Physics2D.OverlapPoint(neighbour.pos, TileLayer).gameObject.tag != "Walkable")
                {
                    continue;
                }

                //Same thing as the closedPos list
                List<Vector2> openPoss = new List<Vector2>();
                foreach (node opentemp in open)
                {
                    openPoss.Add(opentemp.pos);
                }

                //If the node isn't already in the open list: add it
                if (!openPoss.Contains(neighbour.pos))
                {
                    if (neighbour.myTransform == null)
                    {
                        neighbour.myTransform = Instantiate(nodePrefab, neighbour.pos, new Quaternion(0, 0, 0, 0)).transform;
                    }
                    neighbour.fCost = Vector2.Distance(neighbour.pos, target) + Vector2.Distance(neighbour.pos, startLoc);
                    neighbour.myTransform.SetParent(currentNode.myTransform);
                    open.Add(neighbour);
                }
            }

            //If we reach the target destination, destroy all nodes that aren't on the route
            if (currentNode.pos == target)
            {
                foreach (node opens in open)
                {
                    Destroy(opens.myTransform.gameObject);
                }

                foreach(node closedOnes in closed)
                {
                    if(closedOnes == currentNode || currentNode.myTransform.IsChildOf(closedOnes.myTransform))
                    {
                        continue;
                    }
                    Destroy(closedOnes.myTransform.gameObject);
                }
                break;
            }
        }
        //Debug.Log(currentNode.pos);
        return currentNode.myTransform;
    }

    public Transform findRoomPath(Vector2 startLoc, Vector2 target)
    {
        //list of all the nodes that still need to be checked
        open = new List<node>();

        //nodes that have already been checked
        closed = new List<node>();

        //The node we're currently on
        node currentNode;

        //The first node, where we start
        node startNode = new node { pos = startLoc, gCost = 0f, hCost = 0f, fCost = 0f, myTransform = Instantiate(nodePrefab, startLoc, new Quaternion(0, 0, 0, 0)).transform };
        currentNode = startNode;

        //We run the code until we reach the destination
        while (currentNode.pos != target)
        {
            //This is the global fCost to check what the lowest fCost is
            float fcostTemp = 0;
            foreach (node opennode in open)
            {
                //We set the fCost for each node
                opennode.fCost = Vector2.Distance(opennode.pos, startLoc) + Vector2.Distance(opennode.pos, target);

                //In here we check if the fCost is lower than the last fCost that was saved. This way we work down the list and eventually get the node with the lowest fCost
                if (fcostTemp == 0 || opennode.fCost < fcostTemp)
                {
                    currentNode = opennode;
                    fcostTemp = opennode.fCost;
                }
            }
            //We add the current Node to the closed list so we don't check it again
            open.Remove(currentNode);
            closed.Add(currentNode);

            //A list of the nodes neighbouring the current node. There might be a more efficient way to do this.
            List<node> neighbours;
            neighbours = new List<node>();
            neighbours.Add(new node { pos = new Vector2(currentNode.pos.x, currentNode.pos.y - 1), gCost = 0f, hCost = 0f, fCost = 0f });
            neighbours.Add(new node { pos = new Vector2(currentNode.pos.x, currentNode.pos.y + 1), gCost = 0f, hCost = 0f, fCost = 0f });
            neighbours.Add(new node { pos = new Vector2(currentNode.pos.x - 1, currentNode.pos.y), gCost = 0f, hCost = 0f, fCost = 0f });
            neighbours.Add(new node { pos = new Vector2(currentNode.pos.x + 1, currentNode.pos.y), gCost = 0f, hCost = 0f, fCost = 0f });

            foreach (node neighbour in neighbours)
            {
                //calculate the g- and hCost of each neighbour
                neighbour.gCost = Vector2.Distance(neighbour.pos, startLoc);
                neighbour.hCost = Vector2.Distance(neighbour.pos, target);

                //I'm checking if the nodes already exist in the closed list. The neighbours are new nodes, so I check if the positions already exist in the closed list
                List<Vector2> closedPoss = new List<Vector2>();
                foreach (node closednode in closed)
                {
                    closedPoss.Add(closednode.pos);
                }

                //If the nodes are already in the closed list or the node is on a non walkable tile, we skip to the next neighbour
                if (Physics2D.OverlapPoint(neighbour.pos, TileLayer) == null || Physics2D.OverlapPoint(neighbour.pos, TileLayer).gameObject == null || closedPoss.Contains(neighbour.pos))
                {
                    continue;
                }

                //Same thing as the closedPos list
                List<Vector2> openPoss = new List<Vector2>();
                foreach (node opentemp in open)
                {
                    openPoss.Add(opentemp.pos);
                }

                //If the node isn't already in the open list: add it
                if (!openPoss.Contains(neighbour.pos))
                {
                    if (neighbour.myTransform == null)
                    {
                        neighbour.myTransform = Instantiate(nodePrefab, neighbour.pos, new Quaternion(0, 0, 0, 0)).transform;
                    }
                    neighbour.fCost = Vector2.Distance(neighbour.pos, target) + Vector2.Distance(neighbour.pos, startLoc);
                    neighbour.myTransform.SetParent(currentNode.myTransform);
                    open.Add(neighbour);
                }
            }

            //If we reach the target destination, destroy all nodes that aren't on the route
            if (currentNode.pos == target)
            {
                foreach (node opens in open)
                {
                    Destroy(opens.myTransform.gameObject);
                }

                foreach (node closedOnes in closed)
                {
                    if (closedOnes == currentNode || currentNode.myTransform.IsChildOf(closedOnes.myTransform))
                    {
                        continue;
                    }
                    Destroy(closedOnes.myTransform.gameObject);
                }
                break;
            }
        }
        //Debug.Log(currentNode.pos);
        return currentNode.myTransform;
    }
}
