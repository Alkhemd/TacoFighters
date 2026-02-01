using UnityEngine;

namespace TacoFighter.Characters
{
    /// <summary>
    /// Integra todos los sistemas del personaje (movimiento, combate, defensa).
    /// Este es el componente principal que coordina todos los demás.
    /// Útil para referencias centralizadas y lógica que afecta múltiples sistemas.
    /// </summary>
    [RequireComponent(typeof(CharacterController2D))]
    [RequireComponent(typeof(CharacterStats))]
    [RequireComponent(typeof(CharacterAttack))]
    [RequireComponent(typeof(CharacterDefense))]
    public class CharacterCombat : MonoBehaviour
    {
        // Referencias a componentes
        private CharacterController2D controller;
        private CharacterStats stats;
        private CharacterAttack attack;
        private CharacterDefense defense;

        [Header("Combat State")]
        [SerializeField] private bool canMove = true;
        [SerializeField] private bool canAttack = true;

        public bool CanMove => canMove && stats.IsAlive && !attack.IsAttacking && !defense.IsDodging;
        public bool CanPerformActions => stats.IsAlive;

        private void Awake()
        {
            // Obtener referencias a todos los componentes
            controller = GetComponent<CharacterController2D>();
            stats = GetComponent<CharacterStats>();
            attack = GetComponent<CharacterAttack>();
            defense = GetComponent<CharacterDefense>();

            // Suscribirse a eventos importantes
            stats.OnDeath += OnCharacterDeath;
            attack.OnAttackPerformed += OnAttackPerformed;
            defense.OnDodgePerformed += OnDodgePerformed;
        }

        private void OnDestroy()
        {
            // Desuscribirse de eventos para evitar memory leaks
            if (stats != null) stats.OnDeath -= OnCharacterDeath;
            if (attack != null) attack.OnAttackPerformed -= OnAttackPerformed;
            if (defense != null) defense.OnDodgePerformed -= OnDodgePerformed;
        }

        private void Update()
        {
            // Aplicar modificador de velocidad por bloqueo
            if (defense.IsBlocking)
            {
                // Aquí podrías modificar la velocidad del controller si es necesario
                // Por ejemplo: controller.SetSpeedMultiplier(defense.GetMovementMultiplier());
            }
        }

        /// <summary>
        /// Callback cuando el personaje muere.
        /// Desactiva todos los controles y sistemas.
        /// </summary>
        private void OnCharacterDeath()
        {
            canMove = false;
            canAttack = false;
            Debug.Log($"{gameObject.name} - Sistemas de combate desactivados por muerte");
            
            // Aquí puedes agregar lógica adicional:
            // - Desactivar controles
            // - Reproducir animación de muerte
            // - Mostrar pantalla de Game Over
            // - etc.
        }

        /// <summary>
        /// Callback cuando se realiza un ataque.
        /// </summary>
        private void OnAttackPerformed(int comboIndex)
        {
            // Aquí puedes agregar efectos adicionales:
            // - Partículas de ataque
            // - Sonidos
            // - Screen shake
            // - etc.
        }

        /// <summary>
        /// Callback cuando se realiza una esquiva.
        /// </summary>
        private void OnDodgePerformed()
        {
            // Aquí puedes agregar efectos adicionales:
            // - Partículas de esquiva
            // - Sonidos
            // - Efecto de blur
            // - etc.
        }

        // Métodos públicos para acceso fácil a los componentes
        public CharacterController2D GetController() => controller;
        public CharacterStats GetStats() => stats;
        public CharacterAttack GetAttack() => attack;
        public CharacterDefense GetDefense() => defense;

        /// <summary>
        /// Reinicia el personaje a su estado inicial (útil para reiniciar partida).
        /// </summary>
        public void ResetCharacter()
        {
            canMove = true;
            canAttack = true;
            // Aquí podrías resetear vida, posición, etc.
        }
    }
}
