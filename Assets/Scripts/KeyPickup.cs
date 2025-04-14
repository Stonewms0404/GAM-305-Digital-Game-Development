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
        particleContainer = GameObject.FindGameObjectWithTag("ParticleContainer");
        int towerCount = GameObject.FindGameObjectsWithTag("Tower").Length;
        gameObject.SetActive(towerCount <= 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Equals("Player"))
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
