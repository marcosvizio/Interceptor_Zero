/*

 Author: Marcos Vizio
 Email: marcosfvizio@gmail.com

 Creation Date: 2026-10-01 21:33:23
 Last Modification Date: 2026-10-22 14:00:44

*/

using UnityEngine;

public class PlayerController : Character
{
    private float _turnInput;
    private float _moveInput;
    private Vector3 _moveDirection;
    private CharacterController _characterController;
    private float _verticalVelocity;

    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _rotationSpeed = 120f;
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private BulletPool _bulletPool;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private Transform _spawnPoint;
    private BulletController bullet;

    protected override void Awake()
    {
        base.Awake();
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        _turnInput = Input.GetAxis("Horizontal"); // Leemos las teclas por separado. Ej: A = -1, D = +1.
        _moveInput = Input.GetAxis("Vertical"); // Ej: S = -1, W = +1

        transform.Rotate(0f, _turnInput * _rotationSpeed * Time.deltaTime, 0f); // Rotamos el tanque sobre el eje Y

        _moveDirection = transform.forward * _moveInput * _speed; // Para avanzar y retroceder el tanque con W y S

        // Gravedad simulada
        if (_characterController.isGrounded && _verticalVelocity < 0f)
        {
            _verticalVelocity = -3f; // pequeño empuje para mantenerlo pegado al suelo
        }
        _verticalVelocity += _gravity * Time.deltaTime;
        _moveDirection.y = _verticalVelocity;

        _characterController.Move(_moveDirection * Time.deltaTime);

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (_bulletPool == null || _shootPoint == null)
        {
            Debug.LogError($"{name}: falta asignar Bullet Prefab o Shoot Point en el Inspector.", this);
            return;
        }

        bullet = _bulletPool.Get(_shootPoint.position, _shootPoint.rotation);

        if (bullet.TryGetComponent(out Collider bulletCollider))
        {
            Physics.IgnoreCollision(bulletCollider, _characterController);
        }

        // La dirección sale del Shoot Point (su eje Z azul), no de la rotación de la bala.
        bullet.GetComponent<BulletController>().Launch(_shootPoint.forward);
    }

    private void Respawn()
    {
        if(_spawnPoint == null){ 
            Debug.LogError("No hay un Spawn Point asignado");
            return;
        }
        _characterController.enabled = false;
        transform.SetPositionAndRotation(_spawnPoint.position, _spawnPoint.rotation);
        _characterController.enabled = true;
        _verticalVelocity = 0f;
        ResetHealth();
    }

    protected override void Die()
    {
        Debug.Log("El Player murió");
        Respawn();
    }
}