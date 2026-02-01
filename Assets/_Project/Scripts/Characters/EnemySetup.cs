using UnityEngine;

namespace TacoFighter.Characters
{
    /// <summary>
    /// Script de configuración inicial para enemigos.
    /// Este script configura automáticamente los componentes necesarios
    /// para que un enemigo funcione correctamente en la escena.
    /// 
    /// INSTRUCCIONES DE USO:
    /// 1. Crear un GameObject vacío en la escena
    /// 2. Asignar el sprite del enemigo (Enemy_Knife.png) al SpriteRenderer
    /// 3. Agregar este script al GameObject
    /// 4. El script configurará automáticamente todos los componentes necesarios
    /// </summary>
    public class EnemySetup : MonoBehaviour
    {
        [Header("Sprite Configuration")]
        [Tooltip("Arrastra aquí el sprite del enemigo (Enemy_Knife.png)")]
        [SerializeField] private Sprite enemySprite;

        [Header("Physics Configuration")]
        [Tooltip("Layer del suelo para detección de colisiones")]
        [SerializeField] private LayerMask groundLayer;

        [Header("Auto-Setup")]
        [Tooltip("Ejecutar configuración automática al iniciar")]
        [SerializeField] private bool autoSetupOnStart = true;

        [Header("Debug")]
        [SerializeField] private bool showDebugLogs = true;

        // Referencias a componentes
        private SpriteRenderer spriteRenderer;
        private Rigidbody2D rb;
        private Collider2D col;
        private Transform groundCheck;

        private void Start()
        {
            if (autoSetupOnStart)
            {
                SetupEnemy();
            }
        }

        /// <summary>
        /// Configura automáticamente todos los componentes del enemigo.
        /// Puede ser llamado manualmente desde el Inspector con el botón derecho.
        /// </summary>
        [ContextMenu("Setup Enemy Components")]
        public void SetupEnemy()
        {
            Log("=== Iniciando configuración del enemigo ===");

            // 1. Configurar SpriteRenderer
            SetupSpriteRenderer();

            // 2. Configurar Rigidbody2D
            SetupRigidbody();

            // 3. Configurar Collider2D
            SetupCollider();

            // 4. Configurar GroundCheck
            SetupGroundCheck();

            // 5. Configurar Layer
            SetupLayer();

            Log("=== Configuración del enemigo completada ===");
        }

        /// <summary>
        /// Configura el SpriteRenderer del enemigo.
        /// Si no existe, lo crea. Si existe, lo actualiza con el sprite asignado.
        /// </summary>
        private void SetupSpriteRenderer()
        {
            // Obtener o agregar SpriteRenderer
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
                Log("✓ SpriteRenderer agregado");
            }

            // Asignar sprite si está configurado
            if (enemySprite != null)
            {
                spriteRenderer.sprite = enemySprite;
                Log($"✓ Sprite asignado: {enemySprite.name}");
            }
            else
            {
                LogWarning("⚠ No se asignó sprite. Arrastra Enemy_Knife.png al campo 'Enemy Sprite'");
            }

            // Configurar sorting
            spriteRenderer.sortingLayerName = "Default";
            spriteRenderer.sortingOrder = 0;
        }

        /// <summary>
        /// Configura el Rigidbody2D para física 2D.
        /// Configuración optimizada para juegos de lucha.
        /// </summary>
        private void SetupRigidbody()
        {
            // Obtener o agregar Rigidbody2D
            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody2D>();
                Log("✓ Rigidbody2D agregado");
            }

            // Configurar propiedades del Rigidbody2D
            rb.bodyType = RigidbodyType2D.Dynamic; // Afectado por física
            rb.gravityScale = 3f; // Gravedad para juegos de lucha (más pesado)
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; // Detección precisa
            rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Evitar rotación

            // Configurar interpolación para movimiento suave
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;

            Log("✓ Rigidbody2D configurado (Gravity: 3, Freeze Rotation: ON)");
        }

        /// <summary>
        /// Configura el Collider2D para detección de colisiones.
        /// Usa CapsuleCollider2D por defecto (mejor para personajes).
        /// </summary>
        private void SetupCollider()
        {
            // Verificar si ya existe un collider
            col = GetComponent<Collider2D>();
            
            if (col == null)
            {
                // Crear CapsuleCollider2D (mejor para personajes)
                CapsuleCollider2D capsule = gameObject.AddComponent<CapsuleCollider2D>();
                
                // Configurar tamaño aproximado (ajustar según sprite)
                capsule.size = new Vector2(0.8f, 1.5f);
                capsule.offset = new Vector2(0, 0.75f); // Centrar en el sprite
                capsule.direction = CapsuleDirection2D.Vertical;

                col = capsule;
                Log("✓ CapsuleCollider2D agregado (Size: 0.8x1.5)");
            }
            else
            {
                Log($"✓ Collider existente detectado: {col.GetType().Name}");
            }

            // Crear Physics Material 2D para evitar fricción en paredes
            PhysicsMaterial2D material = new PhysicsMaterial2D("EnemyPhysicsMaterial");
            material.friction = 0f; // Sin fricción
            material.bounciness = 0f; // Sin rebote
            col.sharedMaterial = material;

            Log("✓ Physics Material configurado (Friction: 0)");
        }

        /// <summary>
        /// Crea un GameObject hijo "GroundCheck" para detección de suelo.
        /// Este punto se usa para verificar si el enemigo está en el suelo.
        /// </summary>
        private void SetupGroundCheck()
        {
            // Buscar si ya existe GroundCheck
            groundCheck = transform.Find("GroundCheck");

            if (groundCheck == null)
            {
                // Crear nuevo GameObject hijo
                GameObject groundCheckObj = new GameObject("GroundCheck");
                groundCheckObj.transform.SetParent(transform);
                
                // Posicionar en los pies del enemigo
                // Ajustar según el tamaño del sprite (aproximadamente -0.75 en Y)
                groundCheckObj.transform.localPosition = new Vector3(0, -0.75f, 0);
                groundCheckObj.transform.localRotation = Quaternion.identity;
                groundCheckObj.transform.localScale = Vector3.one;

                groundCheck = groundCheckObj.transform;
                Log("✓ GroundCheck creado en posición (0, -0.75, 0)");
            }
            else
            {
                Log("✓ GroundCheck existente detectado");
            }
        }

        /// <summary>
        /// Configura el Layer del GameObject como "Enemy".
        /// IMPORTANTE: Debes crear el layer "Enemy" manualmente en Unity.
        /// </summary>
        private void SetupLayer()
        {
            // Intentar asignar layer "Enemy"
            int enemyLayer = LayerMask.NameToLayer("Enemy");

            if (enemyLayer != -1)
            {
                gameObject.layer = enemyLayer;
                Log("✓ Layer 'Enemy' asignado");
            }
            else
            {
                LogWarning("⚠ Layer 'Enemy' no existe. Créalo en: Edit → Project Settings → Tags and Layers");
                LogWarning("  Mientras tanto, usando layer 'Default'");
            }
        }

        /// <summary>
        /// Método de ayuda para mostrar información en consola.
        /// </summary>
        private void Log(string message)
        {
            if (showDebugLogs)
            {
                Debug.Log($"[EnemySetup] {message}", this);
            }
        }

        /// <summary>
        /// Método de ayuda para mostrar advertencias en consola.
        /// </summary>
        private void LogWarning(string message)
        {
            if (showDebugLogs)
            {
                Debug.LogWarning($"[EnemySetup] {message}", this);
            }
        }

        /// <summary>
        /// Dibuja gizmos en el editor para visualizar el GroundCheck.
        /// </summary>
        private void OnDrawGizmos()
        {
            if (groundCheck != null)
            {
                // Dibujar esfera en la posición del GroundCheck
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(groundCheck.position, 0.1f);
            }
        }

        /// <summary>
        /// Dibuja gizmos seleccionados para mejor visualización.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            if (groundCheck != null)
            {
                // Dibujar área de detección de suelo
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(groundCheck.position, new Vector3(0.5f, 0.1f, 0));
            }
        }
    }
}
