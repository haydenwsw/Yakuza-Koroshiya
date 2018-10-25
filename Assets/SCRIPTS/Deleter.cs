using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deleter : MonoBehaviour {

    private float time = 0;

    void Update ()
    {
        time += Time.deltaTime;
        if (time >= 2)
        {
            Destroy(this.gameObject);
        }
    }
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