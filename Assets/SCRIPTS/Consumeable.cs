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

        Bob();
    }

    private void Rotate()
    {
        transform.Rotate(0, 0, Speed);
    }

    private void Bob()
    {
        if (time >= 1)
        {
            transform.Translate(0, 0, -bob);
        }
        else
        {
            transform.Translate(0, 0, bob);
        }

        if (time >= 2)
        {
            time = 0;
            transform.position = pos;
        }

        time += Time.deltaTime;
    }
}
