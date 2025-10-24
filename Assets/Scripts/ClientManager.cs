using System.Collections.Generic;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    [SerializeField] private GameObject clientPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnInterval = 5f;

    private float timer;
    public List<ClientOrder> activeClients = new List<ClientOrder>();

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
           //SpawnClient();
            timer = 0f;
        }
    }

    private void SpawnClient()
{
        Debug.Log("SpawnClient appelé");
        Instantiate(clientPrefab, spawnPoint.position, Quaternion.identity);
}


    public void RemoveClient(ClientOrder client)
    {
        if (activeClients.Contains(client))
        {
            activeClients.Remove(client);
        }
    }

    public ClientOrder GetNextClient()
    {
        if (activeClients.Count > 0)
            return activeClients[0];  
        return null;
    }
}