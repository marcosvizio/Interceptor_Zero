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
- **Enemigos (2 tipos; la consigna pide mínimo 3 enemigos, no 3 tipos):**
  - *Centinela:* tanque enemigo **quieto** que gira hacia el jugador y le dispara a intervalos cuando está en rango.
  - *Embestidor (`Charger`):* persigue al jugador y hace daño por contacto.
  - ~~*Patrullero*~~: descartado por ahora (2026-10-08); se puede retomar en el Día 6 si sobra tiempo.
- **Desafío:** 3 generadores repartidos por el nivel, cada uno custodiado por enemigos. Se pueden destruir
  en **cualquier orden**; al destruir los 3 se abre la salida.
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
- [x] Diseño del nivel en papel (inicio, obstáculos, generadores, compuerta) — `Docs/Boceto Nivel.jpeg`
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
- [x] Primer enemigo (Embestidor / `Charger`): persecución y daño por contacto

### Día 3 — Nivel y spawn
- [x] Armar el nivel (terreno, paredes, obstáculos) y organizar la Hierarchy
- [x] Punto de inicio y respawn al morir
- [x] Condición de llegada al final del nivel (compuerta/trigger)

### Día 4 — Desafío y enemigos restantes
- [x] Generadores (destruir) y lógica para abrir la compuerta (evento `Destroyed` + `LevelExit`)
- [ ] Enemigo Centinela (tanque enemigo quieto que apunta y dispara; reutiliza `BulletController`/`BulletData`)
- [x] ~~Enemigo Patrullero~~ — descartado por ahora (2026-10-08)
- [x] Ubicar enemigos custodiando los generadores (10 Chargers con modelo de tanque)

### Día 5 — Menú y primer build WebGL
- [x] Escena de menú con botón Play
- [x] Pantalla de victoria / fin de nivel
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
_Última actualización: 2026-10-18, fin de la sesión. Juego jugable de punta a punta (menú → nivel con 3
generadores y 10 Chargers → victoria). Falta lo de entrega: build WebGL en itch.io, PDF y Drive._

**ENTREGA: 2026-10-21.** Marcos quiere tener casi todo el 2026-10-18 (tiene otros TPs). Prioridad, en orden;
si falta tiempo se recorta desde abajo:
1. Generadores (paso A + B): son el **desafío obligatorio** (consigna 3) y abren la salida.
2. Menú con botón Play + pantalla de victoria (consignas 2 y 8).
3. **Build WebGL subido a itch.io** (entregable obligatorio; hacerlo apenas estén 1 y 2, no al final).
4. Enemigos: ya hay Chargers (alcanza para la consigna de ≥3 enemigos). Centinela solo si sobra tiempo.
5. Object Pooling de balas (punto extra).
6. PDF + Drive (día 21 a más tardar). Pulido (sonido, partículas, UI de vida) solo si sobra.

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
  - Prefab `Charger`: cubo 1.5×3×1.5, Rigidbody con rotación X/Z congelada e Interpolate, `_maxHealth` 60
    (3 disparos de 20).
  - Tag "Player" en el prefab; carpeta `Prefab` renombrada a `Prefabs` (GUIDs conservados).
- **Día 2, Parte B (2026-10-07), probada — commit `d686fd9` en `dev`:**
  - `Enemy`: `_detectionRange` (10) + `protected bool IsPlayerInRange()` (`Vector3.Distance`) +
    `OnDrawGizmosSelected` (esfera amarilla del rango). Lo heredan los 3 tipos de enemigo.
  - `Charger.Behave()`: fuera de rango frena en X/Z conservando `linearVelocity.y` + `return`; en rango
    dirección = player − posición, `y = 0`, `.normalized`, `linearVelocity` con la Y conservada. `_moveSpeed` 4.
  - Daño por contacto: `OnCollisionStay` + `CompareTag("Player")` + guard de cooldown
    (`_nextAttackTime >= Time.time` → return) + `TryGetComponent<IDamagable>` + `_nextAttackTime = Time.time + _attackCooldown`.
    20 de daño cada 1 s.

- **Día 3, paso 1 (2026-10-07), probado — Respawn:** `SpawnPoint` vacío en la escena;
  `Character.ResetHealth()` (protected: vida al máximo + `IsDead = false`), reutilizado por `Awake`;
  `PlayerController.Die()` → `Respawn()`: guard de `_spawnPoint`, apaga el `CharacterController`,
  `SetPositionAndRotation`, lo prende, `_verticalVelocity = 0`, `ResetHealth()`.
  Ramas `dev`/`prod`/`main` sincronizadas en `9bf9434` (Día 2).
- **Día 3, paso 2 (2026-10-07), probado — commit `541186f`:** `LevelExit` (cubo chato con `BoxCollider`
  Is Trigger alto) + script `LevelExit`: `OnTriggerEnter(Collider other)` con guards `CompareTag("Player")`,
  `_isOpen` (gancho del Día 4: los generadores la abren) y `_levelCompleted` (una sola vez) → Debug.Log.

- **Día 3, paso 3 — Nivel (2026-10-18), hecho:** boceto en `Docs/Boceto Nivel.jpeg` (escala 1 cuadrado =
  2 u; generadores en cualquier orden; Centinela = tanque enemigo quieto que dispara; Patrullero descartado).
  Blockout y decoración final hechos por Marcos: captura en `Docs/Nivel Final.png`. Caminos arriba y abajo,
  pasto con árboles en el centro, hangares y edificios del pack `RPG_FPS_game_assets_industrial`, prefabs
  propios nuevos (`Barril`, `BloqueoCamino`, `Grass`, `Hangar`, `Hangar2`, `Road`, `VallaConcreto`, `VallaConcreto2`).
  Ya hay un modelo de generador del pack en uso (`Other_props/Generators/Generator_v1`).
  Herramienta de editor `Assets/Editor/SnapToGroundEditor.cs` (Tools → Alinear Árboles al Suelo, Ctrl+G):
  raycast hacia abajo contra los colliders hijos de `Ground`. Está en `Editor/`, así que no entra al build.
- SpawnPoint abajo a la izquierda (sobre el camino de abajo); LevelExit arriba a la derecha (marcados en
  `Docs/Nivel Final.png`).
- **Limpieza de assets (2026-10-18, hecha por Claude):** se calcularon las dependencias reales (escena `Scene1`,
  `Prefabs/`, `Scripts/`, `Materials/`, `Settings/`, `Editor/`, ProjectSettings, incluyendo los `.meta` de los
  modelos) y se **movieron** a `C:\Users\marco\Proyectos_Unity\Interceptor_Zero_assets_sin_usar\` (fuera del
  proyecto, con la misma estructura de carpetas) los archivos sin usar: `RPG_FPS_game_assets_industrial`
  (321 archivos, 407 MB), `WZ PAK` (162, 32 MB) y `PBS Materials Variety Pack` (230, 1,07 GB). `Assets/` pasó a
  ~60 MB. Si algo aparece rosado o como "Missing", se restaura copiándolo de esa carpeta a la misma ruta.
  `SimpleNaturePack` y `BTM_Assets` no se tocaron (son chicos).
  Ojo: `PBS Materials Variety Pack` (~1 GB) ya estaba commiteado y pusheado en `541186f`; borrarlo ahora no
  libera el espacio LFS en GitHub (queda en el historial). No reescribir el historial antes de la entrega.

- **Día 4, pasos A + B (2026-10-18), probados — el desafío obligatorio está completo:**
  - `Generator : MonoBehaviour, IDamagable` (`[RequireComponent(typeof(MeshRenderer))]`): `_maxHealth` 100,
    `IsDestroyed { get; private set; }`, `public event System.Action Destroyed`; `TakeDamage` con guard,
    `-=`, `Mathf.Max(0f, ...)`, log; `OnDestroyed()` marca `IsDestroyed`, invoca `Destroyed?.Invoke()` y pone
    el material en negro (`.material`, copia por instancia). Prefab `Prefabs/Generador.prefab` con el script;
    3 instancias en la escena.
  - `LevelExit`: `_isOpen` privado (lo controla el código), `[SerializeField] GameObject _barriers`
    (prefab `BloqueoSalida`), `[SerializeField] Generator[] _generators` (los 3 asignados), `_remaining`.
    `Start` se suscribe con `+=` (guard de null); `OnGeneratorDestroyed()` descuenta y al llegar a 0 llama a
    `Open()` (abre + `SetActive(false)` de la barrera); `OnDestroy()` desuscribe con `-=`. Patrón Observer.
  - Detalles menores en `LevelExit`: `_remaining` cuenta también los huecos null del array; `Open()` no
    chequea `_barriers == null`; `_isOpen = true` repetido en `Start`; comentarios guía en `Open()`.

- **Día 5, paso A (2026-10-18), hecho:** escenas `MainMenu` y `Victory` (Canvas + Button TMP + fondos
  `Assets/UI/Backgrounds/*_BG.png` dibujados por Claude con código); `SceneLoader.LoadScene(string)` reutilizable
  desde **On Click ()**; `LevelExit` carga `_victorySceneName` ("Victory") al completar. Sin botón Salir (WebGL).
  `Packages/manifest.json` sumó `com.unity.2d.sprite` (lo agrega Unity al usar sprites).
- Se restauraron desde el respaldo los modelos `WZ PAK/vehicles/armored carrier/*` (16 FBX, 5,5 MB; todos usan
  el material compartido `al1`) para darle forma de tanque al Charger. Los que no se usen se pueden volver a quitar.

- **Charger con forma de tanque (2026-10-18, fin de la sesión):** el prefab `Charger` usa el modelo
  `apc_plaz2` (el Player usa `apc_b1`); `Behave()` rota con `Quaternion.LookRotation` + `RotateTowards` +
  `_rb.MoveRotation` a `_turnSpeed`. Hay **10 Chargers** en la escena, ubicados por Marcos.
  Menú, victoria, fondos y Charger commiteados y pusheados en `dev`.
  Pendientes chicos del Charger: guard de `direction.sqrMagnitude` antes de `LookRotation` (warning si
  queda encima del player) y Freeze Rotation **Y** en el Rigidbody (hoy solo X y Z: los choques lo pueden girar).
  Los otros 14 modelos `apc_*` restaurados no se usan: se pueden volver a quitar.

### Próximo paso (empezar por acá) — quedan 3 días para la entrega (21/10)
1. **Día 5, paso B — Primer build WebGL y subida a itch.io (OBLIGATORIO, lo más urgente).**
   Antes: pasar `dev` → `prod` → `main`. Revisar compresión (Gzip o Decompression Fallback), tamaño de
   texturas (Max Size), probar en el navegador que el flujo Menú → Nivel → Victoria funcione.
2. PDF (controles, desafío, links a Drive e itch.io) + subir el proyecto a Drive (sin `Library/`, `Temp/`,
   `Logs/`, `.git/`).
3. Object Pooling de balas (punto extra).
4. **Día 4, paso C — Centinela (solo si sobra tiempo):** tanque enemigo quieto (`Enemy`), gira hacia el player en rango y dispara con
   cooldown reutilizando `BulletController`/`BulletData` (ojo: la bala enemiga no debe chocar con su tirador).
4. Ubicar enemigos custodiando los generadores y balancear.

### Pendientes menores
- Convención de nombres en `BulletData`: los campos públicos usan `_` (`_speed`, `_damage`) y `bulletPrefab` no.
- Los dos `Canon` del Player tienen `CapsuleCollider`: revisar si hacen falta (las balas nacen al lado y
  `IgnoreCollision` solo ignora el `CharacterController`).

### Forma de trabajo acordada
Marcos tiene que poder explicarle cada script al profesor: Claude **no escribe sistemas completos** aunque se le pida "necesito X". Guía con pasos, pistas y preguntas, revisa el código que escribe Marcos y solo aplica directamente los arreglos puntuales que se le pidan de forma explícita.