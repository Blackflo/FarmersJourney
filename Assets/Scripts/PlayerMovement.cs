using UnityEngine;
using UnityEngine.InputSystem;



public class MovementHandler : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction moveAction;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Animator animator;

  
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
    }

    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        transform.position += new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed * Time.deltaTime;

          animator.SetFloat("MoveX", moveInput.x);
          animator.SetFloat("MoveY", moveInput.y);


        animator.SetFloat("Speed", moveInput.sqrMagnitude);


}
}
