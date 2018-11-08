using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    public float LenghtScale;
    public float DeleteTime;

    private float time = 0;
    private float laserIncrease = 1;

    void Update ()
    {
        time += Time.deltaTime;

        laserIncrease += time * LenghtScale;

        transform.localScale = new Vector3(0.5f, 0.5f, -laserIncrease);

        if (time >= DeleteTime)
        {
            Destroy(this.gameObject);
            time = 0;
        }
    }
}
