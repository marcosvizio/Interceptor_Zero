/*

 Author: Marcos Vizio
 Email: marcosfvizio@gmail.com

 Creation Date: 2026-10-02

*/

using UnityEngine;

// Hace que la cámara siga al objetivo (el Player) desde arriba, siempre a la misma distancia.
// La cámara NO rota con el tanque: en top view, si la cámara girara con el tanque,
// el jugador perdería la referencia de dónde queda "arriba" en el nivel.
public class CameraFollow : MonoBehaviour
{
    // A quién sigue la cámara. Se arrastra el Player desde la Hierarchy.
    [SerializeField] private Transform _target;

    // Inclinación de la cámara. 90 = totalmente desde arriba; ~60-70 = top view con algo de perspectiva.
    [SerializeField, Range(30f, 90f)] private float _pitch = 65f;

    // Distancia (en línea recta) entre la cámara y el Player.
    [SerializeField] private float _distance = 22f;

    // Tiempo aproximado (en segundos) que tarda la cámara en alcanzar al Player.
    // 0 = la cámara está pegada al Player; valores chicos (0.1 - 0.3) dan un seguimiento suave.
    [SerializeField] private float _smoothTime = 0.15f;

    // Velocidad actual de la cámara. SmoothDamp la lee y la actualiza en cada llamada,
    // por eso tiene que guardarse entre frames.
    private Vector3 _velocity;

    void Start()
    {
        if (_target == null)
        {
            Debug.LogError($"{name}: falta asignar el Target en el Inspector.", this);
            enabled = false; // desactiva este script para no seguir tirando errores en cada frame
            return;
        }

        // Arrancamos ya ubicados sobre el Player, sin el "viaje" suave del primer frame.
        transform.rotation = GetRotation();
        transform.position = GetDesiredPosition();
    }

    // LateUpdate corre después de TODOS los Update del frame.
    // Así la cámara se mueve cuando el Player ya terminó de moverse en este frame;
    // si usáramos Update, según el orden de ejecución la cámara podría ir un frame atrasada y "temblar".
    void LateUpdate()
    {
        // Se recalcula cada frame para poder ajustar _pitch y _distance desde el Inspector durante el Play.
        transform.rotation = GetRotation();

        // SmoothDamp acerca la posición actual a la deseada de forma suave e independiente de los FPS.
        transform.position = Vector3.SmoothDamp(transform.position, GetDesiredPosition(), ref _velocity, _smoothTime);
    }

    private Quaternion GetRotation()
    {
        return Quaternion.Euler(_pitch, 0f, 0f);
    }

    // Posición donde debería estar la cámara: partimos del Player y retrocedemos _distance
    // en la dirección contraria a la que mira la cámara. Así el Player siempre queda en el centro de la pantalla.
    private Vector3 GetDesiredPosition()
    {
        return _target.position - GetRotation() * Vector3.forward * _distance;
    }
}
