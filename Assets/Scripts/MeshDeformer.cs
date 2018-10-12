using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// get the compontent meshfilter so we can use it
[RequireComponent(typeof(MeshFilter))]
public class MeshDeformer : MonoBehaviour
{
    // this determines radius of the hole you displace
    public float Radius;

    // this determines the maxium depth of the hole you displace
    public float maxiumDepth;

    // setting up the verties varables
    public List<Vector3> ORIGINALVERTS;
    public List<Vector3> modifiedVerts;

    // getting the MeshFilter class
    private MeshFilter meshFilter;

    void Start()
    {
        // getting the component
        meshFilter = GetComponent<MeshFilter>();

        // here where going to move all the verties maually because the engine dosn't do it for us
        SetVertsTransform();        
    }

    // funtion the creates the hole passes through the point of collision and the direction
    public void DisplaceVerties(Vector3 point, Vector3 dir)
    {
        // loop through all the verties so we can check them
        for (int i = 0; i < modifiedVerts.Count; ++i)
        {
            // gets the distance between the collison point and one of the verties in the meshFilter
            var distance = (point - modifiedVerts[i]).magnitude;
            // checks if the distance we just calulated is in the radius
            if (distance <= Radius)
            {
                // y=x^2 dose that parabola equation to get curvature in hole we wanna displace
                float newDepth = Mathf.Pow((Radius - distance) / Radius, 2) * maxiumDepth;
                // adds direction to the new verties
                var newVert = modifiedVerts[i] - dir * newDepth;
                // Replaces the verties in the list
                modifiedVerts[i] = newVert;
            }
        }
        // Update the mesh filter
        meshFilter.mesh.SetVertices(modifiedVerts);
    }

    private void SetVertsTransform()
    {
        // get all the verties so we can set them
        List<Vector3> Verts = meshFilter.sharedMesh.vertices.ToList();

        // getting some useful varables we will use later
        Vector3 objPos = transform.position;
        Quaternion objRot = new Quaternion();
        objRot.eulerAngles = transform.rotation.eulerAngles;
        Vector3 objScale = transform.lossyScale;

        // set the position to zero
        transform.position = Vector3.zero;

        // set the rotation to zero
        transform.rotation = new Quaternion();

        // set childs scale to the percentage of the parents scale
        Vector3 Scale = new Vector3(1, 1, 1);
        Scale.x /= transform.lossyScale.x;
        Scale.y /= transform.lossyScale.y;
        Scale.z /= transform.lossyScale.z;
        transform.localScale = Scale;

        //setting the verties to the objects transform
        for (int i = 0; i < Verts.Count; ++i)
        {
            //doing transform.scale
            var vertex = Verts[i];
            vertex.x *= objScale.x;
            vertex.y *= objScale.y;
            vertex.z *= objScale.z;
            Verts[i] = vertex;

            //doing transform.position
            Verts[i] += objPos;

            //doing transform.rotation
            //Verts[i] = objRot * (Verts[i] - objPos) + objPos;
        }
        
        // set the verties to the mesh filter
        meshFilter.mesh.SetVertices(Verts);

        // recalulateBounds because the mesh renderer is at zero
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.RecalculateBounds();

        // opitmizes the mesh for updating
        meshFilter.mesh.MarkDynamic();

        // gets all the original verties from the meshfilter (this varable is suposed to be const)
        ORIGINALVERTS = Verts;
        // gets all the original verties from the meshfilter but this one is for modifying
        modifiedVerts = Verts;
    }

    void Update()
    {
        for (int i = 0; i < ORIGINALVERTS.Count; ++i)
        {
            Vector3 v = new Vector3(0, 0.1f, 0);
            Debug.DrawLine(ORIGINALVERTS[i], ORIGINALVERTS[i] + v);
        }
    }
}