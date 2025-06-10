using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private PlayerActions actions;

    [SerializeField] private AnyStateAnimator anyStateAnimator;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private FishingManager fishingManager;

    // Movement
    private Vector2 moveInput;
    private float horizontalMouseInput;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float rotationSpeed = 80.0f;
    [SerializeField] private float jumpHeight = 0.2f;
    private float gravityValue = -9.81f;
    private Vector3 playerVelocity;

    private bool canMove = true;

    #region Unity Lifecycle

    private void Awake()
    {
        if (fishingManager == null) Debug.LogWarning("FishingManager not assigned in Player.");
        if (anyStateAnimator == null) Debug.LogWarning("AnyStateAnimator not assigned in Player.");
        if (characterController == null) Debug.LogWarning("CharacterController not assigned in Player.");

        actions = new PlayerActions();
        RegisterInputActions();
    }

    private void Start()
    {
        AnyStateAnimation stand = new AnyStateAnimation("Stand", "Jump");
        AnyStateAnimation walk = new AnyStateAnimation("Walk", "Jump");
        AnyStateAnimation jump = new AnyStateAnimation("Jump","Walk");
        AnyStateAnimation casting = new AnyStateAnimation("Casting");
        AnyStateAnimation castIdle = new AnyStateAnimation("CastIdle", "Reeling");
        AnyStateAnimation reeling = new AnyStateAnimation("Reeling");

        anyStateAnimator.AddAnimation(stand, walk, jump, casting, castIdle, reeling);
    }

        
    

    private void OnEnable() => actions?.Enable();
    private void OnDisable() => actions?.Disable();

    private void Update()
    {
        Gravity();
        if (canMove)
        {
            Move();
            Rotate();
        }
    }

    #endregion

    #region Input Setup

    private void RegisterInputActions()
    {
        actions.ControlsMap.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        //actions.ControlsMap.Move.canceled += ctx => moveInput = Vector2.zero;
        actions.ControlsMap.MouseMovement.performed += ctx => horizontalMouseInput = ctx.ReadValue<float>();
        actions.ControlsMap.Jump.performed += ctx => Jump();
        //actions.ControlsMap.Cast.performed += ctx => StartCasting();
        actions.ControlsMap.Cast.started += ctx => OnCast(ctx);
        // Remove fishing input handling here, FishingManager should handle reeling inputs
    }

    #endregion

    #region Movement

    private void Move()
    {
        Vector3 movement = transform.right * moveInput.x + transform.forward * moveInput.y;
        characterController.Move(movement * moveSpeed * Time.deltaTime);

        bool isMoving = moveInput.x != 0 || moveInput.y != 0;

        if (isMoving)
        {
            anyStateAnimator.TryPlayAnimation("Walk");
        }
        else
        {
            anyStateAnimator.TryPlayAnimation("Stand");
        }
    }

    private void Rotate()
    {
        if (!Mouse.current.rightButton.isPressed) return;

        float mouseX = horizontalMouseInput * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
    }

    private void Gravity()
    {
        if (characterController.isGrounded && playerVelocity.y < 0)
            playerVelocity.y = 0;

        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }

    private void Jump()
    {
        if (!canMove) return; // Prevent jumping during fishing

        anyStateAnimator.TryPlayAnimation("Jump");
        playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
    }

    #endregion

    #region Fishing Delegation

    private void StartCasting()
    {
        if (fishingManager == null) return;

        // Disable player movement while fishing
        canMove = false;

        // Tell FishingManager to start fishing sequence (which will handle fishing animations)
        fishingManager.StartFishing();
    }

    //when player presses cast forward that input to the fishingmanager script
    private void OnCast(InputAction.CallbackContext ctx)
    {
        fishingManager.OnCast(ctx);
    }   
        // Optional: expose methods for FishingManager to enable player movement again after fishing
    public void EnableMovement() => canMove = true;
    public void DisableMovement() => canMove = false;

    #endregion
}
