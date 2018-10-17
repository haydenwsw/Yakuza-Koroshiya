using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deleter : MonoBehaviour {

	void Update ()
    {
        if(this.transform.position.y < -50)
        {
            Destroy(this.gameObject);
        }

        
        //if ()
        //{

        //}
	}
    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        Destroy(this.gameObject);
    }
}
