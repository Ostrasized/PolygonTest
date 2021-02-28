// Importing
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEditor;
using UnityEngine;

// Getting Mesh from Unity--this statement allows script objects to be changed
[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    // Declaring a mesh
    Mesh mesh;

    // Declaring the vertices and triangles of the 3D object
    Vector3[] vertices;
    int[] triangles;

    // Public integers for the map size, can be changed via Unity Editor (due to public property)
    public int xSize = 20;
    public int zSize = 20;

    // As the name suggests, runs when game starts.
    void Start()
    {
        // Creating a new Mesh object
        mesh = new Mesh();
        // Getting the MeshFilter data
        GetComponent<MeshFilter>().mesh = mesh;

        // Running functions to generate and display map
        CreateShape();
        UpdateMesh();
    }

    // "Updating" the Mesh. This function is only really called once and is meant to display the mesh in the game
    void UpdateMesh()
    {
        // Clearing any Mesh artifacts, useful for an actual update function
        mesh.Clear();

        // Assigning the mesh vertices and triangles
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        
        // Recalculating Normals for shading
        mesh.RecalculateNormals();
    }
    
    // Creating the Mesh shape (the land)
    void CreateShape()
    {
        // Creating a new Vector for vertices, vertices are always 1 more than the amount of tiles/squares
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        
        // Iterating through the Z-Axis, <= used due to the + 1 from earlier
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            // Iterating through the X-Axis
            for (int x = 0; x <= xSize; x++, i++)
            {
                // Getting simple Perlin Noise
                float y = Mathf.PerlinNoise(x * .3f, z * .3f) * 2f;
                // Assigning the given vertex
                vertices[i] = new Vector3(x, y, z);
            }
            
        }

        // Giving triangles an array of integers, 6 variables for each quad
        triangles = new int[xSize * zSize * 6];
        // Iterating through z values, declaring vert and tris
        for (int z = 0, vert = 0, tris = 0; z < zSize; z++)
        {
            // Iterating through x values
            for (int x = 0; x < xSize; x++, vert++, tris += 6)
            {
                // Assigning scale points
                triangles[tris] = vert;
                triangles[tris + 1] = triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 2] = triangles[tris + 3] = vert + 1;
                triangles[tris + 5] = vert + xSize + 2;   
            }
            vert++;
        }
    }

    // Drawing "Gizmos" to visualize the dot grid in Scene Editor, doesn't appear in game
    private void OnDrawGizmos()
    {
        // Escaping if vertices are non-existent
        if (vertices == null)
        {
            return;
        }
        // Iterating through the vertices, assigning them a sphere
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], .1f);
        }
    }
}
