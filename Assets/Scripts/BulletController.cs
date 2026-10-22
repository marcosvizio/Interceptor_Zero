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
    private BulletPool _pool;

    private Rigidbody _rb;


    // Awake se ejecuta apenas se crea el objeto (dentro del Instantiate),
    // antes de que el Player llame a Launch(). Por eso acá buscamos el Rigidbody:
    // tiene que estar listo para cuando Launch() lo use.
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void SetPool(BulletPool pool)
    {
        _pool = pool;
    }

    private void ReturnToPool()
    {
        if (!gameObject.activeSelf)
        {
            return;
        }
        if (!_pool)
        {
            Destroy(gameObject);
            return;
        }
        _pool.Return(this);
    }

    void OnEnable()
    {
        Invoke(nameof(ReturnToPool), _lifeTime);
    }

    void OnDisable()
    {
        CancelInvoke();
    }

    public void Launch(Vector3 direction, int bulletType = 0)
    {
        _rb.angularVelocity = Vector3.zero;
        _rb.linearVelocity = Vector3.zero;
        BulletData data = GetBulletData(bulletType);
    
        if (data == null)
        {
            Debug.LogError($"{name}: falta asignar el BulletData para el tipo {bulletType} en el Inspector.", this);
            ReturnToPool();
            return;
        }

        _speed = data._speed;
        _damage = data._damage;

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
        if (collision.gameObject.TryGetComponent<IDamagable>(out IDamagable damagable))
        {
            damagable.TakeDamage(_damage);
        }

        ReturnToPool();
    }

}
