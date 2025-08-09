using UnityEngine;

public class EnemyLaserManager : MonoBehaviour
{

    private Transform myTransform;
    public PlayerManager playerManager;
    public float projectileSpeed;
    public AudioClip playerHurtSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        myTransform.Translate(Vector3.down * projectileSpeed * Time.deltaTime);

        if (myTransform.position.z < -18)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameController gameController = FindObjectOfType<GameController>();
            if (gameController != null)
            {
                SoundFXManager.instance.PlaySoundFXClip(playerHurtSound, transform, 1.0f);
                gameController.AddEnergy(-1);
            }
            Destroy(gameObject);
        }
    }
}
