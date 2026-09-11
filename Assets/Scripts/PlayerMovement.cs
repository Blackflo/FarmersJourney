using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
public class MovementHandler : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float jumpSpeed = 6f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    private InputAction jumpAction;
    private bool jumpRequested;

    private Rigidbody rb;
    private InputAction moveAction;

    private Vector2 moveInput;
    private Vector2 lastMoveDirection = Vector2.down;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        moveAction = GetComponent<PlayerInput>().actions["Move"];
        jumpAction = GetComponent<PlayerInput>().actions["Jump"];
    }

    private void Update()
    {
        // Verhindert schnellere Bewegung in diagonaler Richtung.
        moveInput = Vector2.ClampMagnitude(
            moveAction.ReadValue<Vector2>(), 1f
        );

        UpdateAnimation();
        // Tastendruck bis zum nächsten Physikschritt merken.
        if (jumpAction.WasPressedThisFrame())
        {
            jumpRequested = true;
        }
    }

    private void FixedUpdate()
{
    bool isGrounded = Physics.CheckSphere(
        groundCheck.position,
        groundCheckRadius,
        groundLayer,
        QueryTriggerInteraction.Ignore
    );

    Vector3 velocity = rb.linearVelocity;

    velocity.x = moveInput.x * moveSpeed;
    velocity.z = moveInput.y * moveSpeed;

    // Nur am Boden und nicht während des Aufsteigens springen.
    if (jumpRequested && isGrounded && velocity.y <= 0.1f)
    {
        velocity.y = jumpSpeed;
    }

    rb.linearVelocity = velocity;
    jumpRequested = false;
}

    private void UpdateAnimation()
    {
        animator.SetFloat("MoveX", moveInput.x);
        animator.SetFloat("MoveY", moveInput.y);

        if (moveInput.sqrMagnitude > 0.01f)
        {
            lastMoveDirection = moveInput;
            spriteRenderer.flipX = false;

            Vector3 scale = spriteRenderer.transform.localScale;

            if (moveInput.x > 0.01f)
                scale.x = -Mathf.Abs(scale.x);
            else if (moveInput.x < -0.01f)
                scale.x = Mathf.Abs(scale.x);

            spriteRenderer.transform.localScale = scale;
        }

        animator.SetFloat("LastMoveX", lastMoveDirection.x);
        animator.SetFloat("LastMoveY", lastMoveDirection.y);
        animator.SetFloat("Speed", moveInput.sqrMagnitude);
    }
}