using UnityEngine;

public class Fodder : MonoBehaviour, IDamageable
{

    private Transform myTransform;
    public float speed;
    public float hp;
    public float enemyEnergyValue;
    public GameObject Energy;
    public float score;

    public AudioClip enemyDeathSound;
    public AudioClip enemyHurtSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        myTransform.Translate(Vector3.back * speed * Time.deltaTime);
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
}
