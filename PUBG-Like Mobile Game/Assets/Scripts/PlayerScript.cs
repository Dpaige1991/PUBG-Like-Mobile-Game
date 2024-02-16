using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Health Things")]
    private float playerHealth = 1000f;
    private float presentHealth;
    public HealthBar healthBar;

    [Header("Player Movement")]
    public float playerSpeed = 1.9f;
    public float currentPlayerSpeed = 0f;
    public float playerSprint = 3f;
    public float currentPlayerSprint = 0f;

    [Header("Player Camera")]
    public Transform playerCamera;

    [Header("Player Animator and Gravity")]
    public CharacterController cc;
    public float gravity = -9.81f;
    public Animator animator;

    [Header("Player Jumping & Velocity")]
    public float jumpRange = 1f;
    public float turnCalmTime = 0.1f;
    float turnCalmVelocity;
    Vector3 velocity;
    public Transform surfaceCheck;
    bool onSurface;
    public float surfaceDistance = 0.4f;
    public LayerMask surfaceMask;

    public bool mobileInputs;
    public FixedJoystick joystick;
    public FixedJoystick sprintJoystick;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        presentHealth = playerHealth;
        healthBar.GiveFullHealth(playerHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentPlayerSpeed > 0)
        {
            sprintJoystick = null;
        }
        else
        {
            FixedJoystick sprintJS = GameObject.Find("PlayerSprintJoystick").GetComponent<FixedJoystick>();
            sprintJoystick = sprintJS;
        }

        onSurface = Physics.CheckSphere(surfaceCheck.position, surfaceDistance, surfaceMask);

        if(onSurface && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);

        playerMove();

        Jump();

        Sprint();
    }

    void playerMove()
    {
        if (mobileInputs == true)
        {
            float horizontal_axis = joystick.Horizontal;
            float vertical_axis = joystick.Vertical;

            Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;

            if (direction.magnitude >= 0.1f)
            {
                animator.SetBool("Walk", true);
                animator.SetBool("Running", false);
                animator.SetBool("Idle", false);
                animator.SetTrigger("Jump");
                animator.SetBool("AimWalk", false);
                animator.SetBool("IdleAim", false);

                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                cc.Move(moveDirection.normalized * playerSprint * Time.deltaTime);
                currentPlayerSprint = playerSprint;
            }
            else
            {
                animator.SetBool("Idle", true);
                animator.SetTrigger("Jump");
                animator.SetBool("Walk", false);
                animator.SetBool("Running", false);
                animator.SetBool("AimWalk", false);
                currentPlayerSpeed = 0f;
            }
        }
        else
        {
            float horizontal_axis = Input.GetAxisRaw("Horizontal");
            float vertical_axis = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis);

            if (direction.magnitude >= 0.1f)
            {
                animator.SetBool("Walk", true);
                animator.SetBool("Running", false);
                animator.SetBool("Idle", false);
                animator.SetTrigger("Jump");
                animator.SetBool("AimWalk", false);
                animator.SetBool("IdleAim", false);

                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                cc.Move(moveDirection.normalized * playerSprint * Time.deltaTime);
                currentPlayerSprint = playerSprint;
            }
            else
            {
                animator.SetBool("Idle", true);
                animator.SetTrigger("Jump");
                animator.SetBool("Walk", false);
                animator.SetBool("Running", false);
                animator.SetBool("AimWalk", false);
                currentPlayerSpeed = 0f;
            }
        }
    }

    void Jump()
    {
        if (mobileInputs == true)
        {
            if (CrossPlatformInputManager.GetButtonDown("Jump") && onSurface)
            {
                animator.SetBool("Walk", false);
                animator.SetTrigger("Jump");
                velocity.y = Mathf.Sqrt(jumpRange * -2 * gravity);
            }
            else
            {
                animator.ResetTrigger("Jump");
            }
        }
        else
        {
            if (Input.GetButtonDown("Jump") && onSurface)
            {
                animator.SetBool("Walk", false);
                animator.SetTrigger("Jump");
                velocity.y = Mathf.Sqrt(jumpRange * -2 * gravity);
            }
            else
            {
                animator.ResetTrigger("Jump");
            }
        }
    }

    void Sprint()
    {
        if (mobileInputs == true)
        {
            float horizontal_axis = sprintJoystick.Horizontal;
            float vertical_axis = sprintJoystick.Vertical;

            Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;

            if (direction.magnitude >= 0.1f)
            {
                animator.SetBool("Walk", true);
                animator.SetBool("Running", false);
                animator.SetBool("Idle", false);
                animator.SetBool("IdleAim", false);

                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                cc.Move(moveDirection.normalized * playerSprint * Time.deltaTime);
                currentPlayerSprint = playerSprint;
            }
            else
            {
                animator.SetBool("Idle", true);
                animator.SetBool("Walk", false);
                currentPlayerSprint = 0f;
            }
        }
        else
        {
            float horizontal_axis = Input.GetAxisRaw("Horizontal");
            float vertical_axis = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis);

            if (direction.magnitude >= 0.1f)
            {             
                animator.SetBool("Running", true);
                animator.SetBool("Idle", false);
                animator.SetBool("Walk", false);
                animator.SetBool("IdleAim", false);

                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                cc.Move(moveDirection.normalized * playerSprint * Time.deltaTime);
                currentPlayerSprint = playerSprint;
            }
            else
            {
                animator.SetBool("Idle", false);
                animator.SetBool("Walk", false);
                currentPlayerSpeed = 0f;
            }
        }
    }

    public void playerHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;
        healthBar.SetHealth(presentHealth);

        if(presentHealth <= 0)
        {
            PlayerDie();
        }
    }

    private void PlayerDie()
    {
        Cursor.lockState = CursorLockMode.None;
        Object.Destroy(gameObject);
    }
}
