using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public LayerMask TileLayer;
    public float moveSpeed = 1f;

    public GameObject pathfinderObject;
    public Pathfinding2 pathFinder;

    public GameObject highlight;
    public GameObject TargetPrefab;
    public GameObject percentageVisual;
    public GameObject shield;

    public Transform walkTo;
    public Transform walktoParent;

    private List<GameObject> highlights;
    public List<Vector2> cover;
    public List<GameObject> shields;
    public List<GameObject> enemyObjects;

    public PlayerStateMachine PlayerSM;
    public WalkingState walking;
    public IdleState Idle;
    public AimState Aim;
    public Still still;

    public bool leftCover, rightCover, upCover, downCover;

    public int hitPercentage;

    // Start is called before the first frame update
    void Start()
    {
        PlayerSM = new PlayerStateMachine();
        walking = new WalkingState(this, PlayerSM);
        Idle = new IdleState(this, PlayerSM);
        Aim = new AimState(this, PlayerSM);
        still = new Still(this, PlayerSM);

        PlayerSM.Initialize(Idle);

        pathFinder = pathfinderObject.GetComponent<Pathfinding2>();
        highlights = new List<GameObject>();
        cover = new List<Vector2>();
        shields = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerSM.CurrentState.LogicUpdate();
    }

    public void enterAiming()
    {
        if (PlayerSM.CurrentState == Aim)
        {
            Aim.ViewingEnemy++;

            if(Aim.ViewingEnemy > Aim.enemies.Count - 1)
            {
                Aim.ViewingEnemy = 0;
            }
        }

        if (PlayerSM.CurrentState == Idle)
        {
            PlayerSM.ChangeState(Aim);
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
            Aim.Shoot();
        }
    }

    public void switchToEnemy()
    {
        
    }

    public IEnumerator wacht1sec()
    {
        //We wait one frame, because destroying all the useless nodes takes a frame
        yield return null;
        walkTo = walkTo.root;
        walktoParent = walkTo;
    }

    /*    public IEnumerator wachtVoorRouteCheck(Transform checkRoute, Vector3 spawnVector)
        {
            //We wait one frame, because checking the correct node takes a frame
            yield return null;
            if (checkRoute.root.childCount <= 6)
            {
                highlights.Add(Instantiate(highlight, spawnVector, new Quaternion(0, 0, 0, 0)));
            }
            Destroy(checkRoute.root.gameObject);
        }*/
}
