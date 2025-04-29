using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovt : MonoBehaviour
{
   private PlayerActions actions;

    [SerializeField]
    private CharacterController characterController;
    
  
    #region INPUT
    private Vector2 moveInput;
    private float horizontalMouseInput;
    #endregion

    #region VALUE
    [SerializeField]
    private float moveSpeed = 2.0F;
    [SerializeField]
    private float rotationSpeed = 80.0F;
    [SerializeField]
    private float jumpHeight = 0.2F; 
    #endregion

    private Vector3 playerVelocity;
    private float gravityValue = -9.81F;

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
        Vector3 movement = transform.right * moveInput.x + transform.forward *moveInput.y;
        characterController.Move(movement*moveSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        if(characterController.isGrounded)
        {
            //Applying physics 
            playerVelocity.y += Mathf.Sqrt(jumpHeight *-3.0f*gravityValue);
        }
        
        
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
        actions.ControlsMap.Move.canceled += cxt => moveInput = Vector2.zero;
        actions.ControlsMap.MouseMovement.performed += cxt => horizontalMouseInput = cxt.ReadValue<float>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotate();
        Gravity();
        
    }
}
