using NUnit.Framework.Internal;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerActions actions;
    [SerializeField]
   
    private Animator animator;

    [SerializeField]
    private CharacterController characterController;
    
    [SerializeField]
    #region INPUT

    private Vector2 moveInput;
    #endregion
    [SerializeField]
    #region VALUE

    private float moveSpeed =2.0f;
    #endregion

    
    
    private void Move()
    {
        Vector3 movement = transform.right * moveInput.x + transform.forward *moveInput.y;
        characterController.Move(movement*moveSpeed * Time.deltaTime);

        if (moveInput.x!=0||moveInput.y!=0){
            animator.TryPlayAnimation("Walk");
        }else{
            animator.TryPlayAnimation("Stand");
        }
    }

    private void Jump()
    {
        animator.TryPlayAnimation("Jump");
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
        animator.AddAnimation(stand, walk, jump);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
