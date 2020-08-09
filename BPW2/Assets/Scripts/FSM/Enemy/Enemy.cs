using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public float hitPercentage;

    public Transform walkTo;
    public Transform walktoParent;

    public Pathfinding2 pathFinder;
    public Player player;

    public GameObject coverShieldVisual;
    public List<Vector2> enemyCover;
    public List<GameObject> VisualCoverList;
    public bool upCoverE, downCoverE, rightCoverE, leftCoverE;

    public EnemyStateMachine enemySM;
    public EnemyWalking walking;
    public EnemyAiming Aim;
    public EnemyIdle Idle;
    public EnemyDeath Die;
    public EnemyTurnOn StartTurn;
    // Start is called before the first frame update
    void Awake()
    {
        pathFinder = FindObjectOfType<Pathfinding2>();
        player = FindObjectOfType<Player>();
        enemySM = new EnemyStateMachine();
        walking = new EnemyWalking(this, enemySM);
        Aim = new EnemyAiming(this, enemySM);
        Idle = new EnemyIdle(this, enemySM);
        Die = new EnemyDeath(this, enemySM);
        StartTurn = new EnemyTurnOn(this, enemySM);

        VisualCoverList = new List<GameObject>();

        StartCoroutine(Initialize());

        health = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemySM.CurrentState != null)
            enemySM.CurrentState.LogicUpdate();

        if (health <= 0)
        {
            enemySM.ChangeState(Die);
        }
    }

    public IEnumerator wacht1sec()
    {
        //We wait one frame, because destroying all the useless nodes takes a frame
        yield return null;
        walkTo = walkTo.root;
        walktoParent = walkTo;
    }

    public IEnumerator wachtVoorSwitch()
    {
        yield return new WaitForSeconds(2f);
        player.switchToEnemy();
    }

    public IEnumerator wachtVoorLopen()
    {
        yield return new WaitForSeconds(1f);
        enemySM.ChangeState(walking);
    }

    public IEnumerator Initialize()
    {
        yield return null;
        enemySM.Initialize(Idle);
    }
}
