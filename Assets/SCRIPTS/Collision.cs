using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    public MeshDeformer meshdefomer;

    public int NUMBEROFHITS;

    private int Hits;

    void OnCollisionEnter(UnityEngine.Collision collision)
    {
        //if (collision.transform.gameObject.tag == "Block")
        {
            Hits++;
            foreach (var contact in collision.contacts)
            {
                if (Hits >= NUMBEROFHITS)
                {
                    Hits = 0;
                    // calulates the direction
                    Vector3 dir = (Camera.main.transform.position - contact.point).normalized;
                    //collision.gameObject.transform.position

                    // calls the funtion so we can create the hole
                    meshdefomer.DisplaceVerties(contact.point, dir);
                }
            }
        }
    }
}
