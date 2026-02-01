using UnityEngine;

namespace TacoFighter.Characters
{
    /// <summary>
    /// Gestiona las estadísticas del personaje (vida, daño, defensa).
    /// Este componente es la base para el sistema de combate.
    /// </summary>
    public class CharacterStats : MonoBehaviour
    {
        [Header("Health Settings")]
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float currentHealth;

        [Header("Combat Stats")]
        [SerializeField] private float baseDamage = 10f; // Daño base de ataques normales
        [SerializeField] private float defense = 5f; // Reduce el daño recibido

        [Header("State")]
        public bool IsAlive => currentHealth > 0;
        public bool IsInvulnerable { get; private set; } // Para frames de invulnerabilidad

        // Eventos para notificar cambios de estado
        public event System.Action<float, float> OnHealthChanged; // (currentHealth, maxHealth)
        public event System.Action OnDeath;
        public event System.Action<float> OnDamageTaken; // (damage)

        private void Awake()
        {
            // Inicializar vida al máximo
            currentHealth = maxHealth;
        }

        /// <summary>
        /// Aplica daño al personaje considerando la defensa.
        /// </summary>
        /// <param name="damage">Cantidad de daño bruto</param>
        public void TakeDamage(float damage)
        {
            if (!IsAlive || IsInvulnerable) return;

            // Calcular daño final aplicando defensa
            float finalDamage = Mathf.Max(damage - defense, 1f); // Mínimo 1 de daño
            currentHealth = Mathf.Max(currentHealth - finalDamage, 0);

            // Notificar eventos
            OnDamageTaken?.Invoke(finalDamage);
            OnHealthChanged?.Invoke(currentHealth, maxHealth);

            Debug.Log($"{gameObject.name} recibió {finalDamage} de daño. Vida restante: {currentHealth}/{maxHealth}");

            // Verificar muerte
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// Restaura vida al personaje.
        /// </summary>
        /// <param name="amount">Cantidad de vida a restaurar</param>
        public void Heal(float amount)
        {
            if (!IsAlive) return;

            currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
            OnHealthChanged?.Invoke(currentHealth, maxHealth);

            Debug.Log($"{gameObject.name} se curó {amount}. Vida actual: {currentHealth}/{maxHealth}");
        }

        /// <summary>
        /// Activa invulnerabilidad temporal (útil para i-frames después de recibir daño).
        /// </summary>
        /// <param name="duration">Duración en segundos</param>
        public void SetInvulnerable(float duration)
        {
            if (IsInvulnerable) return;
            StartCoroutine(InvulnerabilityCoroutine(duration));
        }

        private System.Collections.IEnumerator InvulnerabilityCoroutine(float duration)
        {
            IsInvulnerable = true;
            Debug.Log($"{gameObject.name} es invulnerable por {duration}s");
            yield return new WaitForSeconds(duration);
            IsInvulnerable = false;
        }

        private void Die()
        {
            Debug.Log($"{gameObject.name} ha muerto");
            OnDeath?.Invoke();
            // Aquí puedes agregar lógica adicional de muerte (desactivar controles, animación, etc.)
        }

        // Getters públicos para acceder a las stats
        public float GetMaxHealth() => maxHealth;
        public float GetCurrentHealth() => currentHealth;
        public float GetBaseDamage() => baseDamage;
        public float GetDefense() => defense;
        public float GetHealthPercentage() => currentHealth / maxHealth;

        // Métodos para modificar stats (útil para power-ups o buffs)
        public void ModifyDefense(float amount) => defense = Mathf.Max(0, defense + amount);
        public void ModifyBaseDamage(float amount) => baseDamage = Mathf.Max(1, baseDamage + amount);
    }
}
