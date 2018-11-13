using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour {

    [SerializeField]
    private float Speed;

    [SerializeField]
    private float Sensitivity;

    [SerializeField]
    private float JumpHeight;

    public Canvas MainCanvas;

    public Canvas PauseCanvas;

    private Controller con;

    private bool zoom;

    private bool Weapon1;

    private bool Weapon2;

    private bool Weapon3;

    private bool Weapon4;

    private bool isPaused = false;

    private Quaternion CameraRot;
    private Quaternion CharacterRot;

    // Use this for initialization
    void Start ()
    {
        // get controller script
        con = GetComponent<Controller>();

        // Lock the mouse
        Cursor.lockState = CursorLockMode.Locked;

        CameraRot = Camera.main.transform.localRotation;
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

        CharacterRot *= Quaternion.Euler(0f, yRot, 0f);
        CameraRot *= Quaternion.Euler(-xRot, 0f, 0f);

        CameraRot = ClampRotationAroundXAxis(CameraRot);

        con.moveMouseX(CameraRot);
        con.moveMouseY(CharacterRot);

        // jump
        float jump = Input.GetAxisRaw("Jump");

        Vector3 vecUP = new Vector3(0f, jump, 0f) * JumpHeight;

        con.Jump(vecUP);

        // Fire weapon
        float fire = Input.GetAxisRaw("Fire1");

        con.Shoot(fire);

        // reload weapon
        float reload = Input.GetAxisRaw("Reload");

        con.Reload(reload);

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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Time.timeScale = 1;
                MainCanvas.enabled = true;
                PauseCanvas.enabled = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Time.timeScale = 0;
                MainCanvas.enabled = false;
                PauseCanvas.enabled = true;
                Cursor.lockState = CursorLockMode.None;
            }

            isPaused = !isPaused;
        }

        // change FOV
        if (Input.GetKeyDown("c"))
        {
            zoom = true;
            con.cameraZoom(zoom);
        }
        else if (Input.GetKeyUp("c"))
        {
            zoom = false;
            con.cameraZoom(zoom);
        }
    }

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
}