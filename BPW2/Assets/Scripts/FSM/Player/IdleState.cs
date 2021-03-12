using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : Still
{
    private List<GameObject> availableTiles;
    public IdleState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        availableTiles = new List<GameObject>();
        for(int i = -4; i < 4; i++)
        {
            for(int j = -4; j <4; j++)
            {
                float distance = Mathf.Sqrt(i * i + j * j);
                if (distance < 3.5f && 
                    Physics2D.OverlapPoint(new Vector2(player.transform.position.x + i, player.transform.position.y + j), player.TileLayer) != null &&
                    Physics2D.OverlapPoint(new Vector2(player.transform.position.x + i, player.transform.position.y + j), player.TileLayer).gameObject.tag == "Walkable")
                {
                    GameObject temp = Object.Instantiate(player.availableTilePrefab);
                    temp.transform.position = new Vector2(player.transform.position.x + i, player.transform.position.y + j);
                    availableTiles.Add(temp);

                }
            }
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Input.GetMouseButtonDown(1) && player.walkTo == null && player.isItMyTurn)
            player.PlayerSM.ChangeState(player.walking);
    }

    public override void Exit()
    {
        base.Exit();
        foreach(GameObject tile in availableTiles)
        {
            Object.Destroy(tile);
        }
    }
}
