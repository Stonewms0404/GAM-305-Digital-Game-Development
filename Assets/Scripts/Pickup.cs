using UnityEngine;

public class Pickup : MonoBehaviour
{
    public PickupType type;
    public PlayerStats pickupStats;
    public int health;
    [Tooltip("Particles that spawn when the pickup is picked up.")]
    public GameObject particles;
    [Tooltip("The object that stores the scene's particles.")]
    [SerializeField] GameObject particleContainer;

    private void Start()
    {
        particleContainer = GameObject.FindGameObjectWithTag("ParticleContainer");
        if (type == PickupType.Health)
            health = Random.Range(5, 20);
    }
    public void PickedUp()
    {
        if (particles)
        {
            var particlesObj = Instantiate(particles, transform.position, Quaternion.identity, particleContainer.transform);
            Destroy(particlesObj, 5f);
        }
        Destroy(gameObject);
    }
}

public enum PickupType
{
    Health,
    Ammo,
    GunChange
}
