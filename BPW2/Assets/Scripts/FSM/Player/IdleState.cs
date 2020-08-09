using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : Still
{
    public IdleState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
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
    }
}
