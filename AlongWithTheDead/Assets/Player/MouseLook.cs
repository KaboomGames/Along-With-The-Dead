using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    #region Variables
    private float mouseX;
    private float mouseY;
    [Header("Sensitivity")]
    public float mouseSentivity;
    public Transform playerBody;
    private float xRotation;

    private InputSystem_Actions inputActions;
    #endregion
    void Awake()
    {
        inputActions = new InputSystem_Actions();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        #region Camera Code
        Vector2 lookInput = inputActions.Player.Look.ReadValue<Vector2>();

        mouseX = lookInput.x * mouseSentivity * Time.deltaTime;
        mouseY = lookInput.y * mouseSentivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);
        playerBody.Rotate(Vector3.up * mouseX);
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        #endregion
    }
}
