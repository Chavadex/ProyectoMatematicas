using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GradientController : MonoBehaviour
{
    public Gradient slopeGradient; // El gradiente que define los colores según la pendiente

    private Mesh mesh;
    private Vector3[] vertices;
    private Vector3[] normals;

    void Start()
    {
        if (slopeGradient == null)
        {
            // Si no se asignó un gradiente, creamos uno por defecto
            CreateDefaultGradient();
        }

        ApplySlopeColors();
    }

    void ApplySlopeColors()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        normals = mesh.normals;

        Color[] colors = new Color[vertices.Length];

        // Aplicamos el color según la pendiente
        for (int i = 0; i < vertices.Length; i++)
        {
            // La pendiente se calcula usando el producto punto de la normal con el vector hacia arriba (Vector3.up)
            float slope = Mathf.Abs(Vector3.Dot(normals[i], Vector3.up));

            // Usamos el gradiente para obtener el color según la pendiente
            colors[i] = slopeGradient.Evaluate(slope);
        }

        // Asignamos los colores a la malla
        mesh.colors = colors;

        // Necesitamos que la malla sea refrescada para que el cambio sea visible
        mesh.RecalculateNormals();  // Recalcular normales para mejorar el sombreado
        mesh.RecalculateBounds();   // Asegura que los límites de la malla se recalculen
    }

    // Método para crear un gradiente por defecto si no se asigna uno en el Inspector
    void CreateDefaultGradient()
    {
        slopeGradient = new Gradient();
        slopeGradient.colorKeys = new GradientColorKey[]
        {
            new GradientColorKey(Color.green, 0f),     // Verde para pendiente baja
            new GradientColorKey(Color.yellow, 0.5f),  // Amarillo para pendiente media
            new GradientColorKey(Color.red, 1f)         // Rojo para pendiente alta
        };

        slopeGradient.alphaKeys = new GradientAlphaKey[]
        {
            new GradientAlphaKey(1f, 0f),  // Totalmente opaco al principio
            new GradientAlphaKey(1f, 1f)   // Totalmente opaco al final
        };
    }
}
