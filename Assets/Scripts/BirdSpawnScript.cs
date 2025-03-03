using Unity.VisualScripting;
using UnityEngine;

public class BirdSpawnScript : MonoBehaviour
{
    public GameObject bird;
    public float minSpawnTime = 0f;  // Thời gian spawn tối thiểu
    public float maxSpawnTime = 20f; // Thời gian spawn tối đa
    public float minY = -8f, maxY = 8f; // Phạm vi spawn theo trục Y
    private float nextSpawnTime; // Lưu thời gian spawn tiếp theo

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetNextSpawnTime();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnBird();
            SetNextSpawnTime();
        }
    }

    void SpawnBird()
    {
        float randomY = Random.Range(minY,maxY);
        Vector2 spawnPosition = new Vector2(transform.position.x, randomY);
        Instantiate(bird, spawnPosition, Quaternion.identity);
    }

    void SetNextSpawnTime()
    {
        // Random thời gian cho lần spawn tiếp theo
        float randomInterval = Random.Range(minSpawnTime, maxSpawnTime);
        nextSpawnTime = Time.time + randomInterval;
    }
}
