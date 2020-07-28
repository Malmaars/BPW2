using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : Moving
{
    private Vector2 movePosition;
    public WalkingState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        getWalkPosition();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (player.walkTo != null && player.walktoParent != null)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, player.walkTo.position, player.moveSpeed * Time.deltaTime);
            if (player.transform.position == player.walkTo.position)
            {
                if (player.walkTo.childCount == 0)
                {
                    UnityEngine.Object.Destroy(player.walktoParent.gameObject);
                    player.PlayerSM.ChangeState(player.Idle);
                    /*foreach (GameObject temp in player.highlights)
                    {
                        Destroy(temp);
                    }

                    //With this loop we kinda draw our own circle/rectangle where we can walk to
                    for (int i = (int)transform.position.y - 2; i < (int)transform.position.y + 3; i++)
                    {
                        for (int k = (int)transform.position.x - 2; k < (int)transform.position.x + 3; k++)
                        {
                            if (Physics2D.OverlapPoint(new Vector3(k, i, 0), TileLayer) != null && Physics2D.OverlapPoint(new Vector3(k, i, 0), TileLayer).gameObject.tag == "Walkable")
                            {
                                Transform checkRoute = pathFinder.findPath(this.transform.position, new Vector2(k, i));
                                //If the route to get there takes more than 4 tiles, we don't use it
                                Debug.Log(checkRoute.root);
                                Debug.Log(checkRoute.root.childCount);
                                StartCoroutine(wachtVoorRouteCheck(checkRoute, new Vector3(k, i, 0)));
                            }
                        }
                    }*/
                }

                if (player.walkTo.childCount != 0)
                {
                    player.walkTo = player.walkTo.GetChild(0);
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public void getWalkPosition()
    {
        //Get the mouseposition on click
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        //Round it to the tile you click on
        Vector2 roundedPosition = new Vector2(Mathf.RoundToInt(worldPosition.x), Mathf.RoundToInt(worldPosition.y));
        Debug.Log(roundedPosition);
        Debug.Log(Physics2D.OverlapPoint(roundedPosition, player.TileLayer));

        //If you click on a walkable tile, walk to it
        if (Physics2D.OverlapPoint(roundedPosition, player.TileLayer) != null && Physics2D.OverlapPoint(roundedPosition, player.TileLayer).gameObject.tag == "Walkable")
        {
            movePosition = roundedPosition;
            Debug.Log(Physics2D.OverlapPoint(roundedPosition, player.TileLayer).gameObject.tag);
            if (new Vector2(player.transform.position.x, player.transform.position.y) != movePosition)
            {
                //We find the path we need to walk
                player.walkTo = player.pathFinder.findPath(player.transform.position, movePosition);
                player.StartCoroutine(player.wacht1sec());
            }

            else
                player.PlayerSM.ChangeState(player.Idle);
        }

        else
            player.PlayerSM.ChangeState(player.Idle);
    }
}
