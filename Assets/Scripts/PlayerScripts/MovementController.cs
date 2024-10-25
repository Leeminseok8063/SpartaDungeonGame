using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    [Header("MovementsValue")]
    [SerializeField] private float speedVal;   
    [SerializeField] private float speedAcceleration;   
    [SerializeField] private float maxAcceleration;   
    [SerializeField] private float jumpVal;
    
    private Vector2 movementInput = Vector2.zero;
    private Rigidbody rb;
    private Animator animator;

    private float turningVelocity = 3f;
    private float accel = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        Vector3 movementVector = (Vector3.forward * movementInput.y  +  Vector3.right * movementInput.x) * (speedVal + accel);
        rb.velocity = new Vector3(movementVector.x, rb.velocity.y, movementVector.z);

        if (movementInput != Vector2.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(movementVector, Vector3.up);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, lookRotation, turningVelocity * Time.deltaTime));
            accel = Mathf.Min(maxAcceleration, accel + (Time.deltaTime * speedAcceleration));
            animator.SetBool("isMove", true);
        }
        else
        {
            animator.SetBool("isMove", false);
            accel = 0f;
        }

        animator.SetBool("isRunning", accel > maxAcceleration * 0.5f);
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        movementInput = (context.phase != InputActionPhase.Performed ? Vector2.zero : context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //context.phase = InputActionPhase.Started
    }
}
