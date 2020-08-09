using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Still : PlayerState
{
    public Still(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player.cover = new List<Vector2>();
        player.cover.Add(new Vector2(player.transform.position.x + 1, player.transform.position.y));
        player.cover.Add(new Vector2(player.transform.position.x - 1, player.transform.position.y));
        player.cover.Add(new Vector2(player.transform.position.x, player.transform.position.y + 1));
        player.cover.Add(new Vector2(player.transform.position.x, player.transform.position.y - 1));

        foreach (Vector2 singleCover in player.cover)
        {
            if (Physics2D.OverlapPoint(singleCover, player.TileLayer) == null || Physics2D.OverlapPoint(singleCover, player.TileLayer).gameObject.tag == "Unwalkable")
            {
                player.shields.Add(UnityEngine.Object.Instantiate(player.shield, singleCover, new Quaternion(0, 0, 0, 0)));
                if(new Vector2(player.transform.position.x + 1, player.transform.position.y) == singleCover)
                {
                    player.rightCover = true;
                }
                if(new Vector2(player.transform.position.x - 1, player.transform.position.y) == singleCover)
                {
                    player.leftCover = true;
                }
                if(new Vector2(player.transform.position.x, player.transform.position.y + 1) == singleCover)
                {
                    player.upCover = true;
                }
                if(new Vector2(player.transform.position.x, player.transform.position.y - 1) == singleCover)
                {
                    player.downCover = true;
                }
            }
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void Exit()
    {
        base.Exit();
        foreach (GameObject singleShield in player.shields)
        {
            UnityEngine.Object.Destroy(singleShield);
        }
        player.rightCover = false;
        player.leftCover = false;
        player.upCover = false;
        player.downCover = false;
    }
}
