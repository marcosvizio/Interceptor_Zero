/*

 Author: Marcos Vizio
 Email: marcosfvizio@gmail.com

 Creation Date: 2026-10-02 00:57:31
 Last Modification Date: 2026-10-02 00:57:40

*/

using UnityEngine;

[CreateAssetMenu(fileName = "BulletData", menuName = "Scriptable Objects/BulletData")]
public class BulletData : ScriptableObject
{
    public GameObject bulletPrefab;

    public float _speed;
    public float _damage;

    [TextArea(3, 10)] public string _description;
}
