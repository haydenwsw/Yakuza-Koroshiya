using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {
    // gameobject hand
    public Transform onhand;
    // gameobject varable
    private GameObject currentlyHolding;
    // controller script
    private Controller con;
    // boolean for toggle input
    private bool toggle = false;
    // throwing force
    public float force;

    // Use this for initialization
    void Start () {
        // get the controller script
        con = GetComponent<Controller>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        RaycastHit hit;

        // if currently holding nothing and if button is down and the raycast is hitting an object
        if (currentlyHolding == null && Input.GetButtonDown("Use") && Physics.Raycast(con.cam.transform.position, con.cam.transform.forward, out hit, 100) && toggle == false)
        {
            // if the raycast hit an object that is able to be carried
            if (hit.transform.gameObject.tag == "Block")
            {
                toggle = !toggle;
                // set the object that was hit to a gameobject varable
                currentlyHolding = hit.transform.gameObject;
                // freeze the position of the object
                currentlyHolding.GetComponent<Rigidbody>().isKinematic = true;
                // move the position of the object to your hand gameobject
                currentlyHolding.transform.position = onhand.position;
                // add it as a child in hand gameobject
                currentlyHolding.transform.parent = GameObject.Find("Hand").transform;
            }
        }
        // if you drop the object
        else if (Input.GetButtonDown("Use") && toggle == true && currentlyHolding != null)
        {
            toggle = !toggle;
            // set hand gameobject to parenting to null
            currentlyHolding.transform.parent = null;
            // un freeze the object 
            currentlyHolding.GetComponent<Rigidbody>().isKinematic = false;
            // set the gameobject vatable back to null
            currentlyHolding = null;
        }
        // through an object
        else if (Input.GetButtonDown("Fire1") && currentlyHolding != null)
        {
            toggle = !toggle;
            // set hand gameobject to parenting to null
            currentlyHolding.transform.parent = null;
            // un freeze the object 
            currentlyHolding.GetComponent<Rigidbody>().isKinematic = false;
            // though object
            currentlyHolding.GetComponent<Rigidbody>().AddForce(con.cam.transform.forward * force);
            // set the gameobject vatable back to null
            currentlyHolding = null;
        }
    }
}
