using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{

    //Code reference https://www.youtube.com/watch?v=lclDl-NGUMg

    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Action Map Name")]
    [SerializeField] private String actionMapName = "PlayerControls";

    [Header("Action Name References")]
    [SerializeField] private String move = "Move";
    [SerializeField] private String look = "Look";
    [SerializeField] private String jump = "Jump";

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;

    public Vector2 MoveInput{get; private set;}
    public Vector2 LookInput {get; private set;}
    public bool JumpTriggered{get; private set;}

    public static PlayerInputHandler Instance {get; private set;}

    private void Awake()
    {
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        moveAction = playerControls.FindActionMap(actionMapName).FindAction(move);
        lookAction = playerControls.FindActionMap(actionMapName).FindAction(look);
        jumpAction = playerControls.FindActionMap(actionMapName).FindAction(jump);
        RegisterInputActions();

    }
    void RegisterInputActions(){
        moveAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        moveAction.canceled += context => MoveInput = Vector2.zero;

        lookAction.performed += context => LookInput = context.ReadValue<Vector2>();
        lookAction.canceled += context => LookInput = Vector2.zero;

        jumpAction.performed += context => JumpTriggered = true;
        jumpAction.canceled += context => JumpTriggered = false;
    }
    

    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
        jumpAction.Enable();
    }
    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        jumpAction.Disable();
    }

}
 