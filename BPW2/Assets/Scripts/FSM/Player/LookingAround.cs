using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingAround : Still
{
    private List<GameObject> availableTiles;
    public LookingAround(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player.FreeView.SetActive(true);

        availableTiles = new List<GameObject>();
        for (int i = -4; i < 4; i++)
        {
            for (int j = -4; j < 4; j++)
            {
                float distance = Mathf.Sqrt(i * i + j * j);
                if (distance < 3.5f &&
                    Physics2D.OverlapPoint(new Vector2(player.transform.position.x + i, player.transform.position.y + j), player.TileLayer) != null &&
                    Physics2D.OverlapPoint(new Vector2(player.transform.position.x + i, player.transform.position.y + j), player.TileLayer).gameObject.tag == "Walkable")
                {
                    GameObject temp = Object.Instantiate(player.availableTilePrefab);
                    temp.transform.position = new Vector2(player.transform.position.x + i, player.transform.position.y + j);
                    availableTiles.Add(temp);

                }
            }
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Input.GetKey(KeyCode.W))
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + (5f * Time.deltaTime), -1);
        }

        if (Input.GetKey(KeyCode.A))
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x - (5f * Time.deltaTime), Camera.main.transform.position.y, -1);
        }

        if (Input.GetKey(KeyCode.S))
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - (5f * Time.deltaTime), -1);
        }

        if (Input.GetKey(KeyCode.D))
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + (5f * Time.deltaTime), Camera.main.transform.position.y, -1);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.FreeView.SetActive(false);

        foreach (GameObject tile in availableTiles)
        {
            Object.Destroy(tile);
        }
    }
}
