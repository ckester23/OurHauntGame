using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public Transform player; // Reference to the player's Transform
    public float sensitivity = 2.0f; // Mouse sensitivity
    public float smoothing = 1.0f; // Mouse smoothing

    private Vector2 smoothMouseInput;
    private Vector2 currentMouseInput;
    private Vector2 mouseLook;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
        Cursor.visible = false; // Hide the cursor
    }

    private void Update()
    {
        // Get mouse input
        currentMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        // Apply smoothing to the mouse input
        smoothMouseInput = Vector2.Lerp(smoothMouseInput, currentMouseInput, 1.0f / smoothing);

        // Calculate the mouse look
        mouseLook += smoothMouseInput * sensitivity;

        // Clamp the vertical rotation
        mouseLook.y = Mathf.Clamp(mouseLook.y, -90f, 90f);

        // Rotate the camera vertically
        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);

        // Rotate the player horizontally
        player.localRotation = Quaternion.AngleAxis(mouseLook.x, player.up);
    }
}
