using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f; // Velocidad base
    public float jumpForce = 5f; // Fuerza de salto
    private Rigidbody rb;
    private bool isGrounded; // Para verificar si est� tocando el suelo

    void Start()
    {
        // Obtener el componente Rigidbody
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Capturar input horizontal y vertical
        float moveHorizontal = Input.GetAxis("Horizontal"); // Flechas o A/D
        float moveVertical = Input.GetAxis("Vertical");     // Flechas o W/S

        // Movimiento base
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical).normalized;

        // Calcular pendiente y ajustar velocidad
        float slopeMultiplier = CalculateSlopeMultiplier();
        rb.AddForce(movement * moveSpeed * slopeMultiplier);

        // Salto
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    // Funci�n para calcular el multiplicador de velocidad en funci�n de la pendiente
    private float CalculateSlopeMultiplier()
    {
        if (!isGrounded) return 1f; // No afecta si no est� en el suelo

        // Obtener la normal del suelo
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f))
        {
            Vector3 normal = hit.normal;
            float angle = Vector3.Angle(normal, Vector3.up); // �ngulo con respecto al eje Y

            // Si el �ngulo es peque�o, mantener velocidad normal; si es grande, reducir
            if (angle < 10f) return 1f;                     // �ngulo peque�o, sin reducci�n
            if (angle > 45f) return 0.3f;                   // �ngulo muy inclinado, gran reducci�n
            return Mathf.Lerp(1f, 0.3f, (angle - 10f) / 35f); // Interpolaci�n entre 10� y 45�
        }

        return 1f; // Por defecto
    }

    // Detectar si est� tocando el suelo
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
