using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float knockback = 1f;
    public float bulletSpeed = 1f;
    Rigidbody2D rb;
    public GameObject bullet;
    public Transform muzzle;
    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPos = (transform.position);
        Vector2 dir = (mousePos - playerPos).normalized;

        transform.right = dir;

        if (Input.GetMouseButtonDown(0))
        {
            shoot(dir, playerPos);
            //playerPos -= dir;
        }
    }

    void shoot(Vector2 direction, Vector2 playerPos)
    {
        rb.velocity += -direction * knockback;
        transform.position = playerPos;
        GameObject bulletTemp = Instantiate(bullet, muzzle.position, muzzle.rotation);
        bulletTemp.GetComponent<Rigidbody2D>().velocity += direction * bulletSpeed;
    }
}
