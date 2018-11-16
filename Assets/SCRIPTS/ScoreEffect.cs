using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreEffect : MonoBehaviour {

    private Vector3 face;

    private float time = 0;

    void Update ()
    {
        time += Time.deltaTime;
        if (time >= 1)
        {
            Destroy(this.gameObject);
        }

        face = GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector3 dir = (face - transform.position).normalized;
        Quaternion lookRot = Quaternion.LookRotation(new Vector3(dir.x, 0f, dir.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 20);
    }
}
