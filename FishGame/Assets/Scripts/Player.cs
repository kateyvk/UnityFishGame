using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private PlayerActions actions;
    [SerializeField]
    private AnyStateAnimator anyStateAnimator;

    [SerializeField]
    private CharacterController characterController;

    private bool isCasting;
    private bool hasJustCast;

    #region INPUT
    private Vector2 moveInput;
    private float horizontalMouseInput;
    #endregion

   [SerializeField]
    #region VALUE
    private float moveSpeed =2.0F;
    [SerializeField]
    private float rotationSpeed = 80.0F;
    #endregion

    private Vector3 playerVelocity;
    private float gravityValue = -9.81F;
    [SerializeField]
    private float jumpHeight = 0.2F;    
    
    [SerializeField] private FishingManager fishingManager;


    private void Rotate()
    {
        if (!Mouse.current.rightButton.isPressed)
        {
            float mouseX = horizontalMouseInput * rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.up * mouseX);

        }
    }


    private void Gravity()
    {
        if(characterController.isGrounded && playerVelocity.y <0)
        {
            playerVelocity.y =0;
        }
        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }

    private bool canMove = true;
    private void Move()
    {
        if (!canMove || isCasting) return;

        Vector3 movement = transform.right * moveInput.x + transform.forward * moveInput.y;
        characterController.Move(movement * moveSpeed * Time.deltaTime);
        
        if (moveInput.x != 0 || moveInput.y != 0)
            {
                anyStateAnimator.TryPlayAnimation("Walk");
                hasJustCast = false; //reset after movement
            }
            else if (hasJustCast)
            {
                anyStateAnimator.TryPlayAnimation("CastIdle");
            }
            else
            {
                anyStateAnimator.TryPlayAnimation("Stand");
            }

    }

    private void Jump()
    {
        //Any state animator plays the StandingJump animation
        anyStateAnimator.TryPlayAnimation("Jump");
        //Applying physics 
        playerVelocity.y += Mathf.Sqrt(jumpHeight *-3.0f*gravityValue);
        
    }

    private void StartCasting()
    {
        isCasting = true;
        hasJustCast = false; //currently running casting animation
        anyStateAnimator.TryPlayAnimation("Casting");
    }
    private void StopCasting()
    {
        isCasting = false;
        hasJustCast = true; //cast animation just ended
        anyStateAnimator.TryPlayAnimation("CastIdle");
    }
    private void OnCast(InputAction.CallbackContext cxt)
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


    public void DisableMovement() => canMove = false;
    public void EnableMovement() => canMove = true;



    private void OnEnable()
    {
        actions.Enable();
        actions.ControlsMap.Cast.started += OnCast;
    }
    private void OnDisable()
    {
        actions.ControlsMap.Cast.started -= OnCast;
        actions.Disable();
    }


    void Awake()
    {
        actions = new PlayerActions();

        actions.ControlsMap.Jump.performed += cxt => Jump();
        actions.ControlsMap.Move.performed += cxt => moveInput = cxt.ReadValue<Vector2>();
        actions.ControlsMap.MouseMovement.performed += cxt => horizontalMouseInput = cxt.ReadValue<float>();


        actions.ControlsMap.Cast.started += cxt => StartCasting();
        actions.ControlsMap.Cast.canceled += cxt => StopCasting();

        actions.ControlsMap.Cast.started += cxt => fishingManager.StartFishing();

    }

    void Start()
    {
        AnyStateAnimation stand = new AnyStateAnimation("Stand", "Jump");
        AnyStateAnimation walk = new AnyStateAnimation("Walk", "Jump");
        AnyStateAnimation jump = new AnyStateAnimation("Jump");
        AnyStateAnimation casting = new AnyStateAnimation("Casting");
        AnyStateAnimation castIdle = new AnyStateAnimation("CastIdle", "Stand");
        AnyStateAnimation reeling = new AnyStateAnimation("Reeling", "Stand", "CastIdle"); 
        anyStateAnimator.AddAnimation(stand, walk, jump,casting, castIdle, reeling);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCasting)
        {
            Move();
            Rotate();
        }
        
        Gravity();
        
    }
}
