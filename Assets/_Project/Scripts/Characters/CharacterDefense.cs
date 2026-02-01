using UnityEngine;

namespace TacoFighter.Characters
{
    /// <summary>
    /// Gestiona el sistema de defensa del personaje.
    /// Incluye bloqueo, esquiva (dodge) y contraataque.
    /// </summary>
    [RequireComponent(typeof(CharacterStats))]
    [RequireComponent(typeof(CharacterController2D))]
    public class CharacterDefense : MonoBehaviour
    {
        [Header("Block Settings")]
        [SerializeField] private float blockDamageReduction = 0.7f; // Reduce 70% del daño
        [SerializeField] private float blockMovementPenalty = 0.3f; // Velocidad reducida al 30% mientras bloquea

        [Header("Dodge Settings")]
        [SerializeField] private float dodgeDuration = 0.3f; // Duración de la esquiva
        [SerializeField] private float dodgeCooldown = 2f; // Tiempo entre esquivas
        [SerializeField] private float dodgeDistance = 3f; // Distancia de la esquiva
        [SerializeField] private float dodgeSpeed = 15f; // Velocidad del dash de esquiva

        [Header("Counter Attack Settings")]
        [SerializeField] private float counterWindowDuration = 0.5f; // Ventana de tiempo para contraatacar
        [SerializeField] private float counterDamageMultiplier = 1.8f; // Multiplicador de daño del contraataque
        [SerializeField] private float counterRange = 1.5f;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private Transform attackPoint;

        // Estado del sistema de defensa
        private CharacterStats stats;
        private CharacterController2D controller;
        private Rigidbody2D rb;
        private float lastDodgeTime;
        private float counterWindowStartTime;
        private bool isCounterWindowActive;

        public bool IsBlocking { get; private set; }
        public bool IsDodging { get; private set; }
        public bool CanDodge => Time.time >= lastDodgeTime + dodgeCooldown && !IsDodging;
        public bool IsInCounterWindow => isCounterWindowActive && 
                                          Time.time <= counterWindowStartTime + counterWindowDuration;

        // Eventos
        public event System.Action OnBlockStart;
        public event System.Action OnBlockEnd;
        public event System.Action OnDodgePerformed;
        public event System.Action OnCounterAttackPerformed;

        private void Awake()
        {
            stats = GetComponent<CharacterStats>();
            controller = GetComponent<CharacterController2D>();
            rb = GetComponent<Rigidbody2D>();

            // Suscribirse al evento de daño para aplicar reducción por bloqueo
            stats.OnDamageTaken += OnDamageReceived;
        }

        private void OnDestroy()
        {
            if (stats != null)
            {
                stats.OnDamageTaken -= OnDamageReceived;
            }
        }

        private void Update()
        {
            // Input de prueba para PC (esto será llamado por UI móvil)
            if (Input.GetKeyDown(KeyCode.L) && CanDodge)
            {
                PerformDodge();
            }

            // Mantener bloqueo mientras se presiona la tecla
            if (Input.GetKey(KeyCode.LeftShift))
            {
                StartBlock();
            }
            else if (IsBlocking)
            {
                EndBlock();
            }

            // Verificar si la ventana de contraataque expiró
            if (isCounterWindowActive && !IsInCounterWindow)
            {
                isCounterWindowActive = false;
            }
        }

        /// <summary>
        /// Inicia el bloqueo. Reduce el daño recibido pero limita el movimiento.
        /// </summary>
        public void StartBlock()
        {
            if (IsDodging || !stats.IsAlive) return;

            if (!IsBlocking)
            {
                IsBlocking = true;
                OnBlockStart?.Invoke();
                Debug.Log("Bloqueando - Daño reducido");
            }
        }

        /// <summary>
        /// Termina el bloqueo.
        /// </summary>
        public void EndBlock()
        {
            if (IsBlocking)
            {
                IsBlocking = false;
                OnBlockEnd?.Invoke();
                Debug.Log("Bloqueo terminado");
            }
        }

        /// <summary>
        /// Ejecuta una esquiva rápida (dodge/dash).
        /// Durante la esquiva, el personaje es invulnerable.
        /// </summary>
        public void PerformDodge()
        {
            if (!CanDodge || !stats.IsAlive) return;

            StartCoroutine(DodgeCoroutine());
        }

        private System.Collections.IEnumerator DodgeCoroutine()
        {
            IsDodging = true;
            lastDodgeTime = Time.time;

            // Activar invulnerabilidad durante la esquiva
            stats.SetInvulnerable(dodgeDuration);

            // Determinar dirección de la esquiva (hacia atrás)
            float dodgeDirection = transform.localScale.x > 0 ? -1f : 1f;

            // Aplicar impulso de esquiva
            rb.linearVelocity = new Vector2(dodgeDirection * dodgeSpeed, rb.linearVelocity.y);

            OnDodgePerformed?.Invoke();
            Debug.Log("¡Esquiva!");

            yield return new WaitForSeconds(dodgeDuration);

            // Detener el movimiento de esquiva
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            IsDodging = false;
        }

        /// <summary>
        /// Activa la ventana de contraataque después de un bloqueo exitoso.
        /// </summary>
        public void ActivateCounterWindow()
        {
            if (!IsBlocking) return;

            isCounterWindowActive = true;
            counterWindowStartTime = Time.time;
            Debug.Log("¡Ventana de contraataque activada!");
        }

        /// <summary>
        /// Ejecuta un contraataque si está dentro de la ventana de tiempo.
        /// </summary>
        public void PerformCounterAttack()
        {
            if (!IsInCounterWindow || !stats.IsAlive) return;

            isCounterWindowActive = false;
            EndBlock(); // Terminar bloqueo al contraatacar

            float damage = stats.GetBaseDamage() * counterDamageMultiplier;

            // Detectar enemigos en rango
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
                attackPoint.position,
                counterRange,
                enemyLayer
            );

            foreach (Collider2D enemy in hitEnemies)
            {
                CharacterStats enemyStats = enemy.GetComponent<CharacterStats>();
                if (enemyStats != null)
                {
                    enemyStats.TakeDamage(damage);
                    Debug.Log($"¡Contraataque! - Daño: {damage}");
                }
            }

            OnCounterAttackPerformed?.Invoke();
        }

        /// <summary>
        /// Método público para que la UI móvil active/desactive el bloqueo.
        /// </summary>
        public void SetBlocking(bool blocking)
        {
            if (blocking)
                StartBlock();
            else
                EndBlock();
        }

        /// <summary>
        /// Método público para que la UI móvil llame a la esquiva.
        /// </summary>
        public void RequestDodge()
        {
            if (CanDodge)
            {
                PerformDodge();
            }
        }

        /// <summary>
        /// Método público para que la UI móvil llame al contraataque.
        /// </summary>
        public void RequestCounterAttack()
        {
            if (IsInCounterWindow)
            {
                PerformCounterAttack();
            }
        }

        /// <summary>
        /// Callback cuando el personaje recibe daño.
        /// Aplica reducción si está bloqueando y activa ventana de contraataque.
        /// </summary>
        private void OnDamageReceived(float damage)
        {
            if (IsBlocking)
            {
                // La reducción ya se aplicó en CharacterStats, pero podemos activar contraataque
                ActivateCounterWindow();
            }
        }

        /// <summary>
        /// Obtiene el multiplicador de velocidad actual (afectado por bloqueo).
        /// </summary>
        public float GetMovementMultiplier()
        {
            if (IsBlocking) return blockMovementPenalty;
            if (IsDodging) return 0f; // Sin movimiento manual durante esquiva
            return 1f;
        }

        /// <summary>
        /// Obtiene el multiplicador de reducción de daño actual.
        /// </summary>
        public float GetDamageReductionMultiplier()
        {
            if (IsBlocking) return blockDamageReduction;
            return 1f; // Sin reducción
        }

        // Visualización en el editor
        private void OnDrawGizmosSelected()
        {
            if (attackPoint == null) return;

            // Rango de contraataque
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(attackPoint.position, counterRange);
        }
    }
}
