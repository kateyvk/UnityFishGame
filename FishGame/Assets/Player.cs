using NUnit.Framework.Internal;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerActions actions;
    [SerializeField]

    private Animator animator;

    private CharacterController characterController;
    [SerializeField]
    #region INPUT

    private Vector2 moveInput;
    #endregion
    [SerializeField]
    #region VALUE

    private float moveSpeed =2.0f;
    #endregion

    private void Jump(){

    }
    
    private void Move()
    {
        Vector3 movement = transform.right * moveInput.x + transform.forward *moveInput.y;
        characterController.Move(movement*moveSpeed * Time.deltaTime);
    }


    private void OnEnable()
    {
        actions.Enable();
    }
    private void OnDisable()
    {
        actions.Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Awake()
    {
        actions = new PlayerAction();
        actions.PlayerControls.Move.performed += cxt => Move();
        ActionsHelper.PlayerControls.Move.performed += ContextMenu => moveInput = ContextMenu.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
