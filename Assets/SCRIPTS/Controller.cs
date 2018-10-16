using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Reflection;

[RequireComponent(typeof(Rigidbody))]
public class Controller : MonoBehaviour {

    public Camera cam;

    private Vector3 velocity = Vector3.zero;
    private Vector3 MouseY = Vector3.zero;
    private Vector3 MouseX = Vector3.zero;
    private Vector3 jump = Vector3.zero;

    float shoot = 0;
    public float BulletSpeed;

    private Rigidbody rb;

    public GameObject Prefab;
    public Transform Barrel;

    public GameObject Laser;

    public Transform Weapon;
    private GameObject currentlyHolding;

    public float aimOffset;

    public GameObject Weapon1;
    public GameObject Weapon2;
    public GameObject Weapon3;
    public GameObject Weapon4;

    private Vector3 weaponPos;

    private bool Hasweapon = false;

    private int firingMode;
    private bool fired = true;

    Animator Anime;

    // Use this for initialization
    void Start ()
    {
        Anime = GetComponent<Animator>();

        // Get component Rigidbody
        rb = GetComponent<Rigidbody>();

        // Save weapon position
        weaponPos = Weapon.transform.localPosition;

        // spawn with kendo stick
        currentlyHolding = Instantiate(Weapon2, Weapon.position, Weapon.rotation);
        currentlyHolding.transform.parent = GameObject.Find("Weapon").transform;
        currentlyHolding.transform.localPosition = Weapon2.transform.position;
        currentlyHolding.transform.localRotation = Weapon2.transform.rotation;
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
                    pos = inputRay.GetPoint(200);
                }

                Vector3 dir = (pos - Barrel.transform.position);
                GameObject bullet = Instantiate(Prefab, Barrel.position, Quaternion.LookRotation(dir)) as GameObject;
                bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * BulletSpeed);
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

                Vector3 dir = (pos - Barrel.transform.position);

                Vector3 Offset = new Vector3(Random.Range(-aimOffset, aimOffset), Random.Range(-aimOffset, aimOffset), Random.Range(-aimOffset, aimOffset));

                Quaternion rot = Quaternion.LookRotation(dir + Offset);

                GameObject bullet = Instantiate(Prefab, Barrel.position, rot) as GameObject;
                bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * BulletSpeed);
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
            Destroy(currentlyHolding);
            currentlyHolding = Instantiate(Weapon2, Weapon.position, Weapon.rotation);
            currentlyHolding.transform.parent = GameObject.Find("Weapon").transform;
            currentlyHolding.transform.localPosition = Weapon2.transform.position;
            currentlyHolding.transform.localRotation = Weapon2.transform.rotation;
            Hasweapon = false;
            firingMode = 0;
        }

        if (Input.GetKeyDown("2"))
        {
            Destroy(currentlyHolding);
            Weapon.transform.localPosition = weaponPos;
            Weapon.transform.localRotation = new Quaternion();
            currentlyHolding = Instantiate(Weapon1, Weapon.position, Weapon.rotation);
            currentlyHolding.transform.parent = GameObject.Find("Weapon").transform;
            Hasweapon = true;
            firingMode = 1;
        }
        if (Input.GetKeyDown("3"))
        {
            Destroy(currentlyHolding);
            Weapon.transform.localPosition = weaponPos;
            Weapon.transform.localRotation = Weapon3.transform.rotation;
            currentlyHolding = Instantiate(Weapon3, Weapon.position, Weapon.rotation);
            currentlyHolding.transform.parent = GameObject.Find("Weapon").transform;
            currentlyHolding.transform.localPosition = Weapon3.transform.position;
            currentlyHolding.transform.localRotation = Weapon3.transform.rotation;
            Hasweapon = true;
            firingMode = 2;
        }
        if (Input.GetKeyDown("4"))
        {
            Destroy(currentlyHolding);
            Weapon.transform.localPosition = weaponPos;
            Weapon.transform.localRotation = new Quaternion();
            currentlyHolding = Instantiate(Weapon4, Weapon.position, Weapon.rotation);
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
