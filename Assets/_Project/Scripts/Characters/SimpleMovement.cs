using UnityEngine;

namespace TacoFighter.Characters
{
    /// <summary>
    /// Controlador de movimiento simple para personajes 2D.
    /// Permite movimiento horizontal con las teclas A/D y voltea el sprite según la dirección.
    /// 
    /// FUNCIONAMIENTO:
    /// - Lee input del teclado (A = izquierda, D = derecha)
    /// - Mueve el personaje usando Transform (sin física compleja)
    /// - Voltea el sprite automáticamente según la dirección de movimiento
    /// - Se integra con SpriteAnimator para animaciones
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class SimpleMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [Tooltip("Velocidad de movimiento horizontal (unidades por segundo)")]
        [SerializeField] private float moveSpeed = 5f;

        [Header("Sprite Flip")]
        [Tooltip("Voltear sprite automáticamente según dirección")]
        [SerializeField] private bool autoFlip = true; // verifica esto

        [Tooltip("¿El sprite mira a la derecha por defecto?")]
        [SerializeField] private bool facingRight = true;

        [Header("Animation Integration")]
        [Tooltip("Referencia al SpriteAnimator (opcional)")]
        [SerializeField] private SpriteAnimator spriteAnimator;

        [Tooltip("Sprites para animación de caminar (opcional)")]
        [SerializeField] private Sprite[] walkFrames;

        [Tooltip("Sprites para animación idle (opcional)")]
        [SerializeField] private Sprite[] idleFrames;

        [Header("Debug")]
        [SerializeField] private bool showDebugInfo = false;

        // Referencias
        private SpriteRenderer spriteRenderer;

        // Estado
        private float horizontalInput = 0f; // Input horizontal (-1 a 1)
        private bool isMoving = false; // ¿Se está moviendo?

        // Propiedades públicas
        public bool IsMoving => isMoving;
        public bool FacingRight => facingRight;
        public float MoveSpeed => moveSpeed;

        private void Awake()
        {
            // Obtener referencia al SpriteRenderer
            spriteRenderer = GetComponent<SpriteRenderer>();

            // Obtener SpriteAnimator si no está asignado
            if (spriteAnimator == null)
            {
                spriteAnimator = GetComponent<SpriteAnimator>();
            }
        }

        private void Update()
        {
            // ===== 1. LEER INPUT DEL TECLADO =====
            // Input.GetAxisRaw devuelve:
            // -1 si presionas A o Flecha Izquierda
            //  0 si no presionas nada
            //  1 si presionas D o Flecha Derecha
            horizontalInput = Input.GetAxisRaw("Horizontal");

            // ===== 2. DETERMINAR SI SE ESTÁ MOVIENDO =====
            isMoving = Mathf.Abs(horizontalInput) > 0.01f;

            // ===== 3. MOVER EL PERSONAJE =====
            if (isMoving)
            {
                MoveCharacter();
            }

            // ===== 4. VOLTEAR SPRITE SEGÚN DIRECCIÓN =====
            if (autoFlip && isMoving)
            {
                FlipSprite();
            }

            // ===== 5. CAMBIAR ANIMACIÓN (si está configurado) =====
            UpdateAnimation();
        }

        /// <summary>
        /// Mueve el personaje horizontalmente.
        /// Usa Transform.Translate para movimiento simple sin física.
        /// </summary>
        private void MoveCharacter()
        {
            // Calcular movimiento:
            // horizontalInput = dirección (-1 o 1)
            // moveSpeed = velocidad en unidades/segundo
            // Time.deltaTime = tiempo desde el último frame (para movimiento suave)
            float movement = horizontalInput * moveSpeed * Time.deltaTime;

            // Aplicar movimiento al Transform
            // Vector3.right = (1, 0, 0) = dirección horizontal
            transform.Translate(Vector3.right * movement);

            if (showDebugInfo)
            {
                Debug.Log($"[SimpleMovement] Moviendo: {horizontalInput} | Posición: {transform.position.x}");
            }
        }

        /// <summary>
        /// Voltea el sprite según la dirección de movimiento.
        /// Usa localScale.x para voltear (-1 = izquierda, 1 = derecha).
        /// </summary>
        private void FlipSprite()
        {
            // Si se mueve a la derecha y está mirando a la izquierda, voltear
            if (horizontalInput > 0 && !facingRight)
            {
                Flip();
            }
            // Si se mueve a la izquierda y está mirando a la derecha, voltear
            else if (horizontalInput < 0 && facingRight)
            {
                Flip();
            }
        }

        /// <summary>
        /// Voltea el sprite invirtiendo la escala en X.
        /// </summary>
        private void Flip()
        {
            // Cambiar estado de dirección
            facingRight = !facingRight;

            // Obtener escala actual
            Vector3 scale = transform.localScale;

            // Invertir escala en X (voltear horizontalmente)
            scale.x *= -1;

            // Aplicar nueva escala
            transform.localScale = scale;

            if (showDebugInfo)
            {
                Debug.Log($"[SimpleMovement] Sprite volteado. Mirando a: {(facingRight ? "Derecha" : "Izquierda")}");
            }
        }

        /// <summary>
        /// Actualiza la animación según el estado de movimiento.
        /// Cambia entre animación idle y walk automáticamente.
        /// </summary>
        private void UpdateAnimation()
        {
            // Solo si hay SpriteAnimator configurado
            if (spriteAnimator == null) return;

            // Si se está moviendo y hay animación de caminar
            if (isMoving && walkFrames != null && walkFrames.Length > 0)
            {
                // Cambiar a animación de caminar
                // (solo si no está ya reproduciéndola)
                if (spriteAnimator.TotalFrames != walkFrames.Length)
                {
                    spriteAnimator.SetAnimationFrames(walkFrames, true);
                    if (showDebugInfo) Debug.Log("[SimpleMovement] Animación: WALK");
                }
            }
            // Si está quieto y hay animación idle
            else if (!isMoving && idleFrames != null && idleFrames.Length > 0)
            {
                // Cambiar a animación idle
                // (solo si no está ya reproduciéndola)
                if (spriteAnimator.TotalFrames != idleFrames.Length)
                {
                    spriteAnimator.SetAnimationFrames(idleFrames, true);
                    if (showDebugInfo) Debug.Log("[SimpleMovement] Animación: IDLE");
                }
            }
        }

        /// <summary>
        /// Establece la velocidad de movimiento.
        /// </summary>
        /// <param name="speed">Nueva velocidad</param>
        public void SetMoveSpeed(float speed)
        {
            moveSpeed = Mathf.Max(0, speed);
            if (showDebugInfo) Debug.Log($"[SimpleMovement] Velocidad cambiada a: {moveSpeed}");
        }

        /// <summary>
        /// Activa o desactiva el volteo automático del sprite.
        /// </summary>
        public void SetAutoFlip(bool enabled)
        {
            autoFlip = enabled;
        }

        /// <summary>
        /// Voltea manualmente el sprite (útil para llamar desde otros scripts).
        /// </summary>
        public void FlipManually()
        {
            Flip();
        }

        // ===== MÉTODOS DE DEBUG EN EL INSPECTOR =====
        [ContextMenu("Flip Sprite")]
        private void DebugFlip() => Flip();

        [ContextMenu("Test Move Right")]
        private void DebugMoveRight()
        {
            horizontalInput = 1f;
            MoveCharacter();
        }

        [ContextMenu("Test Move Left")]
        private void DebugMoveLeft()
        {
            horizontalInput = -1f;
            MoveCharacter();
        }
    }
}
