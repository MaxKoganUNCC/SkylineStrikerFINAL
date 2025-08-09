using UnityEngine;

public class EnergyManager : MonoBehaviour
{

    public float value = 0.1f;
    public float descentSpeed = 1f;
    public float followSpeed = 5f;
    public float attractRange = 3f;

    private Transform player;
    private GameController gameController;
    private bool isFollowing = false;
    private Transform myTransform;

    public AudioClip energySound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        myTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, -90 * Time.deltaTime);

        if (player == null)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attractRange)
        {
            isFollowing = true;
        }
        else
        {
            isFollowing = false;
        }

        if (isFollowing)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, followSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.back * descentSpeed * Time.deltaTime, Space.World);
        }

        // Destroy offscreen
        if (myTransform.position.z < -15)
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
                gameController.AddEnergy(value);
                SoundFXManager.instance.PlaySoundFXClip(energySound, transform, 1.0f);
            }

            Destroy(gameObject);
        }
    }
}
