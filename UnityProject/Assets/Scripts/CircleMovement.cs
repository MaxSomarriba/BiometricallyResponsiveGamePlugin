using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMovement : MonoBehaviour
{
    bool moveRight = true;
    // Update is called once per frame
    void FixedUpdate()
    {
        // move right if not near right edge
        if (transform.position.x > 8)
        {
            moveRight = false;
        }
        // move left if not near left edge
        if (transform.position.x < -8)
        {
            moveRight = true;
        }
        // move right
        if (moveRight)
        {
            transform.position += new Vector3(0.1f, 0, 0);
        }
        // move left
        else
        {
            transform.position += new Vector3(-0.1f, 0, 0);
        }
    }
}
