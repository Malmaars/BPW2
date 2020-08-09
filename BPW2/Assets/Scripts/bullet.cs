using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public Vector2 startLoc;
    public Vector2 target;
    private void Update()
    {
        this.transform.position = Vector3.MoveTowards(transform.position, target, 5f);
    }
}
