using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour, IOnPlatformable
{
    [Header("MovementsValue")]
    [SerializeField] private float baseJumpVal;
    [SerializeField] private float baseSpeedVal;
    [SerializeField] private float maxSpeedVal;
    [SerializeField] private float IncreaseAcceleration;

    public float BaseJumpVal
    {
        set { baseJumpVal = value; }
    }
    
    private Vector2 movementInput = Vector2.zero;
    private Vector3 movementVector = Vector3.zero;

    private Vector3 currentPlatformPos = Vector3.zero;
    private Vector3 distPlatform = Vector3.zero;

    private Rigidbody rb;
    private Animator animator;
    private PlayerObject player;

    private float turningVelocity = 5f;
    private float acceleration = 0f;

    public bool isGround = false;
    public bool isInteract = false;
    public bool isAttack = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        player = GetComponent<PlayerObject>();
    }

    private void Update()
    {
        GroundCasting();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void LateUpdate()
    {
        UpdatePlayerAnim();
    }

    private void Movement()
    {
        if(!isAttack && !player.isDamaged)
        {
            movementVector = (Vector3.forward * movementInput.y + Vector3.right * movementInput.x) * (baseSpeedVal + acceleration);
            rb.velocity = new Vector3(movementVector.x, rb.velocity.y, movementVector.z);

            if (movementVector != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(movementVector, Vector3.up);
                rb.MoveRotation(Quaternion.Slerp(rb.rotation, lookRotation, turningVelocity * Time.deltaTime));
                acceleration = Mathf.Min(maxSpeedVal, acceleration + (Time.deltaTime * IncreaseAcceleration));
            }
            else
            {
                acceleration = 0f;
            }
        }      
    }

    private void Jumping()
    {
        if(isGround && !isAttack && !player.isDamaged) rb.AddForce(Vector3.up * baseJumpVal, ForceMode.Impulse);
    }
    private IEnumerator Attacking()
    {
        if (!isAttack && player.DecreaseEnerge())
        {
            if(player.GunObject.activeSelf)
            {
                
                GameObject projectile = Instantiate(player.Projectile, player.GunFirePosition.transform.position, Quaternion.identity);
                projectile.GetComponent<Rigidbody>().AddForce(transform.forward * 30f, ForceMode.Impulse);
            }
            else
            {
                player.MobTargetToDamage();
            }
            
            isAttack = true;
            animator.SetTrigger("isAttack");
            yield return new WaitForSeconds(1f);
            isAttack = false;
        }
        
        
    }

    private void UpdatePlayerAnim()
    {
        animator.SetBool("isMove", movementVector != Vector3.zero ? true : false);
        animator.SetBool("isRunning", acceleration > maxSpeedVal * 0.3f);
        animator.SetBool("isJump", !isGround);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = (context.phase != InputActionPhase.Performed ? Vector2.zero : context.ReadValue<Vector2>());      
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started) Jumping();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        isInteract = context.phase == InputActionPhase.Performed;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (movementVector != Vector3.zero || !isGround) return;
        if(context.phase == InputActionPhase.Started) StartCoroutine(Attacking());
    }

    public void GroundCasting()
    {
        Vector3 boxSize = new Vector3(1f, 0.1f, 1f);
        Vector3 boxCenter = transform.position + Vector3.up * 0.1f;
        float groundDist = 0.1f;
        isGround = Physics.BoxCast(boxCenter, boxSize / 2, Vector3.down, out RaycastHit hitInfo, Quaternion.identity, groundDist, LayerMask.GetMask("Ground"));
        Funtions.DrawBoxCasting(boxSize, boxCenter);
    }

    public bool IsControll()
    {
        if(movementInput == Vector2.zero) return false;
        return true;
    }
    public Rigidbody GetRigidbody()
    {
        return rb;
    }
    public Vector3 GetMoveDir()
    {
        return movementVector;
    }
    public void PlatformPos(Vector3 pos)
    {
        if (!isGround) distPlatform = Vector3.zero;

        currentPlatformPos = pos;
        if(movementInput != Vector2.zero || distPlatform == Vector3.zero)
        {
            distPlatform = currentPlatformPos - transform.position;
        }
        
        transform.position = currentPlatformPos - distPlatform;       
    }
    
}
