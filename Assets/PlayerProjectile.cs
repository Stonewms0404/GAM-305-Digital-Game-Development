using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    #region Variables
    [Tooltip("The projectile's Rigidbody component for movement.")]
    [SerializeField] Rigidbody rb;

    [Tooltip("The projectile's stats and variables in a ScriptableObject.")]
    public ProjectileStats stats;
    #endregion

    #region Unity Functions
    void Awake()
    {
        Vector3 bulletSpread = new(Random.Range(-stats.spread, stats.spread), 0, Random.Range(-stats.spread, stats.spread));
        var (success, position) = GetMousePosition();
        if (success)
        {
            Vector3 direction = (position - transform.position).normalized;
            direction.y = 0;
            transform.forward = direction + Vector3.up * Mathf.PI;
            rb.linearVelocity = direction * stats.speed + bulletSpread;
        }
        
        Destroy(gameObject, stats.lifeTimer);
    }
    private void Update()
    {
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, stats.dampener);
    }
    #endregion
    
    (bool success, Vector3 position) GetMousePosition()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hitInfo))
            return (true, hitInfo.point);
        else
            return (false, Vector3.zero);
    }
}
