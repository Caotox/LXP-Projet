using UnityEngine;

public class DeliveryZoneHandler : MonoBehaviour
{
    private OrderValidator validator;

    private void Start()
    {
        validator = Object.FindFirstObjectByType<OrderValidator>();
        if (validator == null)
            Debug.LogWarning("⚠ Aucun OrderValidator trouvé dans la scène !");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Vérifie si c'est un plat livrable
        bool isBurger = other.CompareTag("Burger");
        bool hasAliments = other.GetComponent<AlimentController>() != null;

        if (!(isBurger || hasAliments)) return;

        ClientOrder client = Object.FindFirstObjectByType<ClientOrder>();
        if (client != null && validator != null)
        {
            bool valid = validator.TryDeliver(other.gameObject, client);
            if (valid)
            {
                // ✅ Plat validé → on le détruit ici
                Destroy(other.gameObject);
            }
        }
    }
}