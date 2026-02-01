# 🎬 Guía: Animar el Personaje Taco

Esta guía te ayudará a configurar la animación idle del personaje Taco usando los 4 fotogramas del sprite.

---

## 📋 Método 1: Animación Simple por Código (Recomendado para empezar)

Este método usa el script `SpriteAnimator.cs` que acabamos de crear.

### **Paso 1: Preparar el GameObject del Taco**

1. En la **Hierarchy**, selecciona el GameObject del personaje Taco
   - Si no existe, créalo: GameObject → 2D Object → Sprite → Renombrar a "Player_Taco"

2. En el **Inspector**, verifica que tenga:
   - ✅ Transform
   - ✅ Sprite Renderer
   - ✅ Rigidbody2D (si quieres física)
   - ✅ Collider2D (si quieres colisiones)

---

### **Paso 2: Agregar el Script de Animación**

1. Con el GameObject "Player_Taco" seleccionado
2. En el **Inspector**, haz clic en **Add Component**
3. Busca: `SpriteAnimator`
4. Selecciona el script

---

### **Paso 3: Configurar los Fotogramas**

En el **Inspector**, en el componente `SpriteAnimator`:

1. **Animation Frames:**
   - Haz clic en el número a la derecha (debería decir "0")
   - Cambia a: **4** (porque tenemos 4 fotogramas)
   - Presiona Enter

2. Ahora verás 4 slots: **Element 0, Element 1, Element 2, Element 3**

3. **Asignar sprites:**
   
   **Opción A: Arrastrar desde Project**
   - En la ventana **Project**, navega a: `Assets/_Project/Sprites/Characters/`
   - Expande el sprite `Taco_Idle.png` (haz clic en la flecha)
   - Verás 4 sub-sprites: `Taco_Idle_0`, `Taco_Idle_1`, `Taco_Idle_2`, `Taco_Idle_3`
   - Arrastra cada uno a su slot correspondiente:
     - `Taco_Idle_0` → Element 0
     - `Taco_Idle_1` → Element 1
     - `Taco_Idle_2` → Element 2
     - `Taco_Idle_3` → Element 3

   **Opción B: Usar el selector**
   - Haz clic en el círculo pequeño a la derecha de cada Element
   - Busca y selecciona el sprite correspondiente

4. **Frame Rate:**
   - Ajusta la velocidad de la animación
   - Valores recomendados:
     - `6-8 FPS` = Animación lenta y suave (idle relajado)
     - `10-12 FPS` = Animación normal
     - `15-20 FPS` = Animación rápida

5. **Loop:**
   - Dejar marcado ✓ (para que la animación se repita)

6. **Play On Start:**
   - Dejar marcado ✓ (para que empiece automáticamente)

7. **Show Debug Info:**
   - Marcar ✓ si quieres ver logs en la consola (útil para debugging)

---

### **Paso 4: Probar la Animación**

1. Presiona **Play** ▶ en Unity
2. Deberías ver al Taco animándose con los 4 fotogramas
3. La animación se repetirá en loop

**Ajustar velocidad:**
- Si va muy rápido: Reduce el **Frame Rate** (ejemplo: 6)
- Si va muy lento: Aumenta el **Frame Rate** (ejemplo: 12)

---

## 🎮 Controles del Script

### Métodos Públicos (para llamar desde otros scripts):

```csharp
// Reproducir animación
GetComponent<SpriteAnimator>().Play();

// Detener animación
GetComponent<SpriteAnimator>().Stop();

// Pausar animación
GetComponent<SpriteAnimator>().Pause();

// Reiniciar desde el inicio
GetComponent<SpriteAnimator>().Restart();

// Cambiar velocidad (FPS)
GetComponent<SpriteAnimator>().SetFrameRate(15f);

// Activar/desactivar loop
GetComponent<SpriteAnimator>().SetLoop(true);
```

### Métodos de Debug en el Inspector:

1. Selecciona el GameObject con `SpriteAnimator`
2. Haz clic derecho en el nombre del script
3. Verás opciones:
   - **Play Animation** - Reproducir
   - **Stop Animation** - Detener
   - **Restart Animation** - Reiniciar

---

## 📊 Método 2: Animator Controller (Avanzado)

Si quieres usar el sistema de animación completo de Unity:

### **Paso 1: Crear Animator Controller**

1. En **Project**, navega a: `Assets/_Project/Animations/`
2. Clic derecho → Create → Animator Controller
3. Nombrar: `TacoAnimatorController`

### **Paso 2: Crear Animation Clip**

1. En **Project**, en la misma carpeta
2. Clic derecho → Create → Animation
3. Nombrar: `Taco_Idle_Animation`

### **Paso 3: Configurar Animation Clip**

1. Selecciona `Taco_Idle_Animation`
2. En el **Inspector**, haz clic en **Add Property**
3. Selecciona: Sprite Renderer → Sprite
4. Haz clic en el botón de grabar (círculo rojo)
5. En la línea de tiempo:
   - Frame 0:00 → Asignar `Taco_Idle_0`
   - Frame 0:10 → Asignar `Taco_Idle_1`
   - Frame 0:20 → Asignar `Taco_Idle_2`
   - Frame 0:30 → Asignar `Taco_Idle_3`
6. Detener grabación (círculo rojo otra vez)

### **Paso 4: Asignar al GameObject**

1. Selecciona el GameObject "Player_Taco"
2. Arrastra `TacoAnimatorController` al componente **Animator**
3. Presiona Play

---

## 🔧 Solución de Problemas

### ❌ La animación no se reproduce

**Solución:**
- Verifica que los 4 sprites estén asignados en **Animation Frames**
- Verifica que **Play On Start** esté marcado
- Verifica que **Frame Rate** sea mayor a 0

### ❌ La animación va muy rápido/lento

**Solución:**
- Ajusta el valor de **Frame Rate**:
  - Más bajo = más lento
  - Más alto = más rápido
- Valores recomendados: 6-12 FPS

### ❌ No veo los sub-sprites en el Project

**Solución:**
1. Selecciona `Taco_Idle.png` en el Project
2. En el Inspector, verifica:
   - **Sprite Mode:** Multiple
   - **Pixels Per Unit:** 100
3. Haz clic en **Sprite Editor**
4. Verifica que haya 4 sprites cortados
5. Si no, usa **Slice → Automatic**

### ❌ El sprite no cambia

**Solución:**
- Verifica que el GameObject tenga un **Sprite Renderer**
- Verifica que el script `SpriteAnimator` esté activo (checkbox marcado)
- Revisa la **Console** para ver si hay errores

---

## 💡 Consejos

### Optimización para Móviles

- Usa **Frame Rate** bajo (6-8 FPS) para ahorrar rendimiento
- El método por código (`SpriteAnimator`) es más ligero que Animator Controller

### Múltiples Animaciones

Si quieres agregar más animaciones (caminar, atacar, etc.):

```csharp
// Crear arrays de sprites para cada animación
public Sprite[] idleFrames;
public Sprite[] walkFrames;
public Sprite[] attackFrames;

// Cambiar entre animaciones
spriteAnimator.SetAnimationFrames(walkFrames, true);
```

### Sincronizar con Movimiento

```csharp
// En tu script de movimiento:
void Update()
{
    if (horizontalInput != 0)
    {
        // Cambiar a animación de caminar
        spriteAnimator.SetAnimationFrames(walkFrames);
    }
    else
    {
        // Cambiar a animación idle
        spriteAnimator.SetAnimationFrames(idleFrames);
    }
}
```

---

## ✅ Checklist Final

Antes de continuar, verifica:

- [ ] Script `SpriteAnimator` agregado al GameObject
- [ ] 4 sprites asignados en **Animation Frames** (Taco_Idle_0 a 3)
- [ ] **Frame Rate** configurado (recomendado: 8)
- [ ] **Loop** marcado ✓
- [ ] **Play On Start** marcado ✓
- [ ] Probado en Play Mode
- [ ] Animación se reproduce correctamente

---

## 📈 Resultado Esperado

Al presionar **Play**:
1. El personaje Taco aparece en la escena
2. La animación idle se reproduce automáticamente
3. Los 4 fotogramas se alternan suavemente
4. La animación se repite en loop infinito

---

¡Listo! Tu personaje Taco ahora tiene animación idle. 🌮✨
