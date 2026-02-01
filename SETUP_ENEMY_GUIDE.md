# 🎮 Guía: Agregar Enemigo a la Escena

Esta guía te ayudará a agregar el enemigo `Enemy_Knife` a la misma escena que el personaje `Taco`.

---

## 📋 Pasos de Implementación

### **Paso 1: Crear Layer "Enemy"**

1. En Unity, ve a: **Edit → Project Settings → Tags and Layers**
2. En la sección **Layers**, encuentra un slot vacío (ejemplo: Layer 8)
3. Escribe: `Enemy`
4. Cierra la ventana

✅ **Verificación:** Ahora puedes seleccionar "Enemy" en el dropdown de Layer de cualquier GameObject.

---

### **Paso 2: Configurar Physics 2D**

1. Ve a: **Edit → Project Settings → Physics 2D**
2. Desplázate hasta **Layer Collision Matrix**
3. Asegúrate de que estas colisiones estén **activadas (✓)**:
   - `Enemy` ↔ `Ground`
   - `Enemy` ↔ `Player`
   - `Player` ↔ `Ground`

✅ **Verificación:** Las casillas correspondientes deben estar marcadas.

---

### **Paso 3: Crear GameObject del Enemigo en la Escena**

1. Abre la escena: `Assets/Scenes/SampleScene.unity`
2. En la **Hierarchy**, haz clic derecho → **Create Empty**
3. Renombra el GameObject a: `Enemy_Knife`
4. En el **Inspector**, configura:
   - **Position:** X=5, Y=1, Z=0
   - **Rotation:** X=0, Y=0, Z=0
   - **Scale:** X=1, Y=1, Z=1

---

### **Paso 4: Agregar el Script de Configuración**

1. Con `Enemy_Knife` seleccionado en la Hierarchy
2. En el **Inspector**, haz clic en **Add Component**
3. Busca: `EnemySetup`
4. Selecciona el script `EnemySetup.cs`

---

### **Paso 5: Configurar el Script**

En el **Inspector**, en el componente `EnemySetup`:

1. **Enemy Sprite:**
   - Haz clic en el círculo pequeño a la derecha
   - Busca y selecciona: `Enemy_Knife`
   - O arrastra el sprite desde: `Assets/_Project/Sprites/Enemies/Enemy_Knife.png`

2. **Ground Layer:**
   - Haz clic en el dropdown
   - Selecciona: `Ground` (o el layer que uses para el suelo)

3. **Auto Setup On Start:**
   - Dejar marcado ✓ (configuración automática al iniciar)

4. **Show Debug Logs:**
   - Dejar marcado ✓ (para ver mensajes en la consola)

---

### **Paso 6: Ejecutar Configuración Automática**

**Opción A: Automática (Recomendada)**
1. Presiona **Play** en Unity
2. El script configurará automáticamente todos los componentes
3. Revisa la **Console** para ver los mensajes de confirmación
4. Presiona **Stop**

**Opción B: Manual**
1. Con `Enemy_Knife` seleccionado
2. En el componente `EnemySetup` del Inspector
3. Haz clic derecho en el nombre del script
4. Selecciona: **Setup Enemy Components**

---

### **Paso 7: Verificar Componentes Creados**

En el **Inspector** de `Enemy_Knife`, deberías ver:

✅ **Transform** - Posición (5, 1, 0)
✅ **Sprite Renderer** - Con sprite Enemy_Knife asignado
✅ **Rigidbody 2D** - Dynamic, Gravity Scale: 3, Freeze Rotation: Z
✅ **Capsule Collider 2D** - Size: (0.8, 1.5)
✅ **Enemy Setup** - Script de configuración
✅ **Layer** - Enemy

En la **Hierarchy**, bajo `Enemy_Knife`:
✅ **GroundCheck** (GameObject hijo)

---

### **Paso 8: Ajustar Posición del Enemigo**

1. En la **Scene View**, selecciona `Enemy_Knife`
2. Usa la herramienta de movimiento (W) para ajustar la posición
3. Asegúrate de que esté **sobre el suelo** de tu escena
4. Posición sugerida: **(5, 1, 0)** - A la derecha del jugador

---

### **Paso 9: Probar en Play Mode**

1. Presiona **Play** ▶
2. Observa que:
   - ✅ El enemigo aparece en la escena
   - ✅ El enemigo cae por gravedad hasta tocar el suelo
   - ✅ El enemigo colisiona con el suelo (no atraviesa)
   - ✅ El jugador (Taco) puede moverse con A/D
   - ✅ El enemigo permanece estático (por ahora)

3. **Gizmos en Scene View:**
   - Amarillo: Punto GroundCheck
   - Verde (seleccionado): Área de detección de suelo

---

## 🎨 Configuración Visual Opcional

### Ajustar Tamaño del Sprite

Si el enemigo se ve muy grande o pequeño:

1. Selecciona `Enemy_Knife`
2. En **Transform**, ajusta **Scale**:
   - Ejemplo: (0.8, 0.8, 1) para hacerlo más pequeño
   - Ejemplo: (1.2, 1.2, 1) para hacerlo más grande

### Ajustar Collider

Si el collider no coincide con el sprite:

1. Selecciona `Enemy_Knife`
2. En **Capsule Collider 2D**, ajusta:
   - **Size:** Ancho y alto del collider
   - **Offset:** Posición del collider relativa al sprite

### Voltear el Sprite

Si quieres que el enemigo mire hacia la izquierda:

1. Selecciona `Enemy_Knife`
2. En **Transform**, cambia **Scale X** a `-1`

---

## 🔧 Solución de Problemas

### ❌ El enemigo atraviesa el suelo

**Solución:**
- Verifica que el suelo tenga un **Collider2D**
- Verifica que el layer "Enemy" colisione con "Ground" en Physics 2D Settings
- Asegúrate de que el **Rigidbody2D** esté en modo **Dynamic**

### ❌ No veo el sprite del enemigo

**Solución:**
- Verifica que asignaste el sprite en el campo **Enemy Sprite**
- Verifica que el **Sprite Renderer** tenga el sprite asignado
- Verifica que la cámara esté enfocando la posición del enemigo

### ❌ El enemigo rota al caer

**Solución:**
- En **Rigidbody2D**, verifica que **Constraints → Freeze Rotation Z** esté marcado

### ❌ El enemigo se pega a las paredes

**Solución:**
- Verifica que el **Collider2D** tenga un **Physics Material 2D** con **Friction: 0**

---

## 📊 Resultado Final

Al completar todos los pasos, tu escena debería verse así:

```
Vista de la Escena (Scene View):

    [Taco_Idle]              [Enemy_Knife]
        🌮                        🔪
        |                          |
    ====================================  ← Suelo (Ground)
        ↑                          ↑
     (0, 1)                     (5, 1)
```

---

## 🚀 Próximos Pasos

Una vez que el enemigo esté en la escena:

1. ✅ **Configuración básica completada**
2. ⏳ **Esperando instrucciones de movimiento**
3. 🔜 Implementar script de movimiento del enemigo
4. 🔜 Agregar comportamiento de IA
5. 🔜 Conectar con sistema de combate

---

## 💡 Notas Importantes

> **📌 El enemigo está estático por ahora**
> 
> El enemigo solo tiene física básica (gravedad y colisiones). No tiene movimiento ni IA hasta que proporciones las instrucciones específicas.

> **🎮 Puedes probar el combate**
> 
> Si agregas el componente `CharacterStats` al enemigo, podrás atacarlo con el jugador presionando **J** (ataque básico).

> **🔍 Usa los Gizmos**
> 
> En la Scene View, verás gizmos amarillos/verdes que muestran el GroundCheck. Esto te ayuda a visualizar dónde el enemigo detecta el suelo.

---

## ✅ Checklist Final

Antes de continuar, verifica:

- [ ] Layer "Enemy" creado
- [ ] Physics 2D configurado (colisiones Enemy-Ground, Enemy-Player)
- [ ] GameObject `Enemy_Knife` creado en la escena
- [ ] Script `EnemySetup` agregado y configurado
- [ ] Sprite `Enemy_Knife.png` asignado
- [ ] Componentes creados automáticamente (Rigidbody2D, Collider, etc.)
- [ ] GroundCheck creado como hijo
- [ ] Enemigo posicionado sobre el suelo
- [ ] Probado en Play Mode (enemigo cae y colisiona correctamente)

---

¡Listo! El enemigo está configurado y esperando tus instrucciones de movimiento. 🎉
