using UnityEngine;
using UnityEngine.UI;

public class ClientOrder : MonoBehaviour
{
    [Header("État")]
    public string orderedDish;

    [Header("UI Bulle")]
    [SerializeField] private Image dishIcon; // L'Image UI dans la bulle

    [Header("Sprites des plats")]
    [SerializeField] private Sprite painTomateSprite;
    [SerializeField] private Sprite painSaladeSprite;
    [SerializeField] private Sprite viandeTomateSprite;
    [SerializeField] private Sprite viandeSaladeSprite;
    [SerializeField] private Sprite saladeTomateSprite;
    [SerializeField] private Sprite painViandeTomateSprite;
    [SerializeField] private Sprite painViandeSaladeSprite;
    [SerializeField] private Sprite painTomateSaladeSprite;
    [SerializeField] private Sprite viandeTomateSaladeSprite;

    public void SetRandomOrder()
    {
        
        string[] dishes = 
        { 
            "Pain tomate", 
            "Pain salade", 
            "Viande tomate", 
            "Viande salade", 
            "Salade tomate", 
            "Pain viande tomate", 
            "Pain viande salade", 
            "Pain tomate salade", 
            "Viande tomate salade" 
        };

        int index = Random.Range(0, dishes.Length);
        orderedDish = dishes[index];

        if (!dishIcon)
        {
            Debug.LogWarning("DishIcon non assigné dans ClientOrder sur " + gameObject.name);
            return;
        }

        switch (orderedDish)
        {
            case "Pain tomate": dishIcon.sprite = painTomateSprite; break;
            case "Pain salade": dishIcon.sprite = painSaladeSprite; break;
            case "Viande tomate": dishIcon.sprite = viandeTomateSprite; break;
            case "Viande salade": dishIcon.sprite = viandeSaladeSprite; break;
            case "Salade tomate": dishIcon.sprite = saladeTomateSprite; break;
            case "Pain viande tomate": dishIcon.sprite = painViandeTomateSprite; break;
            case "Pain viande salade": dishIcon.sprite = painViandeSaladeSprite; break;
            case "Pain tomate salade": dishIcon.sprite = painTomateSaladeSprite; break;
            case "Viande tomate salade": dishIcon.sprite = viandeTomateSaladeSprite; break;
            default:
                dishIcon.sprite = null;
                Debug.LogWarning("Aucune icône trouvée pour la commande: " + orderedDish);
                break;
        }
        dishIcon.enabled = dishIcon.sprite != null;
    }
}
