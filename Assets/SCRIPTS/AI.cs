using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour {

    [Header("Radius Values")]
    public float OuterRadius;
    public float InnerRadius;
    public float AIBulletSpeed;
    public float AISpeed;
    public float AIFireRate;
    public float AIHealth;

    [Header("Game Objects")]
    public GameObject Player;
    public Transform Barrel;
    public GameObject Bullet;

    private float time;

    private Transform target;
    private NavMeshAgent agent;

	void Start ()
    {
        target = Player.transform;
        agent = GetComponent<NavMeshAgent>();

        agent.speed = AISpeed;
    }
	
	void Update ()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        agent.transform.Translate(0, 0.7f, 0);
        agent.transform.Rotate(-90, 0, 0);

        if (distance <= OuterRadius)
        {
            agent.SetDestination(target.position);

            time += Time.deltaTime;

            // attack the player
            if (time >= AIFireRate)
            {
                GameObject bullet = Instantiate(Bullet, Barrel.position, Barrel.rotation) as GameObject;
                bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * AIBulletSpeed);
                bullet.transform.Rotate(90, 0, 0);

                time = 0;
            }

            if (distance <= InnerRadius)
            {
                agent.SetDestination(transform.position);
            }
        }
	}

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, OuterRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, InnerRadius);
    }
}
