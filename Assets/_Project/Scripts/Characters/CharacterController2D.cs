using UnityEngine;

namespace TacoFighter.Characters
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterController2D : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float jumpForce = 12f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Transform groundCheck;

        [Header("State")]
        [SerializeField] private bool isGrounded;
        [SerializeField] private bool facingRight = true;

        private Rigidbody2D rb;
        private Vector2 inputVector;
        private bool jumpRequest;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            // Input processing
            float xInput = Input.GetAxisRaw("Horizontal");
            inputVector = new Vector2(xInput, 0);

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                jumpRequest = true;
            }

            // Flip character
            if (xInput > 0 && !facingRight) Flip();
            else if (xInput < 0 && facingRight) Flip();
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
            rb.linearVelocity = new Vector2(inputVector.x * moveSpeed, rb.linearVelocity.y);
        }

        private void Jump()
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        private void CheckGround()
        {
            // Raycast downward from character position
            Vector2 position = transform.position;
            RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down, 1f, groundLayer);
            isGrounded = hit.collider != null;
        }

        private void Flip()
        {
            facingRight = !facingRight;
            Vector3 scaler = transform.localScale;
            scaler.x *= -1;
            transform.localScale = scaler;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Vector2 position = transform.position;
            Gizmos.DrawLine(position, position + Vector2.down * 1f);
        }
    }
}
