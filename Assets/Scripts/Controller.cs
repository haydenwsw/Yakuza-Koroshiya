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

    public Rigidbody Prefab;
    public Transform Barrel;

    public Transform Weapon;
    private GameObject currentlyHolding;

    public GameObject Weapon1;
    public GameObject Weapon2;

    private Vector3 weaponPos;

    private bool Hasweapon = false;

    private float time = 0;

    Animator _anim;

    // Use this for initialization
    void Start ()
    {
        _anim = GetComponent<Animator>();

        // Get component Rigidbody
        rb = GetComponent<Rigidbody>();

        // Save weapon position
        weaponPos = Weapon.transform.localPosition;
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
        _anim.SetFloat("Speed", velocity.normalized.magnitude);
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
            
            //time += Time.deltaTime;
            //Vector3 v = new Vector3(Time.deltaTime, 0, 0);
            //if (time <= 0.3)
            //{
            //    Weapon.transform.localPosition += v;
            //}
            //else if (time <= 0.6)
            //{
            //    Weapon.transform.localPosition -= v;
            //}
            //else if (time <= 0.61)
            //{
            //    time = 0;
            //}
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
            _anim.SetTrigger("Fire");
            Instantiate(Prefab, Barrel.position, Barrel.rotation).GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * BulletSpeed);
        }
    }

    void PreformWeaponSwitch()
    {
        //Switch weapons
        if (Input.GetKeyDown("1"))
        {
            Destroy(currentlyHolding);
            Weapon.transform.localRotation = Weapon2.transform.rotation;
            Weapon.transform.localPosition = new Vector3(0.52f, -0.71f, 0.07f);
            currentlyHolding = Instantiate(Weapon2, Weapon.position, Weapon.rotation);
            currentlyHolding.transform.parent = GameObject.Find("Weapon").transform;
            Hasweapon = false;
        }

        if (Input.GetKeyDown("2"))
        {
            Destroy(currentlyHolding);
            Weapon.transform.localRotation = new Quaternion();
            Weapon.transform.localPosition = weaponPos;
            currentlyHolding = Instantiate(Weapon1, Weapon.position, Weapon.rotation);
            currentlyHolding.transform.parent = GameObject.Find("Weapon").transform;
            Hasweapon = true;
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
