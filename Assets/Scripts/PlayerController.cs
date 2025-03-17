using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Variables
    [Tooltip("The camera's positional offset.")]
    [SerializeField] Vector3 cameraOffset;
    [Tooltip("The main camera attached to the player.")]
    [SerializeField] GameObject mainCamera;
    [Tooltip("The player's Rigidbody component for movement.")]
    [SerializeField] Rigidbody rb;
    [Tooltip("The player's action map for movement and shooting.")]
    [SerializeField] PlayerInput playerInput;

    [Tooltip("The object that stores the scene's projectiles.")]
    [SerializeField] GameObject projectileContainer;
    [Tooltip("The object that stores the scene's particles.")]
    [SerializeField] GameObject particleContainer;
    [Tooltip("The player's projectile prefab.")]
    [SerializeField] PlayerProjectile playerProjectile;


    [Tooltip("The player's stats and variables in a ScriptableObject.")]
    public PlayerStats stats;

    [Tooltip("The player's action map for movement.")]
    InputAction MovementAction;
    [Tooltip("The player's action map for shooting.")]
    InputAction ShootingAction;

    [Tooltip("The timer counting between shots.")]
    float shootTimer;
    [Tooltip("The boolean telling whether the player can shoot.")]
    bool canShoot = true;
    #endregion

    #region Unity Functions
    private void Awake()
    {
        MovementAction = playerInput.actions.FindAction("Move");
        ShootingAction = playerInput.actions.FindAction("Attack");
    }
    void Update()
    {
        Move();
        LerpCamera();
        Shoot();
    }
    #endregion

    #region Other Functions
    void Move()
    {
        if (MovementAction.ReadValue<Vector2>().normalized == Vector2.zero)
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, stats.dampener);
            return;
        }
        Vector3 movement    = MovementAction.ReadValue<Vector2>().normalized;
        movement            = new(movement.x, 0, movement.y);
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, movement * stats.speed, stats.smoothener);
    }
    void LerpCamera()
    {
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position,
            rb.position + cameraOffset, stats.cameraDampener);
    }
    void Shoot()
    {
        if (ShootingAction.ReadValue<float>() == 0)
        {
            shootTimer = 1f / stats.shotsPerSecond;
            canShoot = true;
            return;
        }

        if (canShoot)
        {
            shootTimer = 1f / stats.shotsPerSecond;
            canShoot = false;
            Instantiate(playerProjectile, transform.position,
                playerProjectile.transform.rotation, projectileContainer.transform);
        }

        canShoot = shootTimer <= 0f;
        shootTimer -= Time.deltaTime;
    }
    #endregion
}
