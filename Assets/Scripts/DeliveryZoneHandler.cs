using UnityEngine;

public class DeliveryZoneHandler : MonoBehaviour
{
    private OrderValidator validator;

    private void Start()
    {
        validator = Object.FindFirstObjectByType<OrderValidator>();
        if (validator == null)
        {
            Debug.LogWarning("Aucun OrderValidator trouvé dans la scène !");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Quelque chose est entré dans la Delivery Zone: " + other.name);

        if (other.CompareTag("Burger") && validator != null)
        {
            Debug.Log("Plat détecté, tentative de livraison...");
            ClientOrder client = Object.FindFirstObjectByType<ClientOrder>();

            if (client != null)
            {
                validator.TryDeliver("Burger", client);
                Destroy(other.gameObject);
                Debug.Log("Plat livré !");
            }
            else
            {
                Debug.LogWarning("Aucun client trouvé !");
            }
        }
    }
}