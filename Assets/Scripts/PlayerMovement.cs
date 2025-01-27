using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float jumpForce = 5f;
    private Rigidbody rb;
    [SerializeField] private bool isGrounded;
    [SerializeField] private GameObject victoryPanel;
    public float groundCheckDistance = 0.2f; // Distancia del Raycast

    void Start()
    {
        victoryPanel.SetActive(false);
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Verificación de si está tocando el suelo con un Raycast
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                isGrounded = true;
            }
        }
        else
        {
            isGrounded = false;
        }

        // Movimiento base
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical).normalized;
        float slopeMultiplier = CalculateSlopeMultiplier();
        rb.AddForce(movement * moveSpeed * slopeMultiplier);

        // Salto
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private float CalculateSlopeMultiplier()
    {
        if (!isGrounded) return 1f;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f))
        {
            Vector3 normal = hit.normal;
            float angle = Vector3.Angle(normal, Vector3.up);

            if (angle < 10f) return 1f;
            if (angle > 45f) return 0.3f;
            return Mathf.Lerp(1f, 0.3f, (angle - 10f) / 35f);
        }

        return 1f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("WinZone"))
        {
            victory();
        }
    }

    private void victory()
    {
        Debug.Log("Winning");
        victoryPanel.SetActive(true);
    }
}
