using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurnOn : EnemyState
{
    public EnemyTurnOn(Enemy enemy, EnemyStateMachine enemystateMachine) : base(enemy, enemystateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        Camera.main.transform.position = enemy.transform.position;
        enemy.StartCoroutine(enemy.wachtVoorLopen());
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
