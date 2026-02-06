using UnityEngine;
using System.Collections;

namespace TacoFighter.Characters
{
    /// <summary>
    /// Crea un efecto de estela (Ghosting/Afterimage) detrás del personaje.
    /// Genera copias estáticas del sprite actual que se desvanecen con el tiempo.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class GhostTrail : MonoBehaviour
    {
        [Header("Configuración del Efecto")]
        [Tooltip("Tiempo entre cada 'fantasma' (menor = más densidad de estela)")]
        [SerializeField] private float ghostDelay = 0.05f; 

        [Tooltip("Tiempo que tarda cada fantasma en desaparecer")]
        [SerializeField] private float activeTime = 0.5f;

        [Tooltip("Color inicial del fantasma (ajusta el Alpha para transparencia)")]
        [SerializeField] private Color ghostColor = new Color(1f, 1f, 1f, 0.5f);

        [Header("Estado")]
        [Tooltip("Activar para generar estela")]
        public bool makeGhost = false;

        private SpriteRenderer sr;
        private float ghostTimer;

        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (makeGhost)
            {
                if (ghostTimer > 0)
                {
                    ghostTimer -= Time.deltaTime;
                }
                else
                {
                    // Es momento de crear un fantasma
                    SpawnGhost();
                    ghostTimer = ghostDelay;
                }
            }
        }

        private void SpawnGhost()
        {
            // 1. Crear un GameObject vacío temporal
            GameObject currentGhost = new GameObject("Ghost_Sprite");
            
            // 2. Copiar posición, rotación y escala exactas del personaje
            currentGhost.transform.position = transform.position;
            currentGhost.transform.rotation = transform.rotation;
            currentGhost.transform.localScale = transform.localScale;

            // 3. Agregar y configurar el SpriteRenderer
            SpriteRenderer currentGhostSr = currentGhost.AddComponent<SpriteRenderer>();
            currentGhostSr.sprite = sr.sprite; // Copiar el sprite exacto (frame actual de animación)
            currentGhostSr.color = ghostColor;
            
            // Ponerlo visualmente detrás del personaje (-1 en orden)
            currentGhostSr.sortingLayerName = sr.sortingLayerName;
            currentGhostSr.sortingOrder = sr.sortingOrder - 1;

            // 4. Iniciar la destrucción gradual
            StartCoroutine(FadeAndDestroy(currentGhost, currentGhostSr));
        }

        private IEnumerator FadeAndDestroy(GameObject ghostObj, SpriteRenderer ghostSr)
        {
            float elapsed = 0f;
            Color startColor = ghostSr.color;

            while (elapsed < activeTime)
            {
                // Si el objeto ya fue destruido externamente, salir
                if (ghostObj == null) yield break;

                elapsed += Time.deltaTime;
                
                // Calcular nuevo Alpha (transparencia) de forma lineal
                float newAlpha = Mathf.Lerp(startColor.a, 0f, elapsed / activeTime);
                ghostSr.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);

                yield return null;
            }

            // Destruir el objeto al terminar
            Destroy(ghostObj);
        }
    }
}
