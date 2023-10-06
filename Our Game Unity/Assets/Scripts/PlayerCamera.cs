using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{

    //camera variables
    [SerializeField] float turnSpeed = 90f;
    [SerializeField] float headUpperAngleLimit = 85f;
    [SerializeField] float headLowerAngleLimit = -80f;

    // Current rotation from our start, in degrees
    float yaw = 0f;
    float pitch = 0f;

    // Orientation of head and body when game is started
    Quaternion bodyStartOrientation;
    Quaternion headStartOrientation;

    // Rotates head camera
    Transform head;

    // Initialization processes
    void Start()
    {

        // Find head camera
        head = GetComponentInChildren<Camera>().transform;

        // Caches orientation of body and head
        bodyStartOrientation = transform.localRotation;
        headStartOrientation = head.transform.localRotation;

        // Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate()
    {

        // Read horizontal movement, and scale it based on amount of time elapsed and move speed
        var horizontal = Input.GetAxis("Mouse X") * Time.deltaTime * turnSpeed;
        // Same for vertical.
        var vertical = Input.GetAxis("Mouse Y") * Time.deltaTime * turnSpeed;

        // Updates our yaw and pitch values
        yaw += horizontal;
        pitch += vertical;

        // Clamp pitch so that we can't look all the way up or down.
        pitch = Mathf.Clamp(pitch, headLowerAngleLimit, headUpperAngleLimit);

        // Computing rotation
        var bodyRotation = Quaternion.AngleAxis(yaw, Vector3.up);
        var headRotation = Quaternion.AngleAxis(pitch, Vector3.right);

        // Creates rotations for body and the head by combining them with their start rotations.
        transform.localRotation = bodyRotation * bodyStartOrientation;
        head.localRotation = headRotation * headStartOrientation;

    }
}
