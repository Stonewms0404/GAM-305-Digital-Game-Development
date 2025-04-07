using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public GameObject gateToOpen;
    [Tooltip("Particles that spawn when the key is picked up.")]
    public GameObject particles;
    [Tooltip("The object that stores the scene's particles.")]
    [SerializeField] GameObject particleContainer;

    private void Start()
    {
        particles = GameObject.FindGameObjectWithTag("ParticleContainer");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (particles)
            {
                var particlesObj = Instantiate(particles, transform.position, Quaternion.identity, particleContainer.transform);
                Destroy(particlesObj, 5f);
            }
            Destroy(gateToOpen);
            Destroy(gameObject);
        }
    }
}
