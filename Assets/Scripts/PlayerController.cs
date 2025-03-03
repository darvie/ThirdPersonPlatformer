using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 5f; 
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravityMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;
    [SerializeField] private Transform cameraTransform;

    private Rigidbody playerRB;
    private bool isGrounded = true;

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
        }
    }

    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void ApplyBetterGravity()
    {
        if (!isGrounded)
        {
            if (playerRB.linearVelocity.y < 0) // Falling down
            {
                playerRB.linearVelocity += Vector3.up * Physics.gravity.y * (gravityMultiplier - 1) * Time.deltaTime;
            }
            else if (playerRB.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space)) // If player lets go of jump early
            {
                playerRB.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }
    }
}