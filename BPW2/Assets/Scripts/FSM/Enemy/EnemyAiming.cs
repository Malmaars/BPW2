using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAiming : EnemyStill
{
    public EnemyAiming(Enemy enemy, EnemyStateMachine enemystateMachine) : base(enemy, enemystateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();


        bool isThereAWall = false;

        Vector2 direction = enemy.player.transform.position - enemy.transform.position;
        RaycastHit2D[] hits = Physics2D.RaycastAll(enemy.transform.position, direction, Vector2.Distance(enemy.transform.position, enemy.player.transform.position), enemy.player.TileLayer);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.gameObject.tag == "Walkable")
                continue;

            isThereAWall = true;
        }

        isThereAWall = checkCover(isThereAWall);

        if (isThereAWall == true)
        {
            enemy.hitPercentage = 0;
        }

        else
            enemy.hitPercentage = 100;

        enemy.hitPercentage -= (int)(Vector2.Distance(enemy.transform.position, enemy.player.transform.position) * 10f);

        if (enemy.hitPercentage > 0)
        {
            if (Random.value < enemy.hitPercentage / 100)
            {
                //play particle effect aimed at enemy
                //to set the distance: lifetime 1 means 30 distance, so 1/30 is 1.
                //lifetime = distance *1/30
                Debug.Log("Schietschiet");

                ParticleSystem particleSystem = Object.Instantiate(enemy.shootParticles);
                ParticleSystem.MainModule m = particleSystem.main;
                ParticleSystem.ShapeModule shape = particleSystem.shape;

                float distance = Mathf.Sqrt(direction.x * direction.x + direction.y * direction.y);
                float lifetime = distance * (1f / enemy.shootParticles.main.startSpeed.constant);
                m.startLifetime = lifetime;

                if(direction.x < 0)
                {
                    m.startRotation = 360 - (Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg * -1);
                   shape.rotation = new Vector3(0, 360 - (Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg * -1));
                }
                else
                {
                    m.startRotation = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
                   shape.rotation = new Vector3(0, Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg);
                }

                m.startRotation = shape.rotation.y;
                particleSystem.gameObject.transform.position = enemy.transform.position;

                particleSystem.Play();

                enemy.player.health -= 20;
                ParticleSystem damageParticles = Object.Instantiate(enemy.player.YouTookDamage);
                damageParticles.transform.position = enemy.player.transform.position;
                damageParticles.Play();
            }
        }

        enemy.enemySM.ChangeState(enemy.Idle);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.StartCoroutine(enemy.wachtVoorSwitch());
    }

    public bool checkCover(bool isThereAWall)
    {
        Vector2 playerDirection = enemy.player.transform.position - enemy.transform.position;
        Vector2 coverPosV2;

        if (enemy.upCoverE == true && playerDirection.y > 0)
        {
            coverPosV2 = new Vector2(enemy.transform.position.x - 1, enemy.transform.position.y + 1);
            if (Physics2D.OverlapPoint(coverPosV2, enemy.player.TileLayer) != null
                && Physics2D.OverlapPoint(coverPosV2, enemy.player.TileLayer).gameObject.tag == "Walkable")
            {
                return checkForWalls(coverPosV2);
            }

            coverPosV2 = new Vector2(enemy.transform.position.x + 1, enemy.transform.position.y + 1);
            if (Physics2D.OverlapPoint(coverPosV2, enemy.player.TileLayer) != null
               && Physics2D.OverlapPoint(coverPosV2, enemy.player.TileLayer).gameObject.tag == "Walkable")
            {
                return checkForWalls(coverPosV2);
            }

            else { return true; }
        }

        if (enemy.downCoverE == true && playerDirection.y < 0)
        {
            coverPosV2 = new Vector2(enemy.transform.position.x - 1, enemy.transform.position.y - 1);
            if (Physics2D.OverlapPoint(coverPosV2, enemy.player.TileLayer) != null
                && Physics2D.OverlapPoint(coverPosV2, enemy.player.TileLayer).gameObject.tag == "Walkable")
            {
                return checkForWalls(coverPosV2);
            }

            coverPosV2 = new Vector2(enemy.transform.position.x + 1, enemy.transform.position.y - 1);
            if (Physics2D.OverlapPoint(coverPosV2, enemy.player.TileLayer) != null
               && Physics2D.OverlapPoint(coverPosV2, enemy.player.TileLayer).gameObject.tag == "Walkable")
            {
                return checkForWalls(coverPosV2);
            }

            else { return true; }
        }

        if (enemy.leftCoverE == true && playerDirection.x < 0)
        {
            coverPosV2 = new Vector2(enemy.transform.position.x - 1, enemy.transform.position.y + 1);
            if (Physics2D.OverlapPoint(coverPosV2, enemy.player.TileLayer) != null
                && Physics2D.OverlapPoint(coverPosV2, enemy.player.TileLayer).gameObject.tag == "Walkable")
            {
                return checkForWalls(coverPosV2);
            }

            coverPosV2 = new Vector2(enemy.transform.position.x - 1, enemy.transform.position.y - 1);
            if (Physics2D.OverlapPoint(coverPosV2, enemy.player.TileLayer) != null
               && Physics2D.OverlapPoint(coverPosV2, enemy.player.TileLayer).gameObject.tag == "Walkable")
            {
                return checkForWalls(coverPosV2);
            }

            else { return true; }
        }

        if (enemy.rightCoverE == true && playerDirection.x > 0)
        {
            coverPosV2 = new Vector2(enemy.transform.position.x + 1, enemy.transform.position.y + 1);
            if (Physics2D.OverlapPoint(coverPosV2, enemy.player.TileLayer) != null
                && Physics2D.OverlapPoint(coverPosV2, enemy.player.TileLayer).gameObject.tag == "Walkable")
            {
                return checkForWalls(coverPosV2);
            }

            coverPosV2 = new Vector2(enemy.transform.position.x + 1, enemy.transform.position.y - 1);
            if (Physics2D.OverlapPoint(coverPosV2, enemy.player.TileLayer) != null
               && Physics2D.OverlapPoint(coverPosV2, enemy.player.TileLayer).gameObject.tag == "Walkable")
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
        Vector2 direction = enemy.player.transform.position - coverPosition;
        RaycastHit2D[] hits = Physics2D.RaycastAll(coverPosition, direction, Vector2.Distance(coverPosition, enemy.player.transform.position), enemy.player.TileLayer);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.gameObject.tag == "Walkable")
                continue;

            return true;
        }
        return false;
    }
}
