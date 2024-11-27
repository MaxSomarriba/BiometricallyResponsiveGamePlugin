using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMovement : MonoBehaviour
{
    bool moveRight = true;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        RunPythonScript.Instance.OnHeartRateTooHigh += RunPythonScript_OnHeartRateTooHigh;
        // Get the SpriteRenderer component attached to the GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void RunPythonScript_OnHeartRateTooHigh(object sender, System.EventArgs e)
    {
        Debug.Log("Heart rate too high signal received!");
    }

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

    // Update is called once per frame
    void Update()
    {
        // Change color when spacebar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Change to a random color
            spriteRenderer.color = new Color(Random.value, Random.value, Random.value);
        }
    }

    private void OnDestroy()
    {
        RunPythonScript.Instance.OnHeartRateTooHigh -= RunPythonScript_OnHeartRateTooHigh;
    }
}
