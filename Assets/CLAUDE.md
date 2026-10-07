# CLAUDE.md — Medio término Programación de Videojuegos 2

## Contexto
Trabajo práctico de medio término de la materia **Programación de Videojuegos 2**.
Plazo total: **7 días**. Estoy cursando y trabajando en paralelo, así que el plan
debe ser sostenible (~2-3 hs por día, un día liviano).

- Motor: **Unity 3D** (versión: 6000.3.15f1), plantilla Universal 3D (URP)
- Lenguaje: **C#**
- Editor: **Visual Studio Code** + Claude Code
- Proyecto: _nombre a definir_ (tentativo: **Monolith Breach**)

## Consignas de la entrega (obligatorias)
1. Vista **top view** (ojo de pájaro).
2. **Pantalla de inicio** con botón **Play**.
3. La escena debe ofrecer un **desafío** que obligue al jugador a resolver una tarea
   (no solo batallar).
4. Prototipo de **un nivel** con una batalla entre el player y **al menos 3 enemigos**.
5. Los avatares deben ser **monolíticos, no humanoides**.
6. Se pueden usar assets del Unity Asset Store, pero el foco es un **gameplay divertido**.
7. Debe haber un **lugar de inicio** donde aparece el player.
8. El juego termina (o cambia de nivel) al **llegar al final del nivel**.
9. Si el jugador muere, **reaparece** en el inicio o en un punto de spawn.
10. **Punto extra:** uso de **Object Pooling**.

## Entregables
- **PDF** con: controles del jugador, descripción del desafío, enlace a **Drive**
  con los archivos del juego y enlace a **itch.io** con el build **WebGL** jugable.
- Build WebGL publicado en itch.io (debe poder jugarse desde el navegador).

## Concepto: tanque contra monolitos
Un tanque debe abrirse paso por una base enemiga y escapar. Todo está hecho con formas
simples o assets de tanques/vehículos del Asset Store (nada humanoide).

- **Player:** tanque con **controles de tanque clásico** (W avanza / S retrocede hacia donde mira el
  tanque, A/D lo rotan sobre su eje vertical), cañón fijo hacia adelante y disparo con click.
- **Enemigos (mínimo 3 tipos):**
  - *Centinela:* torreta fija que dispara a intervalos.
  - *Embestidor:* persigue al jugador y hace daño por contacto.
  - *Patrullero:* recorre una ruta y ataca si el jugador se acerca.
- **Desafío:** 3 generadores repartidos por el nivel, cada uno custodiado por un enemigo.
  Hay que destruirlos o activarlos para abrir la compuerta de salida.
- **Final del nivel:** cruzar la compuerta abierta -> pantalla de victoria (o siguiente nivel).
- **Muerte:** respawn del tanque en el punto de inicio.
- **Object Pooling:** proyectiles del jugador y de los enemigos (y efectos si hay tiempo).
- **Opcional si hay tiempo:** proyectiles que rebotan una vez en las paredes, o escudos en
  los generadores que haya que romper desde cierto ángulo.

## Arquitectura / convenciones
- Reutilizar la jerarquía de clases trabajada en la materia (`Personaje` como
  clase base; `Enemy` y el Player como derivadas). Usar `virtual`/`override`
  solo donde el comportamiento realmente difiera (por ejemplo, el comportamiento
  de cada tipo de enemigo).
- Los callbacks de Unity (`Start`, `Update`, `OnCollisionEnter`, etc.) los llama el
  motor; no se invocan a mano.
- Un script por responsabilidad; nombres consistentes en todo el proyecto.
- Carpetas sugeridas dentro de `Assets/`: `Scripts/`, `Prefabs/`, `Scenes/`, `Materials/`, `UI/`.
- Git con `.gitignore` de Unity; commits chicos y frecuentes.

## Git: ramas y flujo de trabajo
Repo: https://github.com/marcosvizio/Interceptor_Zero (binarios por Git LFS según `.gitattributes`).

| Rama | Para qué |
|---|---|
| `dev` | Rama de trabajo diaria. Todos los cambios se hacen y se commitean acá. |
| `prod` | Versión estable y jugable. Es la que se buildea para itch.io y se sube a Drive. |
| `main` | Rama por defecto de GitHub. Se mantiene igual que `prod` (se actualiza junto con ella). |

- Nunca se trabaja directo en `prod` ni en `main`: solo reciben merges desde `dev` de cosas probadas.
- Día a día (en `dev`): commit chico por cada cosa que funciona + `git push`.
- Pasar a producción cuando algo está probado:
  ```
  git switch prod && git merge dev && git push
  git switch main && git merge prod && git push
  git switch dev
  ```
- Si algo se rompe en `dev`, `prod`/`main` siguen jugables: se puede volver con `git switch prod`.

## Compatibilidad WebGL (tener en cuenta desde el día 1)
- No usar threads ni `System.IO` para guardar archivos.
- Probar un build WebGL temprano (día 5 como máximo) y subirlo a itch.io.
- Al exportar, revisar Compression Format (en itch.io suele funcionar bien con Gzip
  o con "Decompression Fallback" activado).
- Mantener el peso del build bajo (texturas chicas, pocos assets externos).
- URP en WebGL: usar WebGL 2, mantener el pipeline liviano (sombras suaves o desactivadas,
  sin SSAO, sin HDR ni MSAA pesado, post-procesado mínimo o nulo).
- Los materiales y assets del Asset Store deben ser compatibles con URP (shaders URP/Lit o
  URP/Unlit); si salen rosados, convertirlos desde Edit > Rendering > Materials.

## Cómo quiero que trabajes conmigo
Estoy aprendiendo y quiero entender lo que entrego, así que:
- Preferí guiarme con **preguntas y pistas** antes que escribir sistemas completos.
- Cuando te pida código, explicá el **porqué** de las decisiones.
- Revisá mi código y señalá errores, malas prácticas y casos borde.
- Para tareas mecánicas (estructura de carpetas, `.gitignore`, checklists) podés
  hacerlas directamente.
- Si algo se atrasa, recortá pulido antes que requisitos obligatorios.

## Plan de 7 días (~2-3 hs por día)
Regla: si algo se atrasa, se recorta pulido, nunca los requisitos obligatorios.

### Día 1 — Base y movimiento
- [x] Proyecto Unity 3D (URP), repo Git y `.gitignore` de Unity; ramas `main`/`prod`/`dev` en GitHub
- [ ] Diseño del nivel en papel (inicio, obstáculos, generadores, compuerta)
- [x] Importar assets (tanque, entorno) — revisar que no haya materiales rosados en URP
- [x] Cámara top view (`CameraFollow.cs`, se queda la versión actual)
- [x] Movimiento del tanque con `CharacterController` + gravedad y colisión con el suelo
- [x] Controles de tanque: A/D rotan el casco, W/S avanzan/retroceden según `transform.forward` (hecho el 2026-10-04)
- [x] Ajustar el collider del Player

### Día 2 — Combate básico
- [x] ~~Rotación de la torreta hacia el mouse~~ — descartado (2026-10-04): el cañón apunta siempre hacia adelante
- [x] Disparo (`BulletController`) — funciona; `Character` implementa `IDamagable`
- [x] Daño (`TakeDamage` en `Character`, probado disparándole al `Charger`)
- [x] Jerarquía `Character` / `Enemy`: vida y muerte
- [ ] Primer enemigo (Embestidor): persecución y daño por contacto

### Día 3 — Nivel y spawn
- [ ] Armar el nivel (terreno, paredes, obstáculos) y organizar la Hierarchy
- [ ] Punto de inicio y respawn al morir
- [ ] Condición de llegada al final del nivel (compuerta/trigger)

### Día 4 — Desafío y enemigos restantes
- [ ] Generadores (destruir o activar) y lógica para abrir la compuerta
- [ ] Enemigo Centinela (torreta fija que dispara)
- [ ] Enemigo Patrullero (ruta + ataque por cercanía)
- [ ] Ubicar enemigos custodiando los generadores

### Día 5 — Menú y primer build WebGL
- [ ] Escena de menú con botón Play
- [ ] Pantalla de victoria / fin de nivel
- [ ] **Primer build WebGL subido a itch.io** (detectar problemas a tiempo)

### Día 6 — Pooling y pulido (día liviano)
- [ ] Object Pooling para proyectiles (jugador y enemigos)
- [ ] Balance de dificultad: desafiante pero ganable
- [ ] Feedback: sonido, partículas/explosiones, UI de vida

### Día 7 — Entrega
- [ ] Build final WebGL en itch.io y prueba desde otro navegador
- [ ] Subir el proyecto a Drive
- [ ] PDF con controles, descripción del desafío y enlaces (Drive + itch.io)
- [ ] Entregar

## Estado actual
_Última actualización: 2026-10-05 (Día 2: Parte A hecha, sigue la Parte B). Día 1 completo (commit `9838c4b`)._

**Nombres en código (en inglés):** `Personaje` → `Character`, `Embestidor` → `Charger`.
En este documento el diseño puede seguir usando los nombres en español.

### Hecho
- `PlayerController`: controles de tanque (A/D `transform.Rotate` en Y a `_rotationSpeed` °/s, W/S
  `transform.forward * _moveInput * _speed`, ejes leídos con `GetAxis` por separado), `CharacterController`
  con gravedad; disparo con click (Fire1). Commit `48c6072` en `dev`.
  `Shoot()` valida referencias, instancia con `_shootPoint.rotation * rotación del prefab`
  e ignora la colisión entre la bala y el `CharacterController` del Player.
- `BulletController`: velocidad y daño desde `BulletData` (tipos 0 normal, 1 pesada, 2 explosiva);
  `Start` ya no pisa lo que configura `Launch`; sin `* Time.fixedDeltaTime`; error claro si falta el BulletData.
- `BulletData` (ScriptableObject, en `Scripts/Scriptables/`); asset `Scripts/Data/NormalBullet.asset` (speed 20, damage 20).
- Prefab `Misil` con Rigidbody, BoxCollider y `BulletController`. Disparo probado: funciona.
- `CharacterController` y `PlayerController` aplicados al prefab `Player`.
- `CameraFollow` en la Main Camera: sigue al Player con `SmoothDamp` en `LateUpdate`, rotación fija
  (`_pitch`) y offset calculado con `_distance`. Lo escribió Claude; Marcos decidió dejarlo así.
- `BulletPlaceholder` (shoot point) sin BoxCollider ni MeshRenderer.
- Commit `380167d` "Día 1: movimiento, disparo y cámara top view", pusheado a `main`, `prod` y `dev`.
  Se trabaja en `dev`.
- Decisión de controles (2026-10-04, reemplaza a twin-stick): **tanque clásico**. W/S avanzan/retroceden
  hacia donde mira el tanque, A/D rotan el casco sobre el eje Y; la cámara no rota.
- Decisión del cañón (2026-10-04): **fijo hacia adelante**, sin torreta que siga al mouse. El mouse solo
  dispara (click). Para apuntar se gira el tanque entero con A/D.
- Collider del Player ajustado: `CharacterController` Height 0 / Radius 4 / Center 0 (con la escala 0.346
  del root queda una esfera de radio ~1.4 centrada en el pivot).
- **Día 2, Parte A (2026-10-05), probada:** jerarquía `Character` → `PlayerController` / `Enemy` → `Charger`.
  - `Character` (abstract, `IDamagable`): `_maxHealth` (Inspector), `_currentHealth`, `IsDead { get; private set; }`;
    `virtual Awake` carga la vida; `TakeDamage` con guard `if (IsDead) return`, `-=`, `Mathf.Max(0f, ...)`,
    Debug.Log, y al llegar a 0 → `IsDead = true` y después `Die()`; `abstract Die()`.
  - `PlayerController : Character`: `override Awake` con `base.Awake()` primero; `Die()` = Debug.Log (respawn: Día 3).
  - `Enemy : Character` (abstract): en `Awake` busca al Player por tag (guard con `return` si es null);
    `Update` con guard (`IsDead` / sin player) → `abstract Behave()` (patrón Template Method); `Die()` → `Destroy`.
  - `Charger : Enemy` con `Behave()` vacío. Prefab `Charger`: cubo 1.5×3×1.5, Rigidbody con rotación X/Z
    congelada, `_maxHealth` 60 (3 disparos de 20).
  - Tag "Player" en el prefab; carpeta `Prefab` renombrada a `Prefabs` (GUIDs conservados).

### Próximo paso (empezar por acá)
1. **Parte B — Embestidor (`Charger`), en curso (Marcos la escribe con guía):**
   - En `Enemy` (lo comparten los 3 tipos): `_detectionRange`, helper `protected bool IsPlayerInRange()`
     (`Vector3.Distance`) y `OnDrawGizmosSelected` con `Gizmos.DrawWireSphere` para ver el rango.
   - En `Charger`: `[RequireComponent(typeof(Rigidbody))]`, `_moveSpeed` (~4, menos que el Player para que
     pueda escapar), `_contactDamage` (20), `_attackCooldown` (1 s), `_nextAttackTime`, `_rb` en
     `override Awake` (con `base.Awake()`).
   - `Behave()`: fuera de rango → frenar en X/Z conservando `linearVelocity.y`; en rango → dirección =
     player − posición, `y = 0`, `.normalized`, y `linearVelocity = (dir.x * speed, vel.y, dir.z * speed)`.
     Asignar velocidad en `Update` está bien (no acumula); `AddForce` sí iría en `FixedUpdate`.
   - Daño por contacto: `OnCollisionStay` + `CompareTag("Player")` + cooldown con `Time.time` +
     `TryGetComponent<IDamagable>` (no depende de `PlayerController`).
   - Rigidbody del Charger: Interpolate para que no tiemble con la cámara.
   - Prueba: persigue solo dentro del rango, para afuera, daña 20 cada 1 s en contacto, el Player "muere" (log).

### Pendientes menores
- Convención de nombres en `BulletData`: los campos públicos usan `_` (`_speed`, `_damage`) y `bulletPrefab` no.
- Los dos `Canon` del Player tienen `CapsuleCollider`: revisar si hacen falta (las balas nacen al lado y
  `IgnoreCollision` solo ignora el `CharacterController`).

### Forma de trabajo acordada
Marcos tiene que poder explicarle cada script al profesor: Claude **no escribe sistemas completos** aunque se le pida "necesito X". Guía con pasos, pistas y preguntas, revisa el código que escribe Marcos y solo aplica directamente los arreglos puntuales que se le pidan de forma explícita.