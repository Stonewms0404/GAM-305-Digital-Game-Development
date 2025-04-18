using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    #region Variables
    [Tooltip("The projectile's Rigidbody component for movement.")]
    public Rigidbody rb;
    [Tooltip("Particles that spawn when the projectile gets hit.")]
    public GameObject particles;
    [Tooltip("The object that stores the scene's particles.")]
    [SerializeField] GameObject particleContainer;

    [Tooltip("The projectile's stats and variables in a ScriptableObject.")]
    public ProjectileStats stats;
    protected GameObject? target;
    protected Vector3 point;
    protected float speed;
    protected int pierces;
    Transform parentTransform;
    #endregion

    protected abstract (bool success, Vector3 point) GetPosition();
    protected abstract void GetTarget();
    protected abstract void AimTowardsPosition(Vector3 position);

    #region Unity Functions
    protected void Start()
    {
        particleContainer = GameObject.FindGameObjectWithTag("ParticleContainer");
        pierces = stats.pierce;
        if (stats.type == ProjectileType.Melee)
            return;

        speed = stats.speed;
        var (success, pos) = GetPosition();
        point = pos;
        point.y = transform.position.y;
        if (success)
        {
            AimTowardsPosition(point);
            if (stats.homing)
            {
                Vector3 direction = (point - rb.position).normalized;
                rb.linearVelocity = direction * speed * 2;
            }
        }
        else Destroy(gameObject);
    }
    private void Update()
    {
        if (stats.type == ProjectileType.Melee)
            return;
        if (stats.homing)
        {
            if (target)
            {
                Vector3 direction = (target.transform.position - rb.position).normalized;
                rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
                rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, direction * speed, Time.deltaTime);
            }
            else
                GetTarget();
        }     
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, stats.dampener);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Projectile"))
        {
            Projectile projectile = other.GetComponent<Projectile>();
            switch (projectile.stats.type)
            {
                case ProjectileType.Player:
                case ProjectileType.PlayerHeal:
                    if (stats.type == ProjectileType.Enemy)
                    {
                        projectile.GotHit();
                        GotHit();
                    }
                    break;
                case ProjectileType.Enemy:
                case ProjectileType.EnemyHeal:
                    if (stats.type == ProjectileType.Player)
                    {
                        projectile.GotHit();
                        GotHit();
                    }
                    break;
            }
        }
        else if (other.tag.Equals("Tower"))
        {
            if (stats.type == ProjectileType.Player)
            {
                Tower tower = other.GetComponent<Tower>();
                tower.GotHit(-stats.attackDamage);
                GotHit();
            }
        }
        else if (other.tag.Equals("Enviornment"))
        {
            if (stats.type == ProjectileType.Melee)
                return;
            for (int i = pierces; i > 0; i--)
                GotHit();
        }
    }
    public void GotHit()
    {
        pierces -= 1;
        if (pierces <= 0)
            Destroy(gameObject);
        if (particles)
        {
            var particlesObj = Instantiate(particles, transform.position, Quaternion.identity, particleContainer.transform);
            Destroy(particlesObj, 5f);
        }
    }
    #endregion
}
