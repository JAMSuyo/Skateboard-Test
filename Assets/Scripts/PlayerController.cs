using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float jogSpeed = 6f;
    public float jumpForce = 6f;
    public float skateSpeed = 6f;
    private float currentSpeed;

    private Rigidbody rb;
    private Animator animator;
    private bool isJumping = false;
    private bool isSkateboarding = false;
    private bool isPushing = false;

    private Skateboard activeSkateboard;  // Active skateboard (one selected from GameManager)

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        currentSpeed = walkSpeed;

        // Get the currently selected skateboard from the GameManager and set it as the active one
        if (GameManager.instance != null && GameManager.instance.skateboards.Length > 0)
        {
            activeSkateboard = GameManager.instance.skateboards[GameManager.instance.selectedSkateboardIndex];
            activeSkateboard.gameObject.SetActive(false); // Deactivate initially
        }
    }

    void Update()
    {
        MovePlayer();
        HandleJump();
        HandleSkateboarding();
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        if (isSkateboarding && activeSkateboard != null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed = activeSkateboard.GetBoostedSpeed();
                isPushing = true;
            }
            else
            {
                currentSpeed = activeSkateboard.GetSpeed();
                isPushing = false;
            }
        }
        else
        {
            currentSpeed = Input.GetKey(KeyCode.LeftShift) ? jogSpeed : walkSpeed;
        }

        Vector3 movement = new Vector3(moveX, 0, moveZ).normalized * currentSpeed;

        if (movement.magnitude > 0)
        {
            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
            transform.forward = movement;
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }

        animator.SetBool("isIdle", movement.magnitude == 0 && !isSkateboarding);
        animator.SetBool("isWalking", movement.magnitude > 0 && !Input.GetKey(KeyCode.LeftShift) && !isSkateboarding);
        animator.SetBool("isJogging", movement.magnitude > 0 && Input.GetKey(KeyCode.LeftShift) && !isSkateboarding);
        animator.SetBool("isSkateboarding", isSkateboarding && !isJumping);
        animator.SetBool("isPushing", isPushing && !isJumping);
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            float jumpHeight = isSkateboarding && activeSkateboard != null ? activeSkateboard.GetJumpHeight() : jumpForce;
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            isJumping = true;

            animator.SetBool("isJumping", true);
            animator.SetBool("isSkateboarding", isSkateboarding);
            animator.SetBool("isPushing", false);
        }
    }

    void HandleSkateboarding()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isSkateboarding = !isSkateboarding;

            if (isSkateboarding && activeSkateboard != null)
            {
                activeSkateboard.gameObject.SetActive(true); // Activate the selected skateboard
            }
            else
            {
                if (activeSkateboard != null)
                {
                    activeSkateboard.gameObject.SetActive(false); // Deactivate the skateboard
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            animator.SetBool("isJumping", false);
            if (isSkateboarding)
            {
                animator.SetBool("isSkateboarding", true);
            }
        }
    }
}
