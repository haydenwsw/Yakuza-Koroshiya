using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour {

    [Header("Radius Values")]
    public float OuterRadius;
    public float InnerRadius;
    public float WaypointRadius;

    [Header("AI Values")]
    public int Weapon;
    public float AIBulletSpeed;
    public float AIFireRate;
    public float AIHealth;
    public float AIRotationSpeed;
    public float AIShotgunSpread;
    public float AIShotgunPellets;
    public float HeadshotMuilpler;

    [Header("Damage Values")]
    public float BulletDamage;
    public float PelletDamage;
    public float SwordDamage;
    public float LaserDamage;

    [Header("Game Objects")]
    public Transform Barrel;
    public GameObject Bullet;
    public GameObject Laser;
    public GameObject LaserHit;
    public GameObject Pellet;
    public GameObject RifleAmmo;
    public GameObject ShotgunAmmo;

    [Header("UI Objects")]
    public GameObject AIHealthBar;

    [Header("Erv don't touch this")]
    public int ScoreIndex;

    private GameObject Player;

    private Transform Waypoint;

    bool point = false;

    private float time;

    private Transform target;
    private NavMeshAgent agent;

    private Score S;

    void Start ()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        // set target 
        target = Player.transform;
        agent = GetComponent<NavMeshAgent>();

        // get the refecen to the Score script
        S = GetComponentInParent<Score>();
    }
	
	void Update ()
    {
        if (point == false)
        {
            float dist = Vector3.Distance(Waypoint.transform.position, transform.position);

            FaceTarget(Waypoint.transform.position);

            agent.SetDestination(Waypoint.transform.position);

            if (dist < WaypointRadius)
                point = true;
        }

        if (point)
        {
            float distance = Vector3.Distance(target.position, transform.position);

            FaceTarget(target.position);

            // follows player
            if (distance <= OuterRadius)
            {
                agent.SetDestination(target.position);

                Vector3 pos = Vector3.zero;
                RaycastHit hit;

                if (Physics.Raycast(transform.position, transform.forward, out hit))
                {
                    if (hit.transform.gameObject.tag == "Player")
                    {
                        time += Time.deltaTime;

                        if (time >= AIFireRate)
                        {
                            if (Weapon == 0)
                            {
                                // Rifle
                                GameObject bullet = Instantiate(Bullet, Barrel.position, Barrel.rotation) as GameObject;
                                bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * AIBulletSpeed);
                                bullet.transform.Rotate(90, 0, 0);

                            }
                            else if (Weapon == 1)
                            {
                                // Laser Pistol
                                pos = hit.point;
                                Instantiate(LaserHit, pos, LaserHit.transform.rotation);

                                Vector3 dir = Barrel.transform.position - pos;
                                Instantiate(Laser, Barrel.transform.position, Quaternion.LookRotation(dir));
                            }
                            else if (Weapon == 2)
                            {
                                // Shotgun
                                for (int i = 0; i < AIShotgunPellets; i++)
                                {
      
                                    Vector3 Offset = new Vector3(
                                        UnityEngine.Random.Range(-AIShotgunSpread, AIShotgunSpread),
                                        UnityEngine.Random.Range(-AIShotgunSpread, AIShotgunSpread),
                                        UnityEngine.Random.Range(-AIShotgunSpread, AIShotgunSpread));

                                    Vector3 dir = (hit.point - Barrel.transform.position);

                                    Quaternion rot = Quaternion.LookRotation(dir + Offset);

                                    GameObject pellet = Instantiate(Pellet, Barrel.position, rot) as GameObject;
                                    pellet.GetComponent<Rigidbody>().AddForce(pellet.transform.forward * AIBulletSpeed);
                                }
                            }

                            time = 0;
                        }
                    }
                }

                // personal space
                if (distance <= InnerRadius)
                {
                    agent.SetDestination(transform.position);
                }
            }
        }

        if (AIHealth < 0)
        {
            GetComponentInParent<AISpawner>().AIDead();

            S.AddScore(ScoreIndex, transform.position, transform.rotation);

            if (Weapon == 0)
            {
                Instantiate(RifleAmmo, transform.position, RifleAmmo.transform.rotation).transform.parent = transform.parent;
            }

            if (Weapon == 2)
            {
                Instantiate(ShotgunAmmo, transform.position, ShotgunAmmo.transform.rotation).transform.parent = transform.parent;
            }

            Destroy(this.gameObject);
        }
	}

    private void FaceTarget(Vector3 face)
    {
        Vector3 dir = (face - transform.position).normalized;
        Quaternion lookRot = Quaternion.LookRotation(new Vector3(dir.x, 0f, dir.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * AIRotationSpeed);
    }

    public void SetTarget(Transform way)
    {
        Waypoint = way;
    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.collider.tag == "Bullet")
            AIHealth -= BulletDamage;

        if (collision.collider.tag == "Pellet")
            AIHealth -= PelletDamage;

        if (collision.collider.tag == "Laser")
            AIHealth -= LaserDamage;

        if (collision.collider.tag == "Sword")
            AIHealth -= SwordDamage;

        AIHealthBar.transform.localScale = new Vector3(0.1f, 0.1f, AIHealth);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            AIHealth -= BulletDamage * HeadshotMuilpler;
            S.AddScore(1, transform.position, transform.rotation);
        }

        else if (other.tag == "Pellet")
        {
            AIHealth -= PelletDamage * HeadshotMuilpler;
            S.AddScore(1, transform.position, transform.rotation);
        }

        else if (other.tag == "Laser")
        {
            AIHealth -= LaserDamage * HeadshotMuilpler;
            S.AddScore(1, transform.position, transform.rotation);
        }

        else if (other.tag == "Sword")
        {
            AIHealth -= SwordDamage * HeadshotMuilpler;
            S.AddScore(1, transform.position, transform.rotation);
        }

        AIHealthBar.transform.localScale = new Vector3(0.1f, 0.1f, AIHealth);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, OuterRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, InnerRadius);
    }
}