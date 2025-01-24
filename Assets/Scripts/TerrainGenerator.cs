using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainGenerator : MonoBehaviour
{
    public int gridSize = 50; 
    public float scale = 1f;
    public float heightMultiplier = 2f; 
    public float curveSmoothness = 1f; 

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;

    void Update()
    {
        GenerateTerrain();
    }

    void GenerateTerrain()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        vertices = new Vector3[(gridSize + 1) * (gridSize + 1)];
        triangles = new int[gridSize * gridSize * 6];

        for (int z = 0, i = 0; z <= gridSize; z++)
        {
            for (int x = 0; x <= gridSize; x++, i++)
            {
                float adjustedX = x / curveSmoothness;
                float adjustedZ = z / curveSmoothness;
                float y = Mathf.Sin(adjustedX * scale) * Mathf.Cos(adjustedZ * scale) * heightMultiplier; //Esta es la funcion, por si la quieren multiplicar, solo es cambiar el valor de y
                // La funcion que esta ahorita es basicamente un y = 2 * (sen(x/10)) * (cos(z/10)). Si la cambian debe tener Sen o Cos para que el terreno sea suave y manejable, sino quedaran pendientes pronunciadas, como con la de abajo.
                //float y = (adjustedX * adjustedX + adjustedZ * adjustedZ) * heightMultiplier;
                vertices[i] = new Vector3(x, y, z);
            }
        }

        int triIndex = 0;
        for (int z = 0; z < gridSize; z++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                int bottomLeft = z * (gridSize + 1) + x;
                int bottomRight = bottomLeft + 1;
                int topLeft = bottomLeft + gridSize + 1;
                int topRight = topLeft + 1;

                triangles[triIndex] = bottomLeft;
                triangles[triIndex + 1] = topLeft;
                triangles[triIndex + 2] = topRight;

                triangles[triIndex + 3] = bottomLeft;
                triangles[triIndex + 4] = topRight;
                triangles[triIndex + 5] = bottomRight;

                triIndex += 6;
            }
        }


        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals(); 

        MeshCollider meshCollider = GetComponent<MeshCollider>();
        if (meshCollider == null)
        {
            meshCollider = gameObject.AddComponent<MeshCollider>();
        }
        meshCollider.sharedMesh = mesh;
    }

    void OnDrawGizmos()
    {

        if (vertices == null) return;
        Gizmos.color = Color.red;
        foreach (var vertex in vertices)
        {
            Gizmos.DrawSphere(transform.position + vertex, 0.1f);
        }
    }
}
