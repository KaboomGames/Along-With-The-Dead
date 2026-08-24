using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    private bool isMoving;
    private bool isRunning;
    private bool isCrouching;

    [Header("Movement")]
    public float walkSpeed;
    public float runSpeed;
    public float crouchSpeed;

    [Header("Height")]
    public float normalHeight;
    public float crouchHeight;
    private float targetHeight;

    [Header("Jump & Gravity")]
    public float jumpHeight;
    public float gravity = -9.12f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance;
    public LayerMask groundLayer;

    [Header("Components")]
    public CharacterController characterController;
    public CapsuleCollider capsuleCollider;
    private InputSystem_Actions inputActions;

    [Header("Movement")]
    public float speed;
    private Vector3 velocity;
    private bool isGrounded;
    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        inputActions = new InputSystem_Actions();
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
        HandleMovement();
        HandleCrouch();
        HandleJumpAndGravity();
    }

    #region Handle Movement
    private void HandleMovement()
    {
        Vector2 moveInput = inputActions.Player.Move.ReadValue<Vector2>();
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;

        isMoving = moveInput.sqrMagnitude > 0.0001f;
        isRunning = inputActions.Player.Sprint.IsPressed();
        isCrouching = inputActions.Player.Crouch.IsPressed();

        if (!isMoving) speed = 0;
        else if (isCrouching) speed = crouchSpeed;
        else if (isRunning) speed = runSpeed;
        else speed = walkSpeed;

        characterController.Move(move * speed * Time.deltaTime);
    }
    #endregion

    #region Handle Crouching
    private void HandleCrouch()
    {
        isCrouching = inputActions.Player.Crouch.IsPressed();

        if (isCrouching) targetHeight = crouchHeight;
        else targetHeight = normalHeight;

        characterController.height = Mathf.Lerp(characterController.height, targetHeight, 3f * Time.deltaTime);
        capsuleCollider.height = Mathf.Lerp(capsuleCollider.height, targetHeight, 3f * Time.deltaTime);
    }
    #endregion

    #region Handle Jump And Gravity
    private void HandleJumpAndGravity()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (inputActions.Player.Jump.WasPressedThisFrame() && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity* Time.deltaTime);
    }
    #endregion
}
