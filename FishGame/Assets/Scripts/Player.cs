using NUnit.Framework.Internal;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerActions actions;
    [SerializeField]
   
    private Animator animator;
    [SerializeField]
    private CharacterController characterController;
    
    private void Stand(){
        animator.SetBool("Stand", true);
        //animator.SetBool("Fishing", false);
    }

    

    [SerializeField]
    #region INPUT

    private Vector2 moveInput;
    #endregion
    [SerializeField]
    #region VALUE

    private float moveSpeed =2.0f;
    #endregion

    //private void Jump(){

    //}
    
    private void Move()
    {
        Vector3 movement = transform.right * moveInput.x + transform.forward *moveInput.y;
        characterController.Move(movement*moveSpeed * Time.deltaTime);

        //if (moveInput.x!=0||moveInput.y!=0){
        //    animator.TryPlayAnimation("Walk");
        //}
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
        //actions.Controls.Jump.performed += cxt=> Jump();
        actions.Controls.Move.performed += cxt=> moveInput = cxt.ReadValue<Vector2>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
