using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    private float time = 0;

	void Update ()
    {
        time += Time.deltaTime;

        if (time >= 0.1)
        {
            Destroy(this.gameObject);
            time = 0;
        }
    }
}
