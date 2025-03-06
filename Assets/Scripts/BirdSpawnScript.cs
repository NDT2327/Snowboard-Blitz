using UnityEngine;

public class BirdSpawnScript : MonoBehaviour
{
    public GameObject bird;
    public float minSpawnTime = 0f;
    public float maxSpawnTime = 20f;
    public float minY = -8f, maxY = 8f;

    private float nextSpawnTime;
    private Transform cameraTransform; // Lưu Main Camera

    void Start()
    {
        // Lấy Main Camera
        cameraTransform = Camera.main.transform;
        SetNextSpawnTime();
    }

    void Update()
    {
        // Cập nhật vị trí theo Camera
        transform.position = new Vector3(cameraTransform.position.x + 30f, cameraTransform.position.y, transform.position.z);

        if (Time.time >= nextSpawnTime)
        {
            SpawnBird();
            SetNextSpawnTime();
        }
    }

    void SpawnBird()
    {
        float randomY = Random.Range(cameraTransform.position.y + 3f, cameraTransform.position.y - 3f);
        Vector2 spawnPosition = new Vector2(transform.position.x, randomY);
        Instantiate(bird, spawnPosition, Quaternion.identity);
    }

    void SetNextSpawnTime()
    {
        float randomInterval = Random.Range(minSpawnTime, maxSpawnTime);
        nextSpawnTime = Time.time + randomInterval;
    }
}