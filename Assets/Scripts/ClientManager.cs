using UnityEngine;

public class ClientManager : MonoBehaviour
{
    [Header("Références")]
public GameObject clientPrefab;
    public Transform spawnPoint;

    [Header("Paramètres de spawn")]
    public float spawnInterval = 15f;

    private float timer;
    public float spawnDistance = 8f;

    private void Start()
    {
        SpawnClient();

        timer = 0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnClient();
            timer = 0f;
        }
    }

    private void SpawnClient()
    {
        if (clientPrefab == null || spawnPoint == null)
        {
            Debug.LogWarning("Client Prefab ou SpawnPoint non assigné !");
            return;
        }

        Instantiate(clientPrefab, spawnPoint.position + Vector3.down * spawnDistance, Quaternion.identity);
        spawnDistance += 8f;
        Debug.Log("Nouveau client spawn !");
    }
}
