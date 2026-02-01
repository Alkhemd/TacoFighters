# Unity Basics Reference - TacoFighters

## 1. Componentes Esenciales

| Componente | Uso | Propiedades Clave |
|------------|-----|-------------------|
| **Transform** | Posición/Rotación/Escala | Position, Rotation, Scale |
| **BoxCollider2D** | Colisiones 2D | Edit Collider para ajustar |
| **Rigidbody2D** | Física 2D | Gravity Scale, Freeze Rotation Z |
| **Animator** | Animaciones | Controller, Parameters |
| **SpriteRenderer** | Mostrar sprites | Order in Layer, Flip |

## 2. Configuración de Sprites (Pixel Art)

```
Filter Mode: Point (no filter)  ← CRÍTICO para pixel art
Pixels Per Unit: 32 (personajes) / 16 (tiles)
Compression: None
```

### Para Sprite Atlas (múltiples sprites en una imagen):
1. Sprite Mode → **Multiple**
2. Sprite Editor → Slice → Grid By Cell Size
3. Pixel Size: 32x32 o 16x16

## 3. Sistema de Animaciones

### Ventanas Importantes:
- `Window > Animation > Animation` (Ctrl+6)
- `Window > Animation > Animator`

### Flujo:
1. Crear Animation Clip
2. Arrastrar sprites a la línea de tiempo
3. Samples = 24 FPS
4. Crear parámetros en Animator (Bool, Trigger)
5. Configurar transiciones con condiciones

## 4. Código C# Esencial

```csharp
// Variables
private Animator animator;
private Rigidbody2D rb2d;
public float fuerzaSalto = 250f;

void Start()
{
    animator = GetComponent<Animator>();
    rb2d = GetComponent<Rigidbody2D>();
}

void Update()
{
    if (Input.GetKeyDown(KeyCode.Space))
    {
        animator.SetBool("EstaSaltando", true);
        rb2d.AddForce(new Vector2(0, fuerzaSalto));
    }
}

void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.tag == "Suelo")
    {
        animator.SetBool("EstaSaltando", false);
    }
}
```

## 5. Tags Importantes

- `Suelo` - Detectar cuando el personaje toca el piso
- `Enemigo` - Detectar colisiones con enemigos
- `Player` - Identificar al jugador

## 6. Atajos de Teclado

| Atajo | Acción |
|-------|--------|
| Ctrl+6 | Abrir Animation |
| Shift+F | Centrar vista en objeto |
| Ctrl+D | Duplicar objeto |

## 7. Configuración Móvil

- Resolución: Configurar en Game view
- Input: Cambiar de teclado a touch (Input System)
- Performance: Compression y Atlas para optimizar
