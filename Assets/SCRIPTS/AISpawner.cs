using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour {

    [Header("Spawn Locations")]
    public Transform Spawn1;
    //public Transform Spawn2;
    //public Transform Spawn3;

    [Header("AI Prefab")]
    public GameObject AI;

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        BoxCollider b = GetComponent<BoxCollider>();
        b.enabled = false;
        Instantiate(AI, Spawn1.transform.position, Spawn1.transform.rotation);
    }

}
