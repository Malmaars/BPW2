using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialTrigger : MonoBehaviour
{
    public GameObject NextRoom;
    public GameObject removeThese;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && FindObjectOfType<Enemy>() == null)
        {
            NextRoom.SetActive(true);

            removeThese.SetActive(false);
        }
    }
}
