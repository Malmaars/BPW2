using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AimState : Still
{
    public List<GameObject> enemies;
    public int ViewingEnemy;
    public GameObject TargetVisual;

    public AimState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        enemies = new List<GameObject>();

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemies.Add(enemy);
        }
        if(enemies.Count == 0)
        {
            player.PlayerSM.ChangeState(player.Idle);
        }
        ViewingEnemy = 0;
        TargetVisual = Object.Instantiate(player.TargetPrefab, enemies[ViewingEnemy].transform.position, new Quaternion(0, 0, 0, 0));
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (enemies.Count > 0)
        {
            TargetVisual.transform.position = Vector3.MoveTowards(TargetVisual.transform.position, enemies[ViewingEnemy].transform.position, 10f * Time.deltaTime);
            Camera.main.transform.position = Vector2.MoveTowards(Camera.main.transform.position, enemies[ViewingEnemy].transform.position, 10f * Time.deltaTime);
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, -50);

            bool isThereAWall = false;

            if (TargetVisual.transform.position == enemies[ViewingEnemy].transform.position)
            {
                Vector2 direction = enemies[ViewingEnemy].transform.position - player.transform.position;
                Debug.Log(direction);
                RaycastHit2D[] hits = Physics2D.RaycastAll(player.transform.position, direction, Vector2.Distance(player.transform.position, enemies[ViewingEnemy].transform.position), player.TileLayer);
                foreach (RaycastHit2D hit in hits)
                {
                    if (hit.transform.gameObject.tag == "Walkable")
                        continue;

                    isThereAWall = true;
                }
            }

            isThereAWall = checkCover(isThereAWall);

            if (isThereAWall == true)
            {
                //player.hitPercentage = 0;
            }

            else
                player.hitPercentage = 100;

            player.hitPercentage -= (int)(Vector2.Distance(player.transform.position, enemies[ViewingEnemy].transform.position) * 5f);

            if (player.hitPercentage < 0)
                player.hitPercentage = 0;
        }

        if(enemies.Count <= 0)
        {
            player.PlayerSM.ChangeState(player.Idle);
        }
    }

    public override void Exit()
    {
        base.Exit();
        Object.Destroy(TargetVisual);
        Camera.main.transform.position = player.transform.position;
    }

    public void Shoot(float Smallhit)
    {
        Debug.Log(Smallhit);
        if (Random.value < Smallhit)
        {
            enemies[ViewingEnemy].GetComponent<Enemy>().health -= 100;
        }

        enemies.Remove(enemies[ViewingEnemy]);
        //end turn (probably)
        player.turnPoints--;
        player.PlayerSM.ChangeState(player.Idle);
    }

    public bool checkCover(bool isThereAWall)
    {
        Vector2 playerDirection = enemies[ViewingEnemy].transform.position - player.transform.position;
        Vector2 coverPosV2;

        if (player.upCover == true && playerDirection.y > 0)
        {
            coverPosV2 = new Vector2(player.transform.position.x - 1, player.transform.position.y + 1);
            if (Physics2D.OverlapPoint(coverPosV2,player.TileLayer) != null
                && Physics2D.OverlapPoint(coverPosV2, player.TileLayer).gameObject.tag == "Walkable")
            {
                return checkForWalls(coverPosV2);
            }

            coverPosV2 = new Vector2(player.transform.position.x + 1, player.transform.position.y + 1);
            if (Physics2D.OverlapPoint(coverPosV2, player.TileLayer) != null
               && Physics2D.OverlapPoint(coverPosV2, player.TileLayer).gameObject.tag == "Walkable")
            {
                return checkForWalls(coverPosV2);
            }

            else { return true; }
        }

        if (player.downCover == true && playerDirection.y < 0)
        {
            coverPosV2 = new Vector2(player.transform.position.x - 1, player.transform.position.y - 1);
            if (Physics2D.OverlapPoint(coverPosV2, player.TileLayer) != null
                && Physics2D.OverlapPoint(coverPosV2, player.TileLayer).gameObject.tag == "Walkable")
            {
                return checkForWalls(coverPosV2);
            }

            coverPosV2 = new Vector2(player.transform.position.x + 1, player.transform.position.y - 1);
            if (Physics2D.OverlapPoint(coverPosV2, player.TileLayer) != null
               && Physics2D.OverlapPoint(coverPosV2, player.TileLayer).gameObject.tag == "Walkable")
            {
                return checkForWalls(coverPosV2);
            }

            else { return true; }
        }

        if (player.leftCover == true && playerDirection.x < 0)
        {
            coverPosV2 = new Vector2(player.transform.position.x - 1, player.transform.position.y + 1);
            if (Physics2D.OverlapPoint(coverPosV2, player.TileLayer) != null
                && Physics2D.OverlapPoint(coverPosV2, player.TileLayer).gameObject.tag == "Walkable")
            {
                return checkForWalls(coverPosV2);
            }

            coverPosV2 = new Vector2(player.transform.position.x - 1, player.transform.position.y - 1);
            if (Physics2D.OverlapPoint(coverPosV2, player.TileLayer) != null
               && Physics2D.OverlapPoint(coverPosV2, player.TileLayer).gameObject.tag == "Walkable")
            {
                return checkForWalls(coverPosV2);
            }

            else { return true; }
        }

        if (player.rightCover == true && playerDirection.x > 0)
        {
            coverPosV2 = new Vector2(player.transform.position.x + 1, player.transform.position.y + 1);
            if (Physics2D.OverlapPoint(coverPosV2, player.TileLayer) != null
                && Physics2D.OverlapPoint(coverPosV2, player.TileLayer).gameObject.tag == "Walkable")
            {
                return checkForWalls(coverPosV2);
            }

            coverPosV2 = new Vector2(player.transform.position.x + 1, player.transform.position.y - 1);
            if (Physics2D.OverlapPoint(coverPosV2, player.TileLayer) != null
               && Physics2D.OverlapPoint(coverPosV2, player.TileLayer).gameObject.tag == "Walkable")
            {
                return checkForWalls(coverPosV2);
            }

            else { return true; }
        }

        else { return isThereAWall; }

    }

    public bool checkForWalls(Vector2 coverPosV2)
    {
        Vector3 coverPosition = new Vector3(coverPosV2.x, coverPosV2.y, 0);
        Vector2 direction = enemies[ViewingEnemy].transform.position - coverPosition;
        RaycastHit2D[] hits = Physics2D.RaycastAll(coverPosition, direction, Vector2.Distance(coverPosition, enemies[ViewingEnemy].transform.position), player.TileLayer);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.gameObject.tag == "Walkable")
                continue;

            return true;
        }
        return false;
    }
}
