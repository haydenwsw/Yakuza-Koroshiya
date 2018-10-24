using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour {

    public float LookRadius = 10f;

    public GameObject Player;

    Transform target;
    NavMeshAgent agent;

	void Start ()
    {
        target = Player.transform;
        agent = GetComponent<NavMeshAgent>();

        transform.Rotate(-90, 0, 0);
	}
	
	void Update ()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= LookRadius)
        {
            agent.SetDestination(target.position);

            if (distance <= agent.stoppingDistance)
            {
                // attack the player
            }
        }
	}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, LookRadius);
    }
}
