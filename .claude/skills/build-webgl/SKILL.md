---
name: build-webgl
description: Checklist para generar y publicar el build WebGL de Interceptor Zero en itch.io y actualizar la entrega (.unitypackage y PDF). Usar cuando Marcos quiere un build nuevo o reentregar.
---

# Build WebGL + publicación

## Chequeo previo (hacerlo con grep sobre los archivos del proyecto)
- Módulo **Web Build Support** instalado en Unity 6000.3.15f1
  (`C:\Program Files\Unity\Hub\Editor\6000.3.15f1\Editor\Data\PlaybackEngines\` debe tener WebGL).
- `ProjectSettings/EditorBuildSettings.asset`: `MainMenu`, `Scene1`, `Victory` en ese orden y habilitadas.
- `ProjectSettings/ProjectSettings.asset`: `webGLCompressionFormat: 1` (Gzip) y `webGLDecompressionFallback: 1`.
- Ningún `using UnityEditor` fuera de carpetas `Editor/`.
- Ningún material en shader 2D (`4e90a8289da1f3943885176e4f83e35c`) usado por la escena.
- WebGL usa la calidad **Mobile** (`Assets/Settings/Mobile_RPAsset`).

## Pasos para Marcos (en Unity)
1. File → Build Profiles → Web → Build (carpeta `Builds/WebGL`, ignorada por git). Para probar local:
   **Build And Run** (abrir `index.html` con doble click no funciona).
2. Probar: menú → Play → nivel → generadores → salida → victoria → volver al menú. Nada rosado.
3. Zip con el **contenido** de `Builds/WebGL` (`index.html` en la raíz del zip) → itch.io → Upload →
   "played in the browser", viewport 1280×720, Fullscreen button. Probar en incógnito.

## Si se reentrega
- `.unitypackage`: Assets → Export Package (Include dependencies). El `CLAUDE.md` ya no está en `Assets/`.
  Recordar que el package no incluye ProjectSettings (Scene List): está aclarado en el PDF.
- PDF: editar `Docs/Interceptor_Zero.html` (ej. sumar Object Pooling a consignas, nuevos enemigos/UI) y
  regenerar con Edge headless:
  `msedge --headless=new --disable-gpu --no-pdf-header-footer --print-to-pdf="Docs\Interceptor_Zero.pdf" <file:///...html>`
  Verificar el PDF leyéndolo (que no queden páginas casi vacías).
- Después del build probado: pasar `dev` → `prod` → `main` (skill `git-flujo`).
