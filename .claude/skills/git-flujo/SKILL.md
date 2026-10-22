---
name: git-flujo
description: Flujo de git de este proyecto Unity (ramas dev/prod/main y ramas de experimento, commits, merges y push con LFS sin romper Unity abierto). Usar cuando Marcos pide commitear, mergear, pushear o crear una rama.
---

# Git en Interceptor Zero

Repo: https://github.com/marcosvizio/Interceptor_Zero — binarios por **Git LFS** (`.gitattributes` de Unity).

| Rama | Uso |
|---|---|
| `dev` | trabajo diario |
| `<experimento>` (ej. `navmesh`, `pooling`, `ui`) | features nuevas, salen de `dev` y vuelven a `dev` |
| `prod` | lo publicado en itch.io / entregado |
| `main` | igual a `prod` |

## Antes de commitear
1. `git status --short` y `git branch --show-current`.
2. Revisar que no entren cosas indeseadas:
   - materiales `.mat` modificados en masa (posible conversión de shader accidental: comparar `m_Shader`);
   - packs nuevos grandes en `Assets/` (`du -sh`): sacar lo que no se use antes de commitear;
   - nada fuera de lugar en `Builds/`, `Library/`, `Temp/` (ya están en `.gitignore`).
3. Mensaje en español, descriptivo, con viñetas si hay varios cambios, terminando con la línea de
   atribución que indique el sistema.

## Mergear sin que Unity reimporte de más
Unity suele estar abierto: cambiar de rama reescribe archivos y fuerza reimportaciones.
- Avanzar otra rama a la actual **sin hacer checkout** (solo fast-forward):
  `git fetch . <rama-actual>:<rama-destino>` y después `git push origin <rama-destino>`.
- Verificar antes con `git merge-base --is-ancestor <destino> <origen>`; si no es fast-forward,
  explicarle a Marcos y preguntar antes de hacer un merge real.
- Ramas nuevas: `git push -u origin <rama>` la primera vez.

## Pasar a producción
Solo cuando Marcos lo pide y hay un build nuevo probado: `dev` → `prod` → `main` (fast-forward) y push.
Nunca `--force`, nunca reescribir historial (el pack PBS de ~1 GB ya está en LFS; no se toca).
