using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {

        GameObject soundGameObject = new GameObject("SoundFX");
        soundGameObject.transform.position = spawnTransform.position;

        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();

        audioSource.clip = audioClip;

        audioSource.volume = volume;

        audioSource.Play();

        float clipLength = audioSource.clip != null ? audioSource.clip.length : 0f;

        Destroy(soundGameObject, clipLength);
    }
}
