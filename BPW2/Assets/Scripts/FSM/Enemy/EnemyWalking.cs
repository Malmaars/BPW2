using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalking : EnemyState
{
    private Vector2 movePosition;

    public EnemyWalking(Enemy enemy, EnemyStateMachine enemystateMachine) : base(enemy, enemystateMachine)
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
        if (enemy.walkTo != null && enemy.walktoParent != null)
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, enemy.walkTo.position, 10f * Time.deltaTime);
            if (enemy.transform.position == enemy.walkTo.position)
            {
                if (enemy.walkTo.childCount == 0)
                {
                    UnityEngine.Object.Destroy(enemy.walktoParent.gameObject);
                    enemy.enemySM.ChangeState(enemy.Idle);
                }

                if (enemy.walkTo.childCount != 0)
                {
                    enemy.walkTo = enemy.walkTo.GetChild(0);
                }
            }
        }
    }

    public void getWalkPosition()
    {
        Vector2 targetPosition = new Vector2((int)Random.Range(-2, 2), (int)Random.Range(-2, 2));
        Debug.Log(Physics2D.OverlapPoint(targetPosition, enemy.player.TileLayer));

        //If you click on a walkable tile, walk to it
        if (Physics2D.OverlapPoint(targetPosition, enemy.player.TileLayer) != null && Physics2D.OverlapPoint(targetPosition, enemy.player.TileLayer).gameObject.tag == "Walkable")
        {
            movePosition = targetPosition;
            Debug.Log(Physics2D.OverlapPoint(targetPosition, enemy.player.TileLayer).gameObject.tag);
            if (new Vector2(enemy.transform.position.x, enemy.transform.position.y) != movePosition)
            {
                //We find the path we need to walk
                enemy.walkTo = enemy.pathFinder.findPath(enemy.transform.position, movePosition);
                enemy.StartCoroutine(enemy.wacht1sec());
            }

            else
                enemy.enemySM.ChangeState(enemy.Idle);
        }

        else
            enemy.enemySM.ChangeState(enemy.Idle);
    }
}
