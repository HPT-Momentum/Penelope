using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class PlayerMovement : NetworkBehaviour
{
    [SerializeField]
    private float movementSpeed = 1f;

    [SerializeField]
    private NetworkVariable<Vector3> playerMovement = new NetworkVariable<Vector3>();

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    // client cache position
    private Vector3 oldPlayerMovement = Vector3.zero;
    
    public float gravityForce = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (IsClient && IsOwner)
        {
            CalculatePlayerMovement();
        }

        if (playerMovement.Value != Vector3.zero)
        {
            controller.Move(playerMovement.Value);
        }
    }
    
    public void CalculatePlayerMovement() 
    {
        Vector3 verticalDirection = transform.TransformDirection(Vector3.forward);
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 horizontalDirection = transform.TransformDirection(Vector3.right);
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 playerHorizontalMovement = (verticalDirection * verticalInput + horizontalDirection * horizontalInput) * movementSpeed;
        
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = 0f;
        }
        
        // calculate the vertical movement
        velocity.y += gravityForce;
        Vector3 playerVerticalMovement = velocity;

        var newPlayerMovement = playerHorizontalMovement + playerVerticalMovement;
        
        if (oldPlayerMovement != newPlayerMovement)
        {
            oldPlayerMovement = newPlayerMovement;
            SubmitMovementToServerRpc(newPlayerMovement);
        }
    }

    [ServerRpc]
    public void SubmitMovementToServerRpc(Vector3 newPlayerMovement)
    {
        playerMovement.Value = newPlayerMovement;
    }
}
