using UnityEngine;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    public Transform playerTransform;
    public Transform playerModelTransform;
    private float playerSpeed;
    private float normalHeight;
    public float playerLaserLimit;
    public float playerLaserCooldown;
    private float tilt;
    private float laserSpacing = 0.5f;

    public GameObject PlayerLaser;
    public GameController gameController;

    private int shotsPerVolley;
    private int maxLaserLimit;
    private List<GameObject> activeLasers = new List<GameObject>();

    public AudioClip playerShootSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerSpeed = 15f;
        normalHeight = 0.5f;
        playerLaserLimit = 2f;
        playerLaserCooldown = 0f;
        tilt = 5f;

        UpdateWeaponStats();
    }

    // Update is called once per frame
    void Update()
    {
        float currentX = playerTransform.position.x;
        float currentZ = playerTransform.position.z;

        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow))
            direction.z += 1;

        if (Input.GetKey(KeyCode.DownArrow))
            direction.z -= 1;

        if (Input.GetKey(KeyCode.RightArrow))
            direction.x += 1;

        if (Input.GetKey(KeyCode.LeftArrow))
            direction.x -= 1;

        // Normalize to prevent faster diagonal movement
        if (direction != Vector3.zero)
        {
            direction.Normalize();

            Vector3 newPosition = playerTransform.position + direction * playerSpeed * Time.deltaTime;

            // Clamp within movement boundaries
            newPosition.x = Mathf.Clamp(newPosition.x, -16f, 16f);
            newPosition.z = Mathf.Clamp(newPosition.z, -9f, 9f);

            playerTransform.position = new Vector3(newPosition.x, normalHeight, newPosition.z);
        }

        // Firing laser
        playerLaserLimit = maxLaserLimit - activeLasers.Count;

        if ((Input.GetKey(KeyCode.Z)) && (playerLaserCooldown == 0f))
        {
            if (playerLaserLimit >= shotsPerVolley)
            {
                float startOffset = 0f;
                if (shotsPerVolley > 1)
                {
                    // For even numbers of shots, the center is between two lasers.
                    // For odd numbers of shots, the center laser is at the player's position.
                    startOffset = -((shotsPerVolley - 1) * laserSpacing) / 2f;
                }

                for (int i = 0; i < shotsPerVolley; i++)
                {
                    Vector3 offsetPosition = new Vector3(playerTransform.position.x + startOffset + (i * laserSpacing), playerTransform.position.y, playerTransform.position.z + 1);
                    Quaternion rotation = Quaternion.Euler(90f, 0f, 0f);

                    GameObject laser = Instantiate(PlayerLaser, offsetPosition, rotation);

                    PlayerLaserManager laserScript = laser.GetComponent<PlayerLaserManager>();
                    if (laserScript != null)
                    {
                        laserScript.playerManager = this;
                    }

                    activeLasers.Add(laser);
                }
                playerLaserCooldown = 0.2f;
                SoundFXManager.instance.PlaySoundFXClip(playerShootSound, transform, 1.0f);
            }
        }



        //Cooldown
        if (playerLaserCooldown > 0f)
        {
            playerLaserCooldown -= Time.deltaTime;
            if (playerLaserCooldown < 0f)
            {
                playerLaserCooldown = 0f; // Clamp
            }
        }
        // Clamp playerLaserLimit to maxLaserLimit
        playerLaserLimit = Mathf.Min(playerLaserLimit, maxLaserLimit);
    }
    public void UpdateWeaponStats()
    {

        int currentPowerLevel = gameController.GetPowerLevel(); // Fetch power level from GameController

        switch (currentPowerLevel)
        {
            case 0:
                shotsPerVolley = 1;
                maxLaserLimit = 2;
                break;
            case 1:
                shotsPerVolley = 1;
                maxLaserLimit = 3;
                break;
            case 2:
                shotsPerVolley = 2;
                maxLaserLimit = 4;
                break;
            case 3:
                shotsPerVolley = 2;
                maxLaserLimit = 6;
                break;
            case 4:
                shotsPerVolley = 3;
                maxLaserLimit = 6;
                break;
            case 5:
                shotsPerVolley = 3;
                maxLaserLimit = 9;
                break;
            case 6:
                shotsPerVolley = 3;
                maxLaserLimit = 12;
                break;
            default:
                shotsPerVolley = 1;
                maxLaserLimit = 2;
                break;
        }
    }

    public void RemoveActiveLaser(GameObject laser)
    {
        activeLasers.Remove(laser);
    }
}
