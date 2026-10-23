/*

 Author: Marcos Vizio
 Email: marcosfvizio@gmail.com

 Creation Date: 2026-10-23 00:36:19
 Last Modification Date: 2026-10-23 15:33:19

 Another File Header is a Visual Studio Code extension to automatically or by command insert a header to your files.

*/

using UnityEngine;

public class BillBoard : MonoBehaviour
{
    
    private Transform _camera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Camera.main == null) return;
        _camera = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (!_camera) return;

        transform.rotation = _camera.rotation;
    }
}
