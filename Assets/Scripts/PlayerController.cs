using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.ParticleSystem;

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
    [Tooltip("The object that the player's bullets will spawn from.")]
    [SerializeField] GameObject shootPositionObj;
    [Tooltip("Particles that spawn when the player is hit.")]
    [SerializeField] GameObject hitParticles;
    [Tooltip("Particles that spawn when the player is healed.")]
    [SerializeField] GameObject healParticles;
    [Tooltip("Particles that spawn when the player is killed.")]
    [SerializeField] GameObject deathParticles;

    [Tooltip("The object that stores the scene's projectiles.")]
    [SerializeField] GameObject projectileContainer;
    [Tooltip("The object that stores the scene's particles.")]
    [SerializeField] GameObject particleContainer;

    [Tooltip("The player's stats and variables in a ScriptableObject.")]
    public PlayerStats stats;

    [Tooltip("The player's action map for movement.")]
    InputAction MovementAction;
    [Tooltip("The player's action map for shooting.")]
    InputAction ShootingAction;
    [Tooltip("The player's action map for the player's secondary.")]
    InputAction SecondaryAction;

    [Tooltip("The timer counting between shots.")]
    float shootTimer;
    [Tooltip("The boolean telling whether the player can shoot.")]
    bool canShoot = true, holdingFire = false;

    [Tooltip("The player's actual health. (Not to be confused with stats.hitpoints)")]
    protected int health;
    //public HealthBarScript healthBar; //SHINE ADDED FOR TESTING

    #endregion

    #region Unity Functions
    private void Awake()
    {
        health = stats.hitPoints;
        //healthBar.SetMaxHealth(health);  //SHINE ADDED FOR TESTING

        MovementAction  = playerInput.actions.FindAction("Move");
        ShootingAction  = playerInput.actions.FindAction("Attack");
        SecondaryAction  = playerInput.actions.FindAction("Secondary");
        mainCamera      = Camera.main.gameObject;

        projectileContainer = GameObject.FindGameObjectWithTag("ProjectileContainer");
        particleContainer   = GameObject.FindGameObjectWithTag("ParticleContainer");
    }
    void Update()
    {
        Move();
        Rotate();
        LerpCamera();
        Shoot();
        /*
        if(playerInput.GetKeyDown(KeyCode.Space)) //SHINE ADDED FOR TESTING
        {
            TakeDamage(5);
        }
        */
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Projectile"))
        {
            Projectile projectile = other.GetComponent<Projectile>();
            if (projectile.stats.type == ProjectileType.Enemy || projectile.stats.type == ProjectileType.Melee)
            {
                GotHit(-projectile.stats.attackDamage);
                projectile.GotHit();
                Debug.Log(health);
            }
            else if (projectile.stats.type == ProjectileType.PlayerHeal)
            {
                GotHit(projectile.stats.attackDamage);
                projectile.GotHit();
                Debug.Log(health);
            }
        }
        else if (other.tag.Equals("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            GotHit(-enemy.stats.enemyProjectile.stats.attackDamage / 3);
            enemy.GotHit(-stats.playerProjectile.stats.attackDamage / 3);
        }
        else if (other.tag.Equals("Pickup"))
        {
            Pickup pickup = other.GetComponent<Pickup>();
            switch (pickup.type)
            {
                case PickupType.Health:
                    GotHit(pickup.health);
                    break;
                case PickupType.Ammo: break;
                case PickupType.GunChange:
                    stats = pickup.pickupStats;
                    break;
            }
            pickup.PickedUp();
        }
    }
    #endregion

    #region Other Functions
    void Rotate()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hitInfo))
        {
            Vector3 direction = (hitInfo.point - transform.position).normalized;
            direction.y = 0;
            rb.rotation = Quaternion.LookRotation(direction);
        }
    }
    void Move()
    {
        if (MovementAction.ReadValue<Vector2>().normalized == Vector2.zero)
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, stats.dampener);
            return;
        }
        Vector3 movement    = MovementAction.ReadValue<Vector2>().normalized;
        movement            = new(movement.x, 0, movement.y);
        rb.linearVelocity   = Vector3.Lerp(rb.linearVelocity, movement * stats.speed, stats.smoothener);
    }
    void LerpCamera()
    {
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position,
            rb.position + cameraOffset, stats.cameraDampener);
    }
    void Shoot()
    {
        canShoot = shootTimer <= 0f;
        shootTimer -= Time.deltaTime;

        if (ShootingAction.ReadValue<float>() == 0)
        {
            holdingFire = false;
            return;
        }

        if (stats.gunType == GunType.Pistol && !holdingFire)
        {
            shootTimer = 1f / stats.shotsPerSecond;
            canShoot = true;
            holdingFire = true;
        }

        if (canShoot)
        {
            shootTimer = 1f / stats.shotsPerSecond;
            canShoot = false;
            switch (stats.gunType)
            {
                case GunType.Homing:
                    for (int i = 0; i < Random.Range(1, stats.shotsPerFire); i++)
                        Instantiate(stats.playerProjectile, shootPositionObj.transform.position,
                            stats.playerProjectile.transform.rotation, projectileContainer.transform);
                    break;
                case GunType.Shotgun:
                    for (int i = 0; i < stats.shotsPerFire; i++)
                        Instantiate(stats.playerProjectile, shootPositionObj.transform.position,
                            stats.playerProjectile.transform.rotation, projectileContainer.transform);
                    break;
                default:
                    Instantiate(stats.playerProjectile, shootPositionObj.transform.position,
                        stats.playerProjectile.transform.rotation, projectileContainer.transform);
                    break;
            }
        }
    }
    public void GotHit(int value)
    {
        if (hitParticles && value < 0)
        {
            var particlesObj = Instantiate(hitParticles, transform.position, Quaternion.identity, particleContainer.transform);
            Destroy(particlesObj, 5f);
        }
        else if (healParticles && value < 0)
        {
            var particlesObj = Instantiate(healParticles, transform.position, Quaternion.identity, particleContainer.transform);
            Destroy(particlesObj, 5f);
        }
        health = Mathf.Clamp(health + value, 0, stats.hitPoints);
        if (health <= 0)
            Death();
    }
    public void Death()
    {
        if (deathParticles)
        {
            var particlesObj = Instantiate(deathParticles, transform.position, Quaternion.identity, particleContainer.transform);
            Destroy(particlesObj, 5f);
        }
        Destroy(gameObject);
    }
    #endregion

    /*void TakeDamage(int damage)  //SHINE ADDED FOR TESTING
    {
        health -= damage;
        healthBar.SetHealth(health);
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }
    */
}
