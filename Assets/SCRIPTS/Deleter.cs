using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <Summary>

    // This script Delets the bullets and pellets

// </Summary>

public class Deleter : MonoBehaviour {

    // bullet delta time
    private float time = 0;

    // destoys it self after 2 seconds
    void Update ()
    {
        time += Time.deltaTime;
        if (time >= 2)
        {
            Destroy(this.gameObject);
        }
    }

    // destorys itself if it hits a something that isn't the player
    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            if (collision.gameObject.tag != this.gameObject.tag)
            {
                Destroy(this.gameObject);
            }
        }
    }
}