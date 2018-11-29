using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <Summary>

    // This script is used for that floating bobbing effect for Consumeable items

// </Summary>

public class Consumeable : MonoBehaviour
{
    // Item rotation speed
    public float Speed;

    // bobing disance
    public float bob;

    // delta time
    private float time = 0;

    // oringal position of the game object
    private Vector3 pos;

    private void Start()
    {
        // sets orginal position
        pos = transform.position;
    }

    void Update ()
    {
        // rotate game object
        Rotate();

        // stops the object from floating upwards infinatly when game is paused
        if (Time.timeScale != 0)
            Bob();
    }

    // rotate the game object at a set speed
    private void Rotate()
    {
        transform.Rotate(0, Speed, 0);
    }

    // move the object up and down
    private void Bob()
    {
        // gose up
        if (time >= 1)
        {
            transform.Translate(0, -bob, 0);
        }
        // gose down
        else
        {
            transform.Translate(0, bob, 0);
        }

        // reset the position so it dosn't go out of whack
        if (time >= 2)
        {
            time = 0;
            transform.position = pos;
        }

        time += Time.deltaTime;
    }
}
