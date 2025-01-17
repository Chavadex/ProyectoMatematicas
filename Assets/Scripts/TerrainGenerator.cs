using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainGenerator : MonoBehaviour
{
    public int gridSize = 50; // Tamaño del terreno (número de divisiones en la cuadrícula)
    public float scale = 1f; // Escala del terreno
    public float heightMultiplier = 2f; // Altura máxima del terreno
    public float curveSmoothness = 1f; // Suavidad de las curvas (valores mayores hacen las curvas más suaves)

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;

    void Update()
    {
        GenerateTerrain();
    }

    void GenerateTerrain()
    {
        // Crear el mesh y los arrays necesarios
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        vertices = new Vector3[(gridSize + 1) * (gridSize + 1)];
        triangles = new int[gridSize * gridSize * 6];

        // Generar vértices
        for (int z = 0, i = 0; z <= gridSize; z++)
        {
            for (int x = 0; x <= gridSize; x++, i++)
            {
                // Ajustar el cálculo de altura con curveSmoothness
                float adjustedX = x / curveSmoothness;
                float adjustedZ = z / curveSmoothness;
                float y = Mathf.Sin(adjustedX * scale) * Mathf.Cos(adjustedZ * scale) * heightMultiplier;
                vertices[i] = new Vector3(x, y, z);
            }
        }

        // Generar triángulos
        int triIndex = 0;
        for (int z = 0; z < gridSize; z++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                int bottomLeft = z * (gridSize + 1) + x;
                int bottomRight = bottomLeft + 1;
                int topLeft = bottomLeft + gridSize + 1;
                int topRight = topLeft + 1;

                // Primer triángulo
                triangles[triIndex] = bottomLeft;
                triangles[triIndex + 1] = topLeft;
                triangles[triIndex + 2] = topRight;

                // Segundo triángulo
                triangles[triIndex + 3] = bottomLeft;
                triangles[triIndex + 4] = topRight;
                triangles[triIndex + 5] = bottomRight;

                triIndex += 6;
            }
        }

        // Asignar vértices y triángulos al mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals(); // Para calcular las normales y mejorar la iluminación

        // Agregar o actualizar el MeshCollider
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        if (meshCollider == null)
        {
            meshCollider = gameObject.AddComponent<MeshCollider>();
        }
        meshCollider.sharedMesh = mesh;
    }

    void OnDrawGizmos()
    {
        // Dibuja puntos para depuración (opcional)
        if (vertices == null) return;
        Gizmos.color = Color.red;
        foreach (var vertex in vertices)
        {
            Gizmos.DrawSphere(transform.position + vertex, 0.1f);
        }
    }
}
