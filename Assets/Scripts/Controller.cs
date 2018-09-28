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

    private Rigidbody rb;

	// Use this for initialization
	void Start ()
    {
        // Get component Rigidbody
        rb = GetComponent<Rigidbody>();
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

    // update
    void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
        PerformJump();
    }

    // movement function
    void PerformMovement()
    {
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
            //if (angle >= 180)
            //{
            //    angle = 90;
            //}
            //else if (angle >= -180)
            //{
            //    angle = -90;
            //}
            //angle = Mathf.Clamp(angle, -90, 90);

            Vector3 newRot = cam.transform.localRotation.eulerAngles;
            newRot.x = angle;
            newRot.z = 0;
            newRot.y = 0;
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

    // Optifine meme
    public void cameraZoom(bool b)
    {
        if (b)
            cam.fieldOfView = 20;
        else
            cam.fieldOfView = 100;
    }
}
