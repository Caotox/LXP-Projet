using UnityEngine;

public class OrderValidator : MonoBehaviour
{
    [Header("Points attribués pour une bonne commande")]
    [SerializeField] private int pointsPerCorrectDish = 10;

    public void TryDeliver(string deliveredDish, ClientOrder client)
    {
        if (client == null)
        {
            Debug.LogWarning("Aucun client trouvé lors de la livraison !");
            return;
        }
        ScoreManager.Instance.AddScore(pointsPerCorrectDish);
        Debug.Log($"Commande livrée — {pointsPerCorrectDish} points ajoutés ");

        Destroy(client.gameObject);
    }
}
