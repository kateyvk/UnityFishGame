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
        playerVelocity.y += gravityValue *Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);

        if(characterController.isGrounded && playerVelocity.y <0)
        {
            playerVelocity.y =0;
        }
    }
    private void Move()
    {
        Vector3 movement = transform.right * moveInput.x + transform.forward *moveInput.y;
        characterController.Move(movement*moveSpeed * Time.deltaTime);

        if (moveInput.x!=0||moveInput.y!=0){
            anyStateAnimator.TryPlayAnimation("Walk");
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
    }

    void Start()
    {
        AnyStateAnimation stand = new AnyStateAnimation("Stand","Jump");
        AnyStateAnimation walk = new AnyStateAnimation("Walk","Jump");
        AnyStateAnimation jump = new AnyStateAnimation("Jump");
        anyStateAnimator.AddAnimation(stand, walk, jump);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Gravity();
        Rotate();
        
    }
}
