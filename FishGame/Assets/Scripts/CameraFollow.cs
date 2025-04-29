using UnityEngine;
//using UnityEngine.UIElements;
using System.Collections;
//using System.Collections.Generic;

public class CameraFollow : MonoBehaviour
{
    private PlayerActions actions;
    private float mouseX;
    private Vector3 offset;
    [SerializeField]
    private float rotationSpeed = 150.0f;
    [SerializeField]
    private Transform target;
    [SerializeField]
    [Range(0.01f,1.0f)]
    //private  Transform lookAtTarget;
    //[SerializeField]
    //private Transform followTarget;

    private float smoothFactor;
    private void LateUpdate()
    {
        Rotate(mouseX, rotationSpeed);
        transform.LookAt(target);
    }
    private void Rotate(float rotationAmount,float speed)
    {
        Quaternion camTargetAngle = Quaternion.AngleAxis(rotationAmount *speed*Time.deltaTime,Vector3.up);
        offset = camTargetAngle * offset;
        Vector3 newPos = target.position + offset;
        transform.position = Vector3.Slerp(transform.position, newPos, smoothFactor);
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
        actions.ControlsMap.MouseMovement.performed +=cxt => mouseX = cxt.ReadValue<float>();
        offset = transform.position - target.transform.position;
    }
}
