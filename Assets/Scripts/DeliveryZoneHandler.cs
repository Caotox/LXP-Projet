using UnityEngine;

public class DeliveryZoneHandler : MonoBehaviour
{
    private OrderValidator validator;

    private void Start()
    {
        validator = FindObjectOfType<OrderValidator>();

        if (validator == null)
        {
            Debug.LogWarning("Aucun OrderValidator trouvé dans la scène !");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Burger"))
        {
            ClientOrder client = FindObjectOfType<ClientOrder>();
            if (client != null && validator != null)
            {
                validator.TryDeliver("Burger", client);
            }
            Destroy(other.gameObject);
        }
    }
}