using UnityEngine;

namespace TacoFighter.Characters
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterController2D : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 7f; // Un poco más rápido para lucha
        [SerializeField] private float jumpForce = 14f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Transform groundCheck; // Punto en los pies
        [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.1f); // Área de detección

        [Header("State")]
        public bool isGrounded; // Público para que las animaciones lo lean
        private bool facingRight = true;

        private Rigidbody2D rb;
        private float horizontalInput; // Cambiado para recibir de UI o Teclado
        private bool jumpRequest;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            // --- INPUT ---
            // Esto permite probar en PC, pero la variable horizontalInput 
            // podrá ser seteada por tus botones móviles.
            horizontalInput = Input.GetAxisRaw("Horizontal");

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                jumpRequest = true;
            }

            // Flip character
            if (horizontalInput > 0 && !facingRight) Flip();
            else if (horizontalInput < 0 && facingRight) Flip();
        }

        // Función pública para que el botón de la UI móvil la llame
        public void SetHorizontalInput(float value) => horizontalInput = value;
        
        public void RequestJump() 
        {
            if(isGrounded) jumpRequest = true;
        }

        private void FixedUpdate()
        {
            CheckGround();
            Move();
            
            if (jumpRequest)
            {
                Jump();
                jumpRequest = false;
            }
        }

        private void Move()
        {
            // Usamos linearVelocity (Unity 2023+)
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        }

        private void Jump()
        {
            // Resetear velocidad Y para saltos consistentes
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        private void CheckGround()
        {
            // Usamos OverlapBox en el punto groundCheck para mayor precisión
            isGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);
        }
    
        private void Flip()
        {
            facingRight = !facingRight;
            Vector3 scaler = transform.localScale;
            scaler.x *= -1;
            transform.localScale = scaler;
        }

        // Dibujar el área de detección en el editor
        private void OnDrawGizmos()
        {
            if (groundCheck == null) return;
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        }
    }
}