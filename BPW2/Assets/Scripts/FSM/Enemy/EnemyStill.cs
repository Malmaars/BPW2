using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStill : EnemyState
{
    public EnemyStill(Enemy enemy, EnemyStateMachine enemystateMachine) : base(enemy, enemystateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        enemy.enemyCover = new List<Vector2>();
        enemy.enemyCover.Add(new Vector2(enemy.transform.position.x + 1, enemy.transform.position.y));
        enemy.enemyCover.Add(new Vector2(enemy.transform.position.x - 1, enemy.transform.position.y));
        enemy.enemyCover.Add(new Vector2(enemy.transform.position.x, enemy.transform.position.y + 1));
        enemy.enemyCover.Add(new Vector2(enemy.transform.position.x, enemy.transform.position.y - 1));

        foreach (Vector2 singleCover in enemy.enemyCover)
        {
            if (Physics2D.OverlapPoint(singleCover, enemy.player.TileLayer) != null && Physics2D.OverlapPoint(singleCover, enemy.player.TileLayer).gameObject.tag == "Unwalkable")
            {
                enemy.VisualCoverList.Add(UnityEngine.Object.Instantiate(enemy.coverShieldVisual, singleCover, new Quaternion(0, 0, 0, 0)));
                if (new Vector2(enemy.transform.position.x + 1, enemy.transform.position.y) == singleCover)
                {
                    enemy.rightCoverE = true;
                }
                if (new Vector2(enemy.transform.position.x - 1, enemy.transform.position.y) == singleCover)
                {
                    enemy.leftCoverE = true;
                }
                if (new Vector2(enemy.transform.position.x, enemy.transform.position.y + 1) == singleCover)
                {
                    enemy.upCoverE = true;
                }
                if (new Vector2(enemy.transform.position.x, enemy.transform.position.y - 1) == singleCover)
                {
                    enemy.downCoverE = true;
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
        foreach (GameObject singleShield in enemy.VisualCoverList)
        {
            UnityEngine.Object.Destroy(singleShield);
        }
        enemy.rightCoverE = false;
        enemy.leftCoverE = false;
        enemy.upCoverE = false;
        enemy.downCoverE = false;
    }
}
