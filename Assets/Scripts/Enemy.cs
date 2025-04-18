using System.Net;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public abstract class Enemy : MonoBehaviour
{
    #region Variables
    [SerializeField] protected EnemyStats[] listedStats;
    [Tooltip("The enemy's stats and variables in a ScriptableObject.")]
    public EnemyStats stats;
    [Tooltip("The enemy's object for tracking.")]
    PlayerController? player;
    [Tooltip("The enemy's Rigidbody component for movement.")]
    public Rigidbody rb;
    [SerializeField] Pickup[] pickups;

    [Tooltip("The object that stores the scene's projectiles.")]
    protected GameObject projectileContainer;
    [Tooltip("The object that stores the scene's particles.")]
    protected GameObject particleContainer;
    [Tooltip("The object that stores the scene's pickups.")]
    protected GameObject pickupContainer;
    [Tooltip("Particles that spawn when the enemy is hit.")]
    [SerializeField] GameObject hitParticles;
    [Tooltip("Particles that spawn when the enemy is killed.")]
    [SerializeField] GameObject deathParticles;

    [Tooltip("The direction the enemy is facing saved between functions.")]
    protected Vector3 direction;
    
    [Tooltip("The timer counting between attacks.")]
    protected float attackTimer;
    [Tooltip("The boolean telling whether the enemy can attack.")]
    protected bool canAttack;
    [Tooltip("The timer counting between finding the player.")]
    float searchTimer;
    [Tooltip("The timer that starts when the enemy cannot see the player for it to despawn.")]
    float despawnTimer;
    [Tooltip("The boolean that checks if the despawn timer has been set or not.")]
    bool startedDespawn;

    [Tooltip("The enemy's actual health. (Not to be confused with stats.hitPoints)")]
    protected int health;
    #endregion

    #region Unity Functions
    private void Start()
    {
        stats = listedStats[Random.Range(0, listedStats.Length)];
        health = stats.hitPoints;

        projectileContainer = GameObject.FindGameObjectWithTag("ProjectileContainer");
        particleContainer   = GameObject.FindGameObjectWithTag("ParticleContainer");
        pickupContainer = GameObject.FindGameObjectWithTag("PickupContainer");
    }
    void Update()
    {
        if (!player)
        {
            FindPlayer();
            if (startedDespawn)
                DespawnTimer();
        }
        else
        {
            Vector3 offset = player.transform.position - rb.transform.position;
            offset.y = 0;
            direction = offset.normalized;
            float distance = offset.magnitude;
            if (distance <= stats.attackRange)
            {
                rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, stats.dampener * 2);
                Attack();
            }
            else
                MoveTowardsPlayer();
            rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Projectile"))
        {
            Projectile projectile = other.GetComponent<Projectile>();
            if (projectile.stats.type == ProjectileType.Player)
            {
                GotHit(-projectile.stats.attackDamage);
                Destroy(other.gameObject);
            }
            else if (projectile.stats.type == ProjectileType.EnemyHeal)
            {
                GotHit(projectile.stats.attackDamage);
                Destroy(other.gameObject);
            }
        }
        else if (other.tag.Equals("Player"))
        {
            player = other.GetComponent<PlayerController>();
            canAttack = true;
        }
    }
    #endregion

    #region Other Functions
    private void DespawnTimer()
    {
        despawnTimer -= Time.deltaTime;
        if (despawnTimer < 0)
            Death();
    }
    protected abstract void Attack();
    protected void MoveTowardsPlayer()
    {
        if (player)
        {
            rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
            direction.y = 0;
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, direction * stats.movementSpeed, stats.smoothener);
            return;
        }
        else
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, stats.dampener);
    }
    public void GotHit(int value)
    {
        if (hitParticles)
        {
            var particlesObj = Instantiate(hitParticles, transform.position, Quaternion.identity, particleContainer.transform);
            Destroy(particlesObj, 5f);
        }
        health += value;
        if (health < 0)
            Death();
    }
    protected void FindPlayer()
    {
        if (searchTimer <= 0)
        {
            searchTimer = 1f / stats.searchRate;
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj)
            {
                startedDespawn = false;
                despawnTimer = stats.despawnTimer;
                player = (playerObj.transform.position - rb.transform.position).magnitude < stats.sightRange ? playerObj.GetComponent<PlayerController>() : null;
            }
            else
                startedDespawn = true;
        }
        else
            searchTimer -= Time.deltaTime;
    }
    protected void Death()
    {
        if (deathParticles)
        {
            var particlesObj = Instantiate(deathParticles, transform.position, Quaternion.identity, particleContainer.transform);
            Destroy(particlesObj, 5f);
        }
        if (Random.Range(0f, 1f) < 0.2f && player)
        {
            PlayerController pController = player.GetComponent<PlayerController>();
            Pickup pickup;
            do
            {
                pickup = pickups[Random.Range(0, pickups.Length)];
            } while (pickup.pickupStats == pController.stats);
            Vector3 position = transform.position;
            position.y = player.transform.position.y;
            pickup = Instantiate(pickup, transform.position, Quaternion.identity, pickupContainer.transform);
            Destroy(pickup.gameObject, 5);
        }
        Destroy(gameObject);
    }
    #endregion
}
