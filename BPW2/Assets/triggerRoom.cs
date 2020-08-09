using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerRoom : MonoBehaviour
{
    public RoomGenerator roomMaker;

    private void Awake()
    {
        roomMaker = FindObjectOfType<RoomGenerator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Er zit iets in me");
        if (collision.gameObject.tag == "Player" && FindObjectOfType<Enemy>() == null)
        {
            Debug.Log("Er is iemand in me halp");
            roomMaker.maakKamer();
        }
    }
}
