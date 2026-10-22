using UnityEngine;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private BulletController _prefab;
    [SerializeField] private int _initialSize = 20;

    private readonly Queue<BulletController> _available = new Queue<BulletController>();

    void Awake()
    {
        for (int i = 0; i < _initialSize; i++) {
            _available.Enqueue(CreateBullet());
        }
        
    }

    private BulletController CreateBullet()
    {
        BulletController bullet = Instantiate(_prefab, transform);

        bullet.SetPool(this);

        bullet.gameObject.SetActive(false);

        return bullet;
    }

    public BulletController Get(Vector3 position, Quaternion rotation)
    {
        BulletController bullet;

        if (_available.Count > 0)
        {
            bullet = _available.Dequeue();
        }
        else
        {
            bullet = CreateBullet();
        }

        bullet.transform.SetPositionAndRotation(position, rotation * _prefab.transform.rotation);

        bullet.gameObject.SetActive(true);
        
        return bullet;
    }

    public void Return(BulletController bullet)
    {
        bullet.gameObject.SetActive(false);

        _available.Enqueue(bullet);
    }

}
