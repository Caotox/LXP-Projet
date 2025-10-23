using UnityEngine;

public class ClientManager : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private GameObject clientPrefab;
    [SerializeField] private Transform spawnPoint;

    [Header("Paramètres de spawn")]
    [SerializeField] private float spawnInterval = 15f;

    private float timer;

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

        Instantiate(clientPrefab, spawnPoint.position, Quaternion.identity);
        Debug.Log("Nouveau client spawn !");
    }
}
