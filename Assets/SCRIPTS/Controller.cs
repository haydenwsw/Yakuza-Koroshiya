﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Controller : MonoBehaviour {

    // camera
    public Camera cam;

    // global bulet speed
    [Header("Global Gun Varables")]
    public float BulletSpeed;
    public float ReloadDelay;
    public float GunsPersonalSpace;
    public float GunAnimeSpeed;
    public float GunAnimeTime;

    [Header("Laser Pistol varables")]
    public float LaserDecayRate;
    public float LaserHeatRate;

    [Header("Rifle varables")]
    public float RifleFireRate;
    public float RifleSpread;
    public int RifleClipSize;
    public int RifleSpareAmmo;
    public int RifleAmmoPickup;

    [Header("Shotgun varables")]
    public float ShotgunFireRate;
    public float ShotgunSpread;
    public int ShotgunClipSize;
    public int ShotgunSpareAmmo;
    public int ShotgunPellets;
    public int ShotgunAmmoPickup;

    [Header("Damage Values")]
    public float LaserDamage;
    public float RifleDamage;
    public float ShotgunDamage;

    // Gun projectiles
    [Header("Gun projectiles prefabs")]
    public GameObject Bullet;
    public GameObject Laser;
    public GameObject LaserHit;
    public GameObject Pellet;

    // real world positioning objects
    [Header("Real world positions for objects")]
    public Transform Barrel;
    public Transform Weapon;

    // all the weapons
    [Header("All weapons prefabs")]
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
    public Text Reloading;

    [Header("Players values")]
    public float AmourPerentage;
    public float SmallHealth;
    public float MediumHealth;
    public float LargeHealth;

    // movement varables
    private Vector3 velocity = Vector3.zero;
    private Quaternion MouseY;
    private Quaternion MouseX;
    private Vector3 jump = Vector3.zero;
    private float shoot = 0;
    private float reload = 0;

    private int rifleAmmo;
    private int shotgunAmmo;
    private float laserHeat = 0;

    private Rigidbody rb;

    private GameObject currentlyHolding;

    private Vector3 weaponPos;

    private bool Hasweapon = true;

    private int firingMode;

    private bool fired = true;

    private float time = 0;

    private float time2 = 0;

    private float health;

    private float amour;

    private Animator Anime;

    private bool tooHot = false;

    private bool canShoot = true;

    private bool canJump = false;

    private bool WeaponSwitching = false;

    private bool WeaponSwitched = true;

    private MeshCollider kendoCollider;

    private bool Kendo = false;

    private float translatedY = 0;

    private bool pressed = true;

    // Use this for initialization
    void Start()
    {
        Anime = GetComponent<Animator>();

        // Get component Rigidbody
        rb = GetComponent<Rigidbody>();

        // Save weapon position
        weaponPos = Weapon.transform.localPosition;

        // spawn with kendo stick
        firingMode = 0;
        SwitchSword();
        currentlyHolding.transform.localPosition = KendoStick.transform.position;

        // setting the ammo varables
        rifleAmmo = RifleClipSize;
        shotgunAmmo = ShotgunClipSize;

        // setting the viability of the ammo texts
        AmmoText.enabled = false;
        SpareAmmoText.enabled = false;
        LaserHeatBar.enabled = false;
        LaserHearBarBack.enabled = false;
        Reloading.enabled = false;

        // setting the health and amour values
        health = 1;
        amour = 1;
    }

    // set Velocity
    public void Move(Vector3 vel)
    {
        velocity = vel;
    }

    // Set MouseY
    public void moveMouseY(Quaternion rot)
    {
        MouseY = rot;
    }

    // Set mouseX
    public void moveMouseX(Quaternion rot)
    {
        MouseX = rot;
    }

    // Set Jump
    public void Jump(Vector3 j)
    {
        jump = j;
    }

    // Set Shoot
    public void Shoot(float s)
    {
        shoot = s;
    }

    // Set Reload
    public void Reload(float r)
    {
        reload = r;
    }

    public void Get1(bool b)
    {
        // sword
        if (b)
        {
            if (pressed)
            {
                if (firingMode != 0)
                {
                    canShoot = false;
                    WeaponSwitching = true;
                    WeaponSwitchAnime();
                    Kendo = true;
                    pressed = false;
                }
            }
        }
    }

    public void Get2(bool b)
    {
        // laser pistol
        if (b)
        {
            if (pressed)
            {
                if (firingMode != 2)
                {
                    canShoot = false;
                    WeaponSwitching = true;
                    WeaponSwitchAnime();
                    firingMode = 2;
                    pressed = false;
                }
            }
        }
    }

    public void Get3(bool b)
    {
        // rifle
        if (b)
        {
            if (pressed)
            {
                if (firingMode != 1)
                {
                    canShoot = false;
                    WeaponSwitching = true;
                    WeaponSwitchAnime();
                    firingMode = 1;
                    pressed = false;
                }
            }
        }
    }
    public void Get4(bool b)
    {
        // auto shotty
        if (b)
        {
            if (pressed)
            {
                if (firingMode != 3)
                {
                    canShoot = false;
                    WeaponSwitching = true;
                    WeaponSwitchAnime();
                    firingMode = 3;
                    pressed = false;
                }
            }
        }
    }

    // update
    void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
        PerformJump();
        PreformShoot();
        PreformReload();
        UpdateLaserHeat();
        WeaponSwitchAnime();
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
        rb.MoveRotation(MouseY.normalized);
        cam.transform.localRotation = MouseX;
    }

    // jumping function
    void PerformJump()
    {
        if (jump != Vector3.zero && canJump)
        {
            rb.AddForce(jump);
        }
    }

    void PreformShoot()
    {
        if (shoot == 1)
        {
            if (firingMode == 0)
            {
                // sword
                if (canShoot)
                {
                    currentlyHolding.transform.Rotate(22.5f, 0, 0);

                    kendoCollider.enabled = true;
                }
            }
            if (firingMode == 1)
            {
                // rifle
                if (canShoot)
                { 
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

                        Vector3 dir = cam.transform.forward;

                        Vector3 Offset = Vector3.zero;

                        float Mag = (Barrel.transform.position - pos).magnitude;
                        if (Mag < GunsPersonalSpace)
                        {
                            Debug.Log("Too Close personal space plz");
                        }
                        else
                        {
                            Offset = new Vector3(
                                UnityEngine.Random.Range(-RifleSpread, RifleSpread),
                                UnityEngine.Random.Range(-RifleSpread, RifleSpread),
                                UnityEngine.Random.Range(-RifleSpread, RifleSpread));

                            dir = (pos - Barrel.transform.position);
                        }

                        Quaternion rot = Quaternion.LookRotation(dir + Offset);

                        GameObject bullet = Instantiate(Bullet, Barrel.position, rot) as GameObject;
                        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * BulletSpeed);
                        bullet.transform.localRotation = Barrel.rotation;

                        time = 0;

                        rifleAmmo--;
                        AmmoText.text = rifleAmmo.ToString();
                    }
                }
            }
            if (firingMode == 2)
            {
                // laser pistol
                if (canShoot)
                {
                    if (fired)
                    {
                        Anime.SetTrigger("Fire");

                        Vector3 pos = Vector3.zero;
                        Ray inputRay = cam.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;

                        if (Physics.Raycast(inputRay, out hit))
                        {
                            pos = hit.point;
                            Instantiate(LaserHit, pos, LaserHit.transform.rotation);
                        }
                        else
                        {
                            pos = inputRay.GetPoint(200);
                            Instantiate(LaserHit, pos, LaserHit.transform.rotation);
                        }

                        Vector3 dir = -cam.transform.forward;

                        float Mag = (Barrel.transform.position - pos).magnitude;
                        if (Mag < GunsPersonalSpace)
                        {
                            Debug.Log("Too Close personal space plz");
                        }
                        else
                            dir = (Barrel.transform.position - pos);

                        GameObject beam = Instantiate(Laser, Barrel.transform.position, Quaternion.LookRotation(dir)) as GameObject;
                        beam.transform.parent = GameObject.Find("Weapon").transform;

                        laserHeat += LaserHeatRate;

                        fired = false;
                    }
                }
            }
            if (firingMode == 3)
            {
                // auto shot gun
                if (canShoot)
                {
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
                                UnityEngine.Random.Range(-ShotgunSpread, ShotgunSpread),
                                UnityEngine.Random.Range(-ShotgunSpread, ShotgunSpread),
                                UnityEngine.Random.Range(-ShotgunSpread, ShotgunSpread));

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
        }
        else if (shoot == 0)
        {
            fired = true;

            if (firingMode == 0)
                kendoCollider.enabled = false;
        }
    }

    private void WeaponSwitchAnime()
    {
        if (WeaponSwitching)
        { 
            if (WeaponSwitched)
            {
                currentlyHolding.transform.Translate(0, -GunAnimeSpeed, 0);
                translatedY += GunAnimeSpeed;
            }
            else
                currentlyHolding.transform.Translate(0, GunAnimeSpeed, 0);

            time2 += Time.deltaTime;

            if (time2 > GunAnimeTime / 2)
            {
                WeaponSwitched = false;

                if (Hasweapon)
                {
                    if (Kendo)
                        SwitchSword();
                    else if (firingMode == 1)
                        SwitchRifle();
                    else if (firingMode == 2)                       
                        SwitchPistol();
                    else if (firingMode == 3)
                        SwitchShotgun();

                    translatedY = 0;
                }

                Hasweapon = false;
            }
            if (time2 > GunAnimeTime)
            {
                WeaponSwitching = false;

                Hasweapon = true;

                WeaponSwitched = true;

                if (Kendo)
                {
                    currentlyHolding.transform.localPosition = KendoStick.transform.position;
                    Kendo = false;
                }
                else if (firingMode == 1)
                    currentlyHolding.transform.localPosition = AssultRifle.transform.position;
                else if (firingMode == 2)
                    currentlyHolding.transform.localPosition = LaserPistol.transform.position;
                else if (firingMode == 3)
                    currentlyHolding.transform.localPosition = AutoShotty.transform.position;

                canShoot = true;
                pressed = true;
                time2 = 0;
            }
        }
    }

    private void SwitchSword()
    {
        Destroy(currentlyHolding);
        currentlyHolding = Instantiate(KendoStick, Weapon.position, Weapon.rotation);
        currentlyHolding.transform.parent = GameObject.Find("Weapon").transform;
        currentlyHolding.transform.localPosition = new Vector3(KendoStick.transform.position.x, KendoStick.transform.position.y - translatedY / 1.4f, KendoStick.transform.position.z - translatedY / 2);
        currentlyHolding.transform.localRotation = KendoStick.transform.rotation;
        kendoCollider = currentlyHolding.GetComponent<MeshCollider>();
        kendoCollider.enabled = false;

        firingMode = 0;

        AmmoText.enabled = false;
        SpareAmmoText.enabled = false;

        LaserHearBarBack.enabled = false;
        LaserHeatBar.enabled = false;
    }

    private void SwitchPistol()
    {
        Destroy(currentlyHolding);
        Weapon.transform.localPosition = weaponPos;
        Weapon.transform.localRotation = LaserPistol.transform.rotation;
        currentlyHolding = Instantiate(LaserPistol, Weapon.position, Weapon.rotation);
        currentlyHolding.transform.parent = GameObject.Find("Weapon").transform;
        currentlyHolding.transform.localPosition = new Vector3(LaserPistol.transform.position.x, LaserPistol.transform.position.y - translatedY, LaserPistol.transform.position.z);
        currentlyHolding.transform.localRotation = LaserPistol.transform.rotation;
        AmmoText.enabled = false;
        SpareAmmoText.enabled = false;

        LaserHearBarBack.enabled = true;
        LaserHeatBar.enabled = true;
    }

    private void SwitchRifle()
    {
        Destroy(currentlyHolding);
        Weapon.transform.localPosition = weaponPos;
        Weapon.transform.localRotation = new Quaternion();
        currentlyHolding = Instantiate(AssultRifle, Weapon.position, Weapon.rotation);
        currentlyHolding.transform.parent = GameObject.Find("Weapon").transform;
        currentlyHolding.transform.localPosition = new Vector3(AssultRifle.transform.position.x, AssultRifle.transform.position.y - translatedY, AssultRifle.transform.position.z);
        currentlyHolding.transform.localRotation = AssultRifle.transform.rotation;

        AmmoText.enabled = true;
        AmmoText.text = rifleAmmo.ToString();
        SpareAmmoText.enabled = true;
        SpareAmmoText.text = RifleSpareAmmo.ToString();

        LaserHearBarBack.enabled = false;
        LaserHeatBar.enabled = false;
    }

    private void SwitchShotgun()
    {
        Destroy(currentlyHolding);
        Weapon.transform.localPosition = weaponPos;
        Weapon.transform.localRotation = new Quaternion();
        currentlyHolding = Instantiate(AutoShotty, Weapon.position, Weapon.rotation);
        currentlyHolding.transform.parent = GameObject.Find("Weapon").transform;
        currentlyHolding.transform.localPosition = new Vector3(AutoShotty.transform.position.x, AutoShotty.transform.position.y - translatedY, AutoShotty.transform.position.z);
        currentlyHolding.transform.localRotation = AutoShotty.transform.rotation;

        AmmoText.enabled = true;
        AmmoText.text = shotgunAmmo.ToString();
        SpareAmmoText.enabled = true;
        SpareAmmoText.text = ShotgunSpareAmmo.ToString();

        LaserHearBarBack.enabled = false;
        LaserHeatBar.enabled = false;
    }

    void PreformReload()
    {
        if (Reloading.enabled)
        {
            time += Time.deltaTime;
            //Anime.SetTrigger("Reload");
        }

        int spareAmmo = Int32.Parse(SpareAmmoText.text);

        int ammo = Int32.Parse(AmmoText.text);
        int clip = 0;

        if (firingMode == 1)
            clip = RifleClipSize;
        else if (firingMode == 3)
            clip = ShotgunClipSize;

        if (reload == 1 && spareAmmo != 0 && ammo != clip)
        {
            Reloading.enabled = true;
            canShoot = false;
        }

        if (time >= ReloadDelay)
        {
            time = 0;

            Reloading.enabled = false;

            canShoot = true;

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

    private void Heal(float food, GameObject obj)
    {
        if (health != 1)
        {
            health += food;
            if (health >= 1)
                health = 1;

            HealthBar.rectTransform.localScale = new Vector3(health, 1, 1);
            Destroy(obj);
        }
    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.gameObject.tag == "AILaser")
        {
            TakeDamge(LaserDamage);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "AIBullet")
        {
            TakeDamge(RifleDamage);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "AIPellet")
        {
            TakeDamge(ShotgunDamage);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Floor")
            canJump = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Armour")
        {
            if (amour != 1)
            {
                amour = 1;
                AmourBar.rectTransform.localScale = new Vector3(amour, 1, 1);
                Destroy(other.gameObject);
            }
        }

        if (other.gameObject.tag == "HealthSmall")
        {
            Heal(SmallHealth, other.gameObject);
        }

        if (other.gameObject.tag == "HealthMedium")
        {
            Heal(MediumHealth, other.gameObject);
        }

        if (other.gameObject.tag == "HealthLarge")
        {
            Heal(LargeHealth, other.gameObject);
        }

        if (other.gameObject.tag == "RifleAmmo")
        {
            RifleSpareAmmo += RifleAmmoPickup;
            if (firingMode == 1)
                SpareAmmoText.text = RifleSpareAmmo.ToString();
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "ShotgunAmmo")
        {
            ShotgunSpareAmmo += ShotgunAmmoPickup;
            if (firingMode == 3)
                SpareAmmoText.text = ShotgunSpareAmmo.ToString();
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionExit(UnityEngine.Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
            canJump = false;
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
