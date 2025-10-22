using UnityEngine;

public class OrderValidator : MonoBehaviour
{
    [SerializeField] private int pointsPerCorrectDish = 10;

    public void TryDeliver(string deliveredDish, ClientOrder client)
    {
        if (!client) return;

        if (deliveredDish == client.orderedDish)
        {
            Debug.Log($"Bonne commande ({deliveredDish}) !");
            ScoreManager.Instance.AddScore(pointsPerCorrectDish);
            Destroy(client.gameObject);
        }
        else
        {
            Debug.Log($"Mauvais plat : attendu {client.orderedDish}, reçu {deliveredDish}");
        }
    }
}
