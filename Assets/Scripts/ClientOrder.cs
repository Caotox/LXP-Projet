using UnityEngine;
using UnityEngine.UI;

public class ClientOrder : MonoBehaviour
{
    [Header("Commande actuelle")]
    public string orderedDish;

    [Header("UI Bulle")]
    [SerializeField] private Image dishIcon;

    [Header("Sprites des plats")]
    [SerializeField] private Sprite burgerSprite;
    [SerializeField] private Sprite painViandeSaladeSprite;
    [SerializeField] private Sprite painViandeTomateSprite;
    [SerializeField] private Sprite painSaladeTomateSprite;

    private readonly string[] dishes = {
        "Burger",
        "Pain viande salade",
        "Pain viande tomate",
        "Pain salade tomate"
    };

    private void Start()
    {
        SetRandomOrder();
    }

    public void SetRandomOrder()
    {
        int index = Random.Range(0, dishes.Length);
        orderedDish = dishes[index];

        if (!dishIcon) { Debug.LogWarning("DishIcon non assigné !"); return; }

        switch (orderedDish)
        {
            case "Burger":                dishIcon.sprite = burgerSprite; break;
            case "Pain viande salade":    dishIcon.sprite = painViandeSaladeSprite; break;
            case "Pain viande tomate":    dishIcon.sprite = painViandeTomateSprite; break;
            case "Pain salade tomate":    dishIcon.sprite = painSaladeTomateSprite; break;
        }

        dishIcon.enabled = dishIcon.sprite != null;
    }
}
