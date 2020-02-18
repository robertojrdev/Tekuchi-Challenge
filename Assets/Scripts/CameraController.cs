using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float movementSpeed;
    public float zoomSpeed;
    public Vector3 pivot = Vector3.zero;
    public float distance = 15;
    public Vector2 rotation = new Vector2(30, 45);

    private bool moving = false;

    private void Start()
    {
        RotateCamera();
    }

    private void Update()
    {
        if(!MouseUIHandler.isOverUI)
        {
            if (Input.GetMouseButtonDown(0))
            {
                moving = true;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

            Zoom();
        }


        if (moving && Input.GetMouseButtonUp(0))
        {
            moving = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        if (moving)
        {
            UpdateRotation();
            RotateCamera();
        }

    }

    private void Zoom()
    {
        distance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

        //update zoom if is not rotating
        if(!moving)
            RotateCamera();
    }

    private void RotateCamera()
    {
        var rotation = Quaternion.Euler(this.rotation.x, this.rotation.y, 0);

        transform.position = pivot - rotation * (Vector3.forward * distance);
        transform.rotation = rotation;
    }

    private void UpdateRotation()
    {
        rotation.x -= Input.GetAxis("Mouse Y") * movementSpeed * Time.deltaTime;
        rotation.x = Mathf.Clamp(this.rotation.x, -89, 89);

        rotation.y += Input.GetAxis("Mouse X") * movementSpeed * Time.deltaTime;
    }
}