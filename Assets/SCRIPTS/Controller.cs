using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Reflection;

[RequireComponent(typeof(Rigidbody))]
public class Controller : MonoBehaviour {

    // camera
    public Camera cam;

    // global bulet speed
    [Header("Global bullet speed")]
    public float BulletSpeed;

    [Header("Rifle varables")]
    public float RifleFireRate;
    public float RifleSpread;
    
    [Header("Shotgun varables")]
    public float ShotgunFireRate;
    public float ShotgunSpread;
    public int ShotgunPellets;

    // Gun projectiles
    [Header("Gun projectiles prefabs")]
    public GameObject Bullet;
    public GameObject Laser;
    public GameObject Pellet;

    // real world positioning objects
    [Header("Real world positions for objects")]
    public Transform Barrel;
    public Transform Weapon;

    // all the weapons
    [Header ("All weapons prefabs")]
    public GameObject KendoStick;
    public GameObject LaserPistol;
    public GameObject AssultRifle;   
    public GameObject AutoShotty;

    // movement varables
    private Vector3 velocity = Vector3.zero;
    private Vector3 MouseY = Vector3.zero;
    private Vector3 MouseX = Vector3.zero;
    private Vector3 jump = Vector3.zero;
    private float shoot = 0;
    
    private Rigidbody rb;

    private GameObject currentlyHolding;

    private Vector3 weaponPos;

    private bool Hasweapon = false;

    private int firingMode;

    private bool fired = true;

    private float time = 0;

    private Animator Anime;

    // Use this for initialization
    void Start ()
    {
        Anime = GetComponent<Animator>();

        // Get component Rigidbody
        rb = GetComponent<Rigidbody>();

        // Save weapon position
        weaponPos = Weapon.transform.localPosition;

        // spawn with kendo stick
        currentlyHolding = Instantiate(KendoStick, Weapon.position, Weapon.rotation);
        currentlyHolding.transform.parent = GameObject.Find("Weapon").transform;
        currentlyHolding.transform.localPosition = KendoStick.transform.position;
        currentlyHolding.transform.localRotation = KendoStick.transform.rotation;
        Hasweapon = false;
        firingMode = 0;
    }

    // set Velocity
    public void Move (Vector3 vel)
    {
        velocity = vel;
    }

    // Set MouseY
    public void moveMouseY(Vector3 rot)
    {
        MouseY = rot;
    }

    // Set mouseX
    public void moveMouseX(Vector3 rot)
    {
        MouseX = rot;
    }

    // Set Jump
    public void Jump(Vector3 j)
    {
        jump = j;
    }

    public void Shoot(float s)
    {
        shoot = s;
    }

    // update
    void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
        PerformJump();
        if (Hasweapon)
            PreformShoot();
        PreformWeaponSwitch();
    }

    // movement function
    void PerformMovement()
    {
        Anime.SetFloat("Speed", velocity.normalized.magnitude);
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }

    // looking function
    void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(MouseY));
        if (cam != null)
        {
            cam.transform.Rotate(-MouseX);

            float angle = cam.transform.localRotation.eulerAngles.x;
            if (angle >= 180)
            {
                angle = Mathf.Clamp(angle, 0, 0);
            }

            Vector3 newRot = cam.transform.localRotation.eulerAngles;
            newRot.x = angle;
            //if (newRot.z > 1)
            //    newRot.z = 0;
            
            //if (newRot.y > 1)
            //    newRot.y = 0;
            //newRot.z = 0;
            //newRot.y = 0;
            cam.transform.localRotation = Quaternion.Euler(newRot);
        }
    }

    // jumping function
    void PerformJump()
    {
        if (jump != Vector3.zero)
        {
            rb.MovePosition(rb.position + jump * Time.fixedDeltaTime);
        }
    }

    void PreformShoot()
    {
        if (shoot == 1)
        {
            if (firingMode == 0)
            {
                // sword

            }
            if (firingMode == 1)
            {
                // rifel
                Anime.SetTrigger("Fire");
                Vector3 pos = Vector3.zero;
                Ray inputRay = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(inputRay, out hit))
                {
                    pos = hit.point;
                }
                else
                {
                    pos = inputRay.GetPoint(10);
                }

                Debug.DrawLine(cam.transform.position, hit.point);

                time += Time.deltaTime;

                if (time >= RifleFireRate)
                {
                    Vector3 Offset = new Vector3(
                            Random.Range(-RifleSpread, RifleSpread),
                            Random.Range(-RifleSpread, RifleSpread),
                            Random.Range(-RifleSpread, RifleSpread));

                    Vector3 dir = (pos - Barrel.transform.position);

                    Quaternion rot = Quaternion.LookRotation(dir + Offset);
                  
                    // Quaternion.LookRotation(dir)
                    GameObject bullet = Instantiate(Bullet, Barrel.position, rot) as GameObject;
                    bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * BulletSpeed);
                    bullet.transform.localRotation = Barrel.rotation;
                    time = 0;
                }
            }
            if (firingMode == 2)
            {
                // laser pistol
                if (fired)
                {
                    Anime.SetTrigger("Fire");

                    Vector3 pos = Vector3.zero;
                    Ray inputRay = cam.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(inputRay, out hit))
                    {
                        pos = hit.point;
                    }
                    else
                    {
                        pos = inputRay.GetPoint(200);
                    }

                    Vector3 dir = (Barrel.transform.position - pos);

                    GameObject beam = Instantiate(Laser, Barrel.transform.position, Quaternion.LookRotation(dir)) as GameObject;
                    Vector3 laserScale = new Vector3(1, 1, -dir.magnitude);
                    beam.transform.localScale = laserScale;

                    fired = false;
                }
            }
            if (firingMode == 3)
            { 
                // auto shot gun
                Anime.SetTrigger("Fire");

                Vector3 pos = Vector3.zero;
                Ray inputRay = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(inputRay, out hit))
                {
                    pos = hit.point;
                }
                else
                {
                    pos = inputRay.GetPoint(200);
                }

                Debug.DrawLine(cam.transform.position, hit.point);

                Vector3 dir = (inputRay.GetPoint(200) - Barrel.transform.position);

                time += Time.deltaTime;

                if (time >= ShotgunFireRate)
                {
                    for (int i = 0; i < ShotgunPellets; i++)
                    {
                        Vector3 Offset = new Vector3(
                            Random.Range(-ShotgunSpread, ShotgunSpread),
                            Random.Range(-ShotgunSpread, ShotgunSpread),
                            Random.Range(-ShotgunSpread, ShotgunSpread));

                        Quaternion rot = Quaternion.LookRotation(dir + Offset);

                        GameObject pellet = Instantiate(Pellet, Barrel.position, rot) as GameObject;
                        pellet.GetComponent<Rigidbody>().AddForce(pellet.transform.forward * BulletSpeed);
                    }
                    time = 0;
                }
            }
        }
        else if (shoot == 0)
        {
            fired = true;
        }
    }

    void PreformWeaponSwitch()
    {
        //Switch weapons
        if (Input.GetKeyDown("1"))
        {
            // sword
            Destroy(currentlyHolding);
            currentlyHolding = Instantiate(KendoStick, Weapon.position, Weapon.rotation);
            currentlyHolding.transform.parent = GameObject.Find("Weapon").transform;
            currentlyHolding.transform.localPosition = KendoStick.transform.position;
            currentlyHolding.transform.localRotation = KendoStick.transform.rotation;
            Hasweapon = false;
            firingMode = 0;
        }

        if (Input.GetKeyDown("2"))
        {
            // rifel
            Destroy(currentlyHolding);
            Weapon.transform.localPosition = weaponPos;
            Weapon.transform.localRotation = new Quaternion();
            currentlyHolding = Instantiate(AssultRifle, Weapon.position, Weapon.rotation);
            currentlyHolding.transform.parent = GameObject.Find("Weapon").transform;
            Hasweapon = true;
            firingMode = 1;
        }
        if (Input.GetKeyDown("3"))
        {
            // laser pistol
            Destroy(currentlyHolding);
            Weapon.transform.localPosition = weaponPos;
            Weapon.transform.localRotation = LaserPistol.transform.rotation;
            currentlyHolding = Instantiate(LaserPistol, Weapon.position, Weapon.rotation);
            currentlyHolding.transform.parent = GameObject.Find("Weapon").transform;
            currentlyHolding.transform.localPosition = LaserPistol.transform.position;
            currentlyHolding.transform.localRotation = LaserPistol.transform.rotation;
            Hasweapon = true;
            firingMode = 2;
        }
        if (Input.GetKeyDown("4"))
        {
            // auto shotty
            Destroy(currentlyHolding);
            Weapon.transform.localPosition = weaponPos;
            Weapon.transform.localRotation = new Quaternion();
            currentlyHolding = Instantiate(AutoShotty, Weapon.position, Weapon.rotation);
            currentlyHolding.transform.parent = GameObject.Find("Weapon").transform;
            Hasweapon = true;
            firingMode = 3;
        }
    }

    // Optifine meme
    public void cameraZoom(bool b)
    {
        if (b)
            cam.fieldOfView = 20;
        else
            cam.fieldOfView = 100;
    }
}
