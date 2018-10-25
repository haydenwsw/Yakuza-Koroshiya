using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Controller : MonoBehaviour {

    // camera
    public Camera cam;

    // global bulet speed
    [Header("Global bullet speed")]
    public float BulletSpeed;

    [Header("Laser Pistol varables")]
    public float LaserDecayRate;
    public float LaserHeatRate;

    [Header("Rifle varables")]
    public float RifleFireRate;
    public float RifleSpread;
    public int RifleClipSize;
    public int RifleSpareAmmo;
    
    [Header("Shotgun varables")]
    public float ShotgunFireRate;
    public float ShotgunSpread;
    public int ShotgunClipSize;
    public int ShotgunSpareAmmo;
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

    // UI stuff
    [Header("UI objects")]
    public Text AmmoText;
    public Text SpareAmmoText;
    public Image HealthBar;
    public Image AmourBar;
    public Image LaserHearBarBack;
    public Image LaserHeatBar;

    [Header("Players values")]
    public float AmourPerentage;

    // movement varables
    private Vector3 velocity = Vector3.zero;
    private Vector3 MouseY = Vector3.zero;
    private Vector3 MouseX = Vector3.zero;
    private Vector3 jump = Vector3.zero;
    private float shoot = 0;
    private float reload = 0;

    private int rifleAmmo;
    private int shotgunAmmo;
    private float laserHeat = 0;

    private Rigidbody rb;

    private GameObject currentlyHolding;

    private Vector3 weaponPos;

    private bool Hasweapon = false;

    private int firingMode;

    private bool fired = true;

    private float time = 0;

    private float health;

    private float amour;

    private Animator Anime;

    private bool tooHot = false;

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

        // setting the ammo varables
        rifleAmmo = RifleClipSize;
        shotgunAmmo = ShotgunClipSize;

        // setting the viability of the ammo texts
        AmmoText.enabled = false;
        SpareAmmoText.enabled = false;
        LaserHeatBar.enabled = false;
        LaserHearBarBack.enabled = false;

        // setting the health and amour values
        health = 1;
        amour = 1;
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

    public void Reload(float r)
    {
        reload = r;
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
        PreformReload();
        UpdateLaserHeat();

        if (Input.GetKeyDown("q"))
        {
            amour = 1;
        }
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
                // rifle
                if (rifleAmmo > 0)
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
                    pos = inputRay.GetPoint(20);
                }

                time += Time.deltaTime;

                if (time >= RifleFireRate && rifleAmmo > 0)
                {
                    Vector3 Offset = new Vector3(
                            Random.Range(-RifleSpread, RifleSpread),
                            Random.Range(-RifleSpread, RifleSpread),
                            Random.Range(-RifleSpread, RifleSpread));

                    Vector3 dir = (pos - Barrel.transform.position);

                    Quaternion rot = Quaternion.LookRotation(dir + Offset);

                    GameObject bullet = Instantiate(Bullet, Barrel.position, rot) as GameObject;
                    bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * BulletSpeed);
                    bullet.transform.localRotation = Barrel.rotation;

                    time = 0;
 
                    rifleAmmo--;
                    AmmoText.text = rifleAmmo.ToString();
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
                    beam.transform.parent = GameObject.Find("Weapon").transform;

                    laserHeat += LaserHeatRate;
                    
                    fired = false;
                }
            }
            if (firingMode == 3)
            { 
                // auto shot gun
                if (shotgunAmmo != 0)
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
                    pos = inputRay.GetPoint(20);
                }

                Vector3 dir = (inputRay.GetPoint(20) - Barrel.transform.position);

                time += Time.deltaTime;

                if (time >= ShotgunFireRate && shotgunAmmo > 0)
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

                    shotgunAmmo--;
                    AmmoText.text = shotgunAmmo.ToString();
                }
            }
        }
        else if (shoot == 0)
        {
            fired = true;        }
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

            AmmoText.enabled = false;
            SpareAmmoText.enabled = false;

            LaserHearBarBack.enabled = false;
            LaserHeatBar.enabled = false;
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

            AmmoText.enabled = true;
            AmmoText.text = rifleAmmo.ToString();
            SpareAmmoText.enabled = true;
            SpareAmmoText.text = RifleSpareAmmo.ToString();

            LaserHearBarBack.enabled = false;
            LaserHeatBar.enabled = false;
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

            AmmoText.enabled = false;
            SpareAmmoText.enabled = false;

            LaserHearBarBack.enabled = true;
            LaserHeatBar.enabled = true;
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

            AmmoText.enabled = true;
            AmmoText.text = shotgunAmmo.ToString();
            SpareAmmoText.enabled = true;
            SpareAmmoText.text = ShotgunSpareAmmo.ToString();

            LaserHearBarBack.enabled = false;
            LaserHeatBar.enabled = false;
        }
    }

    void PreformReload()
    {
        if (reload == 1)
        {
            if (firingMode == 1)
            {
                if (rifleAmmo != RifleClipSize)
                {
                    if (RifleSpareAmmo - (RifleClipSize - rifleAmmo) < 0)
                    {
                        rifleAmmo += RifleSpareAmmo;
                        RifleSpareAmmo = 0;
                    }
                    else
                    {
                        RifleSpareAmmo = RifleSpareAmmo - (RifleClipSize - rifleAmmo);
                        rifleAmmo = RifleClipSize;
                    }
                    AmmoText.text = rifleAmmo.ToString();
                    SpareAmmoText.text = RifleSpareAmmo.ToString();
                }
            }
            else if (firingMode == 3)
            {
                if (shotgunAmmo != ShotgunClipSize)
                {
                    if (ShotgunSpareAmmo - (ShotgunClipSize - shotgunAmmo) < 0)
                    {
                        shotgunAmmo += ShotgunSpareAmmo;
                        ShotgunSpareAmmo = 0;
                    }
                    else
                    {
                        ShotgunSpareAmmo = ShotgunSpareAmmo - (ShotgunClipSize - shotgunAmmo);
                        shotgunAmmo = ShotgunClipSize;
                    }
                    AmmoText.text = shotgunAmmo.ToString();
                    SpareAmmoText.text = ShotgunSpareAmmo.ToString();
                }
            }
        }
    }

    public void TakeDamge(float damage)
    {
        if (amour > 0)
        {
            amour -= damage;
            health -= damage * AmourPerentage;
            AmourBar.rectTransform.localScale = new Vector3(amour, 1, 1);
        }
        else
        {
            AmourBar.rectTransform.localScale = new Vector3(0, 1, 1);
            health -= damage;
        }
        if (health > 0)
            HealthBar.rectTransform.localScale = new Vector3(health, 1, 1);       
        else
            HealthBar.rectTransform.localScale = new Vector3(0, 1, 1);
    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.gameObject.tag == "Armour")
        {
            amour = 1;
            AmourBar.rectTransform.localScale = new Vector3(amour, 1, 1);
            Destroy(collision.collider.gameObject);
        }

        if (collision.gameObject.tag == "Bullet")
        {
            TakeDamge(0.01f);
            Destroy(collision.gameObject);
        }
    }

    private void UpdateLaserHeat()
    {
        laserHeat -= Time.deltaTime / LaserDecayRate;

        if (laserHeat < 0)
        {
            laserHeat = 0;
            tooHot = false;
        }

        LaserHeatBar.transform.localScale = new Vector3(1, laserHeat, 1);

        if (laserHeat > 1)
            tooHot = true;
            
        if (tooHot == true)
            fired = false;
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
