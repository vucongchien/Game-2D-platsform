
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections),typeof(Damgeable))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float airWalkSpeed = 3f;
    public float jumpImpulse=10f;


    private float jumpTimeCounter;
    public float jumpTime;

    Vector2 moveInput;
    TouchingDirections touchingDirections;
    Damgeable damgeable;
    Rigidbody2D rb;
    Animator animator;

    public bool canAction=true;
    public float CurrentMoveSpeed
    {
        get 
        {
            
            if(IsMoving&& !touchingDirections.IsOnWall)
            {
                if (touchingDirections.IsGrounded)
                {

                    if (IsRunning)
                    {
                        return runSpeed;
                    }
                    else
                    {
                        return walkSpeed;
                    }
                }
                else
                {
                    //air move
                    return airWalkSpeed;
                }
            }
            else
            {
                //idle
                return walkSpeed;
            }
        }
    }

    [SerializeField]
    private bool _isMoving = false;
    public bool IsMoving
    {
        get => _isMoving;
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }

    [SerializeField]
    private bool _isRunning = false;
    public bool IsRunning
    {
        get => _isRunning;
        private set
        {
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }

    private bool _isFacingRight = true; 
    public bool IsFacingRight //giúp nhân vật đổi chiều 
    {
        get
        {
            return _isFacingRight;
        }
        private set
        {
            if(_isFacingRight!= value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }
    public bool IsAlive
    {
        get
        {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections=GetComponent<TouchingDirections>();
        damgeable=GetComponent<Damgeable>();
    }

    private void FixedUpdate()
    {
        if (IsAlive&&!damgeable.LockVelocity)
        {
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
        }
        animator.SetFloat(AnimationStrings.yVelocity,rb.velocity.y);
        if (damgeable.IsAlive == false)
        {
            SceneManager.LoadSceneAsync(2);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!canAction)
            return;
        moveInput = context.ReadValue<Vector2>();

        if (IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;
            SetFacingDirection(moveInput);
        }
        else
        {
            IsMoving = false;
        }
    }

    private void SetFacingDirection(Vector2 moveInput)
    {

        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if(moveInput.x<0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (!canAction)
            return;
        if (context.started)
        {
            IsRunning = true;
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //check
        if (!canAction)
            return;
        if (context.started && touchingDirections.IsGrounded &&IsAlive)
        {
            airWalkSpeed = CurrentMoveSpeed;
            Jump(jumpImpulse);
           
        }
    }

    private void Jump(float jumpImpulse)
    {
        animator.SetTrigger(AnimationStrings.jumpTrigger);  
        rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!canAction)
            return;
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }
    public void OnRangedAttack(InputAction.CallbackContext context)
    {
        if (!canAction)
            return;
        if (context.performed)
        {
            animator.SetTrigger(AnimationStrings.rangedAttackTrigger);
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity=new Vector2(knockback.x,rb.velocity.y+knockback.y);
    }
}
