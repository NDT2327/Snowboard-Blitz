using UnityEngine;

public class SnowFallScript : MonoBehaviour
{
    private Transform cameraTransform; // Lưu Main Camera

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraTransform = Camera.main.transform;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(cameraTransform.position.x , cameraTransform.position.y + 10f, transform.position.z);

    }
}
