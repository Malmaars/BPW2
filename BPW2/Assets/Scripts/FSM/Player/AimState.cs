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
        ViewingEnemy = 0;
        TargetVisual = Object.Instantiate(player.TargetPrefab, enemies[ViewingEnemy].transform.position, new Quaternion(0, 0, 0, 0));
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        TargetVisual.transform.position = Vector3.MoveTowards(TargetVisual.transform.position, enemies[ViewingEnemy].transform.position, 10f * Time.deltaTime);
        Camera.main.transform.position = Vector2.MoveTowards(Camera.main.transform.position, enemies[ViewingEnemy].transform.position, 10f * Time.deltaTime);
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, -50);

        bool isThereAWall = false;

        if (TargetVisual.transform.position == enemies[ViewingEnemy].transform.position)
        {
            Vector2 direction = enemies[ViewingEnemy].transform.position - player.transform.position;
            RaycastHit2D[] hits = Physics2D.RaycastAll(player.transform.position, direction, Vector2.Distance(player.transform.position, enemies[ViewingEnemy].transform.position), player.TileLayer);
            foreach(RaycastHit2D hit in hits)
            {
                if (hit.transform.gameObject.tag == "Walkable")
                    continue;

                isThereAWall = true;
            }
        }

        if (isThereAWall)
        {
            player.hitPercentage = 0;
        }

        else
            player.hitPercentage = 100;

        player.percentageVisual.transform.GetComponent<TextMeshProUGUI>().text = player.hitPercentage.ToString();
    }

    public override void Exit()
    {
        base.Exit();
        Object.Destroy(TargetVisual);
        Camera.main.transform.position = player.transform.position;
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, -50);
    }

    public void Shoot()
    {
        enemies[ViewingEnemy].GetComponent<Enemy>().health -= 100;
        enemies.Remove(enemies[ViewingEnemy]);

        player.PlayerSM.ChangeState(player.still);
        //end turn (probably)
    }
}
