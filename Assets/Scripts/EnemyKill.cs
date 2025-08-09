using UnityEngine;

public class EnemyKill : MonoBehaviour
{
    public AudioClip playerHurtSound;
    //Destroy laser and self
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerLaser"))
        {
            IDamageable damageableObject = GetComponent<IDamageable>();
            if (damageableObject != null)
            {
                damageableObject.TakeDamage(1f);
            }

            // Destroy the laser
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Player"))
        {
            Destroy(gameObject);       // Destroy enemy
            GameController gameController = FindObjectOfType<GameController>();
            if (gameController != null)
            {
                SoundFXManager.instance.PlaySoundFXClip(playerHurtSound, transform, 1.0f);
                gameController.AddEnergy(-1);
            }
        }
     }
}