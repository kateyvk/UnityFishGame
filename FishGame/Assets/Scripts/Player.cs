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

    private void Rotate(){
        if(!Mouse.current.rightButton.isPressed)
        {
            float mouseX = horizontalMouseInput *rotationSpeed*Time.deltaTime;
            transform.Rotate(Vector3.up*mouseX);

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
        if (isCasting) return;

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


        actions.ControlsMap.Cast.started += cxt => StartCasting();
        actions.ControlsMap.Cast.canceled += cxt => StopCasting();
    }

    void Start()
    {
        AnyStateAnimation stand = new AnyStateAnimation("Stand", "Jump", "Casting", "CastIdle");
        AnyStateAnimation walk = new AnyStateAnimation("Walk", "Jump", "Casting", "CastIdle");
        AnyStateAnimation jump = new AnyStateAnimation("Jump");

        AnyStateAnimation casting = new AnyStateAnimation("Casting","Jump");
        AnyStateAnimation castIdle = new AnyStateAnimation("CastIdle", "Jump", "Casting");
        anyStateAnimator.AddAnimation(stand, walk, jump,casting, castIdle);
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
