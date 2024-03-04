using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputCharacterController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerInput playerInputMap;

    [SerializeField] private CharacterController playerController;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject playerBody;

    private float mouseX = 0f;
    private float mouseY = 0f;
    private float xRotation = 0f;
    private float yRotation = 0f;
    private Vector3 cameraX;
    private Vector3 cameraY;

    private Vector2 movementDelta;
    private Vector3 velocityOutput;
    private float velocity;

    [Header("WASD Movement / Gravity / Jumping")]
    [SerializeField] private float gravityStrength = -9.8f;

    [SerializeField] private float gravityMultiplier = 0.8f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    private bool isSprinting;

    private bool isGrounded;
    private float groundDistance = 0.4f;

    [Header("Player settings")]
    [SerializeField] private float mouseSensitivity = 100f;

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpPower = 3.5f;

    private void calculateGravity()
    {
        if (isGrounded == true && velocity < 0)
        {
            velocity = -1f;
        }
        else
        {
            velocity += gravityStrength * gravityMultiplier * Time.deltaTime;
            velocityOutput.y = velocity;
        }
    }

    private void Jump(InputAction.CallbackContext ctx)
    {
        if (isGrounded == true)
        {
            velocity += jumpPower;
        }
    }

    public void Sprint(InputAction.CallbackContext ctx)
    {
        isSprinting = ctx.performed;
    }

    private void calculateInputMovement()
    {
        movementDelta = playerInputMap.FPS.WASD.ReadValue<Vector2>();
        cameraX = playerBody.transform.forward * movementDelta.y;
        cameraY = playerCamera.transform.right * movementDelta.x;

        velocityOutput = cameraX + cameraY;
    }

    private void applyMovements()
    {
        if (isSprinting == true && isGrounded == true)
        {
            moveSpeed = 12f;
        }
        else
        {
            moveSpeed = 7f;
        }
        playerController.Move(velocityOutput * moveSpeed * Time.deltaTime);
    }

    private void applyCamRotation()
    {
        mouseX = playerInputMap.FPS.MouseInput.ReadValue<Vector2>().x * mouseSensitivity * Time.deltaTime;
        mouseY = playerInputMap.FPS.MouseInput.ReadValue<Vector2>().y * mouseSensitivity * Time.deltaTime;

        Vector3 rot = playerCamera.transform.localRotation.eulerAngles;

        xRotation -= mouseY;
        yRotation += mouseX;

        xRotation = Math.Clamp(xRotation, -90, 90);
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        playerBody.transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);
    }

    private void Start()
    {
        playerInputMap = new PlayerInput();
        playerInputMap.Enable();
        playerController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        playerInputMap.FPS.Jump.performed += Jump;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        applyCamRotation();
        calculateInputMovement();
        calculateGravity();
        applyMovements();
    }
}