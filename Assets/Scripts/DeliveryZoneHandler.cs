using UnityEngine;

public class DeliveryZoneHandler : MonoBehaviour
{
    private OrderValidator validator;

    [Header("Assiette vide à respawn")]
    [SerializeField] private GameObject emptyPlatePrefab;

    private void Start()
    {
        validator = Object.FindFirstObjectByType<OrderValidator>();
        if (validator == null)
            Debug.LogWarning("⚠ Aucun OrderValidator trouvé dans la scène !");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        bool isBurger = other.CompareTag("Burger");
        bool hasAliments = other.GetComponent<AlimentController>() != null;

        if (!(isBurger || hasAliments)) return;

        ClientOrder client = Object.FindFirstObjectByType<ClientOrder>();
        if (client != null && validator != null)
        {
            bool valid = validator.TryDeliver(other.gameObject, client);
            if (valid)
            {
                if (emptyPlatePrefab != null)
                {
                    Instantiate(emptyPlatePrefab, other.gameObject.transform.position, Quaternion.identity);
                    Destroy(other.gameObject);
                }
                else
                {
                    Debug.LogWarning("⚠ EmptyPlatePrefab non assigné !");
                }
            }
        }
    }
}