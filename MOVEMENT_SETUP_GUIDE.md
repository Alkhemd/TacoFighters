# 🎮 Guía: Movimiento con Teclas A y D

Esta guía te ayudará a configurar el movimiento del personaje Taco con las teclas A (izquierda) y D (derecha), con volteo automático del sprite.

---

## 📋 Configuración Rápida

### **Paso 1: Agregar el Script de Movimiento**

1. En la **Hierarchy**, selecciona el GameObject del personaje Taco
2. En el **Inspector**, haz clic en **Add Component**
3. Busca: `SimpleMovement`
4. Selecciona el script

---

### **Paso 2: Configurar el Script**

En el **Inspector**, en el componente `SimpleMovement`:

#### **Movement Settings:**

1. **Move Speed:** `5`
   - Velocidad de movimiento (unidades por segundo)
   - Valores recomendados:
     - `3-4` = Lento
     - `5-6` = Normal
     - `7-10` = Rápido

#### **Sprite Flip:**

2. **Auto Flip:** ✓ Marcado
   - Voltea automáticamente el sprite según la dirección

3. **Facing Right:** ✓ Marcado (si el sprite mira a la derecha por defecto)
   - Desmarca si tu sprite mira a la izquierda por defecto

#### **Animation Integration (Opcional):**

4. **Sprite Animator:** 
   - Arrastra el componente `SpriteAnimator` del mismo GameObject
   - O déjalo vacío (se detectará automáticamente)

5. **Walk Frames:**
   - Si tienes sprites de caminar, arrástralos aquí
   - Si no, déjalo vacío (usará solo idle)

6. **Idle Frames:**
   - Arrastra los 4 sprites de Taco_Idle aquí:
     - `Taco_Idle_0`, `Taco_Idle_1`, `Taco_Idle_2`, `Taco_Idle_3`

#### **Debug:**

7. **Show Debug Info:** 
   - Marca ✓ si quieres ver logs en la consola
   - Desmarca para producción

---

### **Paso 3: Probar el Movimiento**

1. Presiona **Play** ▶ en Unity
2. Presiona **A** → El personaje se mueve a la izquierda y voltea
3. Presiona **D** → El personaje se mueve a la derecha y voltea
4. Suelta las teclas → El personaje se detiene

---

## 🎯 Cómo Funciona (Explicación del Código)

### **1. Lectura de Input**

```csharp
// Lee las teclas A/D o Flechas Izquierda/Derecha
horizontalInput = Input.GetAxisRaw("Horizontal");

// Resultado:
// -1 = Presionando A (izquierda)
//  0 = No presionando nada
//  1 = Presionando D (derecha)
```

### **2. Movimiento**

```csharp
// Calcula cuánto mover:
float movement = horizontalInput * moveSpeed * Time.deltaTime;

// horizontalInput = dirección (-1 o 1)
// moveSpeed = velocidad configurada (ej: 5)
// Time.deltaTime = tiempo del frame (para movimiento suave)

// Aplica el movimiento:
transform.Translate(Vector3.right * movement);
```

### **3. Volteo del Sprite**

```csharp
// Si se mueve a la derecha y está mirando a la izquierda:
if (horizontalInput > 0 && !facingRight)
{
    Flip(); // Voltear
}

// Voltear invierte la escala en X:
Vector3 scale = transform.localScale;
scale.x *= -1; // Invertir
transform.localScale = scale;
```

### **4. Integración con Animaciones**

```csharp
// Si se está moviendo:
if (isMoving && walkFrames != null)
{
    spriteAnimator.SetAnimationFrames(walkFrames); // Animación de caminar
}
// Si está quieto:
else if (!isMoving && idleFrames != null)
{
    spriteAnimator.SetAnimationFrames(idleFrames); // Animación idle
}
```

---

## 🔧 Configuración Avanzada

### **Opción 1: Solo Movimiento (Sin Animación)**

Si solo quieres movimiento básico sin animaciones:

1. No asignes nada en **Walk Frames** ni **Idle Frames**
2. Puedes quitar el componente `SpriteAnimator` si no lo necesitas
3. El personaje se moverá con un sprite estático

---

### **Opción 2: Con Animación Idle y Walk**

Si tienes sprites de caminar:

1. **Idle Frames:** Asigna los 4 sprites de `Taco_Idle`
2. **Walk Frames:** Asigna los sprites de caminar (si los tienes)
3. El script cambiará automáticamente entre idle y walk

---

### **Opción 3: Usar CharacterController2D (Con Física)**

Si prefieres usar el sistema completo con física y salto:

1. **No uses** `SimpleMovement`
2. **Usa** `CharacterController2D` (ya existe en tu proyecto)
3. Ese script ya tiene movimiento, salto y volteo integrados

**Diferencias:**

| Característica | SimpleMovement | CharacterController2D |
|----------------|----------------|----------------------|
| Física (Rigidbody2D) | ❌ No | ✅ Sí |
| Salto | ❌ No | ✅ Sí |
| Detección de suelo | ❌ No | ✅ Sí |
| Gravedad | ❌ No | ✅ Sí |
| Complejidad | Simple | Completo |
| Mejor para | Pruebas rápidas | Juego final |

---

## 🎮 Controles

### Teclado:

- **A** o **Flecha Izquierda** → Mover a la izquierda
- **D** o **Flecha Derecha** → Mover a la derecha

### Métodos Públicos (para llamar desde otros scripts):

```csharp
// Cambiar velocidad
GetComponent<SimpleMovement>().SetMoveSpeed(10f);

// Activar/desactivar volteo automático
GetComponent<SimpleMovement>().SetAutoFlip(false);

// Voltear manualmente
GetComponent<SimpleMovement>().FlipManually();
```

---

## 🔧 Solución de Problemas

### ❌ El personaje no se mueve

**Solución:**
- Verifica que el script `SimpleMovement` esté activo (checkbox marcado)
- Verifica que **Move Speed** sea mayor a 0
- Presiona Play y prueba con A/D
- Revisa la **Console** para ver si hay errores

### ❌ El sprite no se voltea

**Solución:**
- Verifica que **Auto Flip** esté marcado ✓
- Verifica que el GameObject tenga un **Sprite Renderer**
- Ajusta **Facing Right** según la dirección inicial de tu sprite

### ❌ El personaje se mueve muy rápido/lento

**Solución:**
- Ajusta el valor de **Move Speed**:
  - Más bajo = más lento
  - Más alto = más rápido
- Valores recomendados: 3-7

### ❌ La animación no cambia

**Solución:**
- Verifica que **Sprite Animator** esté asignado
- Verifica que **Idle Frames** y **Walk Frames** tengan sprites
- Marca **Show Debug Info** para ver logs de cambio de animación

### ❌ El personaje se voltea al revés

**Solución:**
- Cambia el estado de **Facing Right**:
  - Si está marcado, desmárcalo
  - Si está desmarcado, márcalo

---

## 💡 Consejos

### Combinar con SpriteAnimator

Para mejor resultado:

1. **SimpleMovement** → Controla el movimiento y volteo
2. **SpriteAnimator** → Controla las animaciones
3. Asigna los frames en **SimpleMovement** para cambio automático

### Optimización

- Si no necesitas cambio de animación, deja **Walk Frames** e **Idle Frames** vacíos
- Desmarca **Show Debug Info** en producción para mejor rendimiento

### Transición a CharacterController2D

Cuando estés listo para el sistema completo:

1. Quita `SimpleMovement`
2. Agrega `CharacterController2D`
3. Configura Rigidbody2D, Collider y GroundCheck
4. Tendrás movimiento + salto + física

---

## ✅ Checklist Final

Antes de continuar, verifica:

- [ ] Script `SimpleMovement` agregado al GameObject
- [ ] **Move Speed** configurado (recomendado: 5)
- [ ] **Auto Flip** marcado ✓
- [ ] **Facing Right** configurado según tu sprite
- [ ] **Idle Frames** asignados (4 sprites de Taco_Idle)
- [ ] Probado en Play Mode
- [ ] Movimiento funciona con A/D
- [ ] Sprite se voltea correctamente

---

## 📈 Resultado Esperado

Al presionar **Play**:

1. El personaje Taco aparece en la escena
2. Presionas **D** → Se mueve a la derecha, sprite mira a la derecha
3. Presionas **A** → Se mueve a la izquierda, sprite se voltea a la izquierda
4. Sueltas las teclas → Se detiene
5. La animación idle se reproduce mientras está quieto
6. (Si configuraste walk) La animación cambia al moverse

---

¡Listo! Tu personaje Taco ahora se mueve con A y D y voltea automáticamente. 🌮🎮
