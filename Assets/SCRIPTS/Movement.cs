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

    private Controller con;

    private bool zoom;

    private bool Weapon1;

    private bool Weapon2;

    private bool Weapon3;

    private bool Weapon4;

    // Use this for initialization
    void Start ()
    {
        // get controller script
        con = GetComponent<Controller>();

        // Lock the mouse
        Cursor.lockState = CursorLockMode.Locked;
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

        Vector3 vecY = new Vector3(0f, yRot, 0f) * Sensitivity;

        con.moveMouseY(vecY);

        // camera x axis
        float xRot = Input.GetAxisRaw("Mouse Y");

        Vector3 vecX = new Vector3(xRot, 0f, 0f) * Sensitivity;

        con.moveMouseX(vecX);

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
}