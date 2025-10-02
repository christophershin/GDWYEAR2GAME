using Unity.Netcode;
using UnityEngine;

public class CameraController : NetworkBehaviour
{
    // public Transform orientation;
    //
    // public float sensX;
    // public float sensY;
    //
    // private float _xRotation;
    // private float _yRotation;
    //
    //
    // void Start()
    // {
    //     if (!IsOwner) return;
    //     Cursor.lockState = CursorLockMode.Locked;
    //     Cursor.visible = false;
    // }
    //
    // void Update()
    // {
    //     if (!IsOwner) return;
    //     // Get mouse input
    //     float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
    //     float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;
    //
    //     _yRotation += mouseX;
    //     _xRotation -= mouseY;
    //     
    //     _xRotation =  Mathf.Clamp(_xRotation, -90f, 90f);
    //     
    //     // Rotate camera and orientation
    //     transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
    //     orientation.rotation = Quaternion.Euler(0, _yRotation, 0);
    // }
}
