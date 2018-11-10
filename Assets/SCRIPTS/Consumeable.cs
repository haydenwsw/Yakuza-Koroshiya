using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumeable : MonoBehaviour
{
    public float Speed;
    public float bob;

    private float time = 0;
    private Vector3 pos;

    private void Start()
    {
        pos = transform.position;
    }

    void Update ()
    {
        Rotate();

        if (Time.timeScale != 0)
            Bob();
    }

    private void Rotate()
    {
        transform.Rotate(0, Speed, 0);
    }

    private void Bob()
    {
        if (time >= 1)
        {
            transform.Translate(0, -bob, 0);
        }
        else
        {
            transform.Translate(0, bob, 0);
        }

        if (time >= 2)
        {
            time = 0;
            transform.position = pos;
        }

        time += Time.deltaTime;
    }
}
