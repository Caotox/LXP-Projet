using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private GameObject clientPrefab;
    [SerializeField] private Transform spawnPoint;

    [Header("Paramètres")]
    [SerializeField] private float spawnInterval = 15f;

    private readonly List<GameObject> clients = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            if (!clientPrefab || !spawnPoint) continue;

            GameObject c = Instantiate(clientPrefab, spawnPoint.position, Quaternion.identity);
            clients.Add(c);

            var order = c.GetComponent<ClientOrder>();
            if (order) order.SetRandomOrder();
        }
    }
}
