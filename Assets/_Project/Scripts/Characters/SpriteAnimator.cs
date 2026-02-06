using UnityEngine;

namespace TacoFighter.Characters
{
    /// <summary>
    /// Anima sprites mediante código usando un array de sprites.
    /// Útil para animaciones simples sin necesidad de Animator Controller.
    /// 
    /// FUNCIONAMIENTO:
    /// - Cambia el sprite del SpriteRenderer cada cierto tiempo (frameRate)
    /// - Cicla entre todos los sprites del array
    /// - Puede reproducirse en loop o una sola vez
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimator : MonoBehaviour
    {
        [Header("Animation Settings")]
        [Tooltip("Array de sprites que forman la animación (en orden)")]
        [SerializeField] private Sprite[] animationFrames;

        [Tooltip("Fotogramas por segundo (velocidad de la animación)")]
        [SerializeField] private float frameRate = 20f; // 10 FPS por defecto !! VERFICIAR ESTO, ESTABA EN 10

        [Tooltip("Reproducir en loop continuo")]
        [SerializeField] private bool loop = true;

        [Tooltip("Reproducir automáticamente al iniciar")]
        [SerializeField] private bool playOnStart = true;

        [Header("Debug")]
        [SerializeField] private bool showDebugInfo = false;

        // Referencias
        private SpriteRenderer spriteRenderer;
        
        // Estado de la animación
        private int currentFrame = 0; // Fotograma actual
        private float timer = 0f; // Temporizador interno
        private bool isPlaying = false; // ¿Está reproduciéndose?

        // Propiedades públicas
        public bool IsPlaying => isPlaying;
        public int CurrentFrame => currentFrame;
        public int TotalFrames => animationFrames != null ? animationFrames.Length : 0;

        private void Awake()
        {
            // Obtener referencia al SpriteRenderer
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            // Validar que hay sprites asignados
            if (animationFrames == null || animationFrames.Length == 0)
            {
                Debug.LogWarning($"[SpriteAnimator] No hay sprites asignados en {gameObject.name}. " +
                                "Arrastra los sprites al array 'Animation Frames' en el Inspector.", this);
                return;
            }

            // Reproducir automáticamente si está configurado
            if (playOnStart)
            {
                Play();
            }
        }

        private void Update()
        {
            // Solo actualizar si está reproduciéndose
            if (!isPlaying) return;

            // Incrementar temporizador
            timer += Time.deltaTime;

            // Calcular el tiempo por fotograma (1 / frameRate)
            float timePerFrame = 1f / frameRate;

            // ¿Es momento de cambiar de fotograma?
            if (timer >= timePerFrame)
            {
                // Resetear temporizador
                timer -= timePerFrame;

                // Avanzar al siguiente fotograma
                NextFrame();
            }
        }

        /// <summary>
        /// Avanza al siguiente fotograma de la animación.
        /// </summary>
        private void NextFrame()
        {
            // Incrementar índice del fotograma
            currentFrame++;

            // Verificar si llegamos al final
            if (currentFrame >= animationFrames.Length)
            {
                if (loop)
                {
                    // Volver al inicio si está en loop
                    currentFrame = 0;
                    if (showDebugInfo) Debug.Log($"[SpriteAnimator] Loop completado en {gameObject.name}");
                }
                else
                {
                    // Detener animación si no está en loop
                    currentFrame = animationFrames.Length - 1; // Quedarse en el último frame
                    Stop();
                    if (showDebugInfo) Debug.Log($"[SpriteAnimator] Animación completada en {gameObject.name}");
                    return;
                }
            }

            // Actualizar el sprite mostrado
            UpdateSprite();
        }

        /// <summary>
        /// Actualiza el sprite del SpriteRenderer con el fotograma actual.
        /// </summary>
        private void UpdateSprite()
        {
            if (spriteRenderer != null && animationFrames != null && currentFrame < animationFrames.Length)
            {
                spriteRenderer.sprite = animationFrames[currentFrame];
                
                if (showDebugInfo)
                {
                    Debug.Log($"[SpriteAnimator] Frame {currentFrame}/{animationFrames.Length - 1}: " +
                             $"{animationFrames[currentFrame].name}");
                }
            }
        }

        /// <summary>
        /// Inicia la reproducción de la animación.
        /// </summary>
        public void Play()
        {
            if (animationFrames == null || animationFrames.Length == 0)
            {
                Debug.LogWarning($"[SpriteAnimator] No se puede reproducir: no hay sprites asignados.", this);
                return;
            }

            isPlaying = true;
            timer = 0f;
            UpdateSprite(); // Mostrar el fotograma actual inmediatamente

            if (showDebugInfo) Debug.Log($"[SpriteAnimator] Reproduciendo animación en {gameObject.name}");
        }

        /// <summary>
        /// Detiene la reproducción de la animación.
        /// </summary>
        public void Stop()
        {
            isPlaying = false;
            if (showDebugInfo) Debug.Log($"[SpriteAnimator] Animación detenida en {gameObject.name}");
        }

        /// <summary>
        /// Pausa la animación (puede reanudarse con Play).
        /// </summary>
        public void Pause()
        {
            isPlaying = false;
            if (showDebugInfo) Debug.Log($"[SpriteAnimator] Animación pausada en {gameObject.name}");
        }

        /// <summary>
        /// Reinicia la animación desde el primer fotograma.
        /// </summary>
        public void Restart()
        {
            currentFrame = 0;
            timer = 0f;
            UpdateSprite();
            Play();

            if (showDebugInfo) Debug.Log($"[SpriteAnimator] Animación reiniciada en {gameObject.name}");
        }

        /// <summary>
        /// Establece la velocidad de la animación (fotogramas por segundo).
        /// </summary>
        /// <param name="fps">Fotogramas por segundo (mayor = más rápido)</param>
        public void SetFrameRate(float fps)
        {
            frameRate = Mathf.Max(1f, fps); // Mínimo 1 FPS
            if (showDebugInfo) Debug.Log($"[SpriteAnimator] Frame rate cambiado a {frameRate} FPS");
        }

        /// <summary>
        /// Cambia el array de sprites de la animación.
        /// Útil para cambiar entre diferentes animaciones (idle, walk, attack, etc.)
        /// </summary>
        /// <param name="newFrames">Nuevo array de sprites</param>
        /// <param name="autoPlay">Reproducir automáticamente después de cambiar</param>
        public void SetAnimationFrames(Sprite[] newFrames, bool autoPlay = true)
        {
            animationFrames = newFrames;
            currentFrame = 0;
            timer = 0f;

            if (autoPlay && newFrames != null && newFrames.Length > 0)
            {
                Play();
            }

            if (showDebugInfo) Debug.Log($"[SpriteAnimator] Sprites cambiados: {newFrames?.Length ?? 0} frames");
        }

        /// <summary>
        /// Activa o desactiva el modo loop.
        /// </summary>
        public void SetLoop(bool shouldLoop)
        {
            loop = shouldLoop;
            if (showDebugInfo) Debug.Log($"[SpriteAnimator] Loop: {(loop ? "ON" : "OFF")}");
        }

        // Método de ayuda para debugging en el Inspector
        [ContextMenu("Play Animation")]
        private void DebugPlay() => Play();

        [ContextMenu("Stop Animation")]
        private void DebugStop() => Stop();

        [ContextMenu("Restart Animation")]
        private void DebugRestart() => Restart();
    }
}
