using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public GameObject gateToOpen;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Destroy(gateToOpen);
            Destroy(gameObject);
        }
    }
}
