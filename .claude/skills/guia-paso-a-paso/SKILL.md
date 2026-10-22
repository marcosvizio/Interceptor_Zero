---
name: guia-paso-a-paso
description: Formato para guiar a Marcos a programar una feature nueva de Unity él mismo (paso a paso explicado, sin entregar el script completo). Usar cuando pide implementar o agregar algo de gameplay, UI o enemigos.
---

# Guía paso a paso (formato acordado con Marcos)

Marcos está aprendiendo y tiene que poder explicar cada línea. **No escribir el script completo.**
Dar fragmentos chicos con huecos (`...`, comentarios `// 1. ...`) y dejarle decisiones pequeñas.

## Antes de escribir la guía
1. Leer los scripts que se van a tocar (y los prefabs/escena con grep si hace falta) para que los pasos
   coincidan con el código real (nombres de campos, métodos, componentes).
2. Si la feature depende de otra (ej. la UI necesita un evento de vida), decirlo y ordenar los pasos.
3. Proponer una rama nueva desde `dev` si es un experimento (`git switch -c <nombre>`).

## Estructura de la respuesta
1. **Objetivo** en una o dos líneas (qué va a ver al darle Play).
2. **Paso 0: conceptos** — explicar las ideas nuevas antes del código (tablas comparativas si hay
   alternativas; analogías simples; conectar con cosas que ya hizo: guards, eventos, `base.Awake()`,
   `SerializeField` que pisa el default, etc.).
3. **Pasos numerados** — Editor y código separados. Cada paso: fragmento mínimo + por qué existe cada parte
   + trampas de Unity relevantes.
4. **Probar (checklist)** — qué tiene que pasar y, para cada falla probable, qué significa y qué revisar.
   Incluir un `Debug.Log`/`Debug.DrawLine` o gizmo para depurar.
5. **Commit** con mensaje sugerido.
6. **Para defenderlo ante el profesor** — 4 a 6 preguntas.
7. Cerrar con "mostrame el código cuando lo tengas".

## Estilo
- Español rioplatense, tuteo con "vos".
- Explicar el porqué, no solo el qué. Relacionar con errores que ya cometió (ver CLAUDE.md).
- Avanzar de a una parte por mensaje si la feature es grande (Marcos prefiere ir lento y entender).
- Al terminar una parte, actualizar la sección "Roadmap" del `CLAUDE.md` de la raíz.
