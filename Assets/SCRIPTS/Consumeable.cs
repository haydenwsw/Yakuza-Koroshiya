using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <Summary>

    // This script is used for that floating bobbing effect for Consumeable items

// </Summary>

public class Consumeable : MonoBehaviour
{
    // Item rotation speed
    public float Speed = 2;

    // bobing disance
    public float bob; // = 0.01f;

    // Toggle weather to item spawns back or not
    public bool Spawn;

    // how long until the item respawns back
    public float spawnTime;

    // weather or not the item has been consumed by the player or not
    public bool eaten = false;

    // Respawn timer delat time
    private float time2 = 0;

    // movement delta time
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
            if (!eaten)
                Bob();

        // so the item can spawn back into the world
        if (Spawn)
        {
            if (eaten)
            {
                time2 += Time.deltaTime;

                if (time2 > spawnTime)
                {
                    time2 = 0;
                    transform.position = pos;
                    eaten = false;
                }
            }
        }
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
