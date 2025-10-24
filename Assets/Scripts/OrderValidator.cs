using UnityEngine;

public class OrderValidator : MonoBehaviour
{
    [SerializeField] private int pointsPerCorrectDish = 10;
    public GameObject clientPrefab;
    public Transform spawnPoint;

    // 👉 On renvoie un bool ici
    public bool TryDeliver(GameObject deliveredObject, ClientOrder client)
    {
        if (client == null)
        {
            Debug.LogWarning("⚠ Aucun client trouvé !");
            return false;
        }

        string ordered = client.orderedDish;

        // Cas Burger
        if (ordered == "Burger")
        {
            if (deliveredObject.CompareTag("Burger"))
            {
                Validate(client);
                return true;
            }
            else
            {
                Debug.Log("Commande Burger incorrecte.");
                return false;
            }
        }

        // Cas plats composés
        var aliments = deliveredObject.GetComponent<AlimentController>();
        if (aliments == null)
        {
            Debug.LogWarning("⚠ Plat sans AlimentsController !");
            return false;
        }

        bool valid = false;
        switch (ordered)
        {
            case "Pain viande salade":
                valid = aliments.hasPain && aliments.hasSteak && aliments.hasSalade && !aliments.hasTomate;
                break;
            case "Pain viande tomate":
                valid = aliments.hasPain && aliments.hasSteak && aliments.hasTomate && !aliments.hasSalade;
                break;
            case "Pain salade tomate":
                valid = aliments.hasPain && aliments.hasSalade && aliments.hasTomate && !aliments.hasSteak;
                break;
        }

        if (valid)
        {
            Validate(client);
            Instantiate(clientPrefab, spawnPoint.position, Quaternion.identity);
            return true;
        }
        else
        {
            Debug.Log($"Mauvais ingrédients pour '{ordered}'");
            return false;
        }
    }

    private void Validate(ClientOrder client)
    {
        ScoreManager.Instance.AddScore(pointsPerCorrectDish);
        Debug.Log($"Commande '{client.orderedDish}' validée (+{pointsPerCorrectDish} pts)");
        Destroy(client.gameObject);
    }
}