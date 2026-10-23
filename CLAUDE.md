# CLAUDE.md — Interceptor Zero

Juego top-down de tanques hecho por Marcos Vizio para **Programación de Videojuegos 2**.
**Entregado el 2026-10-21** (itch.io: https://marcosvizio.itch.io/interceptor-zero). Ahora el proyecto se usa
**para estudiar**: cada mejora es una excusa para aprender un concepto nuevo.
Historial día por día y detalles de la entrega: `Docs/HISTORIAL.md` (leerlo solo si hace falta).

- Unity **6000.3.15f1**, URP (proyecto **3D**), C#, VS Code + Claude Code, Windows.
- Escenas (Scene List): `MainMenu` (0) → `Scene1` (1) → `Victory` (2).
- Nombres en código en inglés (`Character`, `Charger`); en la charla, español.

## Cómo trabajar conmigo (lo más importante)
- Marcos **escribe el código de gameplay**: Claude guía con la skill `guia-paso-a-paso`, no entrega sistemas
  completos aunque diga "necesito X". Arreglos puntuales pedidos explícitamente ("cambiá", "hacelo") sí.
- Tareas mecánicas (git, configuración, assets, documentos, checklists) Claude las puede hacer directamente.
- Al revisar código usar la skill `revisar-codigo-unity`; para git, `git-flujo`; para builds, `build-webgl`.
- Explicar siempre el **porqué** y cerrar con preguntas "para defender ante el profesor".

## El juego
- **Player** (`PlayerController : Character`): controles de tanque (W/S avanza/retrocede según
  `transform.forward`, A/D rota en Y), `CharacterController` + gravedad, cañón fijo, click dispara.
  Muere → `Respawn()` en `SpawnPoint` (apaga/prende el CC, `ResetHealth()`); el progreso se conserva.
- **Charger** (`Charger : Enemy`): `NavMeshAgent` (Agent Type "Tank"), persigue en `_detectionRange`,
  daño por contacto con un trigger hijo `ContactTrigger` + `OnTriggerStay` + cooldown con `Time.time`.
  Rigidbody **kinematic**, Stopping Distance 3, Base Offset 1.2. 10 en la escena (modelo `apc_plaz2`).
- **Desafío:** 3 `Generator` (`IDamagable`, no `Character`) con evento `Destroyed`; `LevelExit` se suscribe,
  cuenta y al llegar a 0 abre la barrera; al entrar carga `Victory` (patrón Observer).
- **Balas:** `BulletController` + `BulletData` (ScriptableObject); prefab `Misil`; reutilizadas con `BulletPool`.

## Arquitectura (scripts en `Assets/Scripts/`)
| Script | Rol |
|---|---|
| `Character` (abstract, `IDamagable`) | vida, `TakeDamage` (guard `IsDead`), `ResetHealth()`, `abstract Die()` |
| `PlayerController : Character` | movimiento, disparo, respawn |
| `Enemy : Character` (abstract) | busca al Player por tag, `IsPlayerInRange()`, `Update` → `abstract Behave()` (Template Method), gizmo de rango |
| `Charger : Enemy` | persecución con NavMesh + daño por contacto |
| `Generator : MonoBehaviour, IDamagable` | vida propia, evento `Destroyed`, se pone negro con `.material` |
| `LevelExit` | cuenta generadores, abre barrera, carga la victoria |
| `BulletController` / `BulletData` | proyectil / datos de cada tipo de bala |
| `BulletPool` | Object Pooling de balas (`Get` / `Return`), un objeto en la escena |
| `CameraFollow` (lo escribió Claude), `SceneLoader` | cámara top-down con `SmoothDamp` / cambio de escenas |
| `Editor/SnapToGroundEditor` | herramienta: alinear objetos al suelo (no entra al build) |

Convenciones: un script por responsabilidad; campos `private` con `_` + `[SerializeField]`; `virtual`/`override`
solo donde el comportamiento difiere; guards con `return` al principio; `Debug.LogError(..., this)` si falta
una referencia del Inspector.

## Reglas del proyecto (aprendidas a los golpes)
- **Nunca** convertir materiales a **URP 2D**. Materiales rosados → Render Pipeline Converter → *Built-in to URP*.
- Root de objetos con física en escala 1; modelo como hijo (Player y Charger todavía tienen escala 0.346).
- Collider y script que recibe daño en el **mismo objeto** (o en el objeto del Rigidbody).
- Después de mover algo del nivel: **re-bakear el NavMesh**.
- Todo lo que use `UnityEditor` va en una carpeta `Editor/`.
- Assets nuevos: borrar lo que no se use antes de commitear (van por Git LFS). Respaldo de lo quitado en
  `C:\Users\marco\Proyectos_Unity\Interceptor_Zero_assets_sin_usar\`.
- Pack `RPGPP_LT`: sus 3 materiales siguen en shader 2D; pasarlos a `URP/Lit` si se usan.

## Git
Repo https://github.com/marcosvizio/Interceptor_Zero (LFS). `dev` = trabajo diario; `prod` = versión
publicada en itch.io; `main` = igual a `prod`. Experimentos en ramas propias desde `dev` (ej. `navmesh`).
Detalles y comandos: skill `git-flujo`. `prod`/`main` están en la entrega (`15da048`); `dev` tiene NavMesh.

## Roadmap de estudio (en orden)
1. ✅ **Object Pooling de balas** (2026-10-22, rama `pooling`): `BulletPool` (Queue, prewarm 20, crece si
   se vacía, balas hijas del pool); `BulletController.ReturnToPool()` con guard `activeSelf`, vida con
   `Invoke` en `OnEnable` + `CancelInvoke` en `OnDisable`; `PlayerController` usa `_bulletPool.Get(...)`.
   Prefab `Misil` rehecho con el modelo `RMB_21` (Capsule Collider en el root, Continuous Dynamic).
2. **UI** (en partes, rama `ui`):
   - ✅ A (2026-10-23): interfaz `IHealth` (`CurrentHealth`, `MaxHealth`, `event System.Action<float, float>
     HealthChanged`) implementada por `Character` (invoca en `TakeDamage` y `ResetHealth`); script reutilizable
     `HealthBar` (`[SerializeField] GameObject _target` + `GetComponent<IHealth>()` en `Awake`, `+=`/`-=` en
     `OnEnable`/`OnDisable`, valor inicial en `Start`, `Image` Filled); Canvas `HUD` en `Scene1` con la barra del Player.
   - B (siguiente): prefab `WorldHealthBar` (Canvas World Space + `HealthBar` + `Billboard` en `LateUpdate`)
     anidado en los prefabs `Charger` y `Generador`; `Generator` implementa `IHealth`. Ojo: no apagar el
     objeto que tiene el `HealthBar` para ocultarlo (se desuscribe en `OnDisable`): usar `CanvasGroup.alpha`.
   - B: `Generator` implementa `IHealth`; barras world-space sobre enemigos y generadores (mirando a cámara).
   - C: contador de generadores (evento en `LevelExit`). D: flecha estilo *Crazy Taxi* al generador vivo
     más cercano.
3. **Centinela** (`Sentry : Enemy`): tanque quieto que gira hacia el player en rango y dispara con cooldown,
   usando un pool de balas propio; su bala no debe dañarlo a él ni a otros enemigos.
4. Pendientes chicos: Charger (`Freeze Rotation Y` ya no aplica con NavMesh), `Canon` del Player con
   `CapsuleCollider` sobrantes, hijo `ContactTrigger` sobrante en el prefab `Generador`, convención de
   nombres en `BulletData` (`_speed` público).
Si se reentrega: nuevo build, nuevo `.unitypackage` (el `CLAUDE.md` ya está fuera de `Assets/`) y PDF
(`Docs/Interceptor_Zero.html` → Edge headless → `Docs/Interceptor_Zero.pdf`).
