using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC_CameraMovement : MonoBehaviour
{
    [SerializeField, Range(0, 10)] private float mouseSensitivity = 100f;

    private Transform _playerMain = null;
    private Transform _cameraOffset = null;
    private float _rotation = 0f;

    private void Start()
    {
        _cameraOffset = transform.parent.transform;
        _playerMain = transform.parent.parent.transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        _rotation -= mouseY;
        _rotation = Mathf.Clamp(_rotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(_rotation, 0f, 0f);
        _playerMain.Rotate(Vector3.up * mouseX);
    }
}
