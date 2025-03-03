using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 5f; 
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravityMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private float dashSpeed = 15f;  
    [SerializeField] private float dashDuration = 0.2f; 
    [SerializeField] private float dashCooldown = 1f; 

    private Rigidbody playerRB;
    private bool isGrounded = true;
    private int jumpCount = 0;
    private bool isDashing = false;
    private float dashTime = 0f;
    private float lastDashTime = -Mathf.Infinity;

    void Start()
    {
        playerRB = GetComponent<Rigidbody>();

        
        InputManager inputManager = FindAnyObjectByType<InputManager>();

        if (inputManager != null)
        {
            inputManager.OnSpacePressed.AddListener(Jump);
        }
    }

    void Update()
    {
        MovePlayer();
        ApplyBetterGravity();

        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= lastDashTime + dashCooldown && !isDashing)
        {
            StartDash();
        }

        // Handle Dash Timer
        if (isDashing)
        {
            dashTime -= Time.deltaTime;
            if (dashTime <= 0)
            {
                StopDash();
            }
        }

    }
    private void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal"); 
        float vertical = Input.GetAxis("Vertical");

        
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

       
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

      
        Vector3 moveDirection = (camForward * vertical + camRight * horizontal).normalized;

       
      

        if (moveDirection.magnitude > 0.1f)
        {
            Vector3 newVelocity = new Vector3(moveDirection.x * moveSpeed, playerRB.linearVelocity.y, moveDirection.z * moveSpeed);
            playerRB.linearVelocity = newVelocity;
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
        else
        {
            playerRB.linearVelocity = new Vector3(0, playerRB.linearVelocity.y, 0);
        }
    }

    
    private void Jump()
    {
        if (isGrounded)
        {
            playerRB.linearVelocity = new Vector3(playerRB.linearVelocity.x, jumpForce, playerRB.linearVelocity.z);
            isGrounded = false;
            jumpCount = 1;
        }
        else if (jumpCount == 1)
        {
            playerRB.linearVelocity = new Vector3(playerRB.linearVelocity.x, jumpForce, playerRB.linearVelocity.z);
            jumpCount = 2; 
        }
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = 0;
        }
    }

    private void ApplyBetterGravity()
    {
        if (!isGrounded)
        {
            if (playerRB.linearVelocity.y < 0) 
            {
                playerRB.linearVelocity += Vector3.up * Physics.gravity.y * (gravityMultiplier - 1) * Time.deltaTime;
            }
            else if (playerRB.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
            {
                playerRB.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }
    }

    private void StartDash()
    {
        if (Time.time >= lastDashTime + dashCooldown && !isDashing)
        {
            isDashing = true;
            dashTime = dashDuration;  
            lastDashTime = Time.time; 

          
            Vector3 dashDirection = cameraTransform.forward * Input.GetAxis("Vertical") + cameraTransform.right * Input.GetAxis("Horizontal");
            dashDirection.y = 0;  
            dashDirection.Normalize();

            playerRB.linearVelocity = new Vector3(dashDirection.x * dashSpeed, playerRB.linearVelocity.y, dashDirection.z * dashSpeed);
        }
    }

    private void StopDash()
    {
        isDashing = false;
        playerRB.linearVelocity = new Vector3(playerRB.linearVelocity.x, playerRB.linearVelocity.y, playerRB.linearVelocity.z);
    }
}