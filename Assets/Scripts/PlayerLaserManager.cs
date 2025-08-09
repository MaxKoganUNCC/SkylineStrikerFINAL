using UnityEngine;

public class PlayerLaserManager : MonoBehaviour
{

    private Transform myTransform;
    public PlayerManager playerManager;
    public float projectileSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        myTransform.Translate(Vector3.up * projectileSpeed * Time.deltaTime);

        if (myTransform.position.z > 9)
        {
            if (playerManager != null)
            {
                playerManager.RemoveActiveLaser(gameObject);
            }

            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (playerManager != null)
            {
                playerManager.RemoveActiveLaser(gameObject);
            }
            Destroy(gameObject);
        }
    }
}
