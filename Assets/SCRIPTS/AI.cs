using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// <Summary>

// This script controls The AI and the behaviour

// </Summary>

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
    public GameObject SmallHealth;

    [Header("UI Objects")]
    public GameObject AIHealthBar;

    // handles how what score prefab spawns when the eneme dies
    [Header("Erv don't touch this")]
    public int ScoreIndex;

    // location of the player
    private GameObject Player;

    // location of the waypoint
    private Transform Waypoint;

    // toggles if the AI has reached the way point or not
    bool point = false;

    // AI delta time
    private float time;

    // sets the player position
    private Transform target;

    // handles the navmesh
    private NavMeshAgent agent;

    // refence to the Score script
    private Score S;

    // hanldes the AI animations
    private Animator AIanim;

    void Start ()
    {
        // geting refence to the player
        Player = GameObject.FindGameObjectWithTag("Player");

        // set target 
        target = Player.transform;
        agent = GetComponent<NavMeshAgent>();

        // get the refecen to the Score script
        S = GetComponentInParent<Score>();

        // geting animator component
        AIanim = GetComponentInChildren<Animator>();
    }
	
	void Update ()
    {
        // first walk to the way point
        if (point == false)
        {
            AIanim.SetFloat("_isWalking", 1);

            float dist = Vector3.Distance(Waypoint.transform.position, transform.position);

            FaceTarget(Waypoint.transform.position);

            agent.SetDestination(Waypoint.transform.position);

            if (dist < WaypointRadius)
                point = true;
        }

        // if the player is in range or the has reached the way point track down the player
        if (point)  // Movement
        {
            AIanim.SetFloat("_isWalking", 1);

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

                    AIanim.SetFloat("_isWalking", 0);
                }
            }
        }

        // death
        if (AIHealth < 0)
        {
            // remove one from the counter
            GetComponentInParent<AISpawner>().AIDead();

            S.AddScore(ScoreIndex, transform.position, transform.rotation);

            // spawn rifle ammo (rifle enemie)
            if (Weapon == 0)
            {
                Instantiate(RifleAmmo, transform.position, RifleAmmo.transform.rotation).transform.parent = transform.parent;
            }

            // spawn smallhealth (lase pistol enemie)
            if (Weapon == 1)
            {
                Instantiate(SmallHealth, transform.position, SmallHealth.transform.rotation).transform.parent = transform.parent;
            }

            // spawn shotgun ammo (shotgun enemie)
            if (Weapon == 2)
            {
                Instantiate(ShotgunAmmo, transform.position, ShotgunAmmo.transform.rotation).transform.parent = transform.parent;
            }

            // player death sound
            SoundScript.PlaySound("Death");

            Destroy(this.gameObject);
        }
	}

    // face target
    private void FaceTarget(Vector3 face)
    {
        Vector3 dir = (face - transform.position).normalized;
        Quaternion lookRot = Quaternion.LookRotation(new Vector3(dir.x, 0f, dir.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * AIRotationSpeed);
    }

    // set the way point position
    public void SetTarget(Transform way)
    {
        Waypoint = way;
    }

    // recive damager from player
    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        // rifle
        if (collision.collider.tag == "Bullet")
            AIHealth -= BulletDamage;

        // shotgun
        if (collision.collider.tag == "Pellet")
            AIHealth -= PelletDamage;

        // laser pistol
        if (collision.collider.tag == "Laser")
            AIHealth -= LaserDamage;

        // kendo stick
        if (collision.collider.tag == "Sword")
            AIHealth -= SwordDamage;

        // update AI health bar
        AIHealthBar.transform.localScale = new Vector3(0.1f, 0.1f, AIHealth);
    }

    // hand head shots
    private void OnTriggerEnter(Collider other)
    {
        // rifle
        if (other.tag == "Bullet")
        {
            AIHealth -= BulletDamage * HeadshotMuilpler;
            S.AddScore(1, transform.position, transform.rotation);
        }

        // shotgun
        else if (other.tag == "Pellet")
        {
            AIHealth -= PelletDamage * HeadshotMuilpler;
            S.AddScore(1, transform.position, transform.rotation);
        }

        // laser pistol
        else if (other.tag == "Laser")
        {
            AIHealth -= LaserDamage * HeadshotMuilpler;
            S.AddScore(1, transform.position, transform.rotation);
        }

        // kendo stick
        else if (other.tag == "Sword")
        {
            AIHealth -= SwordDamage * HeadshotMuilpler;
            S.AddScore(1, transform.position, transform.rotation);
        }

        // update health bar
        AIHealthBar.transform.localScale = new Vector3(0.1f, 0.1f, AIHealth);
    }

    // draw dection radius for debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, OuterRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, InnerRadius);
    }
}