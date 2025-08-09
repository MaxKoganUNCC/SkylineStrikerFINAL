using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameController : MonoBehaviour
{
    // For Fodder
    public GameObject fodderPrefab;
    public Vector3 spawnValues;
    public int fodderCount;
    public float fodderSpawnWait;
    public float fodderWaveWait;

    // For Ranger
    public GameObject rangerPrefab;
    public int rangerCountPerWave;
    public float rangerSpawnWait;
    public float rangerWaveWait;

    public float rangerSpawnX_Left = -30f;
    public float rangerSpawnX_Right = 30f;
    public float rangerSpawnZ_Min = 4f;
    public float rangerSpawnZ_Max = 8f;
    public float rangerSpeed = 4f;

    // For LargeFodder
    public GameObject largeFodderPrefab;
    public int largeFodderCountPerWave;
    public float largeFodderSpawnWait;
    public float largeFodderWaveWait;

    public float startWait;
    public float totalEnergy = 0f;
    public float maxEnergy = 6f;
    public int powerLevel = 0;
    public PlayerManager playerManager;
    public GameObject EnergyPrefab;

    [Header("UI")]
    public TMP_Text gameOverText;
    public TMP_Text powerText;

    private bool gameOver = false;

    void Start()
    {
        // Hide game over text at start
        if (gameOverText != null)
            gameOverText.gameObject.SetActive(false);

        // Find PlayerManager reference
        if (playerManager == null)
            playerManager = FindObjectOfType<PlayerManager>();

        if (playerManager != null)
            UpdatePowerLevel();

        // Start separate coroutines for each enemy type
        StartCoroutine(SpawnFodderWaves());
        StartCoroutine(SpawnRangerWaves());
        StartCoroutine(SpawnLargeFodderWaves());
    }

    void Update()
    {
        if (gameOver && Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }


    // Coroutine for spawning Fodder enemies
    IEnumerator SpawnFodderWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (!gameOver)
        {
            for (int i = 0; i < fodderCount; i++)
            {
                Instantiate(fodderPrefab,
                    new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z),
                    Quaternion.identity);
                yield return new WaitForSeconds(fodderSpawnWait);
            }
            yield return new WaitForSeconds(fodderWaveWait);
        }
    }

    // Coroutine for spawning Ranger enemies
    IEnumerator SpawnRangerWaves()
    {
        yield return new WaitForSeconds(startWait + rangerWaveWait / 2);
        while (!gameOver)
        {
            for (int i = 0; i < rangerCountPerWave; i++)
            {
                bool spawnLeft = Random.value < 0.5f;
                float spawnX = spawnLeft ? rangerSpawnX_Left : rangerSpawnX_Right;
                float spawnZ = Random.Range(rangerSpawnZ_Min, rangerSpawnZ_Max);

                GameObject ranger = Instantiate(rangerPrefab,
                    new Vector3(spawnX, spawnValues.y, spawnZ),
                    Quaternion.identity);

                Ranger rangerScript = ranger.GetComponent<Ranger>();
                if (rangerScript != null)
                    rangerScript.speed = spawnLeft ? rangerSpeed : -rangerSpeed;

                yield return new WaitForSeconds(rangerSpawnWait);
            }
            yield return new WaitForSeconds(rangerWaveWait);
        }
    }

    // Coroutine for spawning LargeFodder enemies
    IEnumerator SpawnLargeFodderWaves()
    {
        yield return new WaitForSeconds(startWait + largeFodderWaveWait);
        while (!gameOver)
        {
            for (int i = 0; i < largeFodderCountPerWave; i++)
            {
                Instantiate(largeFodderPrefab,
                    new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z),
                    Quaternion.identity);
                yield return new WaitForSeconds(largeFodderSpawnWait);
            }
            yield return new WaitForSeconds(largeFodderWaveWait);
        }
    }

    public void AddEnergy(float amount)
    {
        totalEnergy += amount;
        totalEnergy = Mathf.Min(totalEnergy, maxEnergy);
        UpdatePowerLevel();

        if (totalEnergy < 0 && !gameOver)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        gameOver = true;

        // Stop all enemies
        StopAllCoroutines();

        // Show Game Over UI
        if (gameOverText != null)
        {
            gameOverText.gameObject.SetActive(true);
            gameOverText.text = "GAME OVER\nPress R to Restart";
        }
        Time.timeScale = 0f;
    }

    public int GetPowerLevel()
    {
        return powerLevel;
    }

    private void UpdatePowerLevel()
    {
        int newPowerLevel = Mathf.FloorToInt(totalEnergy);
        powerLevel = Mathf.Max(0, newPowerLevel);

        if (playerManager != null)
            playerManager.UpdateWeaponStats();

        // Update UI
        if (powerText != null)
            powerText.text = $"Power: {totalEnergy:F1}";
    }

}
