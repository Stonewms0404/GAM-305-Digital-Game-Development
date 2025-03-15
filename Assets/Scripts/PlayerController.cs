using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Variables
    [Tooltip("The camera's rotational offset so the player can move up and down.")]
    [SerializeField] Vector2 cameraRotationOffser;
    [Tooltip("The player's Rigidbody component for movement.")]
    [SerializeField] Rigidbody rb;
    [Tooltip("The player's action map for movement and shooting.")]
    [SerializeField] PlayerInput playerInput;
    
    [Tooltip("The player's stats and variables in a ScriptableObject.")]
    public PlayerStats stats;

    [Tooltip("The player's action map for movement.")]
    InputAction MovementAction;
    [Tooltip("The player's action map for shooting.")]
    InputAction ShootingAction;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        MovementAction = playerInput.actions.FindAction("Move");
        ShootingAction = playerInput.actions.FindAction("Attack");
    }
    void Update()
    {
        Move();
        Shoot();
    }
    #endregion

    #region Action Functions
    void Move()
    {
        Vector3 movement = MovementAction.ReadValue<Vector2>().normalized;
        movement = new(movement.x, 0, movement.y);
        if (movement == Vector3.zero)
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, stats.dampener);
            return;
        }
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, movement * stats.speed, stats.smoothener);
    }
    void Shoot()
    {
        if (ShootingAction.ReadValue<float>() == 0) return;

    }
    #endregion

    #region Other Functions
    #endregion
}
