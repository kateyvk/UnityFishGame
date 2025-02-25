using UnityEngine;

public class CharacterMovt : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created    
    
    private CharacterController characterController;
    public float Speed = 5f;
    void Start()
    {
        characterController = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));

        characterController.Move(move*Time.deltaTime*Speed);
    }
}
