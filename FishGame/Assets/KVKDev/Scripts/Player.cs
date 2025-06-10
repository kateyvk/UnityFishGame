using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private PlayerActions actions;

    [SerializeField] private AnyStateAnimator anyStateAnimator;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private FishingManager fishingManager;

    // Internal state flags
    private bool isCasting = false;
    private bool castFinished = false;
    private bool isReeling = false;
    private bool canMove = true;

    // Input
    private Vector2 moveInput;
    private float horizontalMouseInput;

    // Movement
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float rotationSpeed = 80.0f;
    [SerializeField] private float jumpHeight = 0.2f;
    private float gravityValue = -9.81f;
    private Vector3 playerVelocity;

    

    #region Unity Lifecycle

    private void Awake()
    {
        if (fishingManager == null) Debug.LogWarning("FishingManager not assigned in Player.");
        if (anyStateAnimator == null) Debug.LogWarning("AnyStateAnimator not assigned in Player.");
        if (characterController == null) Debug.LogWarning("CharacterController not assigned in Player.");

        actions = new PlayerActions();
        RegisterInputActions();
    }

    private void OnEnable() => actions?.Enable();
    private void OnDisable() => actions?.Disable();

    private void Start()
    {
        // Define animation states and their priorities
        var stand = new AnyStateAnimation("Stand", "Jump", "Casting");
        var walk = new AnyStateAnimation("Walk", "Jump");
        var jump = new AnyStateAnimation("Jump");
        var casting = new AnyStateAnimation("Casting");
        var castIdle = new AnyStateAnimation("CastIdle", "Reeling");
        var reeling = new AnyStateAnimation("Reeling");

        anyStateAnimator?.AddAnimation(stand, walk, jump, casting, castIdle, reeling);
    }

    private void Update()
    {
        if (!isCasting)
        {
            Move();
            Rotate();
        }

        Gravity();
    }

    #endregion

    #region Input Setup

    private void RegisterInputActions()
    {
        actions.ControlsMap.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        actions.ControlsMap.MouseMovement.performed += ctx => horizontalMouseInput = ctx.ReadValue<float>();
        actions.ControlsMap.Jump.performed += ctx => Jump();
        actions.ControlsMap.Cast.performed += ctx => StartCasting();
        actions.ControlsMap.Cast.started += OnCast;
        actions.ControlsMap.Reel.started += ctx => StartReeling();
        actions.ControlsMap.Reel.canceled += ctx => StopReeling();
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
            SetAnimation("Walk");
            castFinished = false;
        }
        else
        {
            if (castFinished && !isReeling)
            {
                SetAnimation("CastIdle");
            }
            else if (!isCasting && !isReeling)
            {
                SetAnimation("Stand");
            }
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
        SetAnimation("Jump");
        playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
    }

    #endregion

    #region Casting & Reeling

    private void StartCasting()
    {
        if (isReeling) return;
        SetAnimation("Casting");
        isCasting = true;
        castFinished = false;
        canMove = false;

       
    }

    public void StopCasting()
    {
        Debug.Log("StopCasting called by animation event");
        SetAnimation("CastIdle");
        isCasting = false;
        castFinished = true;
        canMove = true;

        
    }

    private void StartReeling()
    {
        Debug.Log("StartReeling called");

        if (!castFinished || isCasting) return;
        
        isReeling = true;
        canMove = false;

        SetAnimation("Reeling");
    }

    private void StopReeling()
    {
        Debug.Log("StopReeling called");

        isReeling = false;
        canMove = true;

        SetAnimation(castFinished ? "CastIdle" : "Stand");
    }

    private void OnCast(InputAction.CallbackContext ctx)
    {
        if (fishingManager != null)
        {
            fishingManager.StartFishing();
        }
        else
        {
            Debug.LogWarning("FishingManager is null when casting was attempted.");
        }
    }

    #endregion

    

    public void DisableMovement() => canMove = false;
    public void EnableMovement() => canMove = true;

   
    private string currentAnimation = "";

    private void SetAnimation(string animationName)
    {
        if (animationName == currentAnimation) return;

        currentAnimation = animationName;
        anyStateAnimator?.TryPlayAnimation(animationName);
    }



    
}
