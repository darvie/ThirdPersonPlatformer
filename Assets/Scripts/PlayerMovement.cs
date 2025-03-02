using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour { 
    [SerializeField] private Boolean Grounded = true;
    [SerializeField] private float jumpForce = 2.0f;
    [SerializeField] private Rigidbody capsuleRigidbody;

    public UnityEvent<Vector2> OnMove = new UnityEvent<Vector2>();

    void Update()
    {
        Vector3 inputVector = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            inputVector += Vector3.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector += Vector3.down;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector += Vector3.right;
        }

        if (Input.GetKeyDown(KeyCode.Space) && Grounded)
        {
            capsuleRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Grounded = false;
        }
        OnMove?.Invoke(inputVector);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Grounded = true;
        }
    }

}