using UnityEngine;

namespace TacoFighter.Characters
{
    /// <summary>
    /// Gestiona el sistema de ataques del personaje.
    /// Incluye ataques básicos, combos y ataques especiales.
    /// </summary>
    [RequireComponent(typeof(CharacterStats))]
    public class CharacterAttack : MonoBehaviour
    {
        [Header("Attack Settings")]
        [SerializeField] private float attackRange = 1.5f; // Rango del ataque
        [SerializeField] private float attackCooldown = 0.5f; // Tiempo entre ataques
        [SerializeField] private LayerMask enemyLayer; // Layer de los enemigos
        [SerializeField] private Transform attackPoint; // Punto desde donde se origina el ataque

        [Header("Combo System")]
        [SerializeField] private int maxComboCount = 3; // Máximo de ataques en combo
        [SerializeField] private float comboResetTime = 1.5f; // Tiempo para resetear combo
        [SerializeField] private float[] comboMultipliers = { 1f, 1.2f, 1.5f }; // Multiplicadores de daño por combo

        [Header("Special Attack")]
        [SerializeField] private float specialAttackDamageMultiplier = 2f;
        [SerializeField] private float specialAttackCooldown = 3f;
        [SerializeField] private float specialAttackRange = 2.5f;

        // Estado del sistema de ataque
        private CharacterStats stats;
        private CharacterController2D controller;
        private float lastAttackTime;
        private float lastSpecialAttackTime;
        private int currentComboIndex = 0;
        private float lastComboTime;

        public bool IsAttacking { get; private set; }
        public bool CanAttack => Time.time >= lastAttackTime + attackCooldown && !IsAttacking;
        public bool CanSpecialAttack => Time.time >= lastSpecialAttackTime + specialAttackCooldown;
        public int CurrentCombo => currentComboIndex;

        // Eventos
        public event System.Action<int> OnAttackPerformed; // (comboIndex)
        public event System.Action OnSpecialAttackPerformed;
        public event System.Action OnComboReset;

        private void Awake()
        {
            stats = GetComponent<CharacterStats>();
            controller = GetComponent<CharacterController2D>();
        }

        private void Update()
        {
            // Resetear combo si pasó mucho tiempo
            if (currentComboIndex > 0 && Time.time >= lastComboTime + comboResetTime)
            {
                ResetCombo();
                
            }

            // Input de prueba para PC (esto será llamado por UI móvil)
            if (Input.GetKeyDown(KeyCode.J) && CanAttack)
            {
                PerformAttack();
            }

            if (Input.GetKeyDown(KeyCode.K) && CanSpecialAttack)
            {
                PerformSpecialAttack();
            }
        }

        /// <summary>
        /// Ejecuta un ataque básico. Parte del sistema de combos.
        /// </summary>
        public void PerformAttack()
        {
            if (!CanAttack || !stats.IsAlive) return;

            IsAttacking = true;
            lastAttackTime = Time.time;
            lastComboTime = Time.time;

            // Calcular daño con multiplicador de combo
            float comboMultiplier = currentComboIndex < comboMultipliers.Length 
                ? comboMultipliers[currentComboIndex] 
                : comboMultipliers[comboMultipliers.Length - 1];

            float damage = stats.GetBaseDamage() * comboMultiplier;

            // Detectar enemigos en rango
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
                attackPoint.position, 
                attackRange, 
                enemyLayer
            );

            // Aplicar daño a todos los enemigos en rango
            foreach (Collider2D enemy in hitEnemies)
            {
                CharacterStats enemyStats = enemy.GetComponent<CharacterStats>();
                if (enemyStats != null)
                {
                    enemyStats.TakeDamage(damage);
                    Debug.Log($"Ataque combo {currentComboIndex + 1} - Daño: {damage}");
                }
            }

            // Avanzar combo
            currentComboIndex++;
            if (currentComboIndex >= maxComboCount)
            {
                currentComboIndex = 0; // Reiniciar después del último golpe
            }

            OnAttackPerformed?.Invoke(currentComboIndex);

            // Simular duración de animación de ataque
            Invoke(nameof(EndAttack), 0.3f);
        }

        /// <summary>
        /// Ejecuta un ataque especial con mayor daño y rango.
        /// </summary>
        public void PerformSpecialAttack()
        {
            if (!CanSpecialAttack || !stats.IsAlive) return;

            IsAttacking = true;
            lastSpecialAttackTime = Time.time;
            ResetCombo(); // El ataque especial resetea el combo

            float damage = stats.GetBaseDamage() * specialAttackDamageMultiplier;

            // Detectar enemigos en rango extendido
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
                attackPoint.position, 
                specialAttackRange, 
                enemyLayer
            );

            foreach (Collider2D enemy in hitEnemies)
            {
                CharacterStats enemyStats = enemy.GetComponent<CharacterStats>();
                if (enemyStats != null)
                {
                    enemyStats.TakeDamage(damage);
                    Debug.Log($"¡Ataque Especial! - Daño: {damage}");
                }
            }

            OnSpecialAttackPerformed?.Invoke();

            // Simular duración de animación de ataque especial
            Invoke(nameof(EndAttack), 0.5f);
        }

        /// <summary>
        /// Método público para que la UI móvil llame al ataque básico.
        /// </summary>
        public void RequestAttack()
        {
            if (CanAttack)
            {
                PerformAttack();
            }
        }

        /// <summary>
        /// Método público para que la UI móvil llame al ataque especial.
        /// </summary>
        public void RequestSpecialAttack()
        {
            if (CanSpecialAttack)
            {
                PerformSpecialAttack();
            }
        }

        private void EndAttack()
        {
            IsAttacking = false;
        }

        private void ResetCombo()
        {
            if (currentComboIndex > 0)
            {
                currentComboIndex = 0;
                OnComboReset?.Invoke();
                Debug.Log("Combo reseteado");
            }
        }

        // Visualización en el editor
        private void OnDrawGizmosSelected()
        {
            if (attackPoint == null) return;

            // Rango de ataque normal
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);

            // Rango de ataque especial
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(attackPoint.position, specialAttackRange);
        }
    }
}
