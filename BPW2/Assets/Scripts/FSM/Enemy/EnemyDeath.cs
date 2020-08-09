using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : EnemyState
{
    public EnemyDeath(Enemy enemy, EnemyStateMachine enemystateMachine) : base(enemy, enemystateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        enemy.player.Kills++;

        Object.Destroy(enemy.gameObject);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
