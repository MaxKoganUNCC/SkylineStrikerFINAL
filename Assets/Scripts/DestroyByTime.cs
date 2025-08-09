using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public float lifeTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

}
