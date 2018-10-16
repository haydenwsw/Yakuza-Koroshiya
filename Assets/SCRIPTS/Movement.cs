using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour {

    [SerializeField]
    private float Speed = 5f;

    [SerializeField]
    private float Sensitivity = 10f;

    private Controller con;

    private bool zoom;

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

        Vector3 vecUP = new Vector3(0f, jump, 0f) * Sensitivity;

        con.Jump(vecUP);

        // Fire weapon
        float fire = Input.GetAxisRaw("Fire1");

        con.Shoot(fire);

        // change FOV
        if (Input.GetKeyDown("c"))
        {
            zoom = true;
            con.cameraZoom(zoom);
        }
        if (Input.GetKeyUp("c"))
        {
            zoom = false;
            con.cameraZoom(zoom);
        }
    }
}