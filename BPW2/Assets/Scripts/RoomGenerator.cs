using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public LayerMask tileLayer;

    public int roomCount;
    public GameObject walkTile;
    public GameObject walkTileBlack;
    public GameObject walkTileWhite;
    public GameObject wallTile;
    public GameObject Enemy;
    public GameObject RoomTrigger;
    public GameObject Stairs;

    public int notThisWay;

    //0 is Up, 1 is Right, 2 is Down, 3 is Left, 4 is UR corner, 5 is DR corner, 6 is DL corner, 7 is UL corner, 8 is inside
    public List<GameObject> walls;

    private List<GameObject> insideWalls;

    public Vector2 EntranceLoc;
    public Vector2 ExitLoc;

    public int EntranceSide; //1 is up, 2 is right, 3 is down, 4 is left
    public int ExitSide; //1 is up, 2 is right, 3 is down, 4 is left

    public Vector2 upperLeftLoc;
    private Pathfinding2 pathFinder;
    public GameObject pathfinderObject;

    private void Start()
    {
        pathFinder = pathfinderObject.GetComponent<Pathfinding2>();
        maakKamer();
    }

    public void maakKamer()
    {
        Player player = FindObjectOfType<Player>();
        int roomHeight;
        int roomWidth;

        if (roomCount == 5)
        {
            roomHeight = 5;
            roomWidth = 5;

            if (ExitSide == 1)
            {
                EntranceLoc = new Vector2(ExitLoc.x, ExitLoc.y + 1);

                upperLeftLoc = new Vector2(EntranceLoc.x - (int)Random.Range(1, roomWidth - 1), EntranceLoc.y + roomHeight - 1);
            }

            if (ExitSide == 2)
            {
                EntranceLoc = new Vector2(ExitLoc.x + 1, ExitLoc.y);

                upperLeftLoc = new Vector2(EntranceLoc.x, EntranceLoc.y + (int)Random.Range(1, roomHeight - 1));
            }

            if (ExitSide == 3)
            {
                EntranceLoc = new Vector2(ExitLoc.x, ExitLoc.y - 1);

                upperLeftLoc = new Vector2(EntranceLoc.x - (int)Random.Range(1, roomWidth - 1), EntranceLoc.y);
            }

            if (ExitSide == 4)
            {
                EntranceLoc = new Vector2(ExitLoc.x - 1, ExitLoc.y);

                upperLeftLoc = new Vector2(EntranceLoc.x - roomWidth + 1, EntranceLoc.y + (int)Random.Range(1, roomHeight -1));
            }

                        //i = height
            for (int i = 0; i < roomHeight; i++)
            {
                //k is width
                for (int k = 0; k < roomWidth; k++)
                {
                    if (i == 0 || i == roomHeight - 1 || k == 0 || k == roomWidth - 1)
                    {
                        //I'm destroying the object instead of using an else statement, because that didn't work
                        GameObject temp = spawnWall((int)upperLeftLoc.x + k, (int)upperLeftLoc.y - i, roomHeight - 1, roomWidth - 1, i, k);

                        if (new Vector2(upperLeftLoc.x + k, upperLeftLoc.y - i) == EntranceLoc)
                        {
                            Destroy(temp);
                            spawnTile((int)upperLeftLoc.x + k, (int)upperLeftLoc.y - i);
                        }
                    }

                    else
                        spawnTile((int)upperLeftLoc.x + k, (int)upperLeftLoc.y - i);


                    if (i == 2 && k == 2)
                    {
                        Instantiate(Stairs, new Vector3(upperLeftLoc.x + k, upperLeftLoc.y - i, 1), new Quaternion(0, 0, 0, 0), this.transform);
                    }
                }
            }

            return;
        }

        if (roomCount == 0)
        {
            upperLeftLoc = new Vector2(player.transform.position.x - 2, player.transform.position.y + 2);
            //maak de starting room
            roomHeight = 5;
            roomWidth = 5;

            ExitSide = (int)Random.Range(1, 5);
            int exitNum = 0;

            if (ExitSide == 1 || ExitSide == 3)
            {
                exitNum = (int)Random.Range(1, roomWidth - 1);
            }

            if (ExitSide == 2 || ExitSide == 4)
            {
                exitNum = (int)Random.Range(1, roomHeight - 1);
            }

            //i = height
            for (int i = 0; i < roomHeight; i++)
            {
                //k is width
                for (int k = 0; k < roomWidth; k++)
                {
                    if (i == 0 || i == roomHeight - 1 || k == 0 || k == roomWidth - 1)
                    {
                        //I'm destroying the object instead of using an else statement, because that didn't work
                        GameObject temp = spawnWall((int)upperLeftLoc.x + k, (int)upperLeftLoc.y - i, roomHeight - 1, roomWidth - 1, i, k);


                        if (ExitSide == 1 && i == 0 && k == exitNum)
                        {
                            Destroy(temp);
                            GameObject Exit = spawnTile((int)upperLeftLoc.x + k, (int)upperLeftLoc.y - i);
                            ExitLoc = Exit.transform.position;
                            notThisWay = 3;
                            Instantiate(RoomTrigger, ExitLoc, new Quaternion(0, 0, 0, 0));
                        }
                        if (ExitSide == 2 && k == roomWidth - 1 && i == exitNum)
                        {
                            Destroy(temp);
                            GameObject Exit = spawnTile((int)upperLeftLoc.x + k, (int)upperLeftLoc.y - i);
                            ExitLoc = Exit.transform.position;
                            notThisWay = 4;
                            Instantiate(RoomTrigger, ExitLoc, new Quaternion(0, 0, 0, 0));
                        }
                        if (ExitSide == 3 && i == roomHeight - 1 && k == exitNum)
                        {
                            Destroy(temp);
                            GameObject Exit = spawnTile((int)upperLeftLoc.x + k, (int)upperLeftLoc.y - i);
                            ExitLoc = Exit.transform.position;
                            notThisWay = 1;
                            Instantiate(RoomTrigger, ExitLoc, new Quaternion(0, 0, 0, 0));
                        }
                        if (ExitSide == 4 && k == 0 && i == exitNum)
                        {
                            Destroy(temp);
                            GameObject Exit = spawnTile((int)upperLeftLoc.x + k, (int)upperLeftLoc.y - i);
                            ExitLoc = Exit.transform.position;
                            notThisWay = 2;
                            Instantiate(RoomTrigger, ExitLoc, new Quaternion(0, 0, 0, 0));
                        }
                    }

                    else
                        spawnTile((int)upperLeftLoc.x + k, (int)upperLeftLoc.y - i);
                }
            }
        }

        if (roomCount > 0)
        {
            insideWalls = new List<GameObject>();
            // Stap 1: Kies grootte
            roomHeight = (int)Random.Range(6, 16);
            roomWidth = (int)Random.Range(6, 16);
            List<Vector2> randomLocList = new List<Vector2>();

            //Entrance loc is global
            if (ExitSide == 1)
            {
                EntranceLoc = new Vector2(ExitLoc.x, ExitLoc.y + 1);

                upperLeftLoc = new Vector2(EntranceLoc.x - (int)Random.Range(1, roomWidth), EntranceLoc.y + roomHeight);
            }

            if (ExitSide == 2)
            {
                EntranceLoc = new Vector2(ExitLoc.x + 1, ExitLoc.y);

                upperLeftLoc = new Vector2(EntranceLoc.x, EntranceLoc.y + (int)Random.Range(1, roomHeight));
            }

            if (ExitSide == 3)
            {
                EntranceLoc = new Vector2(ExitLoc.x, ExitLoc.y - 1);

                upperLeftLoc = new Vector2(EntranceLoc.x - (int)Random.Range(1, roomWidth), EntranceLoc.y);
            }

            if (ExitSide == 4)
            {
                EntranceLoc = new Vector2(ExitLoc.x - 1, ExitLoc.y);

                upperLeftLoc = new Vector2(EntranceLoc.x - roomWidth, EntranceLoc.y + (int)Random.Range(1, roomHeight));
            }
            EntranceSide = ExitSide;

            int exitNum = 0;
            Vector2 rightCornerTemp = new Vector2(0, 0);
            Vector2 leftCornerTemp = new Vector2(0, 0);

            while (Physics2D.OverlapArea(leftCornerTemp, rightCornerTemp, tileLayer) != null || exitNum == 0)
            {

                ExitSide = (int)Random.Range(1, 5);

                while (notThisWay == ExitSide || (ExitSide == 1 && EntranceSide == 3) || (ExitSide == 2 && EntranceSide == 4) || (ExitSide == 3 && EntranceSide == 1) || (ExitSide == 4 && EntranceSide == 2))
                {
                    ExitSide = (int)Random.Range(1, 5);
                }

                if (ExitSide == 1 || ExitSide == 3)
                {
                    exitNum = (int)Random.Range(1, roomWidth - 1);

                    if (ExitSide == 1)
                    {
                        leftCornerTemp = new Vector2(upperLeftLoc.x + exitNum - roomWidth, upperLeftLoc.y + roomHeight);
                        rightCornerTemp = new Vector2(upperLeftLoc.x + exitNum + roomWidth, upperLeftLoc.y + 1);
                    }

                    if (ExitSide == 3)
                    {
                        leftCornerTemp = new Vector2(upperLeftLoc.x + exitNum - roomWidth, upperLeftLoc.y - roomHeight);
                        rightCornerTemp = new Vector2(upperLeftLoc.x + exitNum + roomWidth, upperLeftLoc.y - roomHeight - roomHeight);
                    }
                }

                if (ExitSide == 2 || ExitSide == 4)
                {
                    exitNum = (int)Random.Range(1, roomHeight - 1);

                    if (ExitSide == 2)
                    {
                        leftCornerTemp = new Vector2(upperLeftLoc.x + roomWidth, upperLeftLoc.y - exitNum + roomHeight);
                        rightCornerTemp = new Vector2(upperLeftLoc.x + roomWidth + roomWidth, upperLeftLoc.y - exitNum - roomHeight);
                    }

                    if (ExitSide == 4)
                    {
                        leftCornerTemp = new Vector2(upperLeftLoc.x - 1 - roomWidth, upperLeftLoc.y - exitNum + roomHeight);
                        rightCornerTemp = new Vector2(upperLeftLoc.x - 1, upperLeftLoc.y - exitNum - roomHeight);
                    }
                }
            }
           if(Physics2D.OverlapArea(leftCornerTemp, rightCornerTemp, tileLayer) == null)
            {
                Debug.Log("There's space!");
            }

            for (int i = 0; i < 8; i++)
            {
                Vector2 randomPoint = new Vector2(upperLeftLoc.x + (int)Random.Range(1, roomWidth), upperLeftLoc.y - (int)Random.Range(1, roomHeight));
                while (randomLocList.Contains(randomPoint))
                {
                    randomPoint = new Vector2(upperLeftLoc.x + (int)Random.Range(1, roomWidth), upperLeftLoc.y - (int)Random.Range(1, roomHeight));
                }

                randomLocList.Add(randomPoint);
            }
            Debug.Log("ListSize =" + randomLocList.Count);

            Debug.Log(randomLocList.Count);

            for (int i = 0; i <= roomHeight; i++)
            {
                for (int k = 0; k <= roomWidth; k++)
                {
                    if (i == 0 || i == roomHeight || k == 0 || k == roomWidth)
                    {
                        //I'm destroying the object instead of using an else statement, because that didn't work
                        GameObject temp = spawnWall((int)upperLeftLoc.x + k, (int)upperLeftLoc.y - i, roomHeight, roomWidth, i, k);


                        if (ExitSide == 1 && i == 0 && k == exitNum)
                        {
                            Destroy(temp);
                            GameObject Exit = spawnTile((int)upperLeftLoc.x + k, (int)upperLeftLoc.y - i);
                            ExitLoc = Exit.transform.position;
                            Instantiate(RoomTrigger, ExitLoc, new Quaternion(0, 0, 0, 0));
                        }
                        if (ExitSide == 2 && k == roomWidth && i == exitNum)
                        {
                            Destroy(temp);
                            GameObject Exit = spawnTile((int)upperLeftLoc.x + k, (int)upperLeftLoc.y - i);
                            ExitLoc = Exit.transform.position;
                            Instantiate(RoomTrigger, ExitLoc, new Quaternion(0, 0, 0, 0));
                        }
                        if (ExitSide == 3 && i == roomHeight && k == exitNum)
                        {
                            Destroy(temp);
                            GameObject Exit = spawnTile((int)upperLeftLoc.x + k, (int)upperLeftLoc.y - i);
                            ExitLoc = Exit.transform.position;
                            Instantiate(RoomTrigger, ExitLoc, new Quaternion(0, 0, 0, 0));
                        }
                        if (ExitSide == 4 && k == 0 && i == exitNum)
                        {
                            Destroy(temp);
                            GameObject Exit = spawnTile((int)upperLeftLoc.x + k, (int)upperLeftLoc.y - i);
                            ExitLoc = Exit.transform.position;
                            Instantiate(RoomTrigger, ExitLoc, new Quaternion(0, 0, 0, 0));
                        }

                        if (new Vector2(upperLeftLoc.x + k, upperLeftLoc.y - i) == EntranceLoc)
                        {
                            Destroy(temp);
                            spawnTile((int)upperLeftLoc.x + k, (int)upperLeftLoc.y - i);
                        }
                    }

                    else
                    {
                        insideWalls.Add(Instantiate(walls[8], new Vector3((int)upperLeftLoc.x + k, (int)upperLeftLoc.y - i, 2), new Quaternion(0, 0, 0, 0), this.transform));
                    }
                }
            }

            Transform exitRoute = pathFinder.findRoomPath(EntranceLoc, ExitLoc).root;

            StartCoroutine(getRoad(exitRoute));

            foreach (Vector2 randomP in randomLocList)
            {
                //Destroy(Physics2D.OverlapPoint(randomP, tileLayer).gameObject);
                Transform TempRoute = pathFinder.findRoomPath(EntranceLoc, randomP).root;

                StartCoroutine(getRoad(TempRoute));

                Transform otherWay = pathFinder.findRoomPath(randomP, ExitLoc).root;

                StartCoroutine(getRoad(otherWay));
            }
            // Stap 2: Kies ingang en uitgang
            // Stap 3: Kies random punten in de kamer
            // Stap 4: Maak Route naar de punten
            // Eventueel Stap 5: Vergroot de punten en voeg blokjes toe
            int enemyNumber = (int)(roomHeight + roomWidth) / 8;

            for(int i = 0; i < enemyNumber; i++)
            {
                Instantiate(Enemy, randomLocList[i], new Quaternion(0, 0, 0, 0));
            }
        }
        roomCount++;
    }

    public GameObject spawnTile(int xLocation, int yLocation)
    {
        if (((xLocation) + (yLocation)) % 2 == 0)
            return Instantiate(walkTileBlack, new Vector3(xLocation, yLocation, 1), new Quaternion(0, 0, 0, 0), this.transform);

        else
            return Instantiate(walkTileWhite, new Vector3(xLocation, yLocation, 1), new Quaternion(0, 0, 0, 0), this.transform);
    } 

    public GameObject spawnWall(int xLoc, int yLoc, int roomH, int roomW, int relativeY, int relativeX)
    {
        if(relativeX == 0)
        {
            if(relativeY == 0)
            {
                 return Instantiate(walls[7], new Vector3(xLoc, yLoc, 1), new Quaternion(0, 0, 0, 0), this.transform);
                //up left corner
            }

            if(relativeY == roomH)
            {
                return Instantiate(walls[6], new Vector3(xLoc, yLoc, 1), new Quaternion(0, 0, 0, 0), this.transform);
                //Down left corner
            }
            return Instantiate(walls[3], new Vector3(xLoc, yLoc, 1), new Quaternion(0, 0, 0, 0), this.transform);
            //left wall
        }

        if(relativeX == roomW)
        {
            if(relativeY == 0)
            {
                return Instantiate(walls[4], new Vector3(xLoc, yLoc, 1), new Quaternion(0, 0, 0, 0), this.transform);
                //up right corner
            }

            if(relativeY == roomH)
            {
                return Instantiate(walls[5], new Vector3(xLoc, yLoc, 1), new Quaternion(0, 0, 0, 0), this.transform);
                //down right corner
            }
            return Instantiate(walls[1], new Vector3(xLoc, yLoc, 1), new Quaternion(0, 0, 0, 0), this.transform);
            //right wall
        }

        if(relativeY == 0)
        {
            return Instantiate(walls[0], new Vector3(xLoc, yLoc, 1), new Quaternion(0, 0, 0, 0), this.transform);
            //Up wall
        }

        if (relativeY == roomH)
        {
            return Instantiate(walls[2], new Vector3(xLoc, yLoc, 1), new Quaternion(0, 0, 0, 0), this.transform);
            //Down wall
        }

        else
            return Instantiate(walls[3], new Vector3(xLoc, yLoc, 1), new Quaternion(0, 0, 0, 0), this.transform);
    }

    IEnumerator getRoad(Transform Route)
    {
        yield return null;
        Transform temp;
        temp = Route;
        while (temp.transform != null)
        {
            GameObject tempForParent = spawnTile((int)temp.transform.position.x, (int)temp.transform.position.y);
            tempForParent.transform.SetParent(transform);

            if (temp.childCount > 0)
                temp = temp.GetChild(0);

            else
                break;
        }
        GameObject tempforParentRoute = spawnTile((int)Route.transform.position.x, (int)Route.transform.position.y);
        tempforParentRoute.transform.SetParent(transform);
    }
}
