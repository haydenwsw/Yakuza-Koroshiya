using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <Summary>

    // This script dose all the keyinputs

// </Summary>

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour {

    //player walking speed
    [SerializeField]
    private float Speed;

    // looking sensitivity
    public float Sensitivity;

    // Refence to the players UI
    public Canvas MainCanvas;

    // Refence to the pause canvas
    public Canvas PauseCanvas;

    // Refence to the options canvas
    public Canvas OptionsCanvas;

    // toggle if the player can pause or not
    public bool CanPause = true;

    // Refence to the controller script
    private Controller con;

    // "C" input
    private bool zoom;

    //  "1" input
    private bool Weapon1;

    //  "2" input
    private bool Weapon2;

    //  "3" input
    private bool Weapon3;

    // "4" input
    private bool Weapon4;

    // so the script knows if the game is paused or not
    private bool isPaused = false;

    // players camera rotation
    private Quaternion CameraRot;

    // players Character rotation
    private Quaternion CharacterRot;

    // Use this for initialization
    void Start ()
    {
        // get controller script
        con = GetComponent<Controller>();

        // Lock the mouse
        Cursor.lockState = CursorLockMode.Locked;

        // sets the camera rotation varable
        CameraRot = Camera.main.transform.localRotation;

        // sets the Character rotation varable
        CharacterRot = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        // get movement velocity
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical"); 

        Vector3 vecH = transform.right * x;
        Vector3 vecV = transform.forward * z;

        Vector3 vel = (vecH + vecV).normalized * Speed;

        // apply movement
        con.Move(vel);

        // camera y axis
        float yRot = Input.GetAxisRaw("Mouse X");

        // camera x axis
        float xRot = Input.GetAxisRaw("Mouse Y");

        CharacterRot *= Quaternion.Euler(0f, yRot * Sensitivity, 0f);
        CameraRot *= Quaternion.Euler(-xRot * Sensitivity, 0f, 0f);

        CameraRot = ClampRotationAroundXAxis(CameraRot);

        con.moveMouseX(CameraRot);
        con.moveMouseY(CharacterRot);


        // Fire weapon
        {
            float fire = Input.GetAxisRaw("Fire1");

            con.Shoot(fire);
        }

        // reload weapon
        {
            float reload = Input.GetAxisRaw("Reload");

            con.Reload(reload);
        }

        // key input for switch to the kendo stick
        {
            if (Input.GetKeyDown("1"))
            {
                Weapon1 = true;
                con.Get1(Weapon1);
            }
            else if (Input.GetKeyUp("1"))
            {
                Weapon1 = false;
                con.Get1(Weapon1);
            }
        }

        // key input for switch to the laser psitol
        {
            if (Input.GetKeyDown("2"))
            {
                Weapon2 = true;
                con.Get2(Weapon2);
            }
            else if (Input.GetKeyUp("2"))
            {
                Weapon2 = false;
                con.Get2(Weapon2);
            }
        }

        // key input for switch to the rifle
        {
            if (Input.GetKeyDown("3"))
            {
                Weapon3 = true;
                con.Get3(Weapon3);
            }
            else if (Input.GetKeyUp("3"))
            {
                Weapon3 = false;
                con.Get3(Weapon3);
            }
        }

        // key input for switch to the shotgun
        {
            if (Input.GetKeyDown("4"))
            {
                Weapon4 = true;
                con.Get4(Weapon4);
            }
            else if (Input.GetKeyUp("4"))
            {
                Weapon4 = false;
                con.Get4(Weapon4);
            }
        }

        // pause function "esc"
        if (CanPause)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
                {
                    Unpause();
                }
                else
                {
                    Time.timeScale = 0;
                    MainCanvas.enabled = false;
                    PauseCanvas.enabled = true;
                    GameObject.Find("SPAWNS").GetComponent<Score>().score.enabled = false;
                    Cursor.lockState = CursorLockMode.None;
                    isPaused = !isPaused;
                }
            }
        }

        // change FOV
        if (Input.GetKeyDown("c"))
        {
            zoom = true;
            //con.cameraZoom(zoom);
        }
        else if (Input.GetKeyUp("c"))
        {
            zoom = false;
            //con.cameraZoom(zoom);
        }
    }

    // fuction to lock the camera rotaion 
    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, -90, 90);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

    // Unpause funtion 
    public void Unpause()
    {
        Time.timeScale = 1;
        MainCanvas.enabled = true;
        PauseCanvas.enabled = false;
        OptionsCanvas.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameObject.Find("SPAWNS").GetComponent<Score>().score.enabled = true;
        isPaused = !isPaused;
    }
}