using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// <Summary>

    // This script controls the players Weapons, health, UI and movment

// </Summary>

[RequireComponent(typeof(Rigidbody))]
public class Controller : MonoBehaviour {

    // Refence to the camera
    public Camera cam;

    [Header("Global Gun Varables")]
    public float BulletSpeed;
    public float ReloadDelay;

    [Header("Laser stick varables")]
    public float KendoStickRange;
    public float KendoTiming;

    [Header("Laser Pistol varables")]
    public float LaserDecayRate;
    public float LaserHeatRate;

    [Header("Rifle varables")]
    public float RifleFireRate;
    public float RifleSpread;
    public int RifleClipSize;
    public int RifleAmmoPickup;

    [Header("Shotgun varables")]
    public float ShotgunFireRate;
    public float ShotgunSpread;
    public int ShotgunClipSize;
    public int ShotgunPellets;
    public int ShotgunAmmoPickup;

    [Header("Damage Values")]
    public float LaserDamage;
    public float RifleDamage;
    public float ShotgunDamage;

    // Gun projectiles
    [Header("Gun projectiles prefabs")]
    public GameObject KendoHit;
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
    public Image LaserHeatBar;
    public Text Reloading;
    public Canvas DeathCanvas;
    public Image KendoIm;
    public Image KendoGr;
    public Image LaserIm;
    public Image LaserGr;
    public Image RifleIm;
    public Image RifleGr;
    public Image ShotgunIm;
    public Image ShotgunGr;
    public Text ScoreText;

    [Header("Players values")]
    public float AmourPerentage;
    public float SmallHealth;
    public float MediumHealth;
    public float LargeHealth;

    // Refence to the weapon that the player is currently equiped
    public GameObject currentlyHolding;

    // movement varables
    private Vector3 velocity = Vector3.zero;
    private Quaternion MouseY;
    private Quaternion MouseX;
    private Vector3 jump = Vector3.zero;

    // left mouse button input varable
    private float shoot = 0;

    // reload input varable
    private float reload = 0;

    // current rifle clip
    private int rifleAmmo;

    // current shotgun clip
    private int shotgunAmmo;

    // laser overheat varable
    private float laserHeat = 0;

    // rifle spare ammo pool varable
    private int RifleSpareAmmo = 0;

    // shotgun spare ammo pool varable
    private int ShotgunSpareAmmo = 0;

    // Refence to the player rigibody component
    private Rigidbody rb;

    // used for controller what shoots 
    private int firingMode;

    // used for semi-auto weapons 
    private bool fired = true;

    // weapon firing delta time
    private float time = 0;

    // weapon switching delta time
    private float time2 = 0;

    // players health
    private float health;

    // player amour
    private float amour;

    // used for weapon sway
    private Animator Anime;

    // used for all the weapon animations
    private Animator WeaponAnime;

    // for when the laser pistol over heats so the player can't fire
    private bool tooHot = false;

    // used for weapon switching so the player can't shoot while the weapons are switching
    private bool canShoot = true;

    // used for toggling when the weapon switching animation is finished
    private bool WeaponSwitching = false;

    // used for toggling when the kendo sword swinging animation is finished
    private bool Kendo = false;

    // used to time the shooting animation to the firing of the weapon
    private bool Shootgun = false;

    // used to time the shooting animation to the firing of the weapon
    private bool firedShotgun = true;

    // used so the player dosn't switch to another weapon with the swiching animation is playing
    private bool pressed = true;

    // used to know if the player has pickup the shotgun
    private bool hasShotgun = true;

    // used to know if the player has pickup the rifle
    private bool hasRifle = true;

    // so the script knows what weapon switch animations to play when switching weapons
    private int weaponIndex = 0;

    // refence to the movement script
    private Movement move;

    // Use this for initialization
    void Awake()
    {
        // Get Animator component
        Anime = GetComponent<Animator>();

        // Get component Rigidbody
        rb = GetComponent<Rigidbody>();

        // get Movement controller script
        move = GetComponent<Movement>();

        // spawn with kendo stick
        {
            firingMode = 0;
            currentlyHolding = GameObject.Find("WEAPON_Kendo");
            currentlyHolding.transform.parent = Weapon.transform;
            WeaponAnime = currentlyHolding.GetComponentInChildren<Animator>();
            WeaponAnime.SetTrigger("_weaponEquip");

            AmmoText.enabled = false;
            SpareAmmoText.enabled = false;

            LaserHeatBar.enabled = false;

            KendoGr.enabled = false;
            LaserIm.enabled = false;
            RifleIm.enabled = false;
            ShotgunIm.enabled = false;
        }

        // setting the ammo varables
        rifleAmmo = RifleClipSize;
        shotgunAmmo = ShotgunClipSize;

        // setting the viability of the ammo texts
        AmmoText.enabled = false;
        SpareAmmoText.enabled = false;
        LaserHeatBar.enabled = false;
        Reloading.enabled = false;

        // setting the health and amour values
        health = 1;
        amour = 1;

        //int BulletsCount = AllBullets.childCount;
        //for (int i = 0; i < BulletsCount; i++)
        //{
        //    Bullets[i] = AllBullets.GetChild(i);
        //}
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
        // getting the '1' key input for switch to the kendo sword
        if (b)
        {
            if (pressed)
            {
                if (firingMode != 0)
                {
                    pressed = false;
                    canShoot = false;
                    WeaponSwitching = true;
                    // triggering the animation for equiping
                    WeaponAnime.SetTrigger("_weaponUnequip");
                    weaponIndex = 0;
                    canShoot = true;
                    pressed = true;
                }
            }
        }
    }

    public void Get2(bool b)
    {
        // getting the '2' key input for switch to the kendo sword
        if (b)
        {
            if (pressed)
            {
                if (firingMode != 2)
                {
                    pressed = false;
                    canShoot = false;
                    WeaponSwitching = true;
                    // triggering the animation for equiping
                    WeaponAnime.SetTrigger("_weaponUnequip");
                    weaponIndex = 1;
                    pressed = true;
                    canShoot = true;
                }
            }
        }
    }

    public void Get3(bool b)
    {
        // getting the '3' key input for switch to the kendo sword
        if (b)
        {
            if (pressed)
            {
                if (hasRifle)
                {
                    if (firingMode != 1)
                    {
                        pressed = false;
                        canShoot = false;
                        WeaponSwitching = true;
                        // triggering the animation for equiping
                        WeaponAnime.SetTrigger("_weaponUnequip");
                        weaponIndex = 2;
                        canShoot = true;
                        pressed = true;
                    }
                }
            }
        }
    }
    public void Get4(bool b)
    {
        // getting the '4' key input for switch to the kendo sword
        if (b)
        {
            if (pressed)
            {
                if (hasShotgun)
                {
                    if (firingMode != 3)
                    {
                        pressed = false;
                        canShoot = false;
                        WeaponSwitching = true;
                        // triggering the animation for equiping
                        WeaponAnime.SetTrigger("_weaponUnequip");
                        weaponIndex = 3;
                        pressed = true;
                        canShoot = true;
                    }
                }
            }
        }
    }

    // update
    void FixedUpdate()
    {
        // Moving the player on key input
        PerformMovement();

        // Moving the player's camera for looking
        PerformRotation();

        // Shoot function for the player
        PreformShoot();

        // realoading function for the rifle and shotgun weapons
        PreformReload();

        // the overheat function for the laser pistol
        UpdateLaserHeat();

        // checking if the player is dead or not
        IsPlayerAlive();

        // waiting for the weapon switching animations
        SwitchingWeaponAnime();

        // Waiting for the kendo stick's swinging animation
        KendoSwing();

        // Waiting for the shotgun's firing animation
        ShotgunTime();
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

    // attack for the kendo sword
    void KendoSwing()
    {
        if (Kendo)
        {
            time += Time.deltaTime;

            // waiting for the kendo stick's swinging animation
            if (time > KendoTiming)
            {
                time = 0;

                // casting a raycast
                Vector3 pos = Vector3.zero;
                Ray inputRay = cam.ScreenPointToRay(Input.mousePosition);

                pos = inputRay.GetPoint(KendoStickRange);

                // instantiating a hit box
                Instantiate(KendoHit, pos, LaserHit.transform.rotation);

                // kendo stick hit sound FX
                SoundScript.PlaySound("Kendo");

                Kendo = false;
            }
        }
    }

    // shooting function
    void PreformShoot()
    {
        if (shoot == 1)
        {
            if (firingMode == 0)
            {
                // kendo stick
                if (canShoot)
                {
                    if (fired)
                    {
                        // playing the kendo stick firing aniamation
                        WeaponAnime.SetTrigger("_weaponFire");

                        // telling the script that the kendo script that the kendo stick is being swung
                        Kendo = true;

                        fired = false;
                    }
                }
            }
            if (firingMode == 1)
            {
                // rifle
                if (canShoot)
                {
                    // casting a ray cast
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

                    // fire rate
                    if (time >= RifleFireRate && rifleAmmo > 0)
                    {

                        Vector3 dir = cam.transform.forward;

                        Vector3 Offset = Vector3.zero;

                        // randomzing the spay pattern
                        {
                            Offset = new Vector3(
                                UnityEngine.Random.Range(-RifleSpread, RifleSpread),
                                UnityEngine.Random.Range(-RifleSpread, RifleSpread),
                                UnityEngine.Random.Range(-RifleSpread, RifleSpread));

                            dir = (pos - Barrel.transform.position);
                        }

                        Quaternion rot = Quaternion.LookRotation(dir + Offset);

                        // rifle firing animation
                        WeaponAnime.SetTrigger("_weaponFire");

                        // rifle sound FX
                        SoundScript.PlaySound("Rifle");

                        // instantiating the projectile
                        GameObject bullet = Instantiate(Bullet, Barrel.position, rot) as GameObject;
                        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * BulletSpeed);
                        bullet.transform.localRotation = Barrel.rotation;

                        time = 0;

                        // deducing the amuntion
                        rifleAmmo--;

                        // updaing the UI text
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
                        // firing animation
                        WeaponAnime.SetTrigger("_weaponFire");
                        // laser sound FX
                        SoundScript.PlaySound("Laser");

                        // ray casting the laser
                        Vector3 pos = Vector3.zero;
                        Ray inputRay = cam.ScreenPointToRay(Input.mousePosition);
                        RaycastHit hit;

                        if (Physics.Raycast(inputRay, out hit))
                        {
                            pos = hit.point;
                            //instantiating the hitbox of the laser
                            Instantiate(LaserHit, pos, LaserHit.transform.rotation);
                        }
                        else
                        {
                            pos = inputRay.GetPoint(200);
                        }

                        Vector3 dir = -cam.transform.forward;

                         dir = (Barrel.transform.position - pos);

                        // instantiating the laser projectile
                        Instantiate(Laser, Barrel.transform.position, Quaternion.LookRotation(dir));

                        // adding the the over heat bar
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
                    if (firedShotgun)
                    {
                        // ray cast 
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

                        if (shotgunAmmo > 0)
                        {
                            // spawn the amount of pellest
                            for (int i = 0; i < ShotgunPellets; i++)
                            {
                                // randomizing the spread
                                Vector3 Offset = new Vector3(
                                    UnityEngine.Random.Range(-ShotgunSpread, ShotgunSpread),
                                    UnityEngine.Random.Range(-ShotgunSpread, ShotgunSpread),
                                    UnityEngine.Random.Range(-ShotgunSpread, ShotgunSpread));

                                Quaternion rot = Quaternion.LookRotation(dir + Offset);

                                // shotgun firing animation
                                WeaponAnime.SetTrigger("_weaponFire");

                                // shotgun firing sound FX
                                SoundScript.PlaySound("Shotgun");

                                // instantiating the pellets for the shotgn
                                GameObject pellet = Instantiate(Pellet, Barrel.position, rot) as GameObject;
                                pellet.GetComponent<Rigidbody>().AddForce(pellet.transform.forward * BulletSpeed);
                            }

                            // deducing the amuntion
                            shotgunAmmo--;
                            // upadating the UI text
                            AmmoText.text = shotgunAmmo.ToString();

                            Shootgun = true;

                            firedShotgun = false;
                        }
                    }
                }
            }
        }
        else if (shoot == 0)
        {
            fired = true;
        }
    }

    // waits for the shotgun animtion to finish before firing again
    void ShotgunTime()
    {
        if (Shootgun)
        {
            time += Time.deltaTime;

            if (time > 0.6)
            {
                time = 0;

                firedShotgun = true;
                Shootgun = false;
            }
        }
    }

    // weapon switch for the kend stick
    private void SwitchSword()
    {
        firingMode = 0;

        currentlyHolding.transform.parent = null;
        currentlyHolding = GameObject.Find("WEAPON_Kendo");
        currentlyHolding.transform.parent = Weapon.transform;
        WeaponAnime = currentlyHolding.GetComponentInChildren<Animator>();
        WeaponAnime.SetTrigger("_weaponEquip");

        AmmoText.enabled = false;
        SpareAmmoText.enabled = false;

        LaserHeatBar.enabled = false;

        // weapon swithcing UI visual
        KendoIm.enabled = true;
        KendoGr.enabled = false;
        LaserIm.enabled = false;
        LaserGr.enabled = true;
        RifleIm.enabled = false;
        RifleGr.enabled = true;
        ShotgunIm.enabled = false;
        ShotgunGr.enabled = true;
    }

    // weapon switch for the laser pistol
    private void SwitchPistol()
    {
        firingMode = 2;

        currentlyHolding.transform.parent = null;
        currentlyHolding = GameObject.Find("WEAPON_Laser");
        currentlyHolding.transform.parent = Weapon.transform;
        WeaponAnime = currentlyHolding.GetComponentInChildren<Animator>();
        WeaponAnime.SetTrigger("_weaponEquip");
        AmmoText.enabled = false;
        SpareAmmoText.enabled = false;

        LaserHeatBar.enabled = true;

        // weapon swithcing UI visual
        KendoIm.enabled = false;
        KendoGr.enabled = true;
        LaserIm.enabled = true;
        LaserGr.enabled = false;
        RifleIm.enabled = false;
        RifleGr.enabled = true;
        ShotgunIm.enabled = false;
        ShotgunGr.enabled = true;
    }

    // weapon switching for the rifle
    private void SwitchRifle()
    {
        currentlyHolding.transform.parent = null;
        currentlyHolding = GameObject.Find("WEAPON_Rifle");
        currentlyHolding.transform.parent = Weapon.transform;
        WeaponAnime = currentlyHolding.GetComponentInChildren<Animator>();
        WeaponAnime.SetTrigger("_weaponEquip");

        AmmoText.enabled = true;
        AmmoText.text = rifleAmmo.ToString();
        SpareAmmoText.enabled = true;
        SpareAmmoText.text = RifleSpareAmmo.ToString();

        LaserHeatBar.enabled = false;

        firingMode = 1;

        // weapon swithcing UI visual
        KendoIm.enabled = false;
        KendoGr.enabled = true;
        LaserIm.enabled = false;
        LaserGr.enabled = true;
        RifleIm.enabled = true;
        RifleGr.enabled = false;
        ShotgunIm.enabled = false;
        ShotgunGr.enabled = true;
    }

    // weapon switching for the shotgun
    private void SwitchShotgun()
    {
        currentlyHolding.transform.parent = null;
        currentlyHolding = GameObject.Find("WEAPON_Shotgun");
        currentlyHolding.transform.parent = Weapon.transform;
        WeaponAnime = currentlyHolding.GetComponentInChildren<Animator>();
        WeaponAnime.SetTrigger("_weaponEquip");

        AmmoText.enabled = true;
        AmmoText.text = shotgunAmmo.ToString();
        SpareAmmoText.enabled = true;
        SpareAmmoText.text = ShotgunSpareAmmo.ToString();

        LaserHeatBar.enabled = false;

        firingMode = 3;

        // weapon swithcing UI visual
        KendoIm.enabled = false;
        KendoGr.enabled = true;
        LaserIm.enabled = false;
        LaserGr.enabled = true;
        RifleIm.enabled = false;
        RifleGr.enabled = true;
        ShotgunIm.enabled = true;
        ShotgunGr.enabled = false;
    }

    // waiting for the animtion for switching weapons to finish before the player can fire
    private void SwitchingWeaponAnime()
    {
        // weapon switching delta time
        if (WeaponSwitching)
        {
            time2 += Time.deltaTime;
        }

        // kendo stick switching animation
        if (firingMode == 0)
        {
            if (time2 > 1)
            {
                time2 = 0;
                WeaponSwitching = false;

                if (weaponIndex == 0)
                    SwitchSword();
                else if (weaponIndex == 1)
                    SwitchPistol();
                else if (weaponIndex == 2)
                    SwitchRifle();
                else if (weaponIndex == 3)
                    SwitchShotgun();
            }
        }

        // rifle switching animation
        else if (firingMode == 1)
        {
            if (time2 > 1)
            {
                time2 = 0;
                WeaponSwitching = false;

                if (weaponIndex == 0)
                    SwitchSword();
                else if (weaponIndex == 1)
                    SwitchPistol();
                else if (weaponIndex == 2)
                    SwitchRifle();
                else if (weaponIndex == 3)
                    SwitchShotgun();
            }
        }

        // laser pistol switching animation
        else if (firingMode == 2)
        {
            if (time2 > 1)
            {
                time2 = 0;
                WeaponSwitching = false;

                if (weaponIndex == 0)
                    SwitchSword();
                else if (weaponIndex == 1)
                    SwitchPistol();
                else if (weaponIndex == 2)
                    SwitchRifle();
                else if (weaponIndex == 3)
                    SwitchShotgun();
            }
        }

        // auto shotgun switching animation
        else if (firingMode == 3)
        {
            if (time2 > 1)
            {
                time2 = 0;
                WeaponSwitching = false;

                if (weaponIndex == 0)
                    SwitchSword();
                else if (weaponIndex == 1)
                    SwitchPistol();
                else if (weaponIndex == 2)
                    SwitchRifle();
                else if (weaponIndex == 3)
                    SwitchShotgun();
            }
        }
    }

    // weapon reloading function
    void PreformReload()
    {
        // realoading delta time
        if (Reloading.enabled)
        {
            time += Time.deltaTime;
        }

        int spareAmmo = Int32.Parse(SpareAmmoText.text);

        int ammo = Int32.Parse(AmmoText.text);
        int clip = 0;

        if (firingMode == 1)
            clip = RifleClipSize;
        else if (firingMode == 3)
            clip = ShotgunClipSize;

        // while realoading the player can't shoot
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

            // rifle reload
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

            // auto shotgun reload
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

    // take damge function
    public void TakeDamge(float damage)
    {
        // if the there is armour deduct a percentage of the health 
        if (amour > 0)
        {
            amour -= damage;
            health -= damage * AmourPerentage;
            AmourBar.rectTransform.localScale = new Vector3(amour, 1, 1);
        }
        else
        {
            // if the amour is deplted the player take full damage
            AmourBar.rectTransform.localScale = new Vector3(0, 1, 1);
            health -= damage;
        }

        // stops having minus health that is bad
        if (health > 0)
            HealthBar.rectTransform.localScale = new Vector3(health, 1, 1);       
        else
            HealthBar.rectTransform.localScale = new Vector3(0, 1, 1);
    }

    // Health function
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

    // take damge from the AI
    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        // take Laser pistol damage
        if (collision.gameObject.tag == "AILaser")
        {
            TakeDamge(LaserDamage);
            Destroy(collision.gameObject);
        }

        // take Rifle damage
        if (collision.gameObject.tag == "AIBullet")
        {
            TakeDamge(RifleDamage);
            Destroy(collision.gameObject);
        }

        // take shotgun damage
        if (collision.gameObject.tag == "AIPellet")
        {
            TakeDamge(ShotgunDamage);
            Destroy(collision.gameObject);
        }
    }

    // This is for all the pick ups
    private void OnTriggerEnter(Collider other)
    {
        // armout pick up
        if (other.gameObject.tag == "Armour")
        {
            if (amour != 1)
            {
                amour = 1;
                AmourBar.rectTransform.localScale = new Vector3(amour, 1, 1);
                other.gameObject.transform.position = Vector3.zero;
                other.gameObject.GetComponent<Consumeable>().eaten = true;
            }
        }

        // small health (dumpling)
        if (other.gameObject.tag == "HealthSmall")
        {
            Heal(SmallHealth, other.gameObject);
        }

        // medium health  (Suishi rolls)
        if (other.gameObject.tag == "HealthMedium")
        {
            Heal(MediumHealth, other.gameObject);
        }

        // Large health (Bento box)
        if (other.gameObject.tag == "HealthLarge")
        {
            Heal(LargeHealth, other.gameObject);
        }

        // Rifle ammo (Rifle gun)
        if (other.gameObject.tag == "RifleAmmo")
        {
            hasRifle = true;
            RifleSpareAmmo += RifleAmmoPickup;
            if (firingMode == 1)
                SpareAmmoText.text = RifleSpareAmmo.ToString();
            Destroy(other.gameObject);
        }

        // shot gun ammo (Shot gun)
        if (other.gameObject.tag == "ShotgunAmmo")
        {
            hasShotgun = true;
            ShotgunSpareAmmo += ShotgunAmmoPickup;
            if (firingMode == 3)
                SpareAmmoText.text = ShotgunSpareAmmo.ToString();
            Destroy(other.gameObject);
        }
    }

    // laser overheat function
    private void UpdateLaserHeat()
    {
        laserHeat -= Time.deltaTime / LaserDecayRate;

        // making sure the over heat bar dosn't goto minus values
        if (laserHeat < 0)
        {
            laserHeat = 0;
            tooHot = false;
        }

        // updating the bar in the UI
        LaserHeatBar.transform.localScale = new Vector3(1, laserHeat, 1);

        // check if the the laser pistol overheats
        if (laserHeat > 1)
        {
            tooHot = true;
            WeaponAnime.SetTrigger("_weaponOverheat");
        }
            
        if (tooHot == true)
            fired = false;
    }

    // Death function
    private void IsPlayerAlive()
    {
        // check if the player is dead or not
        if (health < 0)
        {
            DeathCanvas.enabled = true;
            move.MainCanvas.enabled = false;

            pressed = false;
            firingMode = -1;

            Cursor.lockState = CursorLockMode.None;

            // saving score and destroying all the AI
            Score Score = GameObject.Find("SPAWNS").GetComponent<Score>();

            if (move.CanPause)
            {
                ScoreText.text = Score.ScoreCount.ToString();
                //FileSaver.WriteString(Score.ScoreCount);
                move.CanPause = false;
            }

            Score.score.enabled = false;

            Score.DestroyAI();
        }
    }

    // retry function
    public void Retry()
    {
        // setting the ammo/weapon varables
        rifleAmmo = RifleClipSize;
        shotgunAmmo = ShotgunClipSize;
        RifleSpareAmmo = 0;
        ShotgunSpareAmmo = 0;
        hasRifle = false;
        hasShotgun = false;

        // spawn with kendo stick
        firingMode = 0;
        SwitchSword();
        WeaponSwitching = true;
        currentlyHolding.transform.localPosition = KendoStick.transform.position;

        // setting the viability of the ammo texts
        AmmoText.enabled = false;
        SpareAmmoText.enabled = false;
        LaserHeatBar.enabled = false;
        Reloading.enabled = false;

        // reseting player values and postion
        transform.position = new Vector3(-40.5f, 1.5f, -4f);
        health = 1;
        amour = 1;
        AmourBar.rectTransform.localScale = new Vector3(amour, 1, 1);
        HealthBar.rectTransform.localScale = new Vector3(health, 1, 1);
        DeathCanvas.enabled = false;
        move.MainCanvas.enabled = true;
        move.CanPause = true;

        GameObject.Find("SPAWNS").GetComponent<Score>().score.enabled = true;

        pressed = true;
    }

    // Zooming in with "C"
    //public void cameraZoom(bool b)
    //{
    //    if (b)
    //        cam.fieldOfView = 20;
    //    else
    //        cam.fieldOfView = 100;
    //}
}
