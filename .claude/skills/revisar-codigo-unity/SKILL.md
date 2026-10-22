---
name: revisar-codigo-unity
description: Checklist para revisar el código C# y los prefabs de Marcos en este proyecto Unity, con los errores que ya cometió antes. Usar cuando pide revisar, chequear o cuando algo "no anda".
---

# Revisar código Unity (Interceptor Zero)

Leer el script completo **y** verificar en los `.prefab`/`.unity` (grep sobre el YAML) los valores que
importan: componentes, `m_IsTrigger`, `m_IsKinematic`, escalas, referencias asignadas (`fileID: 0` = vacío).
No arreglarle el código salvo que lo pida: señalar el bug, explicar por qué falla y dar la pista.

## Errores que Marcos ya cometió (revisar siempre)
- **Guard sin `return`**: un `if (x == null) { Debug.LogError(...); }` que sigue de largo.
- **Condición invertida** en guards o cooldowns (`>=` vs `<`, `!= null` vs `== null`). Leerla en voz alta.
- **Guard que corta de más**: `return` que impide lo que sí debería pasar (ej. `Open()` sin barrera).
- **Asignación al revés** (`_max = _current`).
- **Valor calculado y descartado** (`new Vector3(...)` sin asignar).
- Falta llamar al método después de marcar la bandera (`IsDead = true` sin `Die()`).
- Override de callbacks de Unity sin `protected override` + `base.Awake()` primero.
- `[SerializeField]`: cambiar el default en código no cambia el valor ya guardado en prefab/escena.
- Campos sin usar (warning CS0414) y comentarios guía que quedaron en el código.
- Contar con `.Length` cuando el array puede tener huecos `null`.

## Trampas de Unity de este proyecto
- `Collision` (choque físico, `OnCollision*`, necesita un Rigidbody **dinámico**) vs `Collider`
  (`OnTrigger*`, alcanza con Rigidbody o `CharacterController` en uno de los dos).
- Rigidbody kinematic + NavMeshAgent: no hay `OnCollision*` contra el CharacterController → usar trigger.
- Collider de daño en el mismo objeto que el script `IDamagable` (o en el del Rigidbody).
- `.material` (copia por instancia) vs `.sharedMaterial` (cambia a todos).
- `LookRotation` con vector cero → guard con `sqrMagnitude`.
- Mover un `CharacterController` por transform → apagarlo/prenderlo.
- `using System;` innecesario (choca `Random`); usar `System.Action` con prefijo.
- Escala distinta de 1 en roots con física/NavMeshAgent (multiplica tamaños y offsets).
- Materiales en shader 2D (`Mesh2D-Lit-Default`, guid `4e90a8289da1f3943885176e4f83e35c`): ignoran `color`.

## Formato de la devolución
1. Lo que está bien (breve).
2. Bugs (🐛) con archivo:línea, qué pasa en el juego y la pista para arreglarlo.
3. Detalles menores (🧹).
4. Qué probar después.
