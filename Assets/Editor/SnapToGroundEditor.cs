using UnityEngine;
using UnityEditor;

public class SnapToGroundEditor : Editor
{
    [MenuItem("Tools/Alinear Árboles al Suelo %g")]
    private static void SnapSelectionToGround()
    {
        Undo.IncrementCurrentGroup();
        Undo.SetCurrentGroupName("Alinear Árboles Complejo");

        // Buscamos el objeto padre "ground" en la escena
        GameObject groundParent = GameObject.Find("Ground");
        if (groundParent == null)
        {
            Debug.LogError("No se encontró el GameObject padre llamado 'Ground'. Asegúrate de que se llame exactamente así.");
            return;
        }

        // Obtenemos todos los colliders de los hijos (Grass, Roads, Floors, etc.)
        Collider[] groundColliders = groundParent.GetComponentsInChildren<Collider>();
        if (groundColliders.Length == 0)
        {
            Debug.LogError("¡Cuidado! Ninguno de los hijos de 'Ground' (Grass, Roads, Floors) tiene un componente Collider adjunto.");
            return;
        }

        foreach (GameObject obj in Selection.gameObjects)
        {
            // Lanzamos el rayo desde bastante arriba por si el árbol quedó debajo de algún camino o piso
            Vector3 origin = obj.transform.position + Vector3.up * 50f;
            Ray ray = new Ray(origin, Vector3.down);
            
            float closestDistance = float.MaxValue;
            Vector3 targetPosition = obj.transform.position;
            bool hitAnything = false;

            // Evaluamos el rayo contra cada uno de los colliders hijos de 'ground'
            foreach (Collider col in groundColliders)
            {
                if (col.Raycast(ray, out RaycastHit hit, 200f))
                {
                    // Nos quedamos con el punto de impacto más alto (el suelo real superior)
                    if (hit.distance < closestDistance)
                    {
                        closestDistance = hit.distance;
                        targetPosition = hit.point;
                        hitAnything = true;
                    }
                }
            }

            if (hitAnything)
            {
                Undo.RecordObject(obj.transform, "Movido al suelo");
                obj.transform.position = targetPosition;
            }
        }
    }
}
