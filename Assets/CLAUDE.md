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

- **Player:** tanque con movimiento WASD, torreta que apunta al mouse y disparo con click.
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
- [x] Proyecto Unity 3D (URP), repo Git y `.gitignore` de Unity (falta commitear el trabajo del día)
- [ ] Diseño del nivel en papel (inicio, obstáculos, generadores, compuerta)
- [x] Importar assets (tanque, entorno) — revisar que no haya materiales rosados en URP
- [x] Cámara top view (`CameraFollow.cs`, se queda la versión actual)
- [x] Movimiento del tanque con `CharacterController` + gravedad y colisión con el suelo
- [ ] Rotación del casco hacia la dirección de movimiento (twin-stick)
- [ ] Ajustar el collider del Player

### Día 2 — Combate básico
- [ ] Rotación de la torreta hacia el mouse
- [x] Disparo (`BulletController`) — funciona; falta que algo implemente `IDamagable` para probar el daño
- [ ] Daño
- [ ] Jerarquía `Personaje` / `Enemy`: vida y muerte
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
_Última actualización: 2026-10-02 (fin de la sesión 1). El Día 1 todavía no está cerrado._

### Hecho
- `PlayerController`: movimiento WASD con `CharacterController` y gravedad; disparo con click (Fire1).
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
- Decisión de controles: **twin-stick**. WASD mueve en ejes de pantalla y el casco gira hacia donde se mueve;
  el mouse apunta la torreta (Día 2); click dispara. El mouse NO rota el tanque entero.

### Próximo paso (empezar por acá)
1. **Rotación del casco (la escribe Marcos, Claude solo guía y revisa):** en el `Update` del `PlayerController`,
   - dirección solo horizontal a partir de `_input` (no usar `_moveDirection`, que lleva la gravedad en `.y`);
   - `if` para rotar solo cuando hay input (evita el warning "Look rotation viewing vector is zero");
   - `Quaternion.LookRotation` para la rotación objetivo;
   - `Quaternion.Slerp` (`_rotationSpeed` ≈ 10) o `RotateTowards` (≈ 360–720 °/s), multiplicando por `Time.deltaTime`;
   - si el tanque avanza de costado, rotar el modelo hijo, no el root.
2. **Collider del Player:** root con escala 1 y modelo como hijo; ajustar `Height`/`Radius`/`Center.y = Height/2`.
3. **Commit** del Día 1 (hoy todo está untracked).

### Pendientes menores
- Typo "Mrrcos" en el header de `PlayerController.cs`.
- Convención de nombres en `BulletData`: los campos públicos usan `_` (`_speed`, `_damage`) y `bulletPrefab` no.
- Carpeta `Prefab` vs `Prefabs` sugerida.
- Para la torreta (Día 2): agrupar la base de la torreta y `Canons` bajo un objeto vacío `Turret` con el pivot
  en el centro de giro; mouse → suelo con `Camera.ScreenPointToRay` + `Plane.Raycast`; rotar solo en Y.

### Forma de trabajo acordada
Marcos tiene que poder explicarle cada script al profesor: Claude **no escribe sistemas completos** aunque se
le pida "necesito X". Guía con pasos, pistas y preguntas, revisa el código que escribe Marcos y solo aplica
directamente los arreglos puntuales que se le pidan de forma explícita.