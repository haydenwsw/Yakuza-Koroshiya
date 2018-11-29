using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <Summary>

    // This script is for handleing the laser beam

// </Summary>

public class Laser : MonoBehaviour
{
    // how fast the laser with shoot
    public float LenghtScale;

    // how much time until the laser delete its self
    public float DeleteTime;

    // laser delta time
    private float time = 0;
    
    // how much the laser withh increase by
    private float laserIncrease = 1;

    void Update ()
    {
        time += Time.deltaTime;

        // how the laset shoots at what speeds by
        laserIncrease += time * LenghtScale;

        // scakes the laser
        transform.localScale = new Vector3(0.5f, 0.5f, -laserIncrease);

        // deletes the laser after a certain time
        if (time >= DeleteTime)
        {
            Destroy(this.gameObject);
            time = 0;
        }
    }
}
