using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToGame : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("TurnBased");
        }

        if (collision.gameObject.tag == "Walkable" && collision.gameObject != this.gameObject)
        {
            Destroy(collision.gameObject);
        }
    }
}
