using UnityEngine;

public class Ranger : MonoBehaviour, IDamageable
{

    private Transform myTransform;
    public float speed;
    public float hp;
    public float enemyEnergyValue;
    public GameObject Energy;
    public float score;

    public GameObject EnemyLaser;
    public float fireRate = 1.5f;
    private float nextFireTime;
    public float blasterOffset = 0.5f;
    private bool useLeftBlaster = true;

    public AudioClip enemyShootSound;
    public AudioClip enemyDeathSound;
    public AudioClip enemyHurtSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myTransform = transform;
        nextFireTime = Time.time + fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        myTransform.Translate(Vector3.right * speed * Time.deltaTime);

        if (Time.time >= nextFireTime)
        {
            FireLaser();
            nextFireTime = Time.time + fireRate;
        }
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        SoundFXManager.instance.PlaySoundFXClip(enemyHurtSound, transform, 1.0f);
        if (hp <= 0)
        {
            SoundFXManager.instance.PlaySoundFXClip(enemyDeathSound, transform, 1.0f);
            Die();
        }
    }

    private void Die()
    {
        // Instantiate energy
        GameObject energy = Instantiate(Energy, transform.position, Quaternion.identity);
        EnergyManager energyScript = energy.GetComponent<EnergyManager>();
        energyScript.value = enemyEnergyValue;

        // Destroy the enemy
        Destroy(gameObject);
    }

    private void FireLaser()
    {
        Vector3 spawnPosition = new Vector3(myTransform.position.x, myTransform.position.y, myTransform.position.z - 1);
        Quaternion rotation = Quaternion.Euler(90f, 0f, 0f);

        // Apply offset based on which blaster is being used
        if (useLeftBlaster)
        {
            spawnPosition.x -= blasterOffset;
        }
        else
        {
            spawnPosition.x += blasterOffset;
        }

        spawnPosition.z += 1.0f;

        // Instantiate the laser
        GameObject laser = Instantiate(EnemyLaser, spawnPosition, rotation);
        SoundFXManager.instance.PlaySoundFXClip(enemyShootSound, transform, 1.0f);

        useLeftBlaster = !useLeftBlaster;
    }
}
