using NUnit.Framework.Internal;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerActions actions;
    [SerializeField]
   
    //private Animator animator;
    private AnyStateAnimator anyStateAnimator;

    [SerializeField]
    private CharacterController characterController;
    
  
    #region INPUT
    private Vector2 moveInput;
    #endregion
   
    #region VALUE
    private float moveSpeed =2.0f;
    #endregion

    private Vector3 playerVelocity;
    private float gravityValue = -9.81F;
    [SerializeField]
    private float jumpHeight = 0.2F;    
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
        anyStateAnimator.TryPlayAnimation("Jump");
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

        actions.Controls.Jump.performed += cxt => Jump();
        actions.Controls.Move.performed += cxt => moveInput = cxt.ReadValue<Vector2>();
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
    }
}
