using UnityEngine;

public class OrderValidator : MonoBehaviour
{
    [SerializeField] private int pointsPerCorrectDish = 10;

    public void TryDeliver(GameObject deliveredObject, ClientOrder client)
    {
        if (client == null)
        {
            Debug.LogWarning("⚠ Aucun client trouvé !");
            return;
        }

        string ordered = client.orderedDish;

        // 🥇 Cas Burger
        if (ordered == "Burger")
        {
            if (deliveredObject.CompareTag("Burger"))
            {
                Validate(client);
            }
            else
            {
                Debug.Log("❌ Commande Burger, mais mauvais plat livré.");
            }
            return;
        }

        // 🥈 Cas plats composés
        AlimentController aliments = deliveredObject.GetComponent<AlimentController>();
        if (aliments == null)
        {
            Debug.LogWarning("⚠ Plat livré sans AlimentController !");
            return;
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
            Validate(client);
        else
            Debug.Log($"❌ Mauvais ingrédients pour '{ordered}'");
    }

    private void Validate(ClientOrder client)
    {
        ScoreManager.Instance.AddScore(pointsPerCorrectDish);
        Debug.Log($"✅ Commande '{client.orderedDish}' validée (+{pointsPerCorrectDish} pts)");
        Destroy(client.gameObject);
    }
}
