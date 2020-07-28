using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;

    public Transform walkTo;
    public Transform walktoParent;

    public Pathfinding2 pathFinder;
    public Player player;

    public EnemyStateMachine enemySM;
    public EnemyWalking walking;
    public EnemyAiming Aim;
    public EnemyIdle Idle;
    // Start is called before the first frame update
    void Start()
    {
        pathFinder = FindObjectOfType<Pathfinding2>();
        player = FindObjectOfType<Player>();
        enemySM = new EnemyStateMachine();
        walking = new EnemyWalking(this, enemySM);
        Aim = new EnemyAiming(this, enemySM);
        Idle = new EnemyIdle(this, enemySM);


        health = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            Destroy(transform.gameObject);
        }
    }

    public IEnumerator wacht1sec()
    {
        //We wait one frame, because destroying all the useless nodes takes a frame
        yield return null;
        walkTo = walkTo.root;
        walktoParent = walkTo;
    }
}
