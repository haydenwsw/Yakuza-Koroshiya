using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <Summary>

    // This script Handles bilborading for score effect

// </Summary>

public class ScoreEffect : MonoBehaviour {

    // face of the game object
    private Vector3 face;

    // bilborading delta time
    private float time = 0;

    void Update ()
    {
        time += Time.deltaTime;

        // destroys itself after a second
        if (time >= 1)
        {
            Destroy(this.gameObject);
        }

        // bilborading facing the player
        face = GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector3 dir = (face - transform.position).normalized;
        Quaternion lookRot = Quaternion.LookRotation(new Vector3(dir.x, 0f, dir.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 20);
    }
}
