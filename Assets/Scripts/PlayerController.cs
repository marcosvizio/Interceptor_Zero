/*

 Author: Marcos Vizio
 Email: marcosfvizio@gmail.com

 Creation Date: 2026-10-01 21:33:23
 Last Modification Date: 2026-10-04 23:22:05

*/

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _turnInput;
    private float _moveInput;
    private Vector3 _moveDirection;
    private CharacterController _characterController;
    private float _verticalVelocity;

    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _rotationSpeed = 120f;
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _shootPoint;

    void Awake()
    {
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
        // Si falta asignar algo en el Inspector, avisamos en vez de tirar NullReferenceException.
        if (_bulletPrefab == null || _shootPoint == null)
        {
            Debug.LogError($"{name}: falta asignar Bullet Prefab o Shoot Point en el Inspector.", this);
            return;
        }

        // Rotación final = rotación del punto de disparo * rotación propia del prefab.
        // Así la bala mira hacia donde apunta el cañón y además conserva la rotación
        // que tenga el root del prefab (por ejemplo los 90° en X del Misil).
        // Con Quaternion.identity esa rotación del prefab se perdía.
        Quaternion rotation = _shootPoint.rotation * _bulletPrefab.transform.rotation;

        GameObject bullet = Instantiate(_bulletPrefab, _shootPoint.position, rotation);

        // La bala nace pegada al tanque: le decimos a la física que ignore el choque
        // entre la bala y el collider del Player, si no se destruiría apenas sale.
        if (bullet.TryGetComponent(out Collider bulletCollider))
        {
            Physics.IgnoreCollision(bulletCollider, _characterController);
        }

        // La dirección sale del Shoot Point (su eje Z azul), no de la rotación de la bala.
        bullet.GetComponent<BulletController>().Launch(_shootPoint.forward);
    }
}