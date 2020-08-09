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
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, enemy.walkTo.position, 5f * Time.deltaTime);
            if (enemy.transform.position == enemy.walkTo.position)
            {
                if (enemy.walkTo.childCount == 0)
                {
                    UnityEngine.Object.Destroy(enemy.walktoParent.gameObject);
                    enemy.enemySM.ChangeState(enemy.Aim);
                }

                if (enemy.walkTo.childCount != 0)
                {
                    enemy.walkTo = enemy.walkTo.GetChild(0);
                }
            }
        }

        Camera.main.transform.position = Vector2.MoveTowards(Camera.main.transform.position, enemy.transform.position, 5f);
    }
    public override void Exit()
    {
        base.Exit();
    }

    public void getWalkPosition()
    {
        
        Vector2 targetPosition = new Vector2((int)Random.Range(enemy.transform.position.x - 2, enemy.transform.position.x + 3), (int)Random.Range(enemy.transform.position.y - 2, enemy.transform.position.y + 3));

        while (Physics2D.OverlapPoint(targetPosition, enemy.player.TileLayer) == null || Physics2D.OverlapPoint(targetPosition, enemy.player.TileLayer).gameObject.tag != "Walkable" || Physics2D.OverlapPoint(targetPosition, enemy.player.CharacterLayer) != null)
        {
            targetPosition = new Vector2((int)Random.Range(enemy.transform.position.x - 2, enemy.transform.position.x + 3), (int)Random.Range(enemy.transform.position.y - 2, enemy.transform.position.y + 3));
        }

        //If you click on a walkable tile, walk to it
        if (Physics2D.OverlapPoint(targetPosition, enemy.player.TileLayer) != null 
            && Physics2D.OverlapPoint(targetPosition, enemy.player.TileLayer).gameObject.tag == "Walkable"
            && Physics2D.OverlapPoint(targetPosition, enemy.player.CharacterLayer) == null)
        {
            movePosition = targetPosition;
            //Debug.Log(Physics2D.OverlapPoint(targetPosition, enemy.player.TileLayer).gameObject.tag);
            if (new Vector2(enemy.transform.position.x, enemy.transform.position.y) != movePosition)
            {
                //We find the path we need to walk
                enemy.walkTo = enemy.pathFinder.findPath(enemy.transform.position, movePosition);
                enemy.StartCoroutine(enemy.wacht1sec());
            }

            else
                enemy.enemySM.ChangeState(enemy.Aim);
        }

        else
            enemy.enemySM.ChangeState(enemy.Aim);
    }
}
