using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingAround : Still
{
    public LookingAround(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player.FreeView.SetActive(true);
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
    }
}
