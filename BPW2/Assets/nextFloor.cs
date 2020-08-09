using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nextFloor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.GetComponent<Player>().RoomsDone++;
            collision.GetComponent<Player>().health += 20;
            foreach (Transform child in FindObjectOfType<RoomGenerator>().transform)
            {
                Destroy(child.gameObject);
            }
            FindObjectOfType<RoomGenerator>().roomCount = 0;
            FindObjectOfType<RoomGenerator>().maakKamer();
        }

        if(collision.gameObject.tag == "Walkable" && collision.gameObject != this.gameObject)
        {
            Destroy(collision.gameObject);
        }
    }
}
