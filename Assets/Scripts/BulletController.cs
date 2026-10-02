using UnityEngine;

// Obliga a que el GameObject tenga un Rigidbody: sin él la bala no podría moverse por física.
[RequireComponent(typeof(Rigidbody))]
public class BulletController : MonoBehaviour
{
    // Datos de cada tipo de bala (velocidad y daño). Son assets de BulletData
    // que se arrastran desde el Project al Inspector del prefab.
    [SerializeField] private BulletData _normalBulletData;
    [SerializeField] private BulletData _heavyBulletData;
    [SerializeField] private BulletData _explosiveBulletData;

    // Segundos que vive la bala si no choca con nada (así no quedan balas viajando para siempre).
    [SerializeField] private float _lifeTime = 5f;

    // Valores que usa ESTA bala en particular. Se cargan en Launch() según el tipo elegido.
    private float _speed;
    private float _damage;

    private Rigidbody _rb;


    // Awake se ejecuta apenas se crea el objeto (dentro del Instantiate),
    // antes de que el Player llame a Launch(). Por eso acá buscamos el Rigidbody:
    // tiene que estar listo para cuando Launch() lo use.
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Start se ejecuta DESPUÉS de Launch() (Unity lo llama antes del primer frame del objeto).
    // Por eso acá NO se tocan _speed ni _damage: pisaríamos lo que configuró Launch().
    void Start()
    {
        // Programa la destrucción de la bala dentro de _lifeTime segundos.
        Destroy(gameObject, _lifeTime);
    }


    // La llama quien dispara (por ejemplo el PlayerController) justo después de instanciar la bala.
    // direction: hacia dónde sale la bala.
    // bulletType: 0 = normal, 1 = pesada, 2 = explosiva.
    public void Launch(Vector3 direction, int bulletType = 0)
    {
        BulletData data = GetBulletData(bulletType);

        // Si no hay datos asignados en el Inspector, avisamos con un error claro
        // en vez de disparar una bala con velocidad 0 que no se mueve.
        if (data == null)
        {
            Debug.LogError($"{name}: falta asignar el BulletData para el tipo {bulletType} en el Inspector.", this);
            Destroy(gameObject);
            return;
        }

        _speed = data._speed;
        _damage = data._damage;

        // linearVelocity ya está en unidades por segundo: el motor de física aplica el tiempo
        // por su cuenta, así que NO hay que multiplicar por Time.deltaTime ni fixedDeltaTime.
        // Normalizamos la dirección para que la velocidad sea siempre _speed,
        // sin importar el largo del vector que nos pasen.
        _rb.linearVelocity = direction.normalized * _speed;
    }

    // Devuelve el BulletData que corresponde al tipo pedido.
    private BulletData GetBulletData(int bulletType)
    {
        switch (bulletType)
        {
            case 0:
                return _normalBulletData;
            case 1:
                return _heavyBulletData;
            case 2:
                return _explosiveBulletData;
            default:
                Debug.LogWarning($"Tipo de bala inválido ({bulletType}). Se usa la bala normal.");
                return _normalBulletData;
        }
    }

    // Unity la llama sola cuando el collider de la bala choca con otro collider.
    private void OnCollisionEnter(Collision collision)
    {
        // Si lo que golpeamos puede recibir daño (implementa IDamagable), se lo aplicamos.
        if (collision.gameObject.TryGetComponent<IDamagable>(out IDamagable damagable))
        {
            damagable.TakeDamage(_damage);
        }

        // Pegue en lo que pegue (enemigo, pared, suelo), la bala desaparece.
        Destroy(gameObject);
    }

}
