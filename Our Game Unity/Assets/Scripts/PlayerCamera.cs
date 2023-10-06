using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField] float sensitivityX = 2f;
    [SerializeField] float sensitivityY = 2f;
    [SerializeField] float minimumX = -360f;
    [SerializeField] float maximumX = 360f;
    [SerializeField] float minimumY = -90f;
    [SerializeField] float maximumY = 90f;

    private float rotationX = 0f;
    private float rotationY = 0f;
    private Quaternion originalRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        originalRotation = transform.localRotation;
    }

    void Update()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * sensitivityX;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityY;

        // Calculate new rotation values
        rotationX += mouseX;
        rotationY -= mouseY;

        // Clamp rotation values to prevent over-rotation
        rotationX = Mathf.Clamp(rotationX, minimumX, maximumX);
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

        // Apply rotations to the GameObject
        Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.right);

        transform.localRotation = originalRotation * xQuaternion * yQuaternion;
    }
}
