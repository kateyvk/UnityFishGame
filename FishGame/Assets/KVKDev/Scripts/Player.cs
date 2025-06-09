using NUnit.Framework.Internal;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private PlayerActions actions;
    [SerializeField] private AnyStateAnimator anyStateAnimator;

    [SerializeField] private CharacterController characterController;
    [SerializeField] private FishingManager fishingManager;



    //bools to help manage animation logic
    private bool isCasting = false;
    private bool castFinished = false;
    private bool isReeling = false;
    private bool canMove = true;
    

    #region INPUT
    private Vector2 moveInput;
    private float horizontalMouseInput;
    #endregion

   
    #region MOVEMENT VALUES
    [SerializeField] private float moveSpeed =2.0F;
    
    [SerializeField] private float rotationSpeed = 80.0F;
    [SerializeField] private float jumpHeight = 0.2F; 
    private float gravityValue = -9.81F;

    private Vector3 playerVelocity;
    #endregion

    private void OnEnable()
    {
        actions.Enable();
        
    }
    private void OnDisable()
    {
        
        actions.Disable();
    }


    void Awake()
    {
        actions = new PlayerActions();

        actions.ControlsMap.Jump.performed += cxt => Jump();
        actions.ControlsMap.Move.performed += cxt => moveInput = cxt.ReadValue<Vector2>();
        actions.ControlsMap.MouseMovement.performed += cxt => horizontalMouseInput = cxt.ReadValue<float>();
        actions.ControlsMap.Cast.performed += cxt => StartCasting(); //user should only press the button once
        actions.ControlsMap.Cast.started += OnCast;
        actions.ControlsMap.Reel.started += cxt => StartReeling(); //user must hold down for reeling
        actions.ControlsMap.Reel.canceled += cxt => StopReeling();

        actions.ControlsMap.Cast.started += cxt => fishingManager.StartFishing();

    }

    void Start()
    {
        AnyStateAnimation stand = new AnyStateAnimation("Stand","Jump","Casting");
        AnyStateAnimation walk = new AnyStateAnimation("Walk", "Jump");
        AnyStateAnimation jump = new AnyStateAnimation("Jump");
        AnyStateAnimation casting = new AnyStateAnimation("Casting");
        AnyStateAnimation castIdle = new AnyStateAnimation("CastIdle","Reeling");
        AnyStateAnimation reeling = new AnyStateAnimation("Reeling"); 
        anyStateAnimator.AddAnimation(stand, walk, jump,casting, castIdle, reeling);
    }


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




    private void Move()
    {
        if (!canMove || isCasting || isReeling) return; //preventing movement when casting or reeling

        Vector3 movement = transform.right * moveInput.x + transform.forward * moveInput.y;
        characterController.Move(movement * moveSpeed * Time.deltaTime);

        bool isMoving = moveInput.x != 0 || moveInput.y != 0;

        if (isReeling)
        {
            return;
        }
        if (isMoving)
        {
            anyStateAnimator.TryPlayAnimation("Walk");
            castFinished = false;
        }
        else
        {
            if (castFinished)
            {
                anyStateAnimator.TryPlayAnimation("CastIdle");
            }
            else
            {
                anyStateAnimator.TryPlayAnimation("Stand");
            }
        }
        /*
        if (isMoving == true)
        {
            anyStateAnimator.TryPlayAnimation("Walk");
            castFinished = false;
        }
        else
        {
            if (castFinished && !isReeling)
            {
                anyStateAnimator.TryPlayAnimation("CastIdle");
            }
            else
            {
                anyStateAnimator.TryPlayAnimation("Stand");
            }
        }
        */

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
        if(isCasting|| isReeling) return; //exit if iscasting is already true or currently reeling
        isCasting = true; //currently running casting animation
        castFinished = false;
        canMove = false; //lock movement
        anyStateAnimator.TryPlayAnimation("Casting");
    }
    public void StopCasting()
    {
        Debug.Log("StopCasting called by animation event");
        isCasting = false; 
        castFinished = true;//cast animation just ended
        canMove = true;
        anyStateAnimator.TryPlayAnimation("CastIdle");
    }

    private void StartReeling()
    {
        Debug.Log("start reeling is called");
        if (!castFinished || isCasting) return; //user can only reel when the cast is finished
        isReeling = true;
        canMove = false;
        anyStateAnimator.TryPlayAnimation("Reeling");
    }
    private void StopReeling()
    {
        Debug.Log("stop reeling called");
        isReeling = false;
        canMove = true;
        

        if (castFinished)
            anyStateAnimator.TryPlayAnimation("CastIdle");
        else
            anyStateAnimator.TryPlayAnimation("Stand");
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
