using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    private bool isMoving;
    private bool isRunning;
    private bool isCrouching;
    public int health;
    public int maxHealth;

    [Header("Movement")]
    public float walkSpeed;
    public float runSpeed;
    public float crouchSpeed;

    [Header("Camera Height")]
    public Transform cameraTransform;
    public Transform normalTransform;
    public Transform crouchTransform;

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
    public Animator playerAnimator;

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

        if (speed == 0) playerAnimator.SetFloat("Movement", 0);
        else if (speed == crouchSpeed) playerAnimator.SetFloat("Movement", 1);
        else if (speed == walkSpeed) playerAnimator.SetFloat("Movement", 2);
        else if (speed == runSpeed) playerAnimator.SetFloat("Movement", 3);

        characterController.Move(move * speed * Time.deltaTime);
    }
    #endregion

    #region Handle Crouching
    private void HandleCrouch()
    {
        isCrouching = inputActions.Player.Crouch.IsPressed();
        playerAnimator.SetBool("Crouch", isCrouching);

        if (isCrouching)
            cameraTransform.position = 
                Vector3.MoveTowards(cameraTransform.position, crouchTransform.position, 1f * Time.deltaTime);
        else
            cameraTransform.position =
                Vector3.MoveTowards(cameraTransform.position, normalTransform.position, 1f * Time.deltaTime);
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
