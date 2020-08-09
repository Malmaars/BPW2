using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public LayerMask TileLayer;
    public LayerMask CharacterLayer;
    public float moveSpeed = 1f;
    public int turnPoints;
    public GameObject turnPointsVisual;
    public int health;

    public bool isItMyTurn = true;

    public GameObject pathfinderObject;
    public Pathfinding2 pathFinder;

    public GameObject highlight;
    public GameObject TargetPrefab;
    public GameObject percentageVisual;
    public GameObject shield;
    public GameObject TurnUI;
    public GameObject FreeView;
    public GameObject healthVisual;

    public int RoomsDone = 0;
    public int Kills = 0;
    public GameObject roomsCleared;
    public GameObject PawnKilled;

    public Transform walkTo;
    public Transform walktoParent;

    private List<GameObject> highlights;
    public List<Vector2> cover;
    public List<GameObject> shields;

    public List<GameObject> enemyObjects;
    public int enemyNumber;

    public PlayerStateMachine PlayerSM;
    public WalkingState walking;
    public IdleState Idle;
    public AimState Aim;
    public Still still;
    public LookingAround looking;

    public bool leftCover, rightCover, upCover, downCover;

    public float hitPercentage;

    // Start is called before the first frame update
    void Start()
    {
        PlayerSM = new PlayerStateMachine();
        walking = new WalkingState(this, PlayerSM);
        Idle = new IdleState(this, PlayerSM);
        Aim = new AimState(this, PlayerSM);
        still = new Still(this, PlayerSM);
        looking = new LookingAround(this, PlayerSM);

        StartCoroutine(Initialize());

        pathFinder = pathfinderObject.GetComponent<Pathfinding2>();
        highlights = new List<GameObject>();
        cover = new List<Vector2>();
        shields = new List<GameObject>();

        turnPoints = 3;
        health = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerSM.CurrentState != null)
            PlayerSM.CurrentState.LogicUpdate();

        if(turnPoints <= 0 && isItMyTurn == true)
        {
            StartCoroutine(wachtVoorSwitch("Enemy Turn"));
            isItMyTurn = false;
        }

        percentageVisual.transform.GetComponent<TextMeshProUGUI>().text = (hitPercentage.ToString() + "%");
        turnPointsVisual.transform.GetComponent<TextMeshProUGUI>().text = turnPoints.ToString();
        roomsCleared.transform.GetComponent<TextMeshProUGUI>().text = ("Floors Cleared " + RoomsDone.ToString());
        PawnKilled.transform.GetComponent<TextMeshProUGUI>().text = ("Enemy Pawn Killed: " + Kills.ToString());
        healthVisual.transform.GetComponent<TextMeshProUGUI>().text = ("Health: " + health.ToString());

    }

    public void enterAiming()
    {
        if (isItMyTurn)
        {
            if (PlayerSM.CurrentState == Aim)
            {
                Aim.ViewingEnemy++;

                if (Aim.ViewingEnemy > Aim.enemies.Count - 1)
                {
                    Aim.ViewingEnemy = 0;
                }
            }

            if (PlayerSM.CurrentState == Idle)
            {
                PlayerSM.ChangeState(Aim);
            }
        }
    }

    public void cancelAim()
    {
        if(PlayerSM.CurrentState == Aim)
        {
            PlayerSM.ChangeState(Idle);
        }
    }

    public void shoot()
    {
        if(PlayerSM.CurrentState == Aim)
        {
            Debug.Log(hitPercentage);
            Aim.Shoot(hitPercentage/100);
        }
    }

    public void endTurn()
    {
        if (isItMyTurn)
        {
            PlayerSM.ChangeState(still);
            turnPoints = 0;
        }
    }

    public void cancelLooking()
    {
        if(PlayerSM.CurrentState == looking)
        {
            Camera.main.transform.position = transform.position;
            PlayerSM.ChangeState(Idle);
            return;
        }

        if(PlayerSM.CurrentState != looking)
        {
            PlayerSM.ChangeState(looking);
            return;
        }
    }

    public void switchToEnemy()
    {
        bool weirdRythmCheck = false;
        if (enemyObjects.Count <= 0)
        {
            enemyObjects = new List<GameObject>();
            enemyNumber = 0;
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                if (enemy.GetComponent<Enemy>().enemySM.CurrentState != enemy.GetComponent<Enemy>().Die)
                    enemyObjects.Add(enemy);
            }
            if(enemyObjects.Count <= 0)
            {
                turnPoints = 3;
                PlayerSM.ChangeState(Idle);
                Camera.main.transform.position = transform.position;
                isItMyTurn = true;
                enemyObjects = new List<GameObject>();
                StartCoroutine(wachtVoorSpelerSwitch());
                return;
            }
            enemyObjects[enemyNumber].GetComponent<Enemy>().enemySM.ChangeState(enemyObjects[enemyNumber].GetComponent<Enemy>().StartTurn);
            weirdRythmCheck = true;
            return;
        }

        if (enemyNumber >= enemyObjects.Count - 1)
        {
            turnPoints = 3;
            PlayerSM.ChangeState(Idle);
            Camera.main.transform.position = transform.position;
            isItMyTurn = true;
            enemyObjects = new List<GameObject>();
            StartCoroutine(wachtVoorSpelerSwitch());
            return;
        }

        if ((enemyNumber < enemyObjects.Count || enemyObjects.Count != 0) && weirdRythmCheck == false)
        {
            enemyNumber++;
            enemyObjects[enemyNumber].GetComponent<Enemy>().enemySM.ChangeState(enemyObjects[enemyNumber].GetComponent<Enemy>().StartTurn);
        }
    }

    public IEnumerator wacht1sec()
    {
        //We wait one frame, because destroying all the useless nodes takes a frame
        yield return null;
        walkTo = walkTo.root;
        walktoParent = walkTo;
    }

    public IEnumerator wachtVoorSwitch(string whoseTurn)
    {
        TurnUI.SetActive(true);
        TurnUI.GetComponentInChildren<TextMeshProUGUI>().text = whoseTurn;
        yield return new WaitForSeconds(2f);
        TurnUI.SetActive(false);
        switchToEnemy();
    }

    public IEnumerator wachtVoorSpelerSwitch()
    {
        TurnUI.SetActive(true);
        TurnUI.GetComponentInChildren<TextMeshProUGUI>().text = "Your Turn";
        yield return new WaitForSeconds(2f);
        TurnUI.SetActive(false);
    }

    public IEnumerator Initialize()
    {
        yield return null;
        PlayerSM.Initialize(Idle);
    }
}
